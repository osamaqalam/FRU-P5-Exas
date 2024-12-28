using UnityEngine;
using UnityEngine.UI;

public class ToggleExasVisibility : MonoBehaviour
{
    public GameObject Exas; // The object to toggle

    /// <summary>
    /// 3rd from Dank Tank @14:29
    /// 4th from Dank Tank @16:24
    /// 5th from Dank Tank @17:54
    /// </summary>
    private Vector3[] positions = { new Vector3(0.95f, 0.2f, -1.29f),
                                    new Vector3(2.57f, 0.2f, -3.1f),
                                    new Vector3(0.735f, 0.2f, -1.558f),
                                    new Vector3(-4.15f, 0.2f, -1.4f),
                                    new Vector3(-0.55f, 0.2f, 0.41f)};
    private float[] yEulerAngles = { 0, 0, -45f, 90f, 135f };

    public void RandomizeExas()
    {
        int randomNumber = Random.Range(0, 6); // Upper bound is exclusive for integers

        Exas.transform.position = positions[randomNumber];
        Exas.transform.rotation = Quaternion.Euler(0, yEulerAngles[randomNumber], 0);
    }

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
