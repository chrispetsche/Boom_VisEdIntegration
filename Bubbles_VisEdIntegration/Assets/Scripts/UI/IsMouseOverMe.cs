using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class IsMouseOverMe : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public bool isOver = false;

	public void OnPointerEnter(PointerEventData eventData)
	{
		isOver = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isOver = false;
	}

#if UNITY_IOS || UNITY_ANDROID

	public void OnPointerEnter(TouchInputModule.MouseButtonEventData eventData)
	{
		isOver = true;
	}

	public void OnPointerExit(TouchInputModule.MouseButtonEventData eventData)
	{
		isOver = false;
	}
#endif
}