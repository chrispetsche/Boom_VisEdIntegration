using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToOrEditExistingProject : MonoBehaviour
{
    string typeOfProject;

    [SerializeField]
    UIManager uiManager;

    [SerializeField]
    GameObject[] elementsToHideOnNewProjectPanelArray;

    public void AddToOrEditProject(string projectType)
    {
        typeOfProject = projectType;

        for (int e = 0; e < elementsToHideOnNewProjectPanelArray.Length; e++)
        {
            elementsToHideOnNewProjectPanelArray[e].SetActive(false);
        }
    }

    public void UnhideNewProjectTitleElements()
    {
        for (int e = 0; e < elementsToHideOnNewProjectPanelArray.Length; e++)
        {
            elementsToHideOnNewProjectPanelArray[e].SetActive(true);
        }
    }

    public void UploadNewFloorImage()
    {
        uiManager.OpenEditor(typeOfProject);
    }
}
