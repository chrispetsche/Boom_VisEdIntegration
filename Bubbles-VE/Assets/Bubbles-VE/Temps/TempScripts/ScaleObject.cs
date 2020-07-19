using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObject : MonoBehaviour
{
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

    // This lets the system know if it
    // can scale or not.
    [SerializeField]
    bool scaleActive;

    // These are the points that the 
    // scale marker points will snap to.
    public Transform[] myScalePoints;

    // The Manipulation Manager will load this array so this
    // system knows what points to scale to.
    [SerializeField]
    Transform[] scalePointMarkerArray;
    ScalePointMarkerBehavior[] pointMarker = new ScalePointMarkerBehavior[6];

    public Transform LoadScalePointMarkers(int pt, Transform marker)
    {
        scalePointMarkerArray[pt] = marker;

        return myScalePoints[pt];
    }

    // When this asset is selected and unselected, the Manipulation Manager will call this
    // to active and set the system up to run after its scale point markers are loaded. 
    public void EnableScaling(bool active)
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
        }

        else
        {
            // When the asset is deactivated the scale point marker array
            // is cleared so that it doesn't by chance scale with another asset.
            //for (int sPt = 0; sPt < scalePointMarkerArray.Length; sPt++)
            //{
                //scalePointMarkerArray[sPt] = null;
            //}
        }
    }

    private void Start()
    {
        for (int i = 0; i < scalePointMarkerArray.Length; i++)
        {
            pointMarker[i] = scalePointMarkerArray[i].GetComponent<ScalePointMarkerBehavior>();
        }
    }

    void Update()
    {
        if (!scaleActive)
        {
            if (scalePointMarkerArray[5].parent == null)
            {
                for (int i = 0; i < scalePointMarkerArray.Length; i++)
                {
                    scalePointMarkerArray[i].parent = myScalePoints[i];
                    pointMarker[i].SetPointActive(false, null, myScalePoints[i]);
                }
            }
        }

        else
        {
            CheckAssetScaling();
        }
    }

    void CheckAssetScaling()
    {
        for (int m = 0; m < pointMarker.Length; m++)
        {
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
        if (scaling)
        {
            for (int m = 0; m < scalePointMarkerArray.Length; m++)
            {
                if (m != inUse)
                {
                    pointMarker[m].SetPointActive(false, null, myScalePoints[m]);
                    scalePointMarkerArray[m].parent = myScalePoints[m];
                }
            }
        }

        else
        {
            for (int m = 0; m < scalePointMarkerArray.Length; m++)
            {
                pointMarker[m].SetPointActive(transform, tempCam, myScalePoints[m]);
                scalePointMarkerArray[m].parent = null;
            }
        }

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
