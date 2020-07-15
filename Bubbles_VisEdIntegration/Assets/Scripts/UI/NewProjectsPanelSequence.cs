using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewProjectsPanelSequence : MonoBehaviour
{
    int inputPanelCount;
    string typeOfProject;
    string styleSelection;

    [SerializeField]
    UIManager uiManager;

    [SerializeField]
    GameObject[] newProjectInformationInputPanelArray;

    [SerializeField]
    GameObject ramImg; //!!! This will be changed later to fit the actual needs of the system !!!//

    [SerializeField]
    GameObject addOrEditProjectPanel;

    public void StartOrReset_NewProjectPanel(string projectType)
    {
        typeOfProject = projectType;
        addOrEditProjectPanel.SetActive(false);
        SelectANewStyle();
    }

    void SelectANewStyle()
    {
        inputPanelCount = 0;
        SelectOrResetStyle("");

        AddToOrEditExistingProject addOrEditPanel = addOrEditProjectPanel.GetComponent<AddToOrEditExistingProject>();
        addOrEditPanel.UnhideNewProjectTitleElements();

        newProjectInformationInputPanelArray[0].SetActive(true);
        newProjectInformationInputPanelArray[1].SetActive(false);
        newProjectInformationInputPanelArray[2].SetActive(false);
    }

    public void SelectOrResetStyle(string style)
    {
        styleSelection = style;
    }

    void InputNewProject_FormData()
    {
        //!!! Reset form data inputs for new input !!!//

        newProjectInformationInputPanelArray[0].SetActive(false);
        newProjectInformationInputPanelArray[1].SetActive(true);
        newProjectInformationInputPanelArray[2].SetActive(false);
    }

    void InputNewProject_Image()
    {
        ramImg.SetActive(false);

        newProjectInformationInputPanelArray[0].SetActive(false);
        newProjectInformationInputPanelArray[1].SetActive(false);
        newProjectInformationInputPanelArray[2].SetActive(true);
    }

    void InputNewProject_ImageSelected()
    {
        //!!! Later this will be changed to add the image to the preview canvas !!!//

        ramImg.SetActive(true);
    }

    public void NextInputPanel()
    {
        switch (inputPanelCount)
        {
            case 0:
                {
                    string[] styles = { "A", "B", "C", "D" };
                    for (int s = 0; s < styles.Length; s++)
                    {
                        if (styleSelection == styles[s])
                        {
                            //!!! Log selection !!!//

                            //!!! Call for toggle dot and progress line animations !!!//

                            InputNewProject_FormData();
                            inputPanelCount++;
                        }
                    }

                    break;
                }

            case 1:
                {
                    //!!! If 'Data Status' = 'Complete' !!!//
                    //!!! Log data !!!//

                    InputNewProject_Image();
                    inputPanelCount++;

                    break;
                }

            case 2:
                {
                    //!!! Send all information to server and request feedback for next steps, including the dollhouse extrusion. !!!//
                    //!!! Open approiate color wash and run dollhouse loadbar
                    uiManager.OpenEditor(typeOfProject);

                    break;
                }
        }
    }
}
