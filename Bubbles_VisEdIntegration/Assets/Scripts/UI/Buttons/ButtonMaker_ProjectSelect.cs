using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMaker_ProjectSelect : MonoBehaviour
{
	public GameObject gridParent;
	public Button_Project projectButtonPrefab;
    public DeletePrompt_Project deletePrompt;

    private List<Button_Project> buttons;

    private void Start()
    {
        buttons = new List<Button_Project>();

        AppManager.appManager.ProjectsUpdated += AppManager_ProjectsUpdated;
        AppManager.appManager.RequestProjects(AppManager.appManager.mainUser.userType == UserType.super_admin ? AppManager.appManager.currUser.userId : -1);
    }

    private void OnDestroy()
    {
        AppManager.appManager.ProjectsUpdated -= AppManager_ProjectsUpdated;
    }

    private void AppManager_ProjectsUpdated(object sender, System.EventArgs e)
    {
        SetupButtons();
    }

    private void SetupButtons()
    {
        int i = 0;
        for (; i < AppManager.appManager.currUser.projects.Count; i++)
        {
            if (i >= buttons.Count)
            {
                Button_Project gridButton = Instantiate(projectButtonPrefab, gridParent.transform);
                gridButton.gameObject.SetActive(false);
                buttons.Add(gridButton);
            }

            buttons[i].Initialize(AppManager.appManager.currUser.projects[i], deletePrompt);
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
