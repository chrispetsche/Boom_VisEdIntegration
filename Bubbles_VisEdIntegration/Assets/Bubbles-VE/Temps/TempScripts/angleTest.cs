using UnityEngine;

public class angleTest : MonoBehaviour
{
    public float angle;
    public Transform targetA;
    public Transform targetB;

    public Vector3 currPos;
    public Vector3 currRot;
    public Vector3 currScale;

    // Start is called before the first frame update
    public void SetWallSectionCornerPoints(Transform ptA, Transform ptB)
    {
        targetA = ptA;
        targetB = ptB;
    }

    Vector3 WallCurrentRotation()
    {
        return new Vector3(0f, AngleAdjustment(), 0f);
    }

    float AngleAdjustment()
    {
        return 360 - angle;
    }

    float DistanceBetweenEndPoints()
    {
        return Vector3.Distance(targetB.position, targetA.position);
    }

    Vector3 NewWallPosition()
    {
        float xPos = targetA.position.x + (targetB.position.x - targetA.position.x) / 2;
        float zPos = targetA.position.z + (targetB.position.z - targetA.position.z) / 2;

        return new Vector3(xPos, 0f, zPos);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDir = targetB.position - targetA.position;
        Vector3 forward = targetA.right;
        angle = Vector3.SignedAngle(targetDir, forward, Vector3.up);

        currPos = NewWallPosition();
        transform.position = NewWallPosition();

        currRot = WallCurrentRotation();
        transform.eulerAngles = WallCurrentRotation();

        transform.localScale = new Vector3(DistanceBetweenEndPoints(), 3.0f, 0.25f);
        currScale = transform.localScale;
    }
}
