using SimpleJSON;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum UserType { Null, super_admin, project_admin, project_owner, collaborator}
public enum AppScreen { Login, SuperAdmin, ProjectSelect, FloorSelect, RoomSelect, BubbleEditor}
// This AppScreen enum was made with the intention of making a more modular scene loading system.
// One where you would not have to hard code in the name of a scene you'd like to load. One where scene names could change without breaking the build.
// The idea is that there would be a single LoadScreen function where you feed in this enum and it will call the appropiate scenes.

public class AppManager : MonoBehaviour
{
	// Most variables in the project are public for ease of prototyping
	public static AppManager appManager = null;
	bool loggedIn = false;
	public bool isDemo = false;
	public UIManager currUIManager;
	public string sessionId;
    public User mainUser;
	public User currUser;
    public List<User> projectAdmins;
	public int currProjId = 0;
	public int currFloorId = 0;
	public int currRoomId = 0;
	public int currBubbleId = 0;

    public event EventHandler ProjectAdminsUpdated;
    public event EventHandler ProjectsUpdated;
    public event EventHandler FloorsUpdated;
    public event EventHandler RoomsUpdated;
    public event EventHandler BubblesUpdated;

    private byte[] currUserImg;
    private byte[] currProjImg;
    private byte[] currFloorPlanImg;

    private void Awake()
	{
		#region Singleton Managment
		if (!appManager)
		{
			appManager = this;
		}

		if (appManager != this)
		{
			Destroy(this);
			return;
		}

		DontDestroyOnLoad(this);
		#endregion

		#region permission requests
		NativeGallery.Permission permGallery = NativeGallery.RequestPermission();
		if (permGallery != NativeGallery.Permission.Granted)
		{
			Debug.LogWarning("Application Quit! Gallery permission not granted");
			Application.Quit();
		}

		NativeCamera.Permission permCam = NativeCamera.RequestPermission();
		if (permCam != NativeCamera.Permission.Granted)
		{
			Debug.LogWarning("Application Quit! Camera permission not granted");
			Application.Quit();
		}
		#endregion
	}

	public void Logout()
	{
		loggedIn = false;
		isDemo = false;
		mainUser = null;
        projectAdmins = null;
	}

	#region Server Calls & Login
	public void OnLogin(UnityWebRequest _www)
	{
        //RestFarm.restFarm.DebugWebRequest(_www);

        string result = _www.downloadHandler.text;
        JSONNode node = JSON.Parse(result);

        sessionId = node["result"].AsObject["sessionId"].Value;
        var userId = node["result"].AsObject["userId"].Value;

        if (string.IsNullOrEmpty(sessionId))
        {
            Debug.LogError("The Session ID is blank!");
        }
        else
        {
            loggedIn = true;
            RestFarm.restFarm.GETSINGLE(sessionId, "/user?userId="+ userId, PopUser);
        }

        _www.Dispose();
    }

	// Pop for Populate! How neat.
	void PopUser(UnityWebRequest _www)
	{
		//RestFarm.restFarm.DebugWebRequest(_www);

        string result = _www.downloadHandler.text;
        JSONNode node = JSON.Parse(result);

        int id = node["result"].AsObject["user"].AsObject["id"].AsInt;
        string email = node["result"].AsObject["user"].AsObject["email"].Value;
        string firstName = node["result"].AsObject["user"].AsObject["firstName"].Value;
        string lastName = node["result"].AsObject["user"].AsObject["lastName"].Value;
        string tempInitials = node["result"].AsObject["user"].AsObject["initials"].Value;
        char[] initials = tempInitials.ToCharArray();

        Enum.TryParse(node["result"].AsObject["user"].AsObject["role"].Value, out UserType usertype);
        User newUser = new User(id, email, firstName, lastName, initials, null, usertype);
        mainUser = newUser;

        if(usertype == UserType.super_admin)
        {
            SceneManagement.sceneManagement.LoadScene("ProjectAdminSelect");

        } else if (usertype == UserType.project_admin)
        {
            currUser = mainUser;
            SceneManagement.sceneManagement.LoadScene("ProjectSelect");
        } else
        {
            currUser = mainUser;
            SceneManagement.sceneManagement.LoadScene("FloorSelect");
        }

        _www.Dispose();
    }

    public void RequestProjectAdminData()
    {
        RestFarm.restFarm.GETALL(sessionId, "/users", PopProjectAdmins);
    }

    void PopProjectAdmins(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        string result = _www.downloadHandler.text;
        JSONNode node = JSON.Parse(result);
        var u = node["result"]["users"].AsArray;
        projectAdmins = new List<User>();

        for (int i = 0; i < u.Count; i++)
        {
            int id = u[i].AsObject["id"].AsInt;
            string email = u[i].AsObject["email"].Value;
            string firstName = u[i].AsObject["firstName"].Value;
            string lastName = u[i].AsObject["lastName"].Value;
            string tempInitials = u[i].AsObject["initials"].Value;
            char[] initials = tempInitials.ToCharArray();

            Enum.TryParse(u[i].AsObject["role"].Value, out UserType usertype);

            if (usertype == UserType.project_admin)
            {
                User newUser = new User(id, email, firstName, lastName, initials, null, usertype);
                projectAdmins.Add(newUser);
            }
        }

        OnProjectAdminsUpdated();

        _www.Dispose();
    }

    void PopProjectAdmin(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        string result = _www.downloadHandler.text;
        JSONNode node = JSON.Parse(result);

        int id = node["result"].AsObject["user"].AsObject["id"].AsInt;
        string email = node["result"].AsObject["user"].AsObject["email"].Value;
        string firstName = node["result"].AsObject["user"].AsObject["firstName"].Value;
        string lastName = node["result"].AsObject["user"].AsObject["lastName"].Value;
        string tempInitials = node["result"].AsObject["user"].AsObject["initials"].Value;
        char[] initials = tempInitials.ToCharArray();

        Enum.TryParse(node["result"].AsObject["user"].AsObject["role"].Value, out UserType usertype);
        User newUser = new User(id, email, firstName, lastName, initials, null, usertype);
        projectAdmins.Add(newUser);

        OnProjectAdminsUpdated();

        _www.Dispose();
    }

    public void RequestProjects(int userId = -1)
    {
        if (userId == -1)
        {
            RestFarm.restFarm.GETALL(sessionId, "/projects", PopProjects);
        } else
        {
            RestFarm.restFarm.GETALL(sessionId, "/projects?queryUserId="+userId, PopProjects);
        }
    }

	void PopProjects(UnityWebRequest _www)
	{
        //RestFarm.restFarm.DebugWebRequest(_www);

        string result = _www.downloadHandler.text;
        JSONNode node = JSON.Parse(result);

        List<Project> tempList = new List<Project>();
        foreach (JSONObject project in node["result"].AsObject["projects"].AsArray)
        {
            int id = project.AsObject["id"].AsInt;
            string name = project.AsObject["name"].Value;
            tempList.Add(new Project(id, name, new byte[0], new List<Floor>()));
        }
        currUser.projects = tempList;

        OnProjectsUpdated();

        _www.Dispose();
    }

    void PopProject(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        string result = _www.downloadHandler.text;
        JSONNode node = JSON.Parse(result);

        int id = node["result"].AsObject["project"].AsObject["id"].AsInt;
        string name = node["result"].AsObject["project"].AsObject["name"].Value;
        var project = currUser.projects.Find(p => p.projectId == id);
        if (project == null)
        {
            currUser.projects.Add(new Project(id, name, new byte[0], new List<Floor>()));
        } else
        {
            project.name = name;
        }

        OnProjectsUpdated();

        _www.Dispose();
    }

    void PopProjectPlans(UnityWebRequest _www)
	{
		//RestFarm.restFarm.DebugWebRequest(_www); //Uncomment this to debug info about the web request

		string result = _www.downloadHandler.text;
		JSONNode node = JSON.Parse(result);

        // INCOMPLETE

        _www.Dispose();
    }

    public void RequestFloors()
    {
        RestFarm.restFarm.GETALL(sessionId, "/floors?projectId=" + currProjId, PopFloors);
    }

	void PopFloors(UnityWebRequest _www)
	{
        //RestFarm.restFarm.DebugWebRequest(_www); //Uncomment this to debug info about the web request

        string result = _www.downloadHandler.text;
		JSONNode node = JSON.Parse(result);

        List<Floor> tempList = new List<Floor>();
        foreach (JSONObject project in node["result"].AsObject["floors"].AsArray)
        {
            int id = project.AsObject["id"].AsInt;
            string name = project.AsObject["name"].Value;
            tempList.Add(new Floor(id, name, new byte[0], new List<Room>()));
        }
        FindProjectByID(currProjId).floors = tempList;

        OnFloorsUpdated();

        _www.Dispose();
    }

    void PopFloor(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www); //Uncomment this to debug info about the web request

        string result = _www.downloadHandler.text;
        JSONNode node = JSON.Parse(result);

        int id = node["result"].AsObject["floor"].AsObject["id"].AsInt;
        string name = node["result"].AsObject["floor"].AsObject["name"].Value;
        int projId = node["result"].AsObject["floor"].AsObject["projectId"].AsInt;
        FindProjectByID(projId).floors.Add(new Floor(id, name, new byte[0], new List<Room>()));

        OnFloorsUpdated();

        _www.Dispose();
    }

    public void RequestRooms()
    {
        RestFarm.restFarm.GETALL(sessionId, "/rooms?floorId=" + currFloorId, PopRooms);
    }

    void PopRooms(UnityWebRequest _www)
	{
        //RestFarm.restFarm.DebugWebRequest(_www); //Uncomment this to debug info about the web request

        string result = _www.downloadHandler.text;
		JSONNode node = JSON.Parse(result);

		List<Room> tempList = new List<Room>();

        foreach (JSONObject room in node["result"].AsObject["rooms"].AsArray)
        {
            int id = room.AsObject["id"].AsInt;
            string name = room.AsObject["name"].Value;
            int projectId = room.AsObject["projectId"].AsInt;
            string transform = room.AsObject["transform"].Value;

            Vector2 pos = new Vector2();
            Vector3 scale = new Vector3();

            int i = transform.IndexOf(")");
            if (i > 0)
            {
                pos = StringToVector(transform.Substring(0, i + 1));
                scale = StringToVector(transform.Substring(i + 1));
            }

            tempList.Add(new Room(id, projectId, name, pos, scale));
        }
        FindFloorByID(currProjId, currFloorId).rooms = tempList;

        OnRoomsUpdated();

        _www.Dispose();
    }

    void PopRoom(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www); //Uncomment this to debug info about the web request

        string result = _www.downloadHandler.text;
        JSONNode node = JSON.Parse(result);

        int id = node["result"].AsObject["room"].AsObject["id"].AsInt;
        string name = node["result"].AsObject["room"].AsObject["name"].Value;
        int floorId = node["result"].AsObject["room"].AsObject["floorId"].AsInt;
        string transform = node["result"].AsObject["room"].AsObject["transform"].Value;
        FindFloorByID(currProjId, floorId).rooms.Add(new Room(id, currProjId, name, new Vector2(), new Vector2()));

        OnRoomsUpdated();

        _www.Dispose();
    }

    public void RequestBubbles()
    {
        RestFarm.restFarm.GETALL(sessionId, "/bubbles?roomId=" + currRoomId, PopBubbles);
    }

	void PopBubbles(UnityWebRequest _www)
	{
		//RestFarm.restFarm.DebugWebRequest(_www);

		string result = _www.downloadHandler.text;
		JSONNode node = JSON.Parse(result);

		List<Bubble> tempList = new List<Bubble>();

		foreach (JSONObject bubble in node["result"].AsObject["bubbles"].AsArray)
		{
			int id = bubble.AsObject["id"].AsInt;
			int roomId = bubble.AsObject["roomID"].AsInt;
			int x = bubble.AsObject["x"].AsInt;
			int y = bubble.AsObject["y"].AsInt;
			string name = bubble.AsObject["name"].Value;
			Vector2 pos = new Vector2(x, y);

            tempList.Add(new Bubble(id, roomId, name, pos));
		}

        FindRoomByID(currProjId, currFloorId, currRoomId).bubbles = tempList;

        OnBubblesUpdated();

        _www.Dispose();
    }

    void PopBubble(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        string result = _www.downloadHandler.text;
        JSONNode node = JSON.Parse(result);

        int id = node["result"].AsObject["bubble"].AsObject["id"].AsInt;
        string name = node["result"].AsObject["bubble"].AsObject["name"].Value;
        int roomId = node["result"].AsObject["bubble"].AsObject["roomId"].AsInt;
        int x = node["result"].AsObject["bubble"].AsObject["x"].AsInt;
        int y = node["result"].AsObject["bubble"].AsObject["y"].AsInt;
        Vector2 pos = new Vector2(x, y);

        var bubble = FindRoomByID(currProjId, currFloorId, roomId).bubbles.Find(p => p.bubbleId == id);
        if(bubble == null)
        {
            FindRoomByID(currProjId, currFloorId, roomId).bubbles.Add(new Bubble(id, roomId, name, pos));
        }
        else
        {
            bubble.pos = pos;
        }

        OnBubblesUpdated();

        _www.Dispose();

    }
	#endregion

	/// <summary>
	/// This function does everything needed to put the app into "Demo mode." AKA, no real connection to a server, just a prefilled in user to show off the app.
	/// Right now, this is activated by hitting the "login" button with one of the login fields blank.
	/// </summary>
	public void ActivateDemoMode()
	{
		User dylan = new User(1, "dylan@example.com", "Dylan", "Rader", new char[2], new List<Project>(), UserType.project_admin);
		dylan.initials[0] = 'D';
		dylan.initials[1] = 'R';

		dylan.projects.Add(new Project(1, "Project 1", new byte[0], new List<Floor>()));
		dylan.projects.Add(new Project(2, "Project 2", new byte[0], new List<Floor>()));
		dylan.projects.Add(new Project(3, "Project 3", new byte[0], new List<Floor>()));

		dylan.projects[0].floors.Add(new Floor(1, "Floor 1"));
		dylan.projects[0].floors.Add(new Floor(2, "Floor 2"));
															  
		dylan.projects[1].floors.Add(new Floor(3, "Room 3"));
		dylan.projects[1].floors.Add(new Floor(4, "Room 4"));
															  
		dylan.projects[2].floors.Add(new Floor(5, "Room 5"));
		dylan.projects[2].floors.Add(new Floor(6, "Room 6"));

		
		dylan.projects[0].floors[0].rooms.Add(new Room(1, dylan.projects[0].projectId, "Room 1", new Vector2(), new Vector2()));
        dylan.projects[0].floors[0].rooms.Add(new Room(2, dylan.projects[0].projectId, "Room 2", new Vector2(), new Vector2()));
        dylan.projects[0].floors[1].rooms.Add(new Room(3, dylan.projects[0].projectId, "Room 3", new Vector2(), new Vector2()));

        dylan.projects[1].floors[0].rooms.Add(new Room(4, dylan.projects[1].projectId, "Room 4", new Vector2(), new Vector2()));
        dylan.projects[1].floors[0].rooms.Add(new Room(5, dylan.projects[1].projectId, "Room 5", new Vector2(), new Vector2()));
        dylan.projects[1].floors[1].rooms.Add(new Room(6, dylan.projects[1].projectId, "Room 6", new Vector2(), new Vector2())); ;
											 
		dylan.projects[2].floors[0].rooms.Add(new Room(7, dylan.projects[1].projectId, "Room 7", new Vector2(), new Vector2()));
        dylan.projects[2].floors[0].rooms.Add(new Room(8, dylan.projects[1].projectId, "Room 8", new Vector2(), new Vector2()));
        dylan.projects[2].floors[1].rooms.Add(new Room(9, dylan.projects[1].projectId, "Room 9", new Vector2(), new Vector2()));

        isDemo = true;
		currUser = dylan;

		SceneManagement.sceneManagement.LoadScene("ProjectSelect");
	}

	#region User Data Searching
	// A lot of util functions for searching for particular data inside of the current user.
    public User FinUserById(int _userId)
    {
        foreach (User user in projectAdmins)
        {
            if (user.userId == _userId)
            {
                return user;
            }
        }

        return new User();
    }

	public Project FindProjectByID(int _projId)
	{
		if (currUser == null)
		{
			Debug.Log("There is no current user! Returning a blank project.");
			return new Project();
		}

		foreach(Project proj in currUser.projects)
		{
			if(proj.projectId == _projId)
			{
				return proj;
			}
		}

		Debug.LogWarning("The current user has no projects with that ID! Returning a blank project.");
		return new Project();
	}
	
	public Floor FindFloorByID(int _projId, int _floorId)
	{
		if (currUser == null)
		{
			Debug.Log("There is no current user! Returning a blank floor.");
			return new Floor();
		}

		Project tempProj = FindProjectByID(_projId);

		foreach (Floor floor in tempProj.floors)
		{
			if (floor.floorId == _floorId)
			{
				return floor;
			}
		}

		Debug.LogWarning("The current Project has no Floors with that ID! Returning a blank Floor.");
		return new Floor();
	}

	public Room FindRoomByID(int _projId, int _floorId, int _roomId)
	{
		if (currUser == null)
		{
			Debug.Log("There is no current user! Returning a blank room.");
			return new Room();
		}

		Floor tempFloor = FindFloorByID(_projId, _floorId);

		foreach (Room room in tempFloor.rooms)
		{
			if (room.roomId == _roomId)
			{
				return room;
			}
		}

		Debug.LogWarning("The current user has no room with that ID in that project! Returning a blank room.");
		return new Room();
	}

	public Bubble FindBubbleByID(int _projId, int _floorId, int _roomId, int _bubbleId)
	{
		if (currUser == null)
		{
			Debug.Log("There is no current user! Returning an empty bubble.");
			return new Bubble();
		}

		Room tempRoom = FindRoomByID(_projId, _floorId, _roomId);

		foreach (Bubble bubble in tempRoom.bubbles)
		{
			if (bubble.bubbleId == _bubbleId)
			{
				return bubble;
			}
		}

		Debug.LogWarning("The current user has no bubble with that ID in that room! Returning an empty bubble.");
		return new Bubble();
	}

	public List<Project> FindAllProjects()
	{
		if (currUser == null)
		{
			Debug.Log("There is no current user! Returning a blank list of project.");
			return new List<Project>();
		}

		List<Project> tempList = new List<Project>();

		foreach (Project proj in currUser.projects)
		{
			tempList.Add(proj);
		}

		return tempList;
	}

	public List<Floor> FindAllFloors()
	{
		if (currUser == null)
		{
			Debug.Log("There is no current user! Returning a blank list of floors.");
			return new List<Floor>();
		}

		List<Project> projList = FindAllProjects();
		List<Floor> tempList = new List<Floor>();

		foreach (Project proj in projList)
		{
			foreach (Floor floor in proj.floors)
			{
				tempList.Add(floor);
			}
		}

		return tempList;
	}

	public List<Room> FindAllRooms()
	{
		if (currUser == null)
		{
			Debug.Log("There is no current user! Returning a blank list of rooms.");
			return new List<Room>();
		}

		List<Floor> floorList = FindAllFloors();
		List<Room> tempList = new List<Room>();

		foreach(Floor floor in floorList)
		{
			foreach(Room room in floor.rooms)
			{
				tempList.Add(room);
			}
		}

		return tempList;
	}

	#endregion

    public void AddNewUser(string _name, string _email, string _password, string _role, byte[] _data)
    {
        Dictionary<string, string> formData = new Dictionary<string, string>();
        var n = _name.IndexOf(' ');

        if(n != -1)
        {
            formData.Add("firstName", _name.Substring(0, n));
            formData.Add("lastName", _name.Substring(n + 1));
        } else
        {
            formData.Add("firstName", _name);
            formData.Add("lastName", "");
        }

        formData.Add("email", _email);
        formData.Add("password", _password);
        formData.Add("role", _role);

        currUserImg = _data;
        RestFarm.restFarm.POST(sessionId, "/users", formData, OnUserCreated);
    }

    private void OnUserCreated(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        string result = _www.downloadHandler.text;
        JSONNode node = JSON.Parse(result);

        var userId = node["result"].AsObject["id"].Value;

        Dictionary<string, string> formData = new Dictionary<string, string>();
        formData.Add("userId", userId.ToString());

        RestFarm.restFarm.POSTIMAGE(sessionId, "/user/image/" + userId, formData, currUserImg, (www) =>
        {
            //RestFarm.restFarm.DebugWebRequest(www);

            OnProjectAdminsUpdated();
            www.Dispose();
        });

        RestFarm.restFarm.GETSINGLE(sessionId, "/user?userId=" + userId, PopProjectAdmin);
        _www.Dispose();
    }

    public void DeleteUser()
    {
        RestFarm.restFarm.DELETE(sessionId, "/user?userId=" + currUser.userId, OnUserDeleted);
    }

    public void OnUserDeleted(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);
        projectAdmins.Remove(currUser);

        OnProjectAdminsUpdated();

        _www.Dispose();
    }

    private void OnProjectAdminsUpdated()
    {
        var handler = ProjectAdminsUpdated;
        if (handler != null)
        {
            handler(this, EventArgs.Empty);
        }
    }

    public void AddNewProject(string _projName, string _email, string _password, byte[] _data)
    {
        Dictionary<string, string> formData = new Dictionary<string, string>();
        formData.Add("name", _projName);
        formData.Add("adminId", currUser.userId.ToString());
        formData.Add("ownerEmail", _email);
        formData.Add("ownerPassword", _password);
        formData.Add("ownerFirstName", currUser.firstName);
        formData.Add("ownerLastName", currUser.lastName);

        currProjImg = _data;

        RestFarm.restFarm.POST(sessionId, "/projects", formData, OnProjectCreated);
    }

    private void OnProjectCreated(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        string result = _www.downloadHandler.text;
        JSONNode node = JSON.Parse(result);

        var projectId = node["result"].AsObject["id"].Value;

        Dictionary<string, string> formData = new Dictionary<string, string>();
        formData.Add("projectId", projectId.ToString());

        RestFarm.restFarm.POSTIMAGE(sessionId, "/project/image/" + projectId, formData, currProjImg, (www) =>
        {
            //RestFarm.restFarm.DebugWebRequest(www);

            OnProjectsUpdated();
            www.Dispose();
        });

        RestFarm.restFarm.GETSINGLE(sessionId, "/project?projectId=" + projectId, PopProject);

        _www.Dispose();
    }

    public void DeleteProject()
    {
        RestFarm.restFarm.DELETE(sessionId, "/project?projectId=" + currProjId.ToString(), OnProjectDeleted);
    }

    private void OnProjectDeleted(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);
        var project = currUser.projects.Find(p => p.projectId == currProjId);
        if (project != null)
        {
            currUser.projects.Remove(project);
            OnProjectsUpdated();
        }

        _www.Dispose();
    }

    public void UpdateProject(string _name)
    {
        RestFarm.restFarm.PUT(sessionId, "/project?projectId=" + currProjId + "&name=" + _name, _name, OnProjectUpdated);
    }

    private void OnProjectUpdated(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        var project = currUser.projects.Find(p => p.projectId == currProjId);
        if (project != null)
        {
            RestFarm.restFarm.GETSINGLE(sessionId, "/project?projectId=" + currProjId.ToString(), PopProject);
        }

        _www.Dispose();
    }

    private void OnProjectsUpdated()
    {
        var handler = ProjectsUpdated;
        if (handler != null)
        {
            handler(this, EventArgs.Empty);
        }
    }

    public void AddFloor(string _floorName, byte[] _data)
    {
        Dictionary<string, string> formData = new Dictionary<string, string>();
        formData.Add("name", _floorName);
        formData.Add("projectId", currProjId.ToString());

        currFloorPlanImg = _data;
        RestFarm.restFarm.POST(sessionId, "/floors", formData, OnFloorCreated);
    }

    private void OnFloorCreated(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        string result = _www.downloadHandler.text;
        JSONNode node = JSON.Parse(result);

        var floorId = node["result"].AsObject["id"].Value;

        Dictionary<string, string> formData = new Dictionary<string, string>();
        formData.Add("floorId", floorId);

        RestFarm.restFarm.POSTIMAGE(sessionId, "/floor/plan/"+ floorId, formData, currFloorPlanImg, (www) =>
        {
            //RestFarm.restFarm.DebugWebRequest(www);

            OnFloorsUpdated();
            www.Dispose();
        });

        RestFarm.restFarm.GETSINGLE(sessionId, "/floor?floorId=" + floorId, PopFloor);
        _www.Dispose();
    }

    public void DeleteFloor()
    {
        RestFarm.restFarm.DELETE(sessionId, "/floor?floorId=" + currFloorId.ToString(), OnFloorDeleted);
    }

    private void OnFloorDeleted(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);
        var floor = FindProjectByID(currProjId).floors.Find(f => f.floorId == currFloorId);
        if (floor != null)
        {
            FindProjectByID(currProjId).floors.Remove(floor);
            OnFloorsUpdated();
        }

        _www.Dispose();
    }

    private void OnFloorsUpdated()
    {
        var handler = FloorsUpdated;
        if (handler != null)
        {
            handler(this, EventArgs.Empty);
        }
    }

    public void AddRoom(string _roomName)
    {
        Dictionary<string, string> formData = new Dictionary<string, string>();
        formData.Add("name", _roomName);
        formData.Add("floorId", currFloorId.ToString());
        formData.Add("transform", "");

        RestFarm.restFarm.POST(sessionId, "/rooms", formData, OnRoomCreated);
    }

    private void OnRoomCreated(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        string result = _www.downloadHandler.text;
        JSONNode node = JSON.Parse(result);

        var roomId = node["result"].AsObject["id"].Value;

        RestFarm.restFarm.GETSINGLE(sessionId, "/room?roomId=" + roomId, PopRoom);

        currRoomId = Int32.Parse(roomId);

        SceneManagement.sceneManagement.LoadScene("BubbleEditor");
    }

    public void DeleteRoom()
    {
        RestFarm.restFarm.DELETE(sessionId, "/room?roomId=" + currRoomId, OnRoomDeleted);
    }

    private void OnRoomDeleted(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);
        var room = FindFloorByID(currProjId, currFloorId).rooms.Find(f => f.roomId == currRoomId);
        if (room != null)
        {
            FindFloorByID(currProjId, currFloorId).rooms.Remove(room);
            OnRoomsUpdated();
        }

        _www.Dispose();
    }

    public void UpdateRoom(string _newName, RectTransform _transform)
    {
        string urlPath = string.Format("/room?roomId={0}&floorId={1}&name={2}&transform={3}",
            currRoomId, currFloorId, _newName, _transform.anchoredPosition.ToString() + _transform.localScale.ToString());

        RestFarm.restFarm.PUT(sessionId, urlPath, _newName, OnRoomUpdated);
    }

    private void OnRoomUpdated(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        AppManager.appManager.RequestBubbles();

        _www.Dispose();
    }

    private void OnRoomsUpdated()
    {
        var handler = RoomsUpdated;
        if (handler != null)
        {
            handler(this, EventArgs.Empty);
        }
    }

    public void AddBubble(string _name, float _x, float _y)
    {
        Dictionary<string, string> formData = new Dictionary<string, string>();
        formData.Add("name", _name);
        formData.Add("roomId", currRoomId.ToString());
        formData.Add("x", _x.ToString());
        formData.Add("y", _y.ToString());

        RestFarm.restFarm.POST(sessionId, "/bubbles", formData, OnBubbleCreated);
    }

    public void DeleteBubble()
    {
        RestFarm.restFarm.DELETE(sessionId, "/bubble?bubbleId=" + currBubbleId, OnBubbleDeleted);
    }

    private void OnBubbleDeleted(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);
        var bubble = FindRoomByID(currProjId, currFloorId, currRoomId).bubbles.Find(f => f.bubbleId == currBubbleId);
        if (bubble != null)
        {
            FindRoomByID(currProjId, currFloorId, currRoomId).bubbles.Remove(bubble);
            OnBubblesUpdated();
        }

        _www.Dispose();
    }

    private void OnBubbleCreated(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        string result = _www.downloadHandler.text;
        JSONNode node = JSON.Parse(result);

        var bubbleId = node["result"].AsObject["id"].Value;

        RestFarm.restFarm.GETSINGLE(sessionId, "/bubble?bubbleId=" + bubbleId, PopBubble);
    }

    public void UpdateBubbleStyle(byte[] _data)
    {
        Dictionary<string, string> formData = new Dictionary<string, string>();
        formData.Add("bubbleId", currBubbleId.ToString());

        RestFarm.restFarm.POSTIMAGE(sessionId, "/bubble/style/" + currBubbleId, formData, _data, OnUpdateBubbleImages);
    }

    public void UpdateBubbleColor(byte[] _data)
    {
        Dictionary<string, string> formData = new Dictionary<string, string>();
        formData.Add("bubbleId", currBubbleId.ToString());

        RestFarm.restFarm.POSTIMAGE(sessionId, "/bubble/color/" + currBubbleId, formData, _data, OnUpdateBubbleImages);
    }

    private void OnUpdateBubbleImages(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(www);

        OnBubblesUpdated();
        _www.Dispose();
    }

    public void UpdateBubble(string _name, float x, float y)
    {
        string urlPath = string.Format("/bubble?roomId={0}&bubbleId={1}&name={2}&x={3}&y={4}",
            currRoomId, currBubbleId, _name, x, y);

        RestFarm.restFarm.PUT(sessionId, urlPath, _name, OnBubbleUpdated);
    }

    private void OnBubbleUpdated(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        RestFarm.restFarm.GETSINGLE(sessionId, "/bubble?bubbleId=" + currBubbleId, PopBubble);
        _www.Dispose();
    }

    private void OnBubblesUpdated()
    {
        var handler = BubblesUpdated;
        if (handler != null)
        {
            handler(this, EventArgs.Empty);
        }
    }

    public static Vector3 StringToVector(string sVector)
     {
         // Remove the parentheses
         if (sVector.StartsWith ("(") && sVector.EndsWith (")")) {
             sVector = sVector.Substring(1, sVector.Length-2);
         }
 
         // split the items
         string[] sArray = sVector.Split(',');
 
         // store as a Vector3
         Vector3 result = new Vector3(
             float.Parse(sArray[0]),
             float.Parse(sArray[1]),
             sArray.Length > 2 ? float.Parse(sArray[2]) : 0);
 
         return result;
     }
}