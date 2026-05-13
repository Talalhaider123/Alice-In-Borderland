using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TextMeshProUGUI[] wordLetters;

    int collected = 0;

    void Awake()
    {
        instance = this;
    }

    public void CollectLetter(string letter)
    {
        for (int i = 0; i < wordLetters.Length; i++)
        {
            if (wordLetters[i].text == letter && wordLetters[i].color != Color.green)
            {
                wordLetters[i].color = Color.green;
                collected++;
                break;
            }
        }

        CheckComplete();
    }

    void CheckComplete()
    {
        if (collected >= wordLetters.Length)
        {
            LevelComplete();
        }
    }

    void LevelComplete()
    {
        UIManager.instance.ShowLevelComplete();
    }
}