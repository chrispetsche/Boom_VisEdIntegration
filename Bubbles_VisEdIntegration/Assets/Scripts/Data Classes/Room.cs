using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
	public int projectId;
	public int roomId;
	public string name;
    public Vector2 pos;
    public Vector3 scale;
    public byte[] plan; // Image File, jpg format
	public List<Bubble> bubbles;

	public Room()
	{
		roomId = 0;
		projectId = 0;
		name = "";
		plan = new byte[0];
		bubbles = new List<Bubble>();
	}

	public Room(int _roomId, int _projectId, string _name, Vector2 _pos, Vector2 _scale)
	{
		roomId = _roomId;
		projectId = _projectId;
		name = _name;
		plan = new byte[0];
		bubbles = new List<Bubble>();
        pos = _pos;
        scale = _scale;
	}

	public Room(int _roomId, int _projectId, string _name, byte[] _plan)
	{
		roomId = _roomId;
		projectId = _projectId;
		name = _name;
		plan = _plan;
		bubbles = new List<Bubble>();
	}

	public Room(int _roomId, int _projectId, string _name, byte[] _plan, List<Bubble> _bubbles)
	{
		roomId = _roomId;
		projectId = _projectId;
		name = _name;
		plan = _plan;
		bubbles = _bubbles;
	}
}
