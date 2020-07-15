using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginButton : MonoBehaviour
{
	//public TMP_InputField usernameInputField;
	//public TMP_InputField passwordInputField;

    public void Login()
    {
        //#if UNITY_EDITOR
        //        //usernameInputField.text = "super_admin@example.com";
        //        //usernameInputField.text = "project_admin@example.com";
        //        usernameInputField.text = "project_owner@example.com";
        //        passwordInputField.text = "12345";
        //#endif

        //        if (string.IsNullOrEmpty(usernameInputField.text) || string.IsNullOrEmpty(passwordInputField.text))
        //        {
        //            Debug.LogError("user name and password required to use the app.");
        //            // TODO: Set up error popup here.
        //        }
        //        else
        //        {
        //            RestFarm.restFarm.Login(usernameInputField.text, passwordInputField.text, AppManager.appManager.OnLogin);
        //        }

        SceneManagement.sceneManagement.LoadScene("ProjectSelectV2");
    }
}
