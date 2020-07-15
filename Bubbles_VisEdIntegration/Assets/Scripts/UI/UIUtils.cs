using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUtils : MonoBehaviour
{
    private static UIUtils instance;

    [SerializeField] private Color buttton_gray;
    [SerializeField] private Color button_blue;

    public static Color BUTTON_GRAY {
        get { return instance.buttton_gray; }
    }

    public static Color BUTTON_BLUE {
        get { return instance.button_blue; }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }
}