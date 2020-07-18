using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelectionManager : MonoBehaviour
{
    [SerializeField]
    Camera topCam;
    [SerializeField]
    Camera persCam;
    Camera camInUse;

    bool perspectiveCameraMode;

    [SerializeField]
    GameObject objectCurrentlySelected;
    ClickToSelectTest objectInteractionScript;

    [SerializeField]
    bool currentObjectActive;


    void Start()
    {
        perspectiveCameraMode = false;
        objectCurrentlySelected = null;
        currentObjectActive = false;
    }

    void Update()
    {
        CheckIfObjectNotClicked();
        SetCameraToUse();
    }

    void CheckIfObjectNotClicked()
    {
        if (objectCurrentlySelected != null)
        {
            if (Input.GetMouseButtonDown(0) && !currentObjectActive)
            {
                objectInteractionScript.UnselectThisObject();
                objectCurrentlySelected = null;
            }
        }
    }

    public void ChangeCameraMode()
    {
        if (perspectiveCameraMode)
            perspectiveCameraMode = false;
        else
            perspectiveCameraMode = true;
    }

    public bool PerspectiveCameraMode()
    {
        return perspectiveCameraMode;
    }

    void SetCameraToUse()
    {
        if (perspectiveCameraMode)
        {
            camInUse = persCam;
        }

        else
        {
            camInUse = topCam;
        }
    }

    public Camera SetNewObjectAsSelected(GameObject newObject)
    {
        objectCurrentlySelected = newObject;
        objectInteractionScript = objectCurrentlySelected.GetComponent<ClickToSelectTest>();

        return camInUse;
    }

    public void SelectedObjectIsActive(bool active)
    {
        currentObjectActive = active;
    }
}
