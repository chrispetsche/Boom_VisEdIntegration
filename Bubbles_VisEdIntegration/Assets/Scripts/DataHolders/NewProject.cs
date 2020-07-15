using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewProject : MonoBehaviour
{
	public TMP_InputField projNameField;
	public TMP_InputField clientEmailField;
	public TMP_InputField passwordField;
	public TMP_InputField passwordCheckField;
	public RawImage image;
	public GameObject[] imgButtonsToReset;

    public Button button;
    public Button resetImageButton;

    private void OnEnable()
    {
        projNameField.text = string.Empty;
        clientEmailField.text = string.Empty;
        passwordField.text = string.Empty;
        passwordCheckField.text = string.Empty;

        image.texture = null;
    }

    public void OnClick()
    {
        if (string.IsNullOrEmpty(projNameField.text) || string.IsNullOrEmpty(clientEmailField.text)
            || string.IsNullOrEmpty(passwordField.text) || passwordField.text != passwordCheckField.text && image.texture == null)
        {
            Debug.LogError("You're passwords do not match! or empty field.");
        }
        else
        {
            byte[] encoded = null;
            if (image.texture != null)
            {
                var texture = (Texture2D)image.texture;
                encoded = texture.EncodeToJPG();
            }
            AppManager.appManager.AddNewProject(projNameField.text, clientEmailField.text, passwordField.text, encoded);
        }
    }

    public void Update()
    {
        button.interactable = !string.IsNullOrEmpty(projNameField.text) && !string.IsNullOrEmpty(clientEmailField.text)
            && !string.IsNullOrEmpty(passwordField.text) && passwordField.text == passwordCheckField.text && image.texture != null;

        resetImageButton.gameObject.SetActive(image.texture != null);
    }
}
