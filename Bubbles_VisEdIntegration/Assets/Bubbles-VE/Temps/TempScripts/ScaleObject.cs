using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObject : MonoBehaviour
{
    [SerializeField]
    enum ScalableDirections
    {
        ONE_WAY,
        TWO_WAY,
        THREE_WAY
    };
    [SerializeField]
    ScalableDirections scalableDirections = ScalableDirections.ONE_WAY;
     
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
