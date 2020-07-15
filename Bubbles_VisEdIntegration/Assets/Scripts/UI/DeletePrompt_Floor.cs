using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePrompt_Floor : MonoBehaviour
{
    public void OnClick()
    {
        AppManager.appManager.DeleteFloor();
    }
}
