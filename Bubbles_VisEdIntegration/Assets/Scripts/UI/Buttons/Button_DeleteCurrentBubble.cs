using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_DeleteCurrentBubble : MonoBehaviour
{
	// This is a very messy, and temporary fix for a bubble delete button. I did this in a few minutes so there was 1 way to delete bubbles.

    public void OnClick()
	{
		AppManager temp = AppManager.appManager;
		Room tempRoom = temp.FindRoomByID(temp.currProjId, temp.currFloorId, temp.currRoomId);

		for (int i = 0; i < tempRoom.bubbles.Count; i++)
		{
			if (tempRoom.bubbles[i].bubbleId == temp.currBubbleId)
			{
				tempRoom.bubbles.RemoveAt(i);
			}
		}

		Button_Bubble[] buttons = FindObjectsOfType<Button_Bubble>();

		for (int i = 0; i < buttons.Length; i++)
		{
			if (buttons[i].bubbleData.bubbleId == temp.currBubbleId)
			{
				Destroy(buttons[i].gameObject);
			}
		}
	}
}
