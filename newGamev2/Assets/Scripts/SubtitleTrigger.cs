using UnityEngine;
using TMPro;

public class SubtitleTrigger : MonoBehaviour
{
    public TextMeshProUGUI subtitleText; // Reference to the TextMeshProUGUI component displaying the subtitle

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            subtitleText.enabled = true; // Enable the subtitle text
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            subtitleText.enabled = false; // Disable the subtitle text
        }
    }
}
