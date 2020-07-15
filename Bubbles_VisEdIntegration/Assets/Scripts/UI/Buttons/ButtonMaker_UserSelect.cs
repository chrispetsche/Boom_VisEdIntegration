using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMaker_UserSelect : MonoBehaviour
{
    public GameObject gridParent;
    public Button_User userButtonPrefab;
    public DeletePrompt_ProjectAdmin deletePrompt;

    private List<Button_User> buttons;

    private void Start()
    {
        buttons = new List<Button_User>();

        AppManager.appManager.ProjectAdminsUpdated += AppManager_ProjectAdminsUpdated;
        AppManager.appManager.RequestProjectAdminData();
    }

    private void OnDestroy()
    {
        AppManager.appManager.ProjectAdminsUpdated -= AppManager_ProjectAdminsUpdated;
    }

    private void AppManager_ProjectAdminsUpdated(object sender, System.EventArgs e)
    {
        SetupButtons();
    }

    private void SetupButtons()
    { 
        int i = 0;
        for (; i < AppManager.appManager.projectAdmins.Count; i++)
        {
            if (i >= buttons.Count)
            {
                Button_User gridButton = Instantiate(userButtonPrefab, gridParent.transform);
                gridButton.gameObject.SetActive(false);
                buttons.Add(gridButton);
            }

            buttons[i].Initialize(AppManager.appManager.projectAdmins[i], deletePrompt);
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
