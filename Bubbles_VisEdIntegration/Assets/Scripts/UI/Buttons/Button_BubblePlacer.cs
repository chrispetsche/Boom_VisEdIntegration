using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Button_BubblePlacer : MonoBehaviour
{
	public Transform floorPlanImg;
    public ButtonMaker_RoomEditor roomEditor;
	public TMP_Text nameText;
	/// <summary>
	/// A prefab of the bubble icon that is used to display in the environment.
	/// </summary>
	public Button_Bubble bubbleButtonPrefab;
	/// <summary>
	/// The icon that is displayed on the actual button
	/// </summary>
	public GameObject buttonIcon;
    /// <summary>
    /// The bubbleButton currently attached to the curser
    /// </summary>
    Button_Bubble currBubbleButton;
	bool buttonDepressed = false;

	public Color buttonGreyColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);
	Color buttonStartColor;

	private void Awake()
	{
		if (buttonIcon.GetComponent<Image>())
			buttonStartColor = buttonIcon.GetComponent<Image>().color;

		//Initialize();
	}

	public void OnClick()
	{
		if (!buttonDepressed)
		{
			buttonDepressed = true;
			ToggleIcon(false);
			StartCoroutine(bubblePlacement());
		}
	}

    public void OnPressUp()
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, Input.mousePosition))
        {
            Vector3 localPos = currBubbleButton.GetComponent<RectTransform>().anchoredPosition;
            AppManager.appManager.AddBubble(nameText.text, localPos.x, localPos.y);
            roomEditor.TransferBubble(currBubbleButton);
            currBubbleButton = null;
        }
        else
        {
            Destroy(currBubbleButton);
        }
    }

	private void Update()
	{
		if (buttonDepressed && currBubbleButton)
		{
			Vector3 tempOffset = new Vector3(-10, 10, 0); 
			currBubbleButton.transform.position = Input.mousePosition;
        }
	}

	IEnumerator bubblePlacement()
	{
		AppManager temp = AppManager.appManager;

		currBubbleButton = Instantiate(bubbleButtonPrefab);
        currBubbleButton.transform.SetParent(floorPlanImg.transform, true);

        yield return new WaitUntil(() => { return Input.GetMouseButtonUp(0); });

        ResetButton();
	}

    public void Initialize(string _name, ButtonMaker_RoomEditor _roomEditor, Transform _floorPlanImg)
	{
        floorPlanImg = _floorPlanImg;
        nameText.text = _name;
        roomEditor = _roomEditor;
    }

	private void ResetButton()
	{
		ToggleIcon(true);
		currBubbleButton = null;
		buttonDepressed = false;
	}

	private void OnDisable()
	{
		if (currBubbleButton)
		{
			Destroy(currBubbleButton);
		}
	}

	/// <summary>
	/// Function toggles if the button icon is greyed out or not
	/// </summary>
	void ToggleIcon(bool on)
	{
		if (buttonIcon.GetComponent<Image>())
		{
			Image temp = buttonIcon.GetComponent<Image>();
			if (on)
			{
				temp.color = buttonStartColor;
			}
			else if(!on)
			{
				temp.color = buttonGreyColor;
			}
		}
		else
		{
			Debug.LogWarning("Button Icon does not have an Image component!");
		}
	}
}
