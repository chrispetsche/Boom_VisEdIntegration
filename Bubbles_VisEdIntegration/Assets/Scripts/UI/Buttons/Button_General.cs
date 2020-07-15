using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The purpose of this script is to provide basic functionality that common buttons will need. Then the same script can be tossed on most misc buttons.

public class Button_General : MonoBehaviour
{
    public void LoadScene(string _sceneName)
	{
		if (_sceneName != SceneManagement.sceneManagement.GetCurrScene().name)
			SceneManagement.sceneManagement.LoadScene(_sceneName);
	}

	public void LoadScene(int _sceneIndex)
	{
		if (_sceneIndex != SceneManagement.sceneManagement.sceneCurrActive)
			SceneManagement.sceneManagement.LoadScene(_sceneIndex);
	}

	public void TurnOffParentHolderObj()
	{
		if (transform.GetComponentInParent<Tag_UIHolder>())
		{
			transform.GetComponentInParent<Tag_UIHolder>().gameObject.SetActive(false);
		}
	}
}
