using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallCreateTest : MonoBehaviour
{
    [SerializeField]
    enum WallCreationType
    {
        NONE,
        SINGLE_SECTIONS,
        CHAINED_SECTIONS
    };
    [SerializeField]
    WallCreationType wallCreationType = WallCreationType.NONE;

    [SerializeField]
    GameObject wallSection;
    [SerializeField]
    GameObject cornerPt;
    [SerializeField]
    GameObject editorManagementObject;
    [SerializeField]
    Camera creationCam;

    [SerializeField]
    Transform cornerPtA;
    [SerializeField]
    Transform cornerPtB;

    [SerializeField]
    bool segmentBegun;
    [SerializeField]
    bool segmentEnded;

    List<Vector3> wallSections;
    List<List<Vector3>> wallSegments;

    private void Start()
    {
        segmentBegun = false;
        segmentEnded = false;
        cornerPtA = null;
        cornerPtB = null;


        wallSections = new List<Vector3>();
        wallSegments = new List<List<Vector3>>();
    }

    float x;
    float y;
    float z;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            x++;
            y++;
            z++;

            Vector3 wallSectionPt = new Vector3(x, y, z);
            wallSections.Add(wallSectionPt);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector3 a = Vector3.zero;
            for (int wSec = 0; wSec < wallSections.Count; wSec++)
            {
                if (a == wallSections[wSec])
                {
                    Debug.Log("Same");
                }

                else if (a != wallSections[wSec])
                {
                    Debug.Log("Different");
                }
                //Debug.Log(wallSections[wSec]);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            wallSegments.Add(wallSections);
            wallSections = new List<Vector3>();
            Vector3 test = Vector3.zero;

            for (int i = 0; i < wallSegments.Count; i++)
            {
                for (int j = 0; j < wallSections.Count; j++)
                {
                    if(wallSegments.Count >= 2)
                    Debug.Log(wallSegments[0][j]);

                    //if (test == wallSegments[i][j])
                    //{
                        //Debug.Log("Same");
                    //}

                    //else if (test != wallSegments[i][j])
                    //{
                        //Debug.Log("Different");
                    //}
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            var obj = (GameObject)Instantiate(wallSection, Vector3.zero, cornerPtA.rotation);
            angleTest wallSectionSettings = obj.GetComponent<angleTest>();
            wallSectionSettings.SetWallSectionCornerPoints(cornerPtA, cornerPtB);
        }

        if (Input.GetMouseButtonDown(0))
        {
            CreateNewWallSectionCornerPoints();
        }

        CreateNewWallSection();
    }

    void CreateNewWallSection()
    {
        if (cornerPtA != null && cornerPtB != null)
        {
            float distanceBetweenPoints = Vector3.Distance(cornerPtA.position, cornerPtB.position);
            float acceptableRange = 0.2286f;
            if (distanceBetweenPoints > acceptableRange && segmentBegun)
            {
                var obj = (GameObject)Instantiate(wallSection, Vector3.zero, cornerPtA.rotation);
                angleTest wallSectionSettings = obj.GetComponent<angleTest>();
                wallSectionSettings.SetWallSectionCornerPoints(cornerPtA, cornerPtB);

                switch (wallCreationType)
                {
                    case WallCreationType.SINGLE_SECTIONS:
                        {
                            cornerPtA = null;
                            cornerPtB = null;

                            break;
                        }

                    case WallCreationType.CHAINED_SECTIONS:
                        {
                            cornerPtA = cornerPtB;
                            cornerPtB = null;

                            break;
                        }

                    case WallCreationType.NONE:
                        {
                            if (!segmentEnded)
                            {
                                segmentBegun = false;
                                segmentEnded = true;
                                cornerPtA = null;
                                cornerPtB = null;
                            }

                            break;
                        }
                }
            }

            else if (distanceBetweenPoints <= acceptableRange && segmentBegun)
            {
                GameObject pointToDestroy = cornerPtB.gameObject;
                Destroy(pointToDestroy);
                segmentBegun = false;
            }
        }
    }

    void CreateNewWallSectionCornerPoints()
    {
        switch (wallCreationType)
        {
            case WallCreationType.SINGLE_SECTIONS:
                {
                    if (!segmentBegun)
                    {
                        Vector3 touchPos = creationCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                        var obj = (GameObject)Instantiate(cornerPt, touchPos, Quaternion.identity);
                        cornerPtA = obj.transform;

                        ClickToSelectTest selectObjectScript = obj.GetComponent<ClickToSelectTest>();
                        selectObjectScript.SetThisObject(creationCam, editorManagementObject);

                        segmentBegun = true;
                    }

                    else
                    {
                        Vector3 touchPos = creationCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                        var obj = (GameObject)Instantiate(cornerPt, touchPos, Quaternion.identity);
                        cornerPtB = obj.transform;

                        ClickToSelectTest selectObjectScript = obj.GetComponent<ClickToSelectTest>();
                        selectObjectScript.SetThisObject(creationCam, editorManagementObject);

                        segmentBegun = false;
                    }

                    break;
                }

            case WallCreationType.CHAINED_SECTIONS:
                {
                    if (!segmentBegun)
                    {
                        Vector3 touchPos = creationCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                        var obj = (GameObject)Instantiate(cornerPt, touchPos, Quaternion.identity);
                        cornerPtA = obj.transform;

                        ClickToSelectTest selectObjectScript = obj.GetComponent<ClickToSelectTest>();
                        selectObjectScript.SetThisObject(creationCam, editorManagementObject);

                        segmentBegun = true;
                    }

                    else if (segmentBegun)
                    {
                        Vector3 touchPos = creationCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                        var obj = (GameObject)Instantiate(cornerPt, touchPos, Quaternion.identity);
                        cornerPtB = obj.transform;

                        ClickToSelectTest selectObjectScript = obj.GetComponent<ClickToSelectTest>();
                        selectObjectScript.SetThisObject(creationCam, editorManagementObject);
                    }

                    break;
                }
        }
    }

}
