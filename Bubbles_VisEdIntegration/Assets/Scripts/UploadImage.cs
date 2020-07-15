using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using TMPro;

public class UploadImage : MonoBehaviour
{
	public enum SaveImgType { none = 0, projectPlan = 1, floorPlan = 2, bubbleColor = 3, bubbleStyle = 4 }

    public TMP_InputField urlField;
	[Tooltip("The image component you'd like the uploaded image apear on.")]
	public RawImage image;
	[Tooltip("UI elements to turn off when image apears.")]
	public GameObject[] uiToDisable;
	[Tooltip("This texture will be placed into the image component when the Button_TestImage method is called.")]
	public Texture2D testTexture;
	public SaveImgType saveImgType;

	public void Button_RequestCameraAndSave()
	{
		if (NativeCamera.CheckPermission() != NativeCamera.Permission.Granted)
		{
			Debug.LogWarning("Permission to Camera not granted!");
			return;
		}

		NativeCamera.TakePicture(CamDisplayAndSave);
	}

	void CamDisplayAndSave(string _s)
	{
		if (_s != null && _s.Length > 0)
		{
			Texture2D temp = NativeCamera.LoadImageAtPath(_s, -1, false);
			ImgAppear(temp);
			//SaveImg(saveImgType, temp);
		}
		else
		{
			Debug.LogWarning("Image path returned a null string!");
		}
	}

	#region Upload Image from Gallery

	public void Button_RequestImage()
	{
		if (NativeGallery.CheckPermission() != NativeGallery.Permission.Granted)
		{
			Debug.LogWarning("Permission to gallery not granted!");
			return;
		}
		
		NativeGallery.GetImageFromGallery(DisplayOnly);
	}

	public void Button_RequestImageAndSave()
	{
		if (NativeGallery.CheckPermission() != NativeGallery.Permission.Granted)
		{
			Debug.LogWarning("Permission to gallery not granted!");
			return;
		}

		NativeGallery.GetImageFromGallery(DisplayAndSave);
	}

	void DisplayOnly(string _s)
	{
		if (_s != null && _s.Length > 0)
		{
			Texture2D temp = NativeGallery.LoadImageAtPath(_s, -1, false);
			ImgAppear(temp);
		}
		else
		{
			Debug.LogWarning("Image path returned a null string!");
		}
	}

	void DisplayAndSave(string _s)
	{
		if (_s != null && _s.Length > 0)
		{
			Texture2D temp = NativeGallery.LoadImageAtPath(_s, -1, false);
			ImgAppear(temp);
			//SaveImg(saveImgType, temp);
		}
		else
		{
			Debug.LogWarning("Image path returned a null string!");
		}
	}

	public void Button_TestImage()
	{
		ImgAppear(testTexture);
	} // For Testing

	public void Button_TestImageAndSave()
	{
		ImgAppear(testTexture);
		//SaveImg(saveImgType, testTexture);
	} // For Testing

	#endregion

    public void Button_RequestUrl()
    {
        for (int i = 0; i < uiToDisable.Length; i++)
        {
            uiToDisable[i].SetActive(false);
        }
        urlField.text = string.Empty;
        urlField.gameObject.SetActive(true);
    }

    public void DownloadImageFromUrl(string url)
    {
        urlField.gameObject.SetActive(false);

        RestFarm.restFarm.GETIMAGE(url, OnImageDownloaded);
    }

    void OnImageDownloaded(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        var texture = new Texture2D(2, 2);
        texture.LoadImage(_www.downloadHandler.data);

        if (texture.IsValidTexture())
        {
            Destroy(texture);
            // Look trought meta data for preview image, if set up correctly
            var data = _www.downloadHandler.text;
            if (data != null)
            {
                var replaced = data.Replace("\"", "");
                var lookup = "og:image content=";
                var startingIndex = replaced.IndexOf(lookup);
                if (startingIndex != -1)
                {
                    startingIndex += lookup.Length;
                    var endingIndex = replaced.IndexOf(" ", startingIndex);
                    if (endingIndex != -1)
                    {
                        var imageUrl = replaced.Substring(startingIndex, endingIndex - startingIndex);
                        Debug.Log(imageUrl);
                        RestFarm.restFarm.GETIMAGE(imageUrl, OnImageDownloaded);
                    }
                    else
                    {
                        Reset();
                    }
                }
                else
                {
                    Reset();
                }
            }
            else
            {
                Reset();
            }
        }
        else
        {
            ImgAppear(texture);
        }

        _www.Dispose();
    }

    public void ImgAppear(Texture2D _texture)
	{
		if (image == null)
			return;

        image.enabled = true;
        image.GetComponent<AspectRatioFitter>().aspectRatio = (float)_texture.width / (float)_texture.height;
        image.texture = _texture;
		
		for (int i = 0; i < uiToDisable.Length; i++)
		{
			uiToDisable[i].SetActive(false);
		}
	}

	void SaveImg(SaveImgType _saveImgType, Texture2D _texture)
	{
		byte[] img;

		try {
			img = _texture.EncodeToJPG();
		}
		catch (Exception e) {
			Debug.LogWarning("Failed to encode img. " + e.Message);
			return;
		}

		AppManager temp = AppManager.appManager;
		
		switch (_saveImgType)
		{
			case SaveImgType.none:
				break;
			case SaveImgType.projectPlan:
				temp.FindProjectByID(temp.currProjId).plan = img;
				break;
			case SaveImgType.floorPlan:
				temp.FindFloorByID(temp.currProjId, temp.currFloorId).plan = img;
				break;
			case SaveImgType.bubbleColor:
				temp.FindBubbleByID(temp.currProjId, temp.currFloorId, temp.currRoomId, temp.currBubbleId).colorPlan = img;
				break;
			case SaveImgType.bubbleStyle:
				temp.FindBubbleByID(temp.currProjId, temp.currFloorId, temp.currRoomId, temp.currBubbleId).stylePlan = img;
				break;
		}
	}

    public void Reset()
    {
        image.enabled = false;
        image.texture = null;
        urlField.gameObject.SetActive(false);

        for (int i = 0; i < uiToDisable.Length; i++)
        {
            uiToDisable[i].SetActive(true);
        }
    }
}