using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallCreateTest : MonoBehaviour
{
    [SerializeField]
    enum WallCreationType
    {
        SINGLE_SECTIONS,
        CHAINED_SECTIONS
    };
    [SerializeField]
    WallCreationType wallCreationType = WallCreationType.SINGLE_SECTIONS;

    [SerializeField]
    GameObject wallSection;
    [SerializeField]
    GameObject cornerPt;
    [SerializeField]
    Camera creationCam;

    
    Transform cornerPtA;
    Transform cornerPtB;

    [SerializeField]
    bool segmentBegun;
    [SerializeField]
    bool segmentEnded;

    private void Start()
    {
        segmentBegun = false;
        segmentEnded = false;
        cornerPtA = null;
        cornerPtB = null;
    }

    void Update()
    {
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

                        segmentBegun = true;
                    }

                    else
                    {
                        Vector3 touchPos = creationCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                        var obj = (GameObject)Instantiate(cornerPt, touchPos, Quaternion.identity);
                        cornerPtB = obj.transform;

                        segmentBegun = false;
                    }

                    break;
                }

            case WallCreationType.CHAINED_SECTIONS:
                {
                    if (!segmentBegun && !segmentEnded)
                    {
                        Vector3 touchPos = creationCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                        var obj = (GameObject)Instantiate(cornerPt, touchPos, Quaternion.identity);
                        cornerPtA = obj.transform;

                        segmentBegun = true;
                    }

                    else if (segmentBegun && !segmentEnded)
                    {
                        Vector3 touchPos = creationCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                        var obj = (GameObject)Instantiate(cornerPt, touchPos, Quaternion.identity);
                        cornerPtB = obj.transform;
                    }

                    break;
                }
        }
    }

}
