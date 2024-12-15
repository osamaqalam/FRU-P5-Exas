using UnityEngine;

public class SpawnWaves : MonoBehaviour
{
    public GameObject objectToSpawn; // The prefab to spawn
    public GameObject exasParent; // The object to spawn parallel to
    public Vector3 offset = new Vector3(1, 0, 0); // Offset from the reference object

    public void SpawnParallel()
    {
        if (objectToSpawn != null && exasParent != null)
        {
            // Destroy all instances of the object to spawn
            DestroyAllInstances();

            // If we are in reset mode then don't spawn waves
            if (!exasParent.activeSelf) 
                return;

            // Loop through each child of the parent
            foreach (Transform child in exasParent.transform)
            {
                // Calculate spawn position relative to the child
                Vector3 spawnPosition = child.position + offset;

                // Spawn the object at the calculated position with the child's rotation
                Instantiate(objectToSpawn, spawnPosition, child.rotation);
            }
        }
        else
        {
            Debug.LogWarning("Object to spawn or reference object is not assigned!");
        }
    }

    public void DestroyAllInstances()
    {
        if (objectToSpawn == null)
        {
            Debug.LogWarning("Prefab is not assigned!");
            return;
        }

        // Find all GameObjects in the scene that are instances of the prefab
        //GameObject[] allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        GameObject[] objects = GameObject.FindGameObjectsWithTag(objectToSpawn.tag);


        foreach (GameObject obj in objects)
        {
                Destroy(obj); // Destroy the object
        }

        Debug.Log("All instances of the prefab have been deleted.");
    }
}
