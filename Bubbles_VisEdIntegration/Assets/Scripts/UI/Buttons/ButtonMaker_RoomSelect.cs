using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMaker_RoomSelect : MonoBehaviour
{
	public GameObject gridParent;
	public Button_Room roomButtonPrefab;
    public DeletePrompt_Room deletePrompt;

    private List<Button_Room> buttons;

    private void Start()
    {
        buttons = new List<Button_Room>();

        AppManager.appManager.RoomsUpdated += AppManager_RoomsUpdated;
        AppManager.appManager.RequestRooms();
    }

    private void OnDestroy()
    {
        AppManager.appManager.RoomsUpdated -= AppManager_RoomsUpdated;
    }

    private void AppManager_RoomsUpdated(object sender, System.EventArgs e)
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        var floor = AppManager.appManager.FindFloorByID(AppManager.appManager.currProjId, AppManager.appManager.currFloorId);

        int i = 0;
        for (; i < floor.rooms.Count; i++)
        {
            if (i >= buttons.Count)
            {
                Button_Room gridButton = Instantiate(roomButtonPrefab, gridParent.transform);
                gridButton.gameObject.SetActive(false);
                buttons.Add(gridButton);
            }

            buttons[i].Initialize(floor.rooms[i], deletePrompt);
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
}
