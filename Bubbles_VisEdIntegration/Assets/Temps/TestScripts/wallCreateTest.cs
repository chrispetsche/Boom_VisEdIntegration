﻿using System.Collections;
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
                        selectObjectScript.SetThisObject(creationCam);

                        segmentBegun = true;
                    }

                    else
                    {
                        Vector3 touchPos = creationCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                        var obj = (GameObject)Instantiate(cornerPt, touchPos, Quaternion.identity);
                        cornerPtB = obj.transform;

                        ClickToSelectTest selectObjectScript = obj.GetComponent<ClickToSelectTest>();
                        selectObjectScript.SetThisObject(creationCam);

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
                        selectObjectScript.SetThisObject(creationCam);

                        segmentBegun = true;
                    }

                    else if (segmentBegun)
                    {
                        Vector3 touchPos = creationCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                        var obj = (GameObject)Instantiate(cornerPt, touchPos, Quaternion.identity);
                        cornerPtB = obj.transform;

                        ClickToSelectTest selectObjectScript = obj.GetComponent<ClickToSelectTest>();
                        selectObjectScript.SetThisObject(creationCam);
                    }

                    break;
                }
        }
    }

}
