using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPlanManipulation : MonoBehaviour
{

	#region zoom

	public float minZoom = 2;
	public float maxZoom = 40;

	public void DragZoom()
	{
		if (Input.touchCount < 2)
			return;

		Vector3 pos1 = Input.GetTouch(0).position;
		Vector3 pos2 = Input.GetTouch(1).position;
		Vector3 pos1b = Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition;
		Vector3 pos2b = Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition;

		float zoom = Vector3.Distance(pos1, pos2) / Vector3.Distance(pos1b, pos2b);

		GetComponent<RectTransform>().localScale = transform.localScale * zoom;
		ClampScale();
	}

	void ClampScale()
	{
		RectTransform rectTrans = GetComponent<RectTransform>();
		rectTrans.localScale = new Vector3(Mathf.Clamp(rectTrans.localScale.x, minZoom, maxZoom), Mathf.Clamp(rectTrans.localScale.y, minZoom, maxZoom), Mathf.Clamp(rectTrans.localScale.z, minZoom, maxZoom));
	}

	#endregion

	#region Pan

	Vector3 uiPointerOffset;

	public void BeginDrag()
	{
		Vector3 uiViewportPos = Camera.main.ScreenToViewportPoint(transform.position);
		Vector3 pointerViewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		uiPointerOffset = uiViewportPos - pointerViewportPos;
	}

	public void Drag()
	{
		Vector3 pointerViewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		transform.position = Camera.main.ViewportToScreenPoint(pointerViewportPos + uiPointerOffset);
		ClampPos();
	}

	public float minPanX;
	public float maxPanX;
	public float minPanY;
	public float maxPanY;

	void ClampPos()
	{
		RectTransform rectTrans = GetComponent<RectTransform>();
		rectTrans.anchoredPosition = new Vector2(Mathf.Clamp(rectTrans.anchoredPosition.x, minPanX, maxPanX), Mathf.Clamp(rectTrans.anchoredPosition.y, minPanY, maxPanY));
	} // The clamping of position is being done rigidly right now, with input public values. This will have to become a more dynamic system later, one that can adjust what the min/max is based off the current size of the image.

	#endregion

	public void Save()
	{
        //Save the new position and such in Room Data. ??
        RectTransform rectTrans = GetComponent<RectTransform>();
        var room = AppManager.appManager.FindRoomByID(AppManager.appManager.currProjId, AppManager.appManager.currFloorId ,AppManager.appManager.currRoomId);
        AppManager.appManager.UpdateRoom(room.name, rectTrans);
    }
}