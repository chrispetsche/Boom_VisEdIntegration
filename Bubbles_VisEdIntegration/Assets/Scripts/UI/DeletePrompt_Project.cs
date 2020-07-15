using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePrompt_Project : MonoBehaviour
{
    public void OnClick()
    {
        AppManager.appManager.DeleteProject();
    }
}
