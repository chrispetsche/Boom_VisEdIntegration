using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField]
    Camera usedCamera;
    public float angle;
    [SerializeField]
    Transform objectToRotate;

    public Vector3 currRot;

     Vector3 WallCurrentRotation()
    {
        return new Vector3(0f, AngleAdjustment(), 0f);
    }

    float AngleAdjustment()
    {
        return 360 - angle;
    }
    void Update()
    {
        Vector3 targetDir = objectToRotate.position - transform.position;
        Vector3 forward = transform.forward;
        angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);

        currRot = WallCurrentRotation();
        objectToRotate.eulerAngles = WallCurrentRotation();
    }

    private void OnMouseDrag()
    {
        transform.position = usedCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
    }
}
