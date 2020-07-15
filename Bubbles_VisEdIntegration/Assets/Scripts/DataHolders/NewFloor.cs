using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewFloor : MonoBehaviour
{
	public TMP_Dropdown floorNameField;
	public RawImage image;
	public GameObject[] imgButtonsToReset;

    public Button button;

    private void OnEnable()
    {
        floorNameField.value = 0;
        image.texture = null;
    }

    public void OnClick()
    {
        if (string.IsNullOrEmpty(floorNameField.options[floorNameField.value].text))
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
            AppManager.appManager.AddFloor(floorNameField.options[floorNameField.value].text, encoded);
        }
    }

    public void Update()
    {
        button.interactable = !string.IsNullOrEmpty(floorNameField.options[floorNameField.value].text);
    }
}
