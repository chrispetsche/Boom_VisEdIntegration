using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePointMarkerBehavior : MonoBehaviour
{
    bool markerActive;
    bool markerInUse;
    Transform restPoint;
    Camera cameraInUse;

    public void SetPointActive(bool active, Camera camToUse, Transform restPt)
    {
        markerActive = active;
        cameraInUse = camToUse;
        restPoint = restPt;
    }

    public bool MarkerInUse()
    {
        return markerInUse;
    }

    void Update()
    {
        if(markerActive)
        SetMyPosition();
    }

    void SetMyPosition()
    {
        if (!markerInUse)
        {
            transform.position = restPoint.position;
        }

        markerInUse = false;
    }

    // When this marker is dragged by the mouse/finger...
    private void OnMouseDrag()
    {
        // And the rotate system is active...
        if (markerActive)
        {
            // It's currently rotating
            markerInUse = true;
            // This markers position is equal to the x and y of the mouse/finger dragging, and the adjusted distance from the camera.
            transform.position = cameraInUse.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, AdjustedMarkerDistance()));
        }
    }

    float AdjustedMarkerDistance()
    {
        return Vector3.Distance(cameraInUse.transform.position, restPoint.position);
    }
}
