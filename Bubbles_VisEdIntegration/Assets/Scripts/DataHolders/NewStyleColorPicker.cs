using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NewStyleColorPicker : MonoBehaviour
{
	public TMP_Text header;
	public RawImage stylePlan;
	public RawImage colorPlan;
	[Tooltip("Drag in the img upload buttons to deactivate in the case that there is a saved img")]
	public GameObject[] styleButtonsToDisable;
	[Tooltip("Drag in the img upload buttons to deactivate in the case that there is a saved img")]
	public GameObject[] colorButtonsToDisable;
    public Button resetColor;
    public Button resetStyle;
    public UploadImage styleUploader;
    public UploadImage colorUploader;

    private DeletePrompt_Bubble deletePrompt;
    private int bubbleId;

    private Texture2D styleTexture;
    private Texture2D colorTexture;

    private void OnEnable()
    {
        if (styleTexture == null)
        {
            styleUploader.Reset();
        }
        else
        {
            styleUploader.ImgAppear(styleTexture);
        }

        if (colorTexture == null)
        {
            colorUploader.Reset();
        }
        else
        {
            colorUploader.ImgAppear(colorTexture);
        }
    }

    public void Initialize(int _bubbleId, string _header, byte[] _stylePlan, byte[] _colorPlan, DeletePrompt_Bubble _deletePrompt)
    {
        styleUploader.Reset();
        colorUploader.Reset();

        header.text = _header;
        deletePrompt = _deletePrompt;
        bubbleId = _bubbleId;

        if (_stylePlan.Length > 0)
        {
            LoadImg(stylePlan, _stylePlan);
        }
        else
        {
            RestFarm.restFarm.GETIMAGE(AppManager.appManager.sessionId, "/bubble/style/" + bubbleId + "?bubbleId=" + bubbleId, OnBubbleStyleRetrieved);
        }

        if (_colorPlan.Length > 0)
        {
            LoadImg(colorPlan, _colorPlan);
        }
        else
        {
            RestFarm.restFarm.GETIMAGE(AppManager.appManager.sessionId, "/bubble/color/" + bubbleId + "?bubbleId=" + bubbleId, OnBubbleColorRetrieved);
        }

        styleTexture = stylePlan.texture as Texture2D;
        colorTexture = colorPlan.texture as Texture2D;
    }

    void OnBubbleStyleRetrieved(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        if (_www.downloadHandler.data.Length > 32)
        {
            LoadImg(stylePlan, _www.downloadHandler.data);
            for (int i = 0; i < styleButtonsToDisable.Length; i++)
            {
                styleButtonsToDisable[i].SetActive(false);
            }
        }

        styleTexture = stylePlan.texture as Texture2D;

        _www.Dispose();
    }


    void OnBubbleColorRetrieved(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        if (_www.downloadHandler.data.Length > 32)
        {
            AppManager.appManager.FindBubbleByID(AppManager.appManager.currProjId, AppManager.appManager.currFloorId, AppManager.appManager.currRoomId, AppManager.appManager.currBubbleId).colorPlan = _www.downloadHandler.data;
            LoadImg(colorPlan, _www.downloadHandler.data);
            for (int i = 0; i < colorButtonsToDisable.Length; i++)
            {
                colorButtonsToDisable[i].SetActive(false);
            }
        }

        colorTexture = colorPlan.texture as Texture2D;

        _www.Dispose();
    }

    public void OnClickDelete()
    {
        AppManager.appManager.currBubbleId = bubbleId;
        deletePrompt.gameObject.SetActive(true);
    }

    void LoadImg(RawImage _img, byte[] _texData)
	{
		Texture2D temp = new Texture2D(2, 2, TextureFormat.RGB24, true, false);
		ImageConversion.LoadImage(temp, _texData);
		_img.enabled = true;
        _img.GetComponent<AspectRatioFitter>().aspectRatio = (float)temp.width / (float)temp.height;
        _img.texture = temp;
	}

    public void UploadBubbleStyle()
    {
        AppManager.appManager.currBubbleId = bubbleId;

        if (stylePlan.texture == null)
        {
            AppManager.appManager.FindBubbleByID(AppManager.appManager.currProjId, AppManager.appManager.currFloorId, AppManager.appManager.currRoomId, AppManager.appManager.currBubbleId).stylePlan = new byte[0];
            AppManager.appManager.UpdateBubbleStyle(new byte[1]);
        }
        else
        {
            var texture = (Texture2D)stylePlan.texture;
            AppManager.appManager.UpdateBubbleStyle(texture.EncodeToJPG());
        }

        gameObject.SetActive(false);
    }

    public void UploadBubbleColor()
    {
        AppManager.appManager.currBubbleId = bubbleId;

        if (colorPlan.texture == null)
        {
            AppManager.appManager.FindBubbleByID(AppManager.appManager.currProjId, AppManager.appManager.currFloorId, AppManager.appManager.currRoomId, AppManager.appManager.currBubbleId).colorPlan = new byte[0];
            AppManager.appManager.UpdateBubbleColor(new byte[1]);
        }
        else
        {
            var texture = (Texture2D)colorPlan.texture;
            AppManager.appManager.UpdateBubbleColor(texture.EncodeToJPG());
        }

        gameObject.SetActive(false);
    }

    public void ResetStyleImage()
    {
        stylePlan.texture = null;
    }

    public void ResetColorImage()
    {
        colorPlan.texture = null;
    }

    private void Update()
    {
        resetStyle.interactable = stylePlan.texture != null;
        resetColor.interactable = colorPlan.texture != null;
    }
}
