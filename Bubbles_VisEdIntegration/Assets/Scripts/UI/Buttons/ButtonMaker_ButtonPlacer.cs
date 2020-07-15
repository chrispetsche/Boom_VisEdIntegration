using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMaker_ButtonPlacer : MonoBehaviour
{
    public GameObject gridParent;
    public Transform floorPlanImg;
    public ButtonMaker_RoomEditor roomEditor;
    public Button_BubblePlacer bubbleButtonPrefab;

    private List<Button_BubblePlacer> buttons;

    // get this from the server depending on room type
    string[] bubblePlacers = {
        "Flooring",
        "Countertop",
        "Island",
        "Backsplash Tile",
        "Chandelier",
        "Sink",
        "Cabinets",
        "Fridge",
        "Stove",
        "Oven",
        "Microwave",
        "Main Wall Color/Design"
    };

    private void Awake()
    {
        buttons = new List<Button_BubblePlacer>();

        SetupButtons();
    }

    private void SetupButtons()
    {
        int i = 0;
        for (; i < bubblePlacers.Length; i++)
        {
            if (i >= buttons.Count)
            {
                Button_BubblePlacer gridButton = Instantiate(bubbleButtonPrefab);
                gridButton.transform.SetParent(gridParent.transform, true);
                buttons.Add(gridButton);
            }

            buttons[i].Initialize(bubblePlacers[i], roomEditor, floorPlanImg);
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
