using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class Button_Project : MonoBehaviour
{
	[SerializeField] TMP_Text nameTextObj;
    [SerializeField] RawImage img;
    int projId;
    Texture2D texture;

    private DeletePrompt_Project deletePrompt;

    public void OnSubmit(string value)
    {
        if (string.IsNullOrEmpty(value)) return;
        AppManager.appManager.currProjId = projId;
        AppManager.appManager.UpdateProject(value);
    }

    public void OnClick()
	{
		AppManager.appManager.currProjId = projId;
		SceneManagement.sceneManagement.LoadScene("FloorSelect");
    }

    public void OnClickDelete()
    {
        AppManager.appManager.currProjId = projId;
        deletePrompt.gameObject.SetActive(true);
    }

    public void Initialize(Project _project, DeletePrompt_Project _deletePrompt)
	{
		projId = _project.projectId;
        deletePrompt = _deletePrompt;
		nameTextObj.text = _project.name;

        byte[] tempPlan = AppManager.appManager.FindProjectByID(projId).plan;
        if (tempPlan.Length > 0)
        {
            img.transform.parent.gameObject.SetActive(true);
            LoadImg(img, tempPlan);
        }
        else
        {
            RestFarm.restFarm.GETIMAGE(AppManager.appManager.sessionId, "/project/image/" + projId + "?projectId=" + projId, OnProjectImageRetrieved);
        }
    }

    void OnProjectImageRetrieved(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        if (_www.downloadHandler.data.Length > 32)
        {
            img.transform.parent.gameObject.SetActive(true);
            AppManager.appManager.FindProjectByID(projId).plan = _www.downloadHandler.data;
            LoadImg(img, _www.downloadHandler.data);
        }
        else
        {
            img.transform.parent.gameObject.SetActive(false);
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
