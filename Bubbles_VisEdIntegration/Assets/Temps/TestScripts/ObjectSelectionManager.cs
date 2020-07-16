using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelectionManager : MonoBehaviour
{
    [SerializeField]
    GameObject objectCurrentlySelected;
    ClickToSelectTest objectInteractionScript;

    [SerializeField]
    bool currentObjectActive;


    void Start()
    {
        objectCurrentlySelected = null;
        currentObjectActive = false;
    }

    void Update()
    {
        if (objectCurrentlySelected != null)
        {
            if (Input.GetMouseButtonDown(0) && !currentObjectActive)
            {
                objectInteractionScript.UnselectThisObject();
                objectCurrentlySelected = null;
            }
        }
    }

    public void SetNewObjectAsSelected(GameObject newObject)
    {
        objectCurrentlySelected = newObject;
        objectInteractionScript = objectCurrentlySelected.GetComponent<ClickToSelectTest>();
    }

    public void SelectedObjectIsActive(bool active)
    {
        currentObjectActive = active;
    }
}
