using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject levelCompleteText;
    public GameObject completePanel;

    public int currentLevel = 1;

    void Awake()
    {
        instance = this;
    }

    public void ShowLevelComplete()
    {
        levelCompleteText.SetActive(true);

        UnlockNextLevel();

        Invoke("ShowPanel", 2f);
    }

    void ShowPanel()
    {
        completePanel.SetActive(true);
    }

    void UnlockNextLevel()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (currentLevel >= unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevel + 1);
            PlayerPrefs.Save();
        }
    }
}