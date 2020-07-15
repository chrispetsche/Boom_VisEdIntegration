using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;

// The purpose of this script is to be placed onto the main canvas of a scene and manage setting UI elements active and innactive.
// This is also where all the "New" methods are. As in the methods that create new user data types, like Floors, Rooms and the like.

public class UIManager : MonoBehaviour
{
    private void OnEnable()
	{
		AppManager.appManager.currUIManager = this;
	}

	private void OnDisable()
	{
		AppManager.appManager.currUIManager = null;
	}

    // The UI is seperated into 3 primary categories for the purposes of the app. 
    // This GameObject array contains the 3 main UI panels that holds the functionality
    // of going through the app, step by step.
    [SerializeField]
    GameObject[] primaryUIPanelsArray;
    string currPrimaryAppPanel;

    [SerializeField]
    GameObject[] appStart_SubPanelsArray;

    [SerializeField]
    GameObject[] projectPages_SubPanelsArray;
    [SerializeField]
    GameObject[] projectBackgroundColorWashPanelsArray;
    [SerializeField]
    GameObject[] projectSetupPanelsArray;
    [SerializeField]
    GameObject[] projectDemo_IntroPanelsArray;

    [SerializeField]
    GameObject[] editor_SubPanelsArray;

    private void Start()
    {
        // When the app first starts up, the client initially needs to login or signup
        // before the app knows what to do for them next. This for loop ensures that the
        // 'Project Home' and '3D Editor' is diabled and the 'App Start and Login' page
        // is what the client sees.
        for (int pUI = 0; pUI < primaryUIPanelsArray.Length; pUI++)
        {
            if (primaryUIPanelsArray[pUI].name != "AppStartAndLogin_Panels")
            {
                primaryUIPanelsArray[pUI].SetActive(false);
            }

            if (primaryUIPanelsArray[pUI].name == "AppStartAndLogin_Panels")
            {
                for (int aSP = 0; aSP < appStart_SubPanelsArray.Length; aSP++)
                {
                    primaryUIPanelsArray[pUI].SetActive(true);
                    currPrimaryAppPanel = primaryUIPanelsArray[pUI].name;

                    appStart_SubPanelsArray[aSP].SetActive(false);
                }
            }
        }
    }

    public void LoginToApp()
    {
        ShufflePrimaryPanelActivations(primaryUIPanelsArray[0], primaryUIPanelsArray[1]);
    }


    public void LogoutOFApp(GameObject currPanel)
    {
        ShufflePrimaryPanelActivations(currPanel, primaryUIPanelsArray[0]);
    }

    //!!! Temporary Editor Opener !!!//
    public void OpenEditor(string projectType)
    {
        ShufflePrimaryPanelActivations(primaryUIPanelsArray[1], primaryUIPanelsArray[2]);

        switch (projectType)
        {
            case "New Build":
                {
                    // Open blue colorwash panel
                    // Set loadbar coloring to be blue

                    break;
                }

            case "Remodel":
                {
                    // Open blue colorwash panel
                    // Set loadbar coloring to be blue

                    break;
                }

            case "Decorate":
                {
                    // Open blue colorwash panel
                    // Set loadbar coloring to be blue

                    break;
                }
        }
    }

    void ShufflePrimaryPanelActivations(GameObject currPanel, GameObject newPanel)
    {
        for (int pUI = 0; pUI < primaryUIPanelsArray.Length; pUI++)
        {
            // If shuffling from App Start Panels to Project Page Panels...
            if (currPanel == primaryUIPanelsArray[0] && newPanel == primaryUIPanelsArray[1])
            {
                // Close all App Start Sub Panels
                for (int aSSP = 0; aSSP < appStart_SubPanelsArray.Length; aSSP++)
                {
                    appStart_SubPanelsArray[aSSP].SetActive(false);
                }

                // Make sure the Project Page that's active is the home page
                projectPages_SubPanelsArray[0].SetActive(true);
                projectPages_SubPanelsArray[1].SetActive(false);

                //!!! If this is the first time use of the app, open FTUX panels !!!//
                //projectPages_SubPanelsArray[2].SetActive(true);
            }

            // If shuffling from Project Page Panels to 3D Editor Panels...
            else if (currPanel == primaryUIPanelsArray[1] && newPanel == primaryUIPanelsArray[2])
            {
                // Close all Project Page Sub Panels
                for (int pPSP = 0; pPSP < projectPages_SubPanelsArray.Length; pPSP++)
                {
                    projectPages_SubPanelsArray[pPSP].SetActive(false);
                }
            }

            // If shuffling from Project Page Panels to App Start Panels...
            else if (currPanel == primaryUIPanelsArray[1] && newPanel == primaryUIPanelsArray[0])
            {
                // Close all Project Page Sub Panels
                for (int pPSP = 0; pPSP < projectPages_SubPanelsArray.Length; pPSP++)
                {
                    projectPages_SubPanelsArray[pPSP].SetActive(false);
                }
            }

            // If shuffling from 3D Editor Panels to Project Page Panels...
            else if (currPanel == primaryUIPanelsArray[2] && newPanel == primaryUIPanelsArray[1])
            {
                // Close 3D Editor Sub Panels
                for (int eSP = 0; eSP < editor_SubPanelsArray.Length; eSP++)
                {
                    editor_SubPanelsArray[eSP].SetActive(false);
                }
            }

            // If shuffling from 3D Editor Panels to App Start Panels...
            else if (currPanel == primaryUIPanelsArray[2] && newPanel == primaryUIPanelsArray[0])
            {
                // Close 3D Editor Sub Panels
                for (int eSP = 0; eSP < editor_SubPanelsArray.Length; eSP++)
                {
                    editor_SubPanelsArray[eSP].SetActive(false);
                }
            }
        }

        newPanel.SetActive(true);
        currPanel.SetActive(false);
    }

    public void StartNewProject(string projectType)
    {
        projectPages_SubPanelsArray[2].SetActive(true);

        switch (projectType)
        {
            case "New Build":
                {

                    // Call to active New Build main panel
                    projectBackgroundColorWashPanelsArray[0].SetActive(true);
                    // Call to active New Build Demo Intro panel
                    projectSetupPanelsArray[0].SetActive(true);

                    projectDemo_IntroPanelsArray[0].SetActive(false);

                    break;
                }

            case "Remodel":
                {
                    // Call to active Remodel main panel
                    projectBackgroundColorWashPanelsArray[1].SetActive(true);
                    // Call to active Remodel Demo Intro panel
                    projectSetupPanelsArray[1].SetActive(true);

                    projectDemo_IntroPanelsArray[1].SetActive(false);

                    break;
                }

            case "Decorate":
                {
                    // Call to active Decorate main panel
                    projectBackgroundColorWashPanelsArray[2].SetActive(true);
                    // Call to active Decorate Demo Intro panel
                    projectSetupPanelsArray[2].SetActive(true);

                    projectDemo_IntroPanelsArray[2].SetActive(false);

                    break;
                }
        }
    }

    public void OpenDemoProject(string demoType)
    {
        projectPages_SubPanelsArray[2].SetActive(true);

        switch (demoType)
        {
            case "New Build":
                {

                    // Call to active New Build main panel
                    projectBackgroundColorWashPanelsArray[0].SetActive(true);
                    // Call to active New Build Demo Intro panel
                    projectDemo_IntroPanelsArray[0].SetActive(true);
                    
                    break;
                }

            case "Remodel":
                {
                    // Call to active Remodel main panel
                    projectBackgroundColorWashPanelsArray[1].SetActive(true);
                    // Call to active Remodel Demo Intro panel
                    projectDemo_IntroPanelsArray[1].SetActive(true);

                    break;
                }

            case "Decorate":
                {
                    // Call to active Decorate main panel
                    projectBackgroundColorWashPanelsArray[2].SetActive(true);
                    // Call to active Decorate Demo Intro panel
                    projectDemo_IntroPanelsArray[2].SetActive(true);

                    break;
                }
        }
    }

    public void ReturnToProjectsHome(GameObject currScenePanel)
    {
        ShufflePrimaryPanelActivations(currScenePanel, primaryUIPanelsArray[1]);
    }


    /*
    public void OpenAndClosePrimaryAppPanels(bool open, string panelName)
    {
        for (int pUI = 0; pUI < primaryUIPanelsArray.Length; pUI++)
        {
            if (primaryUIPanelsArray[pUI].name != panelName)
            {
                primaryUIPanelsArray[pUI].SetActive(false);
            }

            if (primaryUIPanelsArray[pUI].name == panelName)
            {
                primaryUIPanelsArray[pUI].SetActive(true);
            }
        }
    }

    public void ClosePrimaryAppPanels(string panelName)
    {
        for (int pUI = 0; pUI < primaryUIPanelsArray.Length; pUI++)
        {
            if (primaryUIPanelsArray[pUI].name == panelName)
            {
                primaryUIPanelsArray[pUI].SetActive(false);
            }
        }
    }

    public void OpenAppStartPanel(string panelName)
    {
        for (int aSP = 0; aSP < appStart_SubPanelsArray.Length; aSP++)
        {
            if (appStart_SubPanelsArray[aSP].name == panelName)
            {
                appStart_SubPanelsArray[aSP].SetActive(true);
            }
        }
    }

    public void CloseAppStartPanel(string panelName)
    {
        for (int aSP = 0; aSP < appStart_SubPanelsArray.Length; aSP++)
        {
            if (appStart_SubPanelsArray[aSP].name == panelName)
            {
                appStart_SubPanelsArray[aSP].SetActive(false);
            }
        }
    }*/









    // CP???Why are these needed in the UIManager? Can they be placed in their own script that handles creating anything realated 
    // CP???to new user data types and checked here if needed to fulfill UI related actions happening on the canvas.
    //public void NewProject(NewProject _newProject)
    //{
    //	if (!_newProject)
    //	{
    //		Debug.LogWarning("Argument for NewProject() is null!");
    //		return;
    //	}
    //	else if (_newProject.passwordField.text != _newProject.passwordCheckField.text)
    //	{
    //		Debug.Log("You're passwords do not match!");
    //		return;
    //	}

    //	AppManager temp = AppManager.appManager;

    //	#region determing new projectID
    //	// What's happening here is I'm finding all of the total project IDs.
    //	// Let's say there are 4 projects. Their IDs are 1, 2, 4, 5. What this section does is finds the lowest room ID number that isn't taken.
    //	// This this example, it would find the number 3.

    //	int newProjId = 0;

    //	List<Project> projects = temp.FindAllProjects();

    //	List<int> projectIds = new List<int>();
    //	foreach (Project proj in projects)
    //	{
    //		projectIds.Add(proj.projectId);
    //	} // Grabbing all project IDs

    //	int highestId = 0;
    //	for (int i = 0; i <= projectIds.Count; i++)
    //	{
    //		if (i > highestId)
    //		{
    //			highestId = i;
    //		}
    //	} // Finding highest Id

    //	for (int i = 1; i <= highestId; i++)
    //	{
    //		if (!projectIds.Contains(i))
    //		{
    //			newProjId = i;
    //		}
    //		else if (i == highestId)
    //		{
    //			newProjId = i + 1;
    //		}
    //	} // Finding lowest unused project ID
    //	#endregion

    //	#region grab uploaded img
    //	byte[] plan = new byte[0];

    //	if (_newProject.image != null)
    //	{
    //		if (_newProject.image.sprite != null)
    //		{
    //			Texture2D tex = _newProject.image.sprite.texture;

    //			try
    //			{
    //				plan = tex.EncodeToJPG();
    //			}
    //			catch (Exception e)
    //			{
    //				Debug.LogWarning("Failed to encode img. " + e.Message);
    //				return;
    //			}
    //		}

    //		//reset new proj panel
    //		_newProject.image.sprite = null;
    //		for (int i = 0; i < _newProject.imgButtonsToReset.Length; i++)
    //		{
    //			_newProject.imgButtonsToReset[i].SetActive(true);
    //		}
    //	}
    //	#endregion

    //	Project tempProj = new Project(newProjId, _newProject.projNameField.text, plan);
    //	temp.currUser.projects.Add(tempProj);

    //	//Upload to server here. I intended for there to be funtions to call in the AppManager that would handle uploading. They could just take in an argument of "Project."

    //	//string url = temp.serverAddress + "api/v1/projects?sessionId=" + temp.sessionId + "&" + "name=" + tempProj.name;
    //	//JSONObject json = new JSONObject();
    //	//json["project"]["id"].AsInt = tempProj.projectId;
    //	//json["project"]["name"] = tempProj.name;
    //	//print(json.AsObject);

    //	//RestFarm.restFarm.POST(url, json.AsObject, RestFarm.restFarm.DebugOnComplete);

    //	//AppManager.appManager.currProjId = tempProj.projectId;
    //	//SceneManagement.sceneManagement.LoadScene("FloorSelect");
    //}

    //public void NewFloor(NewFloor _newFloor)
    //{
    //	if (!_newFloor)
    //	{
    //		Debug.LogWarning("Argument for NewProject() is null!");
    //		return;
    //	}

    //	AppManager temp = AppManager.appManager;

    //	#region determing new projectID
    //	// What's happening here is I'm finding all of the total project IDs.
    //	// Let's say there are 4 projects. Their IDs are 1, 2, 4, 5. What this section does is finds the lowest room ID number that isn't taken.
    //	// This this example, it would find the number 3.

    //	int newFloorId = 0;

    //	List<Floor> floors = temp.FindAllFloors();

    //	List<int> floorIds = new List<int>();
    //	foreach (Floor floor in floors)
    //	{
    //		floorIds.Add(floor.floorId);
    //	} // Grabbing all project IDs

    //	int highestId = 0;
    //	for (int i = 0; i <= floorIds.Count; i++)
    //	{
    //		if (i > highestId)
    //		{
    //			highestId = i;
    //		}
    //	} // Finding highest Id

    //	for (int i = 1; i <= highestId; i++)
    //	{
    //		if (!floorIds.Contains(i))
    //		{
    //			newFloorId = i;
    //		}
    //		else if (i == highestId)
    //		{
    //			newFloorId = i + 1;
    //		}
    //	} // Finding lowest unused project ID
    //	#endregion

    //	#region grab uploaded img
    //	byte[] plan = new byte[0];

    //	if (_newFloor.image != null)
    //	{
    //		if (_newFloor.image.sprite != null)
    //		{
    //			Texture2D tex = _newFloor.image.sprite.texture;

    //			try
    //			{
    //				plan = tex.EncodeToJPG();
    //			}
    //			catch (Exception e)
    //			{
    //				Debug.LogWarning("Failed to encode img. " + e.Message);
    //				return;
    //			}
    //		}

    //		//reset new floor panel
    //		_newFloor.image.sprite = null;
    //		for (int i = 0; i < _newFloor.imgButtonsToReset.Length; i++)
    //		{
    //			_newFloor.imgButtonsToReset[i].SetActive(true);
    //		}
    //	}
    //	#endregion

    //	Floor tempFloor = new Floor(newFloorId, _newFloor.floorNameField.text, plan);
    //	temp.FindProjectByID(temp.currProjId).floors.Add(tempFloor);

    //	//Upload to server here

    //	AppManager.appManager.currFloorId = tempFloor.floorId;
    //	SceneManagement.sceneManagement.LoadScene("RoomSelect");
    //}

    //public void NewRoom(NewRoom _newRoom)
    //{
    //	if (!_newRoom)
    //	{
    //		Debug.LogWarning("Argument for NewRoom() is null!");
    //		return;
    //	}
    //	else if (_newRoom.name.Length == 0)
    //	{
    //		Debug.Log("You're room name is blank!");
    //		return;
    //	}

    //	AppManager temp = AppManager.appManager;

    //	#region determing new roomID
    //	// What's happening here is I'm finding all of the total rooms IDs.
    //	// Let's say there are 4 rooms across ALL projects. Their IDs are 1, 2, 4, 5. What this section does is finds the lowest room ID number that isn't taken.
    //	// This this example, it would find the number 3.

    //	int newRoomId = 0;

    //	List<Room> rooms = temp.FindAllRooms();

    //	List<int> roomIds = new List<int>();
    //	foreach(Room room in rooms)
    //	{
    //		roomIds.Add(room.roomId);
    //	} // Grabbing all roomIDs

    //	int highestId = 0;
    //	for (int i = 0; i <= roomIds.Count; i++)
    //	{
    //		if (i > highestId)
    //		{
    //			highestId = i;
    //		}
    //	} // Finding highest Id

    //	for (int i = 1; i <= highestId; i++)
    //	{
    //		if (!roomIds.Contains(i))
    //		{
    //			newRoomId = i;
    //		}
    //		else if (i == highestId)
    //		{
    //			newRoomId = i + 1;
    //		}
    //	} // Finding lowestin unused project ID
    //	#endregion

    //	Room tempRoom = new Room(newRoomId, temp.currProjId, _newRoom.roomNameField.text);

    //	temp.FindFloorByID(temp.currProjId, temp.currFloorId).rooms.Add(tempRoom);

    //	//upload to server here

    //	AppManager.appManager.currRoomId = tempRoom.roomId;
    //	SceneManagement.sceneManagement.LoadScene("BubbleEditor");
    //}

    //public int NewBubble()
    //{
    //	AppManager temp = AppManager.appManager;

    //	#region determing new bubbleID
    //	// What's happening here is I'm finding all of the total bubble IDs.
    //	// Let's say there are 4 rooms across ALL projects. Their IDs are 1, 2, 4, 5. What this section does is finds the lowest room ID number that isn't taken.
    //	// For this example, it would find the number 3.

    //	int newBubbleId = 0;

    //	Room currRoom = temp.FindRoomByID(temp.currProjId, temp.currFloorId, temp.currRoomId);
    //	List<Bubble> currBubbles = currRoom.bubbles;

    //	List<int> bubbleIds = new List<int>();
    //	foreach (Bubble bubble in currBubbles)
    //	{
    //		bubbleIds.Add(bubble.bubbleId);
    //	} // Grabbing all bubbleIDs

    //	int highestId = 0;
    //	for (int i = 0; i <= bubbleIds.Count; i++)
    //	{
    //		if (i > highestId)
    //		{
    //			highestId = i;
    //		}
    //	} // Finding highest Id

    //	for (int i = 1; i <= highestId; i++)
    //	{
    //		if (!bubbleIds.Contains(i))
    //		{
    //			newBubbleId = i;
    //		}
    //		else if (i == highestId)
    //		{
    //			newBubbleId = i + 1;
    //		}
    //	} // Finding lowestin unused project ID
    //	#endregion

    //	Bubble newBub = new Bubble(newBubbleId, temp.currRoomId);
    //	currRoom.bubbles.Add(newBub);

    //	//Upload to server here

    //	return newBub.bubbleId;
    //}
}
