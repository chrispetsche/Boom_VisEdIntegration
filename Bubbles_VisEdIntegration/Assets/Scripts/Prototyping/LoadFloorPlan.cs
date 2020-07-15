using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadFloorPlan : MonoBehaviour
{
	public Image image;

	private void OnEnable()
	{
        byte[] tempPlan = AppManager.appManager.FindFloorByID(AppManager.appManager.currProjId, AppManager.appManager.currFloorId).plan;
        if (tempPlan.Length > 0)
        {
            LoadImg(image, tempPlan);
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
            AppManager.appManager.FindFloorByID(AppManager.appManager.currProjId, AppManager.appManager.currFloorId).plan = _www.downloadHandler.data;
            LoadImg(image, _www.downloadHandler.data);
        }

        _www.Dispose();
    }

    void LoadImg(Image _img, byte[] _texData)
	{
		Texture2D temp = new Texture2D(2, 2, TextureFormat.RGB24, true, false);
		ImageConversion.LoadImage(temp, _texData);
		_img.enabled = true;
		_img.preserveAspect = true;
		_img.sprite = Sprite.Create(temp, new Rect(0, 0, temp.width, temp.height), Vector2.zero);
	}
}
