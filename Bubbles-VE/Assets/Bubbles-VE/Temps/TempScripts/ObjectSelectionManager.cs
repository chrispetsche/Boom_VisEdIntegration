using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelectionManager : MonoBehaviour
{
    // This script will handle which camera is in use.
    // These will be the two used in the scene.
    [SerializeField]
    Camera topCam;
    [SerializeField]
    Camera persCam;
    // This will be one of the above and then passed
    // to the manipulation systems.
    Camera camInUse;

    // This determines which camera mode is active.
    [SerializeField]
    bool topdownCameraMode;

    // The object the user has selected.
    [SerializeField]
    GameObject objectCurrentlySelected;
    //
    AssetManipulationGovernor objectInteractionScript;

    // If an object is selected, this system needs to
    // know if the used is interacting with it or another.
    [SerializeField]
    bool currentObjectActive;


    void Start()
    {
        //topdownCameraMode = false;
        objectCurrentlySelected = null;
        currentObjectActive = false;
    }

    void Update()
    {
        CheckIfObjectNotClicked();
    }

    void CheckIfObjectNotClicked()
    {
        // If there is an object selected...
        if (objectCurrentlySelected != null)
        {
            // If the screen was clicked/tapped and the selected object
            // isn't being interacted with...
            if (Input.GetMouseButtonDown(0) && !currentObjectActive)
            {
                // Call the object and tell it that it's
                // no longer selected.
                objectInteractionScript.UnselectThisObject();
                // Empty the selected object slot.
                objectCurrentlySelected = null;
            }
        }
    }
    public Camera SetNewObjectAsSelected(GameObject newObject)
    {
        objectCurrentlySelected = newObject;
        objectInteractionScript = objectCurrentlySelected.GetComponent<AssetManipulationGovernor>();

        return camInUse;
    }

    public void SetCameraMode()
    {
        if (topdownCameraMode)
        {
            camInUse = persCam;
            topdownCameraMode = false;
        }

        else
        {
            camInUse = topCam;
            topdownCameraMode = true;
        }
    }

    public Camera CreationCamera()
    {
        return camInUse;
    }

    // Asset calls this to tell this system it's being
    // interacted with.
    public void SelectedObjectIsActive(bool active)
    {
        currentObjectActive = active;
    }

    public bool InTopDownCameraMode()
    {
        return topdownCameraMode;
    }
}
