using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBar_Floor : MonoBehaviour
{
    [Tooltip("Only set for go back buttons based on permissions")]
    [SerializeField] private GameObject goBack;

    // Start is called before the first frame update
    void Start()
    {
        if (goBack)
        {
            goBack.SetActive(AppManager.appManager.mainUser.userType == UserType.super_admin || 
                AppManager.appManager.mainUser.userType == UserType.project_admin);
        }
    }
}
