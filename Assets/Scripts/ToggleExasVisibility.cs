using UnityEngine;
using UnityEngine.UI;

public class ToggleExasVisibility : MonoBehaviour
{
    public GameObject targetObject; // The object to toggle
    public Text buttonText;         // Reference to the button's text


    public void ToggleObjectVisibility()
    {
        if (targetObject != null)
        {
            // Toggle the object's active state
            bool isActive = targetObject.activeSelf;
            targetObject.SetActive(!isActive);

            // Update the button text
            if (buttonText != null)
            {
                buttonText.text = isActive ? "Start" : "Reset";
            }
        }
        else
        {
            Debug.LogWarning("Target object is not assigned!");
        }
    }
}
