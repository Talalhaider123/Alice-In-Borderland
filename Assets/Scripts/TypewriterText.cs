using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterText : MonoBehaviour
{
    [Header("Text Settings")]
    public TextMeshProUGUI textUI;

    [TextArea(2, 5)]
    public string[] sentences;

    public float typingSpeed = 0.05f;
    public float delayBetweenSentences = 4f;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip[] sentenceSounds;

    [Header("Background Music")]
    public AudioSource bgMusicSource;
    public AudioClip bgMusic;

    private Coroutine runningRoutine;

    void OnEnable()
    {
        if (runningRoutine != null)
            StopCoroutine(runningRoutine);

        // 🎵 Play background music
        if (bgMusicSource != null && bgMusic != null)
        {
            bgMusicSource.clip = bgMusic;
            bgMusicSource.loop = true;
            bgMusicSource.Play();
        }

        runningRoutine = StartCoroutine(TypeSentences());
    }

    IEnumerator TypeSentences()
    {
        for (int i = 0; i < sentences.Length; i++)
        {
            yield return StartCoroutine(TypeLine(sentences[i], i));
            yield return new WaitForSeconds(delayBetweenSentences);
        }

        yield return StartCoroutine(FadeOut());
    }

    IEnumerator TypeLine(string sentence, int index)
    {
        textUI.text = "";

        AudioClip currentClip = null;

        if (sentenceSounds != null && sentenceSounds.Length > 0)
        {
            currentClip = sentenceSounds[Mathf.Clamp(index, 0, sentenceSounds.Length - 1)];
        }

        // 🎙 Play sentence audio once
        if (audioSource != null && currentClip != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(currentClip);
        }

        foreach (char letter in sentence.ToCharArray())
        {
            textUI.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    IEnumerator FadeOut()
    {
        if (fadeDuration <= 0f)
        {
            textUI.gameObject.SetActive(false);
            yield break;
        }

        float time = 0;
        Color originalColor = textUI.color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, time / fadeDuration);

            textUI.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                alpha
            );

            yield return null;
        }

        textUI.gameObject.SetActive(false);
    }
}