using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SimpleJSON;

public struct ChatBlockData
{
	public string message;
	public bool isResponse;
}

public class ChatManager : MonoBehaviour
{
#if UNITY_EDITOR
    private const string socketAddress = "ws://192.168.0.50:8080/api/v1/chat";
#else
    private const string socketAddress = "ws://3.17.186.18:8080/api/v1/chat";
#endif

    public enum ChatType { project, bubble_color, bubble_style };

    [Tooltip("The parent Grid Layout Group obj.")]
    public GameObject gridParent;
    [Tooltip("Used mostly for initially filling chat and saving the messages")]
    public ChatType chatType;
    public ChatBlock chatBlockPrefab;
    public ChatBlock chatBlockPrefab_Response;
    public TMP_InputField inputField;
    public GameObject projChatButton;
    public TMP_Text projChatTitle;
    public GameObject parent;
    public TMP_Text notification;

    private WebSocket websocket;

    private List<ChatBlock> messages = new List<ChatBlock>();
    private int messageIndex;

    private string key;
    private int savedViewMessage = 0;

    private void OnEnable()
    {
        messageIndex = 0;
        StartCoroutine(ConnectToSocket());
    }

    private void OnDisable()
    {
        Disconnect();
    }

    private IEnumerator ConnectToSocket()
    {
        websocket = new WebSocket(new System.Uri(socketAddress));
        yield return StartCoroutine(websocket.Connect());

        int resourceId = -1;
        switch (chatType)
        {
            case ChatType.project:
                projChatTitle.text = AppManager.appManager.FindProjectByID(AppManager.appManager.currProjId).name;
                resourceId = AppManager.appManager.currProjId;
                break;
            case ChatType.bubble_color:
                resourceId = AppManager.appManager.currBubbleId;
                break;
            case ChatType.bubble_style:
                resourceId = AppManager.appManager.currBubbleId;
                break;
            default:
                break;
        }

        key = AppManager.appManager.mainUser.userId + "_" + chatType.ToString() + "_" + resourceId;
        savedViewMessage = PlayerPrefs.GetInt(key);
        ConnectToResource(AppManager.appManager.sessionId, chatType.ToString(), resourceId);
    }

    private void Disconnect()
    {
        if (websocket != null && websocket.IsConnected)
        {
            websocket.Close();
        }
    }

    private void ConnectToResource(string _sessionId, string _resourceType, int _resourceId)
    {
        var node = JSON.Parse("{}");
        node.Add("sessionId", _sessionId);
        node.Add("resource", _resourceType);
        node.Add("resourceId", _resourceId);
        if (websocket != null)
        {
            websocket.SendString(node.ToString());
        }
    }

    public void SendMessageToResource()
    {
        if (inputField.text.Length == 0)
            return;

        string message = inputField.text;
        inputField.text = "";

        if (websocket != null)
        {
            websocket.SendString(message);
        }
    }

    private void Update()
    {
        if (websocket != null)
        {
            if (websocket.error != null)
            {
                Debug.LogError(websocket.error);
                return;
            }
            if (websocket.IsConnected)
            {

                if (parent.activeSelf)
                {
                    PlayerPrefs.SetInt(key, messageIndex);
                    savedViewMessage = messageIndex;
                }

                var response = websocket.RecvString();
                if (!string.IsNullOrEmpty(response))
                {
                    //Debug.Log(response);

                    ParseMessageFromResource(response);
                }

                if (notification != null)
                {
                    notification.transform.parent.gameObject.SetActive(messageIndex > savedViewMessage);
                    notification.text = (messageIndex - savedViewMessage).ToString();
                }
            }
        }
    }

    private void ParseMessageFromResource(string _message)
    {
        if (_message == "ok")
        {
            return;
        }
        else
        {
            var node = JSON.Parse(_message);
            var username = node["name"].Value;
            var payload = node["payload"].Value;

            LoadChatBlock(payload, username);
        }
    }

    void LoadChatBlock(string _message, string _username)
    {
        char initial = _username[0];
        var isResponse = initial == AppManager.appManager.currUser.firstName[0];

        var block = GetBlock(isResponse);
        block.Initialize(_message, initial);
    }

    private ChatBlock GetBlock(bool isResponse)
    {
        ChatBlock newBlock;
        if (messageIndex < messages.Count)
        {
            newBlock = messages[messageIndex];
        }
        else
        {
            newBlock = Instantiate(isResponse ? chatBlockPrefab_Response : chatBlockPrefab, gridParent.transform);
            messages.Add(newBlock);
        }

        messageIndex++;
        return newBlock;
    }
}
