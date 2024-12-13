using UnityEngine;

public class ToggleExasVisibility : MonoBehaviour
{
    public GameObject targetObject; // The object to toggle

    public void ToggleObjectVisibility()
    {
        if (targetObject != null)
        {
            // Toggle the object's active state
            bool isActive = targetObject.activeSelf;
            targetObject.SetActive(!isActive);
        }
        else
        {
            Debug.LogWarning("Target object is not assigned!");
        }
    }
}
