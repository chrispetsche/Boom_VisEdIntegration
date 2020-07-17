﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectItem : MonoBehaviour
{
    [SerializeField]
    ObjectSelectionManager selectionManager;
    Camera cameraInUse;
   
    [SerializeField]
    bool canBeSelected;
    [SerializeField]
    bool objectSelected;

    public void SetThisObject(Camera camToUse)
    {
        cameraInUse = camToUse;
        selectionManager = cameraInUse.gameObject.GetComponent<ObjectSelectionManager>();
        UnselectThisObject();
    }

    public void UnselectThisObject()
    {
        canBeSelected = false;
        objectSelected = false;
    }

    private void OnMouseUp()
    {
        if (!objectSelected && canBeSelected)
        {
            objectSelected = true;
            cameraInUse = selectionManager.SetNewObjectAsSelected(this.gameObject);
        }
    }

    private void OnMouseExit()
    {
        if (objectSelected)
        {
            selectionManager.SelectedObjectIsActive(false);
        }

        else
        {
            canBeSelected = false;
        }
    }

    private void OnMouseOver()
    {
        if (objectSelected)
        {
            selectionManager.SelectedObjectIsActive(true);
        }

        else
        {
            selectionManager.SelectedObjectIsActive(true);
            canBeSelected = true;
        }
    }

    private void OnMouseDrag()
    {
        if (objectSelected)
        {
            transform.position = cameraInUse.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        }
    }

    public bool ObjectSelected()
    {
        return objectSelected;
    }

    public Camera CameraToUse()
    {
        return cameraInUse;
    }
}