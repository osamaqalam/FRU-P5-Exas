using UnityEngine;
using TMPro;

public class PlayerCollision : MonoBehaviour
{
    public TextMeshProUGUI feedbackText;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wave"))
        {
            feedbackText.text = $"Player was hit by {other.name}";
        }
    }
}
