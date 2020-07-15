using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class InputField_Reset : MonoBehaviour
{
    TMP_InputField inputfield;

    // Start is called before the first frame update
    private void OnEnable()
    {
        if (inputfield == null)
        {
            inputfield = GetComponent<TMP_InputField>();
        }

        inputfield.text = string.Empty;
    }
}
