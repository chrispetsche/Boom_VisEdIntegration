using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class Button_Room : MonoBehaviour
{
	[SerializeField] TMP_Text nameTextObj;
	[SerializeField] GameObject noPlanImg;
	[SerializeField] Image planImg;
    public DeletePrompt_Room deletePrompt;
    int roomId;

    Texture2D texture;
    Vector3 position;
    Vector3 scale;

	public void OnClick()
	{
		AppManager.appManager.currRoomId = roomId;
		SceneManagement.sceneManagement.LoadScene("BubbleEditor");
    }

    public void OnClickDelete()
    {
        AppManager.appManager.currRoomId = roomId;
        deletePrompt.gameObject.SetActive(true);
    }

    public void Initialize(Room _room, DeletePrompt_Room _deletePrompt)
	{
		roomId = _room.roomId;
		nameTextObj.text = _room.name;
        deletePrompt = _deletePrompt;

        position = _room.pos;
        scale = _room.scale;

        planImg.transform.parent.gameObject.SetActive(false);

        byte[] tempPlan = AppManager.appManager.FindFloorByID(AppManager.appManager.currProjId, AppManager.appManager.currFloorId).plan;
        if (tempPlan.Length > 0)
        {
            planImg.transform.parent.gameObject.SetActive(true);
            LoadImg(planImg, tempPlan);
            (planImg.transform as RectTransform).anchoredPosition = position;
            planImg.transform.localScale = scale == Vector3.zero ? Vector3.one : scale;
        }
        else
        {
            RestFarm.restFarm.GETIMAGE(AppManager.appManager.sessionId, "/floor/plan/" + AppManager.appManager.currFloorId + "?floorId=" + AppManager.appManager.currFloorId, OnFloorPlanRetrieved);
        }
    }

    void OnFloorPlanRetrieved(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        if (_www.downloadHandler.data.Length > 32)
        {
            planImg.transform.parent.gameObject.SetActive(true);
            AppManager.appManager.FindFloorByID(AppManager.appManager.currProjId, AppManager.appManager.currFloorId).plan = _www.downloadHandler.data;
            LoadImg(planImg, _www.downloadHandler.data);
            (planImg.transform as RectTransform).anchoredPosition = position;
            planImg.transform.localScale = scale;
        }
        else
        {
            planImg.transform.parent.gameObject.SetActive(false);
        }

        _www.Dispose();
    }

    void LoadImg(Image _img, byte[] _texData)
    {
        if (texture == null)
        {
            texture = new Texture2D(2, 2, TextureFormat.RGB24, true, false);
        }
        ImageConversion.LoadImage(texture, _texData);
        _img.enabled = true;
        //_img.GetComponent<AspectRatioFitter>().aspectRatio = (float)texture.width / (float)texture.height;
        _img.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}
