using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class Button_User : MonoBehaviour
{
    [SerializeField] TMP_Text nameTextObj;
    [SerializeField] TMP_Text numProjsTextObj;
    [SerializeField] RawImage img;
    User user;
    Texture2D texture;

    private DeletePrompt_ProjectAdmin deletePrompt;

    public void OnClick()
    {
        AppManager.appManager.currUser = user;
        SceneManagement.sceneManagement.LoadScene("ProjectSelect");
    }

    public void OnClickDelete()
    {
        AppManager.appManager.currUser = user;
        deletePrompt.gameObject.SetActive(true);
    }

    public void Initialize(User _user, DeletePrompt_ProjectAdmin _deletePrompt)
    {
        user = _user;
        deletePrompt = _deletePrompt;
        nameTextObj.text = string.Format("{0} {1}", user.firstName, user.lastName);
        numProjsTextObj.text = user.projects == null ? "0" : user.projects.Count.ToString();

        byte[] tempPlan = AppManager.appManager.FinUserById(user.userId).image;
        if (tempPlan.Length > 0)
        {
            img.transform.parent.gameObject.SetActive(true);
            LoadImg(img, tempPlan);
        }
        else
        {
            RestFarm.restFarm.GETIMAGE(AppManager.appManager.sessionId, "/user/image/" + user.userId + "?userId=" + user.userId, OnUserImageRetrieved);
        }
    }

    void OnUserImageRetrieved(UnityWebRequest _www)
    {
        //RestFarm.restFarm.DebugWebRequest(_www);

        if (_www.downloadHandler.data.Length > 32)
        {
            img.transform.parent.gameObject.SetActive(true);
            AppManager.appManager.FinUserById(user.userId).image = _www.downloadHandler.data;
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
