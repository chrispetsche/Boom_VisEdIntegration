using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhatIsBubbles : MonoBehaviour
{
    int slideCount;

    [SerializeField]
    GameObject [] wibImageArray;

    [SerializeField]
    GameObject [] wibTextArray;

    [SerializeField]
    Transform toggleDot;

    [SerializeField]
    Transform[] toggleDotMarkerArray;

    //!!! Add in the TMPro late to change button text !!!/
    

    public void OpenWIB_FTUXPanel()
    {
        Debug.Log(slideCount);
        for (int i = 0; i < wibImageArray.Length; i++)
        {
            for (int t = 0; t < wibTextArray.Length; t++)
            {
                wibImageArray[i].SetActive(false);
                wibImageArray[slideCount].SetActive(true);

                wibTextArray[t].SetActive(false);
                wibTextArray[slideCount].SetActive(true);

                toggleDot.position = toggleDotMarkerArray[0].position;
            }
        }
        // Call to animate white wash background fade in
        // Call to animate display panel to animate sliding in from right
    }

    public void CloseWIB_FTUXPanel()
    {
        slideCount = 0;
        // Change 'Next Button' text back to say 'Next'
        // Call to animate display panel sliding off screen to right
        // Call to animate images animate back to slide 1
        for (int i = 0; i < wibImageArray.Length; i++)
        {
            for (int t = 0; t < wibTextArray.Length; t++)
            {
                wibImageArray[i].SetActive(false);
                wibImageArray[slideCount].SetActive(true);

                wibTextArray[t].SetActive(false);
                wibTextArray[slideCount].SetActive(true);

                toggleDot.position = toggleDotMarkerArray[0].position;
            }
        }
        // Call to reset text display to slide 1
        // Call to reset Toggle Dot
        toggleDot.position = toggleDotMarkerArray[0].position;

        this.gameObject.SetActive(false);
    }

    public void NextSlide()
    {
        if (slideCount < 2)
        {
            slideCount++;

            // Call Image Animation and pass it the slide count
            wibImageArray[slideCount].SetActive(true);
            wibImageArray[slideCount - 1].SetActive(false);

            // Call Text animation to fade text and pass it the slide count
            wibTextArray[slideCount].SetActive(true);
            wibTextArray[slideCount - 1].SetActive(false);

            // Call to animate Toggle Dot to the next marker
            toggleDot.position = toggleDotMarkerArray[slideCount].position;
        }

        else if (slideCount  == 2)
        {
            slideCount++;

            // Change 'Next Button' text to say 'Done'
            

            // Call Image Animation and pass it the slide count
            wibImageArray[slideCount].SetActive(true);
            wibImageArray[slideCount - 1].SetActive(false);

            // Call Text animation to fade text and pass it the slide count
            wibTextArray[slideCount].SetActive(true);
            wibTextArray[slideCount - 1].SetActive(false);

            // Call to animate Toggle Dot to the next marker
            toggleDot.position = toggleDotMarkerArray[slideCount].position;
        }

        else if (slideCount >= 3)
        {
            CloseWIB_FTUXPanel();
        }
    }

    public void ToggleSlides(int nextSlideNumber)
    {
        int oldSlideCount = slideCount;
        slideCount = nextSlideNumber;

        if (slideCount <= 2)
        {
            // Button Name = Next
        }

        else if (slideCount == 3)
        {
            // Button Name = Done
        }

        // Call Image Animation and pass it the slide count
        wibImageArray[slideCount].SetActive(true);
        wibImageArray[oldSlideCount].SetActive(false);

        // Call Text animation to fade text and pass it the slide count
        wibTextArray[slideCount].SetActive(true);
        wibTextArray[oldSlideCount].SetActive(false);

        // Call to animate Toggle Dot to the next marker
        toggleDot.position = toggleDotMarkerArray[slideCount].position;
    }
}
