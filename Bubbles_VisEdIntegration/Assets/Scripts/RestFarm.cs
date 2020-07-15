using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RestFarm : MonoBehaviour
{
	public static RestFarm restFarm;

#if UNITY_EDITOR
    private const string serverAddress = "http://192.168.0.50:8080/api/v1";
#else
    private const string serverAddress = "http://3.17.186.18:8080/api/v1";
#endif

    private void Awake()
	{
#region static instance managment
		restFarm = this;
		if (restFarm == this)
		{
			return;
		}
		else
		{
			Destroy(this);
		}
		Destroy(this);
#endregion
	}
	
	/// <summary>
	/// Prints tons of debug info on a Web Request
	/// </summary>
	public void DebugWebRequest(UnityWebRequest _www)
	{
		print("__DEBUG WEB REQUEST__");
		print("Handler Text: " + _www.downloadHandler.text);
		print("isDone: " + _www.isDone);
		print("error: " + _www.error);
		print("Downloaded Bytes: " + _www.downloadedBytes);
		print("Uploaded Bytes: " + _www.uploadedBytes);
		print("Response Code: " + _www.responseCode);
	}

	/// <summary>
	/// Plug this function in as the "OnComplete" argument in rest calls for easy debugging.
	/// </summary>
	public void DebugOnComplete(UnityWebRequest _www)
	{
		print("Web Request Completed");
		DebugWebRequest(_www);
	}

	// The way these request functions are scructured is that they take in a URL and a method.
	// The URL is the API call, and the Method gets called once the web request is complete.
	// The successful web request is pass through to the onComplete function so that the returned JSON can be accessed.
	public UnityWebRequest Login(string _email, string _password, System.Action<UnityWebRequest> _onComplete)
    {
        Dictionary<string, string> formData = new Dictionary<string, string>();
        formData.Add("email", _email);
        formData.Add("password", _password);

        UnityWebRequest temp = UnityWebRequest.Post(serverAddress + "/login", formData);
        Timing.RunCoroutine(WaitForRequest(temp, _onComplete));
		return temp;
	}

    public UnityWebRequest POST(string _sessionId, string _urlPath, Dictionary<string, string> _formData, System.Action<UnityWebRequest> _onComplete)
    {
        UnityWebRequest temp = UnityWebRequest.Post(serverAddress + _urlPath, _formData);
        temp.SetRequestHeader("X-Session-Id", _sessionId);
        Timing.RunCoroutine(WaitForRequest(temp, _onComplete));
        return temp;
    }

    public UnityWebRequest DELETE(string _sessionId, string _urlPath, System.Action<UnityWebRequest> _onComplete)
    {
        UnityWebRequest temp = UnityWebRequest.Delete(serverAddress + _urlPath);
        temp.SetRequestHeader("X-Session-Id", _sessionId);
        Timing.RunCoroutine(WaitForRequest(temp, _onComplete));
        return temp;
    }

    public UnityWebRequest GETSINGLE(string _sessionId, string _urlPath, System.Action<UnityWebRequest> _onComplete)
    {
        UnityWebRequest temp = UnityWebRequest.Get(serverAddress + _urlPath);
        temp.SetRequestHeader("X-Session-Id", _sessionId);
        Timing.RunCoroutine(WaitForRequest(temp, _onComplete));
        return temp;
    }

    public UnityWebRequest GETALL(string _sessionId, string _urlPath, System.Action<UnityWebRequest> _onComplete)
    {
        UnityWebRequest temp = UnityWebRequest.Get(serverAddress + _urlPath);
        temp.SetRequestHeader("X-Session-Id", _sessionId);
        Timing.RunCoroutine(WaitForRequest(temp, _onComplete));
        return temp;
    }

    public UnityWebRequest PUT(string _sessionId, string _urlPath, string newName, System.Action<UnityWebRequest> _onComplete)
    {
        UnityWebRequest temp = UnityWebRequest.Put(serverAddress + _urlPath, newName);
        temp.SetRequestHeader("X-Session-Id", _sessionId);
        Timing.RunCoroutine(WaitForRequest(temp, _onComplete));
        return temp;
    }

    public UnityWebRequest GETIMAGE(string _sessionId, string _urlPath, System.Action<UnityWebRequest> _onComplete)
    {
        UnityWebRequest temp = UnityWebRequest.Get(serverAddress + _urlPath);
        temp.downloadHandler = new DownloadHandlerBuffer();
        temp.SetRequestHeader("X-Session-Id", _sessionId);
        Timing.RunCoroutine(WaitForRequest(temp, _onComplete));
        return temp;
    }

    public UnityWebRequest GETIMAGE(string _url, System.Action<UnityWebRequest> _onComplete)
    {
        UnityWebRequest temp = UnityWebRequestTexture.GetTexture(_url);
        temp.downloadHandler = new DownloadHandlerBuffer();
        Timing.RunCoroutine(WaitForRequest(temp, _onComplete));
        return temp;
    }

    public UnityWebRequest POSTIMAGE(string _sessionId, string _urlPath, Dictionary<string, string> _formData, byte[] _data, System.Action<UnityWebRequest> _onComplete)
    {
        UnityWebRequest temp = UnityWebRequest.Post(serverAddress + _urlPath, _formData);
        var uploder = new UploadHandlerRaw(_data);
        temp.uploadHandler = uploder;
        temp.SetRequestHeader("X-Session-Id", _sessionId);
        Timing.RunCoroutine(WaitForRequest(temp, _onComplete));
        return temp;
    }

	private IEnumerator<float> WaitForRequest(UnityWebRequest _www, System.Action<UnityWebRequest> _onComplete)
	{
		yield return Timing.WaitUntilDone(_www.SendWebRequest());

		if (_www.error != null || _www.isNetworkError)
		{
			Debug.LogWarning(_www.error);
			DebugWebRequest(_www);
            _onComplete.Invoke(_www);
        }
		else
		{
			_onComplete.Invoke(_www);
		}
	}
}
