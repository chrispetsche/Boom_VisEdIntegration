using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Home : MonoBehaviour
{
    private void LoadScene(string _sceneName)
    {
        if (_sceneName != SceneManagement.sceneManagement.GetCurrScene().name)
            SceneManagement.sceneManagement.LoadScene(_sceneName);
    }

    public void OnClick()
    {
        if(AppManager.appManager.mainUser.userType == UserType.super_admin)
        {
            SceneManagement.sceneManagement.LoadScene("ProjectAdminSelect");
        }
        else if (AppManager.appManager.mainUser.userType == UserType.project_admin)
        {
            SceneManagement.sceneManagement.LoadScene("ProjectSelect");
        }
        else 
        {
            SceneManagement.sceneManagement.LoadScene("FloorSelect");
        }
    }
}
