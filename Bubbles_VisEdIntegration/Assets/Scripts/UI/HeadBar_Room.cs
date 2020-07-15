using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HeadBar_Room : MonoBehaviour
{
    [SerializeField] private TMP_Text title;

    // Start is called before the first frame update
    void Start()
    {
        title.text = AppManager.appManager.FindRoomByID(AppManager.appManager.currProjId, AppManager.appManager.currFloorId, AppManager.appManager.currRoomId).name;
    }
}
