using UnityEngine;
using TMPro;

public class LetterCollect : MonoBehaviour
{
    public string letter;

    [Header("Arrow Settings")]
    public GameObject arrowPrefab;
    public Vector3 spawnOffset = new Vector3(0f, 2f, 0f);

    private GameObject spawnedArrow;

    [Header("UI Settings")]
    public TextMeshProUGUI grabText; // Assign "Grab" text in inspector

    [Header("Sound Settings")]
    public AudioSource audioSource;
    public AudioClip collectSound;

    void Start()
    {
        SpawnArrow();

        // Hide text at start
        if (grabText != null)
            grabText.gameObject.SetActive(false);
    }

    void SpawnArrow()
    {
        if (arrowPrefab == null) return;

        spawnedArrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        spawnedArrow.transform.SetParent(transform);
        spawnedArrow.transform.localPosition = spawnOffset;
        spawnedArrow.transform.localRotation = Quaternion.Euler(180f, 90f, 0f);
    }

    // 🔽 Trigger Enter
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (grabText != null)
                grabText.gameObject.SetActive(true);
        }
    }

    // 🔽 Trigger Exit
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (grabText != null)
                grabText.gameObject.SetActive(false);
        }
    }

    public void TryCollect()
    {
        Debug.Log("Collected: " + letter);

        GameManager.instance.CollectLetter(letter);

        // Play sound once
        if (audioSource != null && collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }

        // Hide text when collected
        if (grabText != null)
            grabText.gameObject.SetActive(false);

        if (spawnedArrow != null)
            Destroy(spawnedArrow);

        Destroy(gameObject);
    }
}