using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class RandomTextSpawner : MonoBehaviour
{
    public GameObject[] textPrefabs; // Array to hold multiple prefabs
    public Canvas canvas;
    public float textDuration = 2.5f;
    public float spawnInterval = 1f;

    private void Start()
    {
        StartCoroutine(SpawnText());
    }

    private IEnumerator SpawnText()
    {
        while (true)
        {
            // Get Canvas RectTransform
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            // Calculate random position within canvas
            Vector2 randomPosition = new Vector2(
                Random.Range(-canvasRect.rect.width / 2, canvasRect.rect.width / 2),
                Random.Range(-canvasRect.rect.height / 2, canvasRect.rect.height / 2)
            );

            // Choose a random prefab from the array
            int randomPrefabIndex = Random.Range(0, textPrefabs.Length);
            GameObject selectedPrefab = textPrefabs[randomPrefabIndex];

            // Instantiate the selected prefab as child of the canvas
            GameObject newText = Instantiate(selectedPrefab, canvas.transform);

            // Set text position using calculated randomPosition
            RectTransform rectTransform = newText.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = randomPosition;

            // Check if the text component exists before accessing it
            TMP_Text text = newText.GetComponent<TMP_Text>();
            if (text != null)
            {
                StartCoroutine(FadeInOutText(newText, textDuration, text)); // Pass the TMP_Text component to the coroutine
            }
            else
            {
                Debug.LogError("Text component not found on prefab: " + selectedPrefab.name);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator FadeInOutText(GameObject textObject, float duration, TMP_Text text) // Add TMP_Text parameter
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration / 2)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(0f, 1f, elapsedTime / (duration / 2)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < duration / 2)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(1f, 0f, elapsedTime / (duration / 2)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(textObject);
    }
}