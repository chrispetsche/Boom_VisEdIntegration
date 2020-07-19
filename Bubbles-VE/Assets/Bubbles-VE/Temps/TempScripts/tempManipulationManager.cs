using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempManipulationManager : MonoBehaviour
{
    [SerializeField]
    GameObject assetToManipulate;
    AssetManipulationGovernor assetManipulationGovernor;

    [SerializeField]
    RotateObject rotatableAsset;
    // This needs to be in the the Asset Manipulation Governor
    [SerializeField]
    Transform tempAssetRotateRestPos;

    [SerializeField]
    Transform[] scale_PointMarkerArray;

    [SerializeField]
    Camera topdownCam;
    [SerializeField]
    Camera perspCam;
    Camera currentCam;
    // This will be adjusted by the 2D to 3D UI toggle.
    [SerializeField]
    bool currentView_TopDown;

    private void Start()
    {
        
    }

    void Update()
    {
        if (currentView_TopDown)
        {
            topdownCam.gameObject.SetActive(true);
            perspCam.gameObject.SetActive(false);

            currentCam = topdownCam;
        }

        else
        {
            topdownCam.gameObject.SetActive(false);
            perspCam.gameObject.SetActive(true);

            currentCam = perspCam;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            EnableAssetManipulationSystems();
        }
    }

    // Selection Manager calls in when an object in the scene has been selected or deselected,
    // and passes in the object if one chosen.
    public void ObjectSelected(bool selected, GameObject objSelected)
    {
        if (selected)
        {
            // When selected, the object is set as an
            // object to manipulate in some way.
            assetToManipulate = objSelected;
            // Set the governor script to communicate with.
            assetManipulationGovernor = assetToManipulate.GetComponent<AssetManipulationGovernor>();

            EnableAssetManipulationSystems();
        }

        else
        {
            DisableAssetManipulationSystems();
        }
    }

    void EnableAssetManipulationSystems()
    {
        if (assetManipulationGovernor.canMove)
        {

        }

        if (assetManipulationGovernor.canRotate)
        {
            rotatableAsset.EnableSystem(true, currentView_TopDown, currentCam, assetToManipulate.transform, assetManipulationGovernor.RotationPointHolder());
        }

        if (assetManipulationGovernor.canScale)
        {
            ScaleObject scalableAssetScript = assetToManipulate.GetComponent<ScaleObject>();

            for (int i = 0; i < scale_PointMarkerArray.Length; i++)
            {
                scale_PointMarkerArray[i].position = scalableAssetScript.LoadScalePointMarkers(i, scale_PointMarkerArray[i]).position;
            }

            scalableAssetScript.EnableScaling(true);
        }
    }

    void DisableAssetManipulationSystems()
    {
        assetManipulationGovernor = null;
        rotatableAsset.EnableSystem(false, currentView_TopDown, null, null, transform);
        DeactivateScalability();

        assetManipulationGovernor = null;
        assetToManipulate = null;
    }

    void DeactivateScalability()
    {
        for (int i = 0; i < scale_PointMarkerArray.Length; i++)
        {
            scale_PointMarkerArray[i].position = transform.position;
            scale_PointMarkerArray[i].gameObject.SetActive(false);
        }

        ScaleObject scalableAssetScript = assetToManipulate.GetComponent<ScaleObject>();
        scalableAssetScript.EnableScaling(false);
    }
}
