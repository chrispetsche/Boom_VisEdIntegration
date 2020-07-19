using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePointMarkerBehavior : MonoBehaviour
{
    // Tells the marker it can be used.
    bool markerActive;
    // Tells the system the marker is being used.
    bool markerInUse;
    // The point the marker returns to when not being used.
    Transform restPoint;
    // The camera running the mouse/finger tracking.
    Camera cameraInUse;

    // System calls this to open the marker up for use, or turn it off.
    public void SetPointActive(bool active, Camera camToUse, Transform restPt)
    {
        markerActive = active;
        cameraInUse = camToUse;
        restPoint = restPt;
    }

    // System calls on this to know if the user
    // is interacting with it.
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
        // If the marker is in use...
        if (!markerInUse)
        {
            // Set its position to the rest position.
            transform.position = restPoint.position;
        }

        // Constantly set the marker to not being used.
        markerInUse = false;
    }

    //!!! THIS NEEDS TO BE MODIFIED ALONG WITH THE ADJUSTMENT DISTANCE AS PART OF TURNING THE SYSTEM !!!//
    // When this marker is dragged by the mouse/finger...
    private void OnMouseDrag()
    {
        // If this marker is active...
        if (markerActive)
        {
            // Tell the system it's being used.
            markerInUse = true;
            // This markers position is equal to the x and y of the mouse/finger dragging, and the adjusted distance from the camera.
            transform.position = cameraInUse.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, AdjustedMarkerDistance()));
        }
    }
    
    // Returns the distance the marker needs to be from the camera.
    float AdjustedMarkerDistance()
    {
        return Vector3.Distance(cameraInUse.transform.position, restPoint.position);
    }
}
