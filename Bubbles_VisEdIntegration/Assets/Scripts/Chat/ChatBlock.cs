using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatBlock : MonoBehaviour
{
	public TMP_Text body;
	public TMP_Text initial;

    public RawImage img;

	public void Initialize(string _message, char _initial)
	{
		body.text = _message;
		initial.text = _initial.ToString();
	}
}
