using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePrompt_Room : MonoBehaviour
{
    public void OnClick()
    {
        AppManager.appManager.DeleteRoom();
    }
}
