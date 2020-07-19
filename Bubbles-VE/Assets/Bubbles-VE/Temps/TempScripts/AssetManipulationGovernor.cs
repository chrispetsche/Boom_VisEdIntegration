using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManipulationGovernor : MonoBehaviour
{
    public bool canMove;
    public bool canRotate;
    public bool canScale;

    [SerializeField]
    Transform assetRotate_ManipulationPoint;

    public Transform RotationPointHolder()
    {
        return assetRotate_ManipulationPoint;
    }
}
