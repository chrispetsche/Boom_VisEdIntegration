
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Button_Bubble : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
	public GameObject bubblePopUpPrefab;
	GameObject PopUp;
	public Bubble bubbleData;
    public RawImage styleImg;
    public RawImage colorImg;

    private Texture2D colorTexture;
    private Texture2D styleTexture;

    public void Initialize(Bubble _bubbleData, DeletePrompt_Bubble _deletePrompt)
	{
		bubbleData = _bubbleData;

        GetComponent<RectTransform>().anchoredPosition = bubbleData.pos;

        AppManager temp = AppManager.appManager;
        if (PopUp == null)
        {
            PopUp = Instantiate(bubblePopUpPrefab, temp.currUIManager.transform.position, Quaternion.identity, temp.currUIManager.transform.transform);
            PopUp.SetActive(false);
        }

        if (bubbleData.stylePlan.Length > 0)
        {
            LoadImg(styleImg, styleTexture, bubbleData.stylePlan);
        }
        else
        {
            RestFarm.restFarm.GETIMAGE(AppManager.appManager.sessionId, "/bubble/style/" + bubbleData.bubbleId + "?bubbleId=" + bubbleData.bubbleId, OnBubbleStyleRetrieved);
        }

        if (bubbleData.colorPlan.Length > 0)
        {
            LoadImg(colorImg, colorTexture, bubbleData.stylePlan);
        }
        else
        {
            RestFarm.restFarm.GETIMAGE(AppManager.appManager.sessionId, "/bubble/color/" + bubbleData.bubbleId + "?bubbleId=" + bubbleData.bubbleId, OnBubbleColorRetrieved);
        }

        string header = bubbleData.name;

		PopUp.GetComponent<NewStyleColorPicker>().Initialize(bubbleData.bubbleId, header, bubbleData.stylePlan, bubbleData.colorPlan, _deletePrompt); // populate it with data from bubble class
    }

    void OnBubbleStyleRetrieved(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        if (_www.downloadHandler.data.Length > 32)
        {
            AppManager.appManager.FindBubbleByID(AppManager.appManager.currProjId, AppManager.appManager.currFloorId, AppManager.appManager.currRoomId, AppManager.appManager.currBubbleId).stylePlan = _www.downloadHandler.data;
            LoadImg(styleImg, styleTexture, _www.downloadHandler.data);
        }
        else
        {
            LoadImg(styleImg, styleTexture, null);
        }

        _www.Dispose();
    }

    void OnBubbleColorRetrieved(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        if (_www.downloadHandler.data.Length > 32)
        {
            AppManager.appManager.FindBubbleByID(AppManager.appManager.currProjId, AppManager.appManager.currFloorId, AppManager.appManager.currRoomId, AppManager.appManager.currBubbleId).colorPlan = _www.downloadHandler.data;
            LoadImg(colorImg, colorTexture, _www.downloadHandler.data);
        }
        else
        {
            LoadImg(colorImg, colorTexture, null);
        }

        _www.Dispose();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        AppManager.appManager.currBubbleId = bubbleData.bubbleId;
        var pos = GetComponent<RectTransform>().anchoredPosition;
        AppManager.appManager.UpdateBubble(bubbleData.name, pos.x, pos.y);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        PopUp.SetActive(true);
        AppManager.appManager.currBubbleId = bubbleData.bubbleId;
    }

    void LoadImg(RawImage _img, Texture2D _texture, byte[] _texData)
    {
        if (_texData == null)
        {
            _img.texture = null;
        }
        else
        {
            if (_texture == null)
            {
                _texture = new Texture2D(2, 2, TextureFormat.RGB24, true, false);
            }

            ImageConversion.LoadImage(_texture, _texData);
            _img.enabled = true;
            _img.GetComponent<AspectRatioFitter>().aspectRatio = (float)_texture.width / (float)_texture.height;
            _img.texture = _texture;
        }
    }
}
