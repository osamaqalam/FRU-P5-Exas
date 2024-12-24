using UnityEngine;
using UnityEngine.UI;

public class ToggleExasVisibility : MonoBehaviour
{
    public GameObject Exas; // The object to toggle

    public void ToggleObjectVisibility()
    {
        if (Exas != null)
        {
            // Toggle the object's active state
            bool isActive = Exas.activeSelf;
            Exas.SetActive(!isActive);
        }
        else
        {
            Debug.LogWarning("Target object is not assigned!");
        }
    }
}
