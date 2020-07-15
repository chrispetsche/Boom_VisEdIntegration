using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleEditorManager : MonoBehaviour
{
    [SerializeField] private GameObject bubbleGrid;
    [SerializeField] private GameObject adjustViewer;
    [SerializeField] private Image floorManipulator;
    [SerializeField] private RectTransform floorMap;

    private void OnEnable()
	{
        var room = AppManager.appManager.FindRoomByID(AppManager.appManager.currProjId, AppManager.appManager.currFloorId, AppManager.appManager.currRoomId);

        if (room.scale == Vector3.zero)
        {
            adjustViewer.SetActive(true);
            bubbleGrid.SetActive(false);
            floorManipulator.raycastTarget = true;
        } else
        {
            adjustViewer.SetActive(false);
            bubbleGrid.SetActive(true);
            floorManipulator.raycastTarget = false;

            floorMap.anchoredPosition = room.pos;
            floorMap.localScale = room.scale;
        }
    }
}
