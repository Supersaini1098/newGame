using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemies;  // Array to store enemy game objects

    void Start()
    {
        // Initialize the array with enemy game objects
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Create and configure the subtitle UI Text element
        CreateSubtitleText();
    }

    void CreateSubtitleText()
    {
        // Create a new UI Text GameObject
        GameObject subtitleGO = new GameObject("Subtitle");
        subtitleGO.transform.SetParent(GameObject.Find("Canvas").transform); // Make sure to have a Canvas GameObject in your scene

        // Add the UI Text component to the newly created GameObject
        Text subtitleText = subtitleGO.AddComponent<Text>();

        // Configure the subtitle text properties
        subtitleText.rectTransform.anchorMin = new Vector2(0.5f, 0f);
        subtitleText.rectTransform.anchorMax = new Vector2(0.5f, 0f);
        subtitleText.rectTransform.pivot = new Vector2(0.5f, 0f);
        subtitleText.rectTransform.anchoredPosition = new Vector2(0f, 20f);
        subtitleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        subtitleText.fontSize = 24;
        subtitleText.color = Color.white;
        subtitleText.alignment = TextAnchor.MiddleCenter;

        // Check if all enemies are killed
        if (AreAllEnemiesKilled())
        {
            // Set the initial subtitle text
            subtitleText.text = "Jump in the water, there's too many more coming";
        }
        else
        {
            // Hide the subtitle initially
            subtitleText.text = "";
        }
    }

    void Update()
    {
        // Check if all enemies are killed
        if (AreAllEnemiesKilled())
        {
            // Get the subtitle UI Text component
            Text subtitleText = GameObject.Find("Subtitle").GetComponent<Text>();

            // Display the subtitle
            subtitleText.text = "Jump in the water, there's too many more coming";
        }
    }

    bool AreAllEnemiesKilled()
    {
        // Check if there are any enemies remaining
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                // There is at least one enemy remaining, so return false
                return false;
            }
        }

        // All enemies are killed, so return true
        return true;
    }
}
