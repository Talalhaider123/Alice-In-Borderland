using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    public Button karachiBtn;
    public Button lahoreBtn;
    public Button peshawarBtn;

    public GameObject karachiLock;
    public GameObject lahoreLock;
    public GameObject peshawarLock;

    void Start()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        karachiBtn.interactable = unlockedLevel >= 2;
        lahoreBtn.interactable = unlockedLevel >= 3;
        peshawarBtn.interactable = unlockedLevel >= 4;

        karachiLock.SetActive(unlockedLevel < 2);
        lahoreLock.SetActive(unlockedLevel < 3);
        peshawarLock.SetActive(unlockedLevel < 4);
    }

    // 🔥 LOAD WITH LOADING SCREEN
    void LoadWithScreen(string sceneName)
    {
        PlayerPrefs.SetString("SceneToLoad", sceneName);
        SceneManager.LoadScene("LoadingScene");
    }

    public void LoadLevel1()
    {
        LoadWithScreen("level1");
    }

    public void LoadLevel2()
    {
        LoadWithScreen("level2");
    }

    public void LoadLevel3()
    {
        LoadWithScreen("level3");
    }

    public void LoadLevel4()
    {
        LoadWithScreen("level4");
    }
}