using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using MEC;

public class Button_Floor : MonoBehaviour
{
	[SerializeField] TMP_Text nameTextObj;
	[SerializeField] GameObject noPlanImg;
	[SerializeField] RawImage planImg;
    public DeletePrompt_Floor deletePrompt;
    int floorId;

    Texture2D texture;

    private void OnDestroy()
    {
        Destroy(texture);
    }

    public void OnClick()
	{
		AppManager.appManager.currFloorId = floorId;
		SceneManagement.sceneManagement.LoadScene("RoomSelect");
    }

    public void OnClickDelete()
    {
        AppManager.appManager.currFloorId = floorId;
        deletePrompt.gameObject.SetActive(true);
    }

    public void Initialize(Floor _floor, DeletePrompt_Floor _deletePrompt)
	{
		floorId = _floor.floorId;
		nameTextObj.text = _floor.name;
        deletePrompt = _deletePrompt;

        byte[] tempPlan = AppManager.appManager.FindFloorByID(AppManager.appManager.currProjId, floorId).plan;
        if (tempPlan.Length > 0)
        {
            planImg.transform.parent.gameObject.SetActive(true);
            LoadImg(planImg, tempPlan);
        }
        else
        {
            RestFarm.restFarm.GETIMAGE(AppManager.appManager.sessionId, "/floor/plan/" + floorId + "?floorId=" + floorId, OnFloorPlanRetrieved);
        }
	}

    void OnFloorPlanRetrieved(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        if (_www.downloadHandler.data.Length > 32)
        {
            planImg.transform.parent.gameObject.SetActive(true);
            AppManager.appManager.FindFloorByID(AppManager.appManager.currProjId, floorId).plan = _www.downloadHandler.data;
            LoadImg(planImg, _www.downloadHandler.data);
        }
        else
        {
            planImg.transform.parent.gameObject.SetActive(false);
        }

        _www.Dispose();
    }

	void LoadImg(RawImage _img, byte[] _texData)
	{
        if (texture == null)
        {
            texture = new Texture2D(2, 2, TextureFormat.RGB24, true, false);
        }

		ImageConversion.LoadImage(texture, _texData);
		_img.enabled = true;
        _img.GetComponent<AspectRatioFitter>().aspectRatio = (float)texture.width / (float)texture.height;
        _img.texture = texture;
	}
}
