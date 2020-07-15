using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMaker_FloorSelect : MonoBehaviour
{
	public GameObject gridParent;
	public Button_Floor floorButtonPrefab;
    public DeletePrompt_Floor deletePrompt;

    private List<Button_Floor> buttons;

    private void Start()
    {
        buttons = new List<Button_Floor>();

        AppManager.appManager.ProjectsUpdated += AppManager_ProjectsUpdated;
        AppManager.appManager.FloorsUpdated += AppManager_FloorsUpdated;
        if(AppManager.appManager.currProjId == 0)
        {
            AppManager.appManager.RequestProjects();
        }
        else
        {
            AppManager.appManager.RequestFloors();
        }

    }

    private void OnDestroy()
    {
        AppManager.appManager.ProjectsUpdated -= AppManager_ProjectsUpdated;
        AppManager.appManager.FloorsUpdated -= AppManager_FloorsUpdated;
    }

    private void AppManager_ProjectsUpdated(object sender, System.EventArgs e)
    {
        AppManager.appManager.currProjId = AppManager.appManager.currUser.projects[0].projectId;
        AppManager.appManager.RequestFloors();
    }

    private void AppManager_FloorsUpdated(object sender, System.EventArgs e)
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        var project = AppManager.appManager.FindProjectByID(AppManager.appManager.currProjId);

        int i = 0;
        for (; i < project.floors.Count; i++)
        {
            if (i >= buttons.Count)
            {
                Button_Floor gridButton = Instantiate(floorButtonPrefab, gridParent.transform);
                gridButton.gameObject.SetActive(false);
                buttons.Add(gridButton);
            }

            buttons[i].Initialize(project.floors[i], deletePrompt);
            buttons[i].gameObject.SetActive(true);
        }

        if (i < buttons.Count)
        {
            for (; i < buttons.Count; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }
}
