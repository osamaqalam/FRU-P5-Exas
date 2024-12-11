using UnityEngine;

public class SpawnWaves : MonoBehaviour
{
    public GameObject prefab; // Assign your Prefab in the Inspector
    public Transform spawnPoint; // Optional: assign where objects should spawn

    void Update()
    {
        // Check if "S" key is pressed
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnObject(); // Call the spawn method
        }
    }

    void SpawnObject()
    {
        // If no spawn point is defined, spawn at the spawner's position
        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;

        // Instantiate the object
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}
