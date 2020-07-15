using System.Collections;
using System.Collections.Generic;

public class Project
{
	public int projectId;
	public string name;
	public byte[] plan; // Image File, jpg format
	public List<Floor> floors;
	public List<ChatBlockData> chatHistory;

	public Project()
	{
		projectId = 0;
		name = "";
		plan = new byte[0];
		floors = new List<Floor>();
		chatHistory = new List<ChatBlockData>();
	}

	public Project(int _projectId, string _name, byte[] _plan)
	{
		projectId = _projectId;
		name = _name;
		plan = _plan;
		floors = new List<Floor>();
		chatHistory = new List<ChatBlockData>();
	}

	public Project(int _projectId, string _name, byte[] _plan, List<Floor> _floors)
	{
		projectId = _projectId;
		name = _name;
		plan = _plan;
		floors = _floors;
		chatHistory = new List<ChatBlockData>();
	}
}
