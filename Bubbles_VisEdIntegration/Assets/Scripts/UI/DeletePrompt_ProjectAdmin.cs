using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePrompt_ProjectAdmin : MonoBehaviour
{
    public void OnClick()
    {
        AppManager.appManager.DeleteUser();
    }
}
