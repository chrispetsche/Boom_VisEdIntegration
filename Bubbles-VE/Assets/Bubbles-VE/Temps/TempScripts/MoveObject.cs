using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    // The camera in use during move control.
    Camera cameraInUse;
    // This allows or prevents the move manipulation.
    [SerializeField]
    bool canBeMoved;

    public void EnableMovement(bool active, Camera camToUse)
    {
        canBeMoved = active;
        cameraInUse = camToUse;
    }

    void Start()
    {
        canBeMoved = false;
    }

    // Controls how the object moves in the world with the drag of the mouse/finger.
    private void OnMouseDrag()
    {
        if (canBeMoved)
        {
            Debug.Log("Should be moving!!");
            gameObject.transform.position = cameraInUse.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        }
    }
}
