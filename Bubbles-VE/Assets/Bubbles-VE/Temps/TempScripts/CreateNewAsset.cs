using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewAsset : MonoBehaviour
{
    [SerializeField]
    GameObject assetToCreate;
    [SerializeField]
    Camera cam;

    ObjectSelectionManager selectionMananger;

    private void Start()
    {
        selectionMananger = GetComponent<ObjectSelectionManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Create();
        }
    }

    public void Create()
    {
        // Set the position the corner point is to be created at.
        Vector3 createPos = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 10.0f));
        // Create the point at the location.
        var obj = (GameObject)Instantiate(assetToCreate, createPos, Quaternion.identity);
    }
}
