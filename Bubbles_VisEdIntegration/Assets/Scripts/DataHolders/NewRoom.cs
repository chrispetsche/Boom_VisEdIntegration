using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NewRoom : MonoBehaviour
{
    public TMP_Dropdown roomDropdown;
    public TMP_InputField roomNameField;

    public void OnClick()
    {
        AppManager.appManager.AddRoom(string.IsNullOrEmpty(roomNameField.text) ? roomDropdown.options[roomDropdown.value].text : roomNameField.text);
    }
}
