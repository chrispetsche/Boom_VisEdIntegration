using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToSelectTest : MonoBehaviour
{
    //!!! THIS WILL BE SET WHEN THE ASSET IS INSTANTIATED. IT'S NEEDED IN THE GOVERNOR !!!//
    [SerializeField]
    ObjectSelectionManager selectionManager;

    [SerializeField]
    bool canBeSelected;

    [SerializeField]
    bool objectSelected;

    public void SetThisObject(Camera camToUse, GameObject managementObj)
    {
        //cameraInUse = camToUse;
        selectionManager = managementObj.GetComponent<ObjectSelectionManager>();
        UnselectThisObject();
    }

    // The Object Selection Manager calls this when asset 
    // is no longer selected based on its conditions.
    public void UnselectThisObject()
    {
        canBeSelected = false;
        objectSelected = false;
    }

    // When the mouse button or finger is lifted from the asset...
    private void OnMouseUp()
    {
        // If it's not already selected and can be...
        if (!objectSelected && canBeSelected)
        {
            // Set the object to selected.
            objectSelected = true;
            // Tell the Object Selection Manager this is the selected asset.
            selectionManager.SetNewObjectAsSelected(this.gameObject);
        }
    }

    // If the mouse/finger is hovering over the asset...
    private void OnMouseOver()
    {
        // If the object is selected...
        if (objectSelected)
        {
            // Tell the Selection Manager it's being interacted with.
            selectionManager.SelectedObjectIsActive(true);
        }

        //!!! ??? !!!//
        else
        {
            selectionManager.SelectedObjectIsActive(true);
            canBeSelected = true;
        }
    }

    // As a back up to the MouseOver, if the mouse/finger leaves the asset...
    private void OnMouseExit()
    {
        // If it was selected...
        if (objectSelected)
        {
            // Tell the Object Selection Manager to unselect it.
            selectionManager.SelectedObjectIsActive(false);
        }

        //!!! ??? !!!//
        else
        {
            canBeSelected = false;
        }
    }
}
