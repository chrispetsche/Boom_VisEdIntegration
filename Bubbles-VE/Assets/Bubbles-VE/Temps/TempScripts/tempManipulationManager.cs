using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempManipulationManager : MonoBehaviour
{
    [SerializeField]
    GameObject assetToManipulate;

    [SerializeField]
    RotateObject rotatableAsset;
    // This needs to be in the the Asset Manipulation Governor
    [SerializeField]
    Transform tempAssetRotateRestPos;

    [SerializeField]
    Transform[] scale_PointMarkerArray;
    ScaleObject scalableAsset;

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

    void EnableAssetManipulationSystems()
    {
        rotatableAsset.EnableSystem(true, currentView_TopDown, currentCam, assetToManipulate.transform, tempAssetRotateRestPos);
        ActivateScalability();
    }

    void DisableAssetManipulationSystems()
    {
        rotatableAsset.EnableSystem(false, currentView_TopDown, null, null, transform);
        DeactivateScalability();
    }

    void ActivateScalability()
    {
        scalableAsset = assetToManipulate.GetComponent<ScaleObject>();

        scalableAsset.scalePointMarkerArray = new Transform[6];
        for (int a = 0; a < scale_PointMarkerArray.Length; a++)
        {
            scalableAsset.scalePointMarkerArray[a] = scale_PointMarkerArray[a];
        }

        scalableAsset.EnableScaling(true);
    }

    void DeactivateScalability()
    {
        scalableAsset.EnableScaling(true);
        scalableAsset = null;
    }
}
