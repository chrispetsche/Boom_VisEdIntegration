using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWallManually : MonoBehaviour
{
    //Sets the type of creation that's supposed to happen.
    [SerializeField]
    enum WallCreationType
    {
        NONE,
        SINGLE_SECTIONS,
        CHAINED_SECTIONS
    };
    [SerializeField]
    WallCreationType wallCreationType = WallCreationType.NONE;

    // Prefab to be created between 2 corner points.
    [SerializeField]
    GameObject wallSection;
    // Prefab to be created as a corner point.
    [SerializeField]
    GameObject cornerPt;
    [SerializeField]
    GameObject editorManagementObject;
    // The camera that is in use to do the creating.
    [SerializeField]
    Camera creationCam;

    // These will be used as the two corner points for the
    // wall to be created.
    [SerializeField]
    Transform cornerPtA;
    [SerializeField]
    Transform cornerPtB;

    // This will let the system know that wall manipulation
    // can happen. Whether it be to create or edit.
    [SerializeField]
    bool systemActive;

    // Lets the system know that at least a corner point has
    // been created and a wall can be placed soon.
    [SerializeField]
    bool segmentBegun;

    // Keeps track of all the corner points that are placed.
    List<Vector3> cornerPointsList;

    private void Start()
    {
        segmentBegun = false;
        cornerPtA = null;
        cornerPtB = null;

        cornerPointsList = new List<Vector3>();
    }

    // The UI will call in here to active or deactive the Manual Creation system.
    public void SetSystemActive(bool active)
    {
        systemActive = active;
    }

    // This is for the UI to call to stop the creation of a wall segment
    // that's been started.
    public void SegmentBegun(bool begun)
    {
        segmentBegun = begun;
    }

    // The UI will call in here to tell the system the type of creating that's supposed happen.
    public void SetCreationType(string type)
    {
        switch (type)
        {
            case "None":
                {
                    wallCreationType = WallCreationType.NONE;

                    break;
                }

            case "Single":
                {
                    wallCreationType = WallCreationType.SINGLE_SECTIONS;

                    break;
                }

            case "Chained":
                {
                    wallCreationType = WallCreationType.CHAINED_SECTIONS;

                    break;
                }
        }
    }

    void Update()
    {
        if (systemActive)
        {
            CheckToCreateCornerPoint();
            CreateNewWallSection();
        }
    }

    // Simply checks the mouse or tap input to see if a corner point is to be created.
    void CheckToCreateCornerPoint()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateNewWallSectionCornerPoints();
        }
    }

    // This creates a corner point where ever the mouse is clicked or finger is tapped. 
    void CreateNewWallSectionCornerPoints()
    {
        switch (wallCreationType)
        {
            case WallCreationType.SINGLE_SECTIONS:
                {
                    // If the user wants a single section to be created at a time and a section hasn't yet been created...
                    if (!segmentBegun)
                    {
                        // Set the position the corner point is to be created at.
                        Vector3 touchPos = creationCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                        // Create the point at the location.
                        var obj = (GameObject)Instantiate(cornerPt, touchPos, Quaternion.identity);
                        cornerPtA = obj.transform;

                        // Set the needed scripts on the corner point.
                        ClickToSelectTest selectObjectScript = obj.GetComponent<ClickToSelectTest>();
                        selectObjectScript.SetThisObject(creationCam, editorManagementObject);

                        // If the list of corner point is empty...
                        if (cornerPointsList.Count == 0)
                        {
                            //Add to the first point to the list
                            cornerPointsList.Add(cornerPtA.position);
                        }

                        // If the list is not empty...
                        else
                        {
                            for (int i = 0; i < cornerPointsList.Count; i++)
                            {
                                // As long as the point being created doesn't have the same position
                                // as any point in the list...
                                if (obj.transform.position != cornerPointsList[i])
                                {
                                    // To be used for snapping within a certain distance from another point later.
                                    //float distBtw = Vector3.Distance(obj.transform.position, cornerPointsList[i]);

                                    //Add to the first point to the list
                                    cornerPointsList.Add(cornerPtA.position);
                                }

                                // But if the points position is the same as one in the list...
                                else if (obj.transform.position == cornerPointsList[i])
                                {
                                    // Make the first point the point in the list.
                                    cornerPtA.position = cornerPointsList[i];
                                }
                            }
                        }

                        // Since a point has been created, 
                        // a wall segment creation has begun.
                        segmentBegun = true;
                    }

                    // If a segment has begun...
                    else
                    {
                        // Repeat the creation steps for the second point needed to create a section.
                        Vector3 touchPos = creationCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                        var obj = (GameObject)Instantiate(cornerPt, touchPos, Quaternion.identity);
                        cornerPtB = obj.transform;

                        ClickToSelectTest selectObjectScript = obj.GetComponent<ClickToSelectTest>();
                        selectObjectScript.SetThisObject(creationCam, editorManagementObject);

                        for (int i = 0; i < cornerPointsList.Count; i++)
                        {
                            // Again, make sure the new points position is not the same as
                            // one that's already been created.
                            if (obj.transform.position != cornerPointsList[i])
                            {
                                // To be used for snapping within a certain distance from another point later.
                                //float distBtw = Vector3.Distance(obj.transform.position, cornerPointsList[i]);

                                // Add the new point to the list.
                                cornerPointsList.Add(cornerPtB.position);
                            }

                            // And if it is the same position...
                            else if (obj.transform.position == cornerPointsList[i])
                            {
                                // Make the second point position the point in the list.
                                cornerPtB.position = cornerPointsList[i];
                            }
                        }

                        // And the wall segment for a single section ends,
                        // allowing a new single section to be created.
                        segmentBegun = false;
                    }

                    break;
                }

            case WallCreationType.CHAINED_SECTIONS:
                {
                    // If the user wants to create a chain of wall sections and a segment has not yet begun...
                    if (!segmentBegun)
                    {
                        // Create the first point that same as before.
                        Vector3 touchPos = creationCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                        var obj = (GameObject)Instantiate(cornerPt, touchPos, Quaternion.identity);
                        cornerPtA = obj.transform;

                        ClickToSelectTest selectObjectScript = obj.GetComponent<ClickToSelectTest>();
                        selectObjectScript.SetThisObject(creationCam, editorManagementObject);

                        // Check to see if it's the first in the list.
                        for (int i = 0; i < cornerPointsList.Count; i++)
                        {
                            if (obj.transform.position != cornerPointsList[i])
                            {
                                // To be used for snapping within a certain distance from another point later.
                                //float distBtw = Vector3.Distance(obj.transform.position, cornerPointsList[i]);

                                cornerPointsList.Add(cornerPtA.position);
                            }

                            else if (obj.transform.position == cornerPointsList[i])
                            {
                                cornerPtA.position = cornerPointsList[i];
                            }
                        }

                        // Tell the system a segment has begun.
                        segmentBegun = true;
                    }

                    // If a segment has begun...
                    else if (segmentBegun)
                    {
                        Vector3 touchPos = creationCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                        var obj = (GameObject)Instantiate(cornerPt, touchPos, Quaternion.identity);
                        cornerPtB = obj.transform;

                        ClickToSelectTest selectObjectScript = obj.GetComponent<ClickToSelectTest>();
                        selectObjectScript.SetThisObject(creationCam, editorManagementObject);

                        for (int i = 0; i < cornerPointsList.Count; i++)
                        {
                            if (cornerPtB.position != cornerPointsList[i])
                            {
                                // To be used for snapping within a certain distance from another point later.
                                //float distBtw = Vector3.Distance(obj.transform.position, cornerPointsList[i]);

                                cornerPointsList.Add(cornerPtB.position);
                            }

                            else if (cornerPtB.position == cornerPointsList[i])
                            {
                                cornerPtB.position = cornerPointsList[i];
                            }
                        }
                    }

                    // The segment must end elsewhere.
                    break;
                }
        }
    }

    // The system is always calling to create a wall section as long as the system is active, and...
    void CreateNewWallSection()
    {
        // both the corner points for making a wall have been create.
        // If they have...
        if (cornerPtA != null && cornerPtB != null)
        {
            // Get the distance between the two points.
            float distanceBetweenPoints = Vector3.Distance(cornerPtA.position, cornerPtB.position);
            // This will be used to make sure the points are created
            // an acceptable distance for each other.
            float acceptableRange = 0.2286f;

            // If the points have been created at the accepted distance, and a segment has begun...
            if (distanceBetweenPoints > acceptableRange && segmentBegun)
            {
                // Create the wall section at the first corner point position.
                var obj = (GameObject)Instantiate(wallSection, cornerPtA.position, cornerPtA.rotation);
                // Set the script to send the wall settings to.
                angleTest wallSectionSettings = obj.GetComponent<angleTest>();
                // Send the wall the two points that govern its position, rotation and scale.
                wallSectionSettings.SetWallSectionCornerPoints(cornerPtA, cornerPtB);

                // Once a wall section is created...
                switch (wallCreationType)
                {
                    // If the type of creation happening is a single section...
                    case WallCreationType.SINGLE_SECTIONS:
                        {
                            // Empty both corner point slots.
                            cornerPtA = null;
                            cornerPtB = null;

                            break;
                        }
                    // If the type is chaining...
                    case WallCreationType.CHAINED_SECTIONS:
                        {
                            // Reset its first point with the second.
                            cornerPtA = cornerPtB;
                            // Empty the second.
                            cornerPtB = null;

                            break;
                        }
                    // If there is no wall sections to be created...
                    case WallCreationType.NONE:
                        {
                            // Any wall segment begun now ends.
                            segmentBegun = false;
                            // Empty both corner point slots.
                            cornerPtA = null;
                            cornerPtB = null;

                            break;
                        }
                }
            }

            // If the distance between the two points is less than or equal to the acceptable
            // amount, and a segment has begun... //!!! Later !!!//
            else if (distanceBetweenPoints <= acceptableRange && segmentBegun)
            {
                // Destroy the second corner point.
                //GameObject pointBToDestroy = cornerPtB.gameObject;
                //Destroy(pointBToDestroy);

                // If the creation type is a single section...
                //if (wallCreationType == WallCreationType.SINGLE_SECTIONS)
                //{
                    // Also destroy its first point so that the 
                    // segment can restart.
                    //GameObject pointAToDestroy = cornerPtA.gameObject;
                    //Destroy(pointAToDestroy);
                //}

                // The segment ends.
                segmentBegun = false;
            }
        }
    }
}
