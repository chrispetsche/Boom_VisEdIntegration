using System.Collections;
using System.Collections.Generic;

public class Floor
{
	public int floorId;
	public string name;
	public byte[] plan; // Image File, jpg format
	public List<Room> rooms;

	public Floor()
	{
		floorId = 0;
		name = "";
		plan = new byte[0];
		rooms = new List<Room>();
	}

	public Floor(int _floorId, string _name)
	{
		floorId = _floorId;
		name = _name;
		plan = new byte[0];
		rooms = new List<Room>();
	}

	public Floor(int _floorId, string _name, byte[] _plan)
	{
		floorId = _floorId;
		name = _name;
		plan = _plan;
		rooms = new List<Room>();
	}

	public Floor(int _floorId, string _name, byte[] _plan, List<Room> _rooms)
	{
		floorId = _floorId;
		name = _name;
		plan = _plan;
		rooms = _rooms;
	}
}
