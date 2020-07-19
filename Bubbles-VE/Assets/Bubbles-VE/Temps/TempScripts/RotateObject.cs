using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // Camera that will allow the grabbing of this object.
    Camera usedCamera;
    // The angle the object rotating will be set to.
    public float angle;
    // This will be the object that will rotate when selected.
    [SerializeField]
    Transform objectToRotate;
    // The full rotational value to set the object to.
    public Vector3 currRot;
    // When an object has an active rotational system, and it's
    // not being used, this will be its point to rest at until use.
    [SerializeField]
    Transform restPoint;
    // This will control if the system is running.
    [SerializeField]
    bool rotateActive;
    // For future itreations this may help with how the rotation
    // system swivels.
    [SerializeField]
    bool topdownView;
    // This will help with resetting the rotate marker when not in use.
    bool currentlyRotating;

    private void Start()
    {
        rotateActive = false;
        currentlyRotating = false;
    }

    void Update()
    {
        if (rotateActive)
        {
            ReadyAndRotate();
        }
    }

    // The Manipulation Manager calls this to activate and deactivate the system by passing in the information needed.
    public void EnableRotate(bool active, bool view, Camera currCam, Transform rotatingObj, Transform pointOfRest)
    {
        rotateActive = active;
        currentlyRotating = false;

        objectToRotate = rotatingObj;
        restPoint = pointOfRest;
        topdownView = view;
        usedCamera = currCam;
    }

    // This tells the object the user wants to rotate how to spin and resets the position of this
    // marker to the rest position of the object in use when the it's let go of by the finger. 
    void ReadyAndRotate()
    {
        if (currentlyRotating)
        {
            Vector3 targetDir = objectToRotate.position - transform.position;
            Vector3 forward = transform.forward;
            angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);

            currRot = WallCurrentRotation();
            objectToRotate.eulerAngles = WallCurrentRotation();
        }

        else
        {
            transform.position = restPoint.position;
        }

        // This will constantly reset the currentlyRotating
        // in case it was and now is not.
        currentlyRotating = false;
    }

    // The rotational value that will be passed to the object
    // that's to be rotated. 
    Vector3 WallCurrentRotation()
    {
        return new Vector3(0f, AngleAdjustment(), 0f);
    }

    // To keep the angle between 0 and 360 it's pulled through here.
    float AngleAdjustment()
    {
        return 360 - angle;
    }

    // This adjusts this markers distance from the camera so that the rotation happens
    // much more smoothly.
    float MarkerDistanceFromCamera()
    {
        return Vector3.Distance(usedCamera.transform.position, objectToRotate.position) - 2;
    }

    // When this marker is dragged by the mouse/finger...
    private void OnMouseDrag()
    {
        // And the rotate system is active...
        if(rotateActive)
        {
            // It's currently rotating
            currentlyRotating = true;
            // This markers position is equal to the x and y of the mouse/finger dragging, and the adjusted distance from the camera.
            transform.position = usedCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, MarkerDistanceFromCamera()));
        }
    }
}
