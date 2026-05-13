using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    public GameObject panelA; // Main panel (e.g., Main Menu)
    public GameObject panelB; // Second panel (e.g., Settings / Help)

    // Call this on "Open Panel B" button
    public void OpenPanelB()
    {
        panelA.SetActive(false);
        panelB.SetActive(true);
    }

    // Call this on "Back" button inside Panel B
    public void BackToPanelA()
    {
        panelB.SetActive(false);
        panelA.SetActive(true);
    }
}