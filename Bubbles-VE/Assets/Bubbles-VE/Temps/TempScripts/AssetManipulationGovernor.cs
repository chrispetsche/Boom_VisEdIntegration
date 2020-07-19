using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManipulationGovernor : MonoBehaviour
{
    [SerializeField]
    GameObject tempManagerHolder;

    // These will control whether or not this asset
    // can be manipulated in each different way.
    public bool canMove;
    public bool canRotate;
    public bool canScale;

    MoveObject moveAsset;
    RotateObject rotateAsset;
    ScaleObject scaleAsset;

    [SerializeField]
    GameObject rotatePointMarker;
    [SerializeField]
    Transform assetRotate_RestPoint;

    // ********************** GOVERN SELECTION SYSTEMS BELOW *************************** //
    ObjectSelectionManager selectionManager;

    [SerializeField]
    bool canBeSelected;

    [SerializeField]
    bool objectSelected;

    private void Start()
    {
        canBeSelected = true;
        objectSelected = false;
        moveAsset = GetComponent<MoveObject>();
        rotateAsset = rotatePointMarker.GetComponent<RotateObject>();
        scaleAsset = GetComponent<ScaleObject>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            InitializeThisObject(tempManagerHolder);
        }
    }

    // This is called when the objected is instantiated.
    public void InitializeThisObject(GameObject managementObj)
    {
        //!!! MAY WANT TO SET THESE DIFFERENTLY !!!//
        //cameraInUse = camToUse;

        canBeSelected = true;
        objectSelected = false;
        //!!! UNCOMMENT WHEN DONE LINKING !!!//
        selectionManager = managementObj.GetComponent<ObjectSelectionManager>();

        // Tell the Object Selection Manager this is the selected asset.
       //Camera camToPass = selectionManager.SetNewObjectAsSelected(this.gameObject);
       //bool inTopDown = selectionManager.InTopDownCameraMode();

        //!!! ENABLE MANIPULATION SYSTEMS HERE !!!//
        // If asset not locked...
        //EnableAssetManipulationSystems(true, inTopDown, camToPass);
    }

    // The Object Selection Manager calls this when asset 
    // is no longer selected based on its conditions.
    public void UnselectThisObject()
    {
        canBeSelected = true;
        objectSelected = false;

        //!!! DISABLE MANIPULATION SYSTEMS HERE !!!//
        DisableAssetManipulationSystems();
    }

    // When the mouse button or finger is lifted from the asset...
    private void OnMouseUp()
    {
        // If it's not already selected and can be...
        if (!objectSelected && canBeSelected)
        {
            // Tell the Object Selection Manager this is the selected asset.
            Camera camToPass = selectionManager.SetNewObjectAsSelected(this.gameObject);
            bool inTopDown = selectionManager.InTopDownCameraMode();

            // Set the object to selected.
            objectSelected = true;

            //!!! ENABLE MANIPULATION SYSTEMS HERE !!!//
            // If asset not locked...
            EnableAssetManipulationSystems(true, inTopDown, camToPass);
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
    }

    // ********************** GOVERN MANIPULATION SYSTEMS BELOW *************************** //

    void EnableAssetManipulationSystems(bool activate,bool camTopDown , Camera camToPass)
    {
        if (canMove)
        {
            moveAsset.EnableMovement(true, camToPass);
        }

        // If an asset can rotate...
        if (canRotate)
        {
            // Turn on the rotate object
            rotateAsset.gameObject.SetActive(true);
            // Call to enable it.
            rotateAsset.EnableRotate(activate, camTopDown, camToPass, transform, assetRotate_RestPoint);
        }

        // If an asset can be scaled..
        if (canScale)
        {
            scaleAsset.EnableScaling(activate, camToPass);
        }
    }

    void DisableAssetManipulationSystems()
    {
        if (canMove)
        {
            moveAsset.EnableMovement(false, null);
        }

        // If an asset can rotate...
        if (canRotate)
        {

            // Call to enable it.
            rotateAsset.EnableRotate(false, false, null, transform, assetRotate_RestPoint);

            // Turn on the rotate object
            rotateAsset.gameObject.SetActive(false);
        }

        // If an asset can be scaled..
        if (canScale)
        {
            scaleAsset.EnableScaling(false, null);
        }
    }
}
