using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMaker_RoomEditor : MonoBehaviour
{
	public GameObject gridParent;
    public Transform floorPlanImg;
    public Button_Bubble bubbleButtonPrefab;
    public DeletePrompt_Bubble deletePrompt;

    private List<Button_Bubble> buttons;

    private void Start()
    {
        buttons = new List<Button_Bubble>();

        AppManager.appManager.BubblesUpdated += AppManager_BubblesUpdated;
        AppManager.appManager.RequestBubbles();
    }

    private void OnDestroy()
    {
        AppManager.appManager.BubblesUpdated -= AppManager_BubblesUpdated;
    }

    private void AppManager_BubblesUpdated(object sender, System.EventArgs e)
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        var room = AppManager.appManager.FindRoomByID(AppManager.appManager.currProjId, 
            AppManager.appManager.currFloorId, AppManager.appManager.currRoomId);

        int i = 0;
        for (; i < room.bubbles.Count; i++)
        {
            if (i >= buttons.Count)
            {
                Button_Bubble gridButton = Instantiate(bubbleButtonPrefab);
                gridButton.transform.SetParent(floorPlanImg, true);
                gridButton.gameObject.SetActive(false);
                buttons.Add(gridButton);
            }

            buttons[i].Initialize(room.bubbles[i], deletePrompt);
            buttons[i].gameObject.SetActive(true);
        }

        if (i < buttons.Count)
        {
            for (; i < buttons.Count; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }

    public void TransferBubble(Button_Bubble _bubble)
    {
        buttons.Add(_bubble);
    }
}
