using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProjChatMessageCounter : MonoBehaviour
{
	public TextMeshProUGUI messageCounterText;

	private void OnEnable()
	{
		AppManager temp = AppManager.appManager;
		messageCounterText.text = temp.FindProjectByID(temp.currProjId).chatHistory.Count.ToString();
	}
}
