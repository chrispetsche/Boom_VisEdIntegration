using UnityEngine;
using UnityEngine.UI;

public class ToggleSequence : MonoBehaviour
{
    [SerializeField] private Toggle[] toggles;

    private int toggleIndex;

    private void OnDisable()
    {
        ResetToggles();
    }

    private void ResetToggles()
    {
        toggleIndex = -1;
        NextToggle();
    }

    public void NextToggle()
    {
        foreach (var toggle in toggles)
        {
            toggle.isOn = false;
        }

        toggleIndex++;
        toggleIndex = Mathf.Clamp(toggleIndex, 0, toggles.Length);
        toggles[toggleIndex].isOn = true;
    }
}