using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewUser : MonoBehaviour
{
    public TMP_InputField projAdminNameField;
    public TMP_InputField projAdminEmailField;
    public TMP_InputField passwordField;
    public TMP_InputField passwordCheckField;
    public RawImage image;
    public GameObject[] imgButtonsToReset;

    public Button button;
    public Button resetImageButton;

    private void OnEnable()
    {
        projAdminNameField.text = string.Empty;
        projAdminEmailField.text = string.Empty;
        passwordField.text = string.Empty;
        passwordCheckField.text = string.Empty;

        image.texture = null;
    }

    public void OnClick()
    {
        if (string.IsNullOrEmpty(projAdminNameField.text) || string.IsNullOrEmpty(projAdminEmailField.text)
            || string.IsNullOrEmpty(passwordField.text) || passwordField.text != passwordCheckField.text || image.texture == null)
        {
            Debug.LogError("You're passwords do not match! or empty field.");
        }
        else
        {
            var texture = (Texture2D)image.texture;
            AppManager.appManager.AddNewUser(projAdminNameField.text, projAdminEmailField.text,
                passwordField.text, UserType.project_admin.ToString(), texture.EncodeToJPG());
        }
    }


    public void Update()
    {
        button.interactable = !string.IsNullOrEmpty(projAdminNameField.text) && !string.IsNullOrEmpty(projAdminEmailField.text)
            && !string.IsNullOrEmpty(passwordField.text) && passwordField.text == passwordCheckField.text && image.texture != null;

        resetImageButton.gameObject.SetActive(image.texture != null);
    }
}
