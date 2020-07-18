using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObject : MonoBehaviour
{
    [SerializeField]
    Transform[] scalePointMarkerArray;

    [SerializeField]
    bool xScalable;
    [SerializeField]
    bool yScalable;
    [SerializeField]
    bool zScalable;

    public Vector3 currPos;
    public Vector3 currScale;

    public void SetWallSectionCornerPoints(Transform ptA, Transform ptB)
    {
        
    }

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

    Vector3 NewWallPosition()
    {
        float xPos = scalePointMarkerArray[2].position.x + (scalePointMarkerArray[3].position.x - scalePointMarkerArray[2].position.x) / 2;
        float yPos = scalePointMarkerArray[4].position.z + (scalePointMarkerArray[5].position.y - scalePointMarkerArray[4].position.y) / 2;
        float zPos = scalePointMarkerArray[0].position.z + (scalePointMarkerArray[1].position.z - scalePointMarkerArray[0].position.z) / 2;

        return new Vector3(xPos, 0f, zPos);
    }

    void Update()
    {
        currPos = NewWallPosition();
        transform.position = NewWallPosition();

        transform.localScale = new Vector3(DistanceBetweenEndPoints("x"), 1.0f, DistanceBetweenEndPoints("z"));
        currScale = transform.localScale;
    }
}
