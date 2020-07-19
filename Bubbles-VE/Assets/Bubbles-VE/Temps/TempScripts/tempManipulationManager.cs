using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempManipulationManager : MonoBehaviour
{
    /*
    //************* no need
    // The object that will be manipulated.
    [SerializeField]
    GameObject assetToManipulate;
    // The script governing how an asset can be manipulated.
    AssetManipulationGovernor assetManipulationGovernor;

    // The script/ object that is used to run the rotation
    // of an asset that is capable of doing so.
    [SerializeField]
    RotateObject rotatableAsset;

    // The array of markers that will be sent to an object
    // to control its scaling abilities.
    [SerializeField]
    Transform[] scale_PointMarkerArray;

    // These are the cameras that the editor will use.
    [SerializeField]
    Camera topdownCam;
    [SerializeField]
    Camera perspCam;
    Camera currentCam;
    // This will be adjusted by the 2D to 3D UI toggle.
    [SerializeField]
    bool currentView_TopDown;

    // On Start...
    private void Start()
    {
        // The position of the rotation marker is set to the 
        // position of the object this script is on.
        rotatableAsset.transform.position = transform.position;
        // Then turn it off so the user can see or access.
        rotatableAsset.gameObject.SetActive(false);

        // Also set the position of the scale markers to to this one,
        // and turn them off as well.
        for (int i = 0; i < scale_PointMarkerArray.Length; i++)
        {
            scale_PointMarkerArray[i].position = transform.position;
            scale_PointMarkerArray[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // If the editor is in topdown view...
        if (currentView_TopDown)
        {
            // Turn the topdown cam on and the 
            // perspective cam off.
            topdownCam.gameObject.SetActive(true);
            perspCam.gameObject.SetActive(false);
            // Set the current cam as the topdown.
            currentCam = topdownCam;
        }

        // Or do the opposite if it's not topdown view.
        else
        {
            topdownCam.gameObject.SetActive(false);
            perspCam.gameObject.SetActive(true);

            currentCam = perspCam;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ObjectSelected(true, assetToManipulate);
        }
    }

    // Selection Manager calls in when an object in the scene has been selected or deselected,
    // and passes in the object if one chosen.
    public void ObjectSelected(bool selected, GameObject objSelected)
    {
        if (selected)
        {
            // When selected, the object is set as an
            // asset to manipulate in some way.
            assetToManipulate = objSelected;
            // Set the governor script to communicate with.
            assetManipulationGovernor = assetToManipulate.GetComponent<AssetManipulationGovernor>();
            // Then call to enable any applicable manipulation systems.
            EnableAssetManipulationSystems();
        }

        else
        {
            // When calling to say an object has been unselected,
            // disable any applicable manipulation systems.
            DisableAssetManipulationSystems();
        }
    }

    void EnableAssetManipulationSystems()
    {
        if (assetManipulationGovernor.canMove)
        {

        }

        // If an asset can rotate...
        if (assetManipulationGovernor.canRotate)
        {
            // Turn on the rotate object
            rotatableAsset.gameObject.SetActive(true);
            // Call to enable it.
            //rotatableAsset.EnableSystem(true, currentView_TopDown, currentCam, assetToManipulate.transform, assetManipulationGovernor.RotationPointHolder());
        }

        // If an asset can be scaled..
        if (assetManipulationGovernor.canScale)
        {
            // Get its scale script.
            ScaleObject scalableAssetScript = assetToManipulate.GetComponent<ScaleObject>();

            for (int i = 0; i < scale_PointMarkerArray.Length; i++)
            {
                // Tell each of the markers where they are to rest when not in use on the asset,
                // and load the asset with those markers so it knows what points to scale to.
                scale_PointMarkerArray[i].gameObject.SetActive(true);
                //!!! THIS NEEDS TO BE FIXED DURING RECONNECT !!!//
                Transform restPt = null;

                // Get the marker behavior script and tell the marker it's active and where to rest.
                ScalePointMarkerBehavior pointMarker = scale_PointMarkerArray[i].GetComponent<ScalePointMarkerBehavior>();
                pointMarker.SetPointActive(true, currentCam, restPt);

                if (i == 5)
                {
                    Debug.Log(i);
                    scalableAssetScript.EnableScaling(true);
                }
            }
        }
    }*/

    /*void DisableAssetManipulationSystems()
    {
        assetManipulationGovernor = null;
        rotatableAsset.EnableSystem(false, currentView_TopDown, null, null, transform);
        DeactivateScalability();

        assetManipulationGovernor = null;
        //assetToManipulate = null;
    }*/

   /* void DeactivateScalability()
    {
        rotatableAsset.transform.position = transform.position;
        rotatableAsset.gameObject.SetActive(false);

        for (int i = 0; i < scale_PointMarkerArray.Length; i++)
        {
            ScalePointMarkerBehavior pointMarker = scale_PointMarkerArray[i].GetComponent<ScalePointMarkerBehavior>();
            pointMarker.SetPointActive(false, null, transform);

            scale_PointMarkerArray[i].position = transform.position;
            scale_PointMarkerArray[i].gameObject.SetActive(false);
        }

        ScaleObject scalableAssetScript = assetToManipulate.GetComponent<ScaleObject>();
        scalableAssetScript.EnableScaling(false);
    }*/
}
