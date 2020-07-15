using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble
{
	public int bubbleId;
	public int roomId;
	public string name;
	public Vector2 pos;
	public byte[] colorPlan; //image file, jpg format
	public byte[] stylePlan; //image file, jpg format
	public List<ChatBlockData> chatHistory_Color;
	public List<ChatBlockData> chatHistory_Style;

	public Bubble()
	{
		bubbleId = 0;
		roomId = 0;
		name = "";
		pos = Vector2.zero;
		colorPlan = new byte[0];
		stylePlan = new byte[0];
		chatHistory_Color = new List<ChatBlockData>();
		chatHistory_Style = new List<ChatBlockData>();
	}

	public Bubble(int _bubbleId, int _roomId)
	{
		bubbleId = _bubbleId;
		roomId = _roomId;
		name = "";
		pos = Vector2.zero;
		colorPlan = new byte[0];
		stylePlan = new byte[0];
		chatHistory_Color = new List<ChatBlockData>();
		chatHistory_Style = new List<ChatBlockData>();
	}

	public Bubble(int _bubbleId, int _roomId, string _name, Vector2 _pos)
	{
		bubbleId = _bubbleId;
		roomId = _roomId;
		name = _name;
		pos = _pos;
		colorPlan = new byte[0];
		stylePlan = new byte[0];
		chatHistory_Color = new List<ChatBlockData>();
		chatHistory_Style = new List<ChatBlockData>();
	}

	public Bubble(int _bubbleId, int _roomId, Vector2 _pos)
	{
		bubbleId = _bubbleId;
		roomId = _roomId;
		name = "";
		pos = _pos;
		colorPlan = new byte[0];
		stylePlan = new byte[0];
		chatHistory_Color = new List<ChatBlockData>();
		chatHistory_Style = new List<ChatBlockData>();
	}

	public Bubble(int _bubbleId, int _roomId, string _name, Vector2 _pos, byte[] _colorPlan, byte[] _stylePlan)
	{
		bubbleId = _bubbleId;
		roomId = _roomId;
		name = _name;
		pos = _pos;
		colorPlan = _colorPlan;
		stylePlan = _stylePlan;
		chatHistory_Color = new List<ChatBlockData>();
		chatHistory_Style = new List<ChatBlockData>();
	}
}
