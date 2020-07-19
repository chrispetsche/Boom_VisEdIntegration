using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObject : MonoBehaviour
{
    //!!! THE MARKERS CAMERA WILL BE SET DIRECTLY THROUGH THE GOVERNOR !!!//
    [SerializeField]
    Camera tempCam;

    // These will control which axis the 
    // asset will scale along.
    [SerializeField]
    bool xScalable;
    [SerializeField]
    bool yScalable;
    [SerializeField]
    bool zScalable;

    // This lets the scale system know if 
    // it can scale or not.
    [SerializeField]
    bool scaleActive;

    // These are the points that the 
    // scale marker points will snap to.
    public Transform[] myScalePoints;

    // These let the scale system knows what points to scale to.
    [SerializeField]
    Transform[] scalePointMarkerArray;
    // Array of scripts to talk to each of markers through.
    ScalePointMarkerBehavior[] pointMarker = new ScalePointMarkerBehavior[6];

    // When this asset is selected and unselected, the Manipulation Manager will call this
    // to active and set the system up to run after its scale point markers are loaded. 
    public void EnableScaling(bool active, Camera cam)
    {
        scaleActive = active;

        if (scaleActive)
        {
            // Each of the markers are turned off or on depending on
            // whether or not its applicable axis is scalable.
            scalePointMarkerArray[0].gameObject.SetActive(zScalable);
            scalePointMarkerArray[1].gameObject.SetActive(zScalable);
            scalePointMarkerArray[2].gameObject.SetActive(xScalable);
            scalePointMarkerArray[3].gameObject.SetActive(xScalable);
            scalePointMarkerArray[4].gameObject.SetActive(yScalable);
            scalePointMarkerArray[5].gameObject.SetActive(yScalable);

            for (int i = 0; i < scalePointMarkerArray.Length; i++)
            {
                pointMarker[i].SetPointActive(true, cam, myScalePoints[i]);
            }
        }

        else
        {
            // If the system isn't active, turn all markers off.
            for (int sPt = 0; sPt < scalePointMarkerArray.Length; sPt++)
            {
                pointMarker[sPt].SetPointActive(false, null, myScalePoints[sPt]);
                scalePointMarkerArray[sPt].parent = myScalePoints[sPt];
                scalePointMarkerArray[sPt].gameObject.SetActive(false);
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < scalePointMarkerArray.Length; i++)
        {
            pointMarker[i] = scalePointMarkerArray[i].GetComponent<ScalePointMarkerBehavior>();
            scalePointMarkerArray[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (scaleActive)
                EnableScaling(true, tempCam);
            else
                EnableScaling(false, null);
        }

        if (scaleActive)
        {
            CheckAssetScaling();
        }
    }

    // If the system is active and checking for scaling...
    void CheckAssetScaling()
    {
        for (int m = 0; m < pointMarker.Length; m++)
        {
            // If one of the markers is in use...
            if (pointMarker[m].MarkerInUse())
            {
                SetMarkers(true, m);
            }

            else
            {
                SetMarkers(false, m);
            }
        }
        
    }

    void SetMarkers(bool scaling, int inUse)
    {
        // If the object is scaling...
        if (scaling)
        {
            for (int m = 0; m < scalePointMarkerArray.Length; m++)
            {
                // Set the parent of all but the marker being used to their rest points.
                if (m != inUse)
                {
                    pointMarker[m].SetPointActive(false, null, myScalePoints[m]);
                    scalePointMarkerArray[m].parent = myScalePoints[m];
                }
            }
        }

        // If the object isn't scaling...
        else
        {
            for (int m = 0; m < scalePointMarkerArray.Length; m++)
            {
                // Unparent all marker points.
                pointMarker[m].SetPointActive(transform, tempCam, myScalePoints[m]);
                scalePointMarkerArray[m].parent = null;
            }
        }

        // Run the scaling of the asset.
        ScaleAsset();
    }

    // This is called to do the actual scaling of the asset.
    void ScaleAsset()
    {
        // While the scale is being adjusted, the position needs
        // to be adjusted along with it. 
        transform.position = NewAssetPosition();
        transform.localScale = new Vector3(DistanceBetweenEndPoints("x"), DistanceBetweenEndPoints("y"), DistanceBetweenEndPoints("z"));
    }

    // While the asset scales, its new position is pulled from here.
    Vector3 NewAssetPosition()
    {
        float xPos = scalePointMarkerArray[2].position.x + (scalePointMarkerArray[3].position.x - scalePointMarkerArray[2].position.x) / 2;
        // The "y" axis works slightly different than the "x" and "z". 
        float yPos = scalePointMarkerArray[5].position.y + (DistanceBetweenEndPoints("y") / 2);
        float zPos = scalePointMarkerArray[0].position.z + (scalePointMarkerArray[1].position.z - scalePointMarkerArray[0].position.z) / 2;

        return new Vector3(xPos, yPos, zPos);
    }

    // For both the position and scale the distance between scale point markers
    // pulled from here.
    float DistanceBetweenEndPoints(string axis)
    {
        float dist = 0f;

        switch (axis)
        {
            case "x":
                {
                    dist = Vector3.Distance(scalePointMarkerArray[3].position, scalePointMarkerArray[2].position);

                    break;
                }

            case "y":
                {
                    dist = Vector3.Distance(scalePointMarkerArray[5].position, scalePointMarkerArray[4].position);

                    break;
                }

            case "z":
                {
                    dist = Vector3.Distance(scalePointMarkerArray[1].position, scalePointMarkerArray[0].position);

                    break;
                }
        }

        return dist;
    }
}
