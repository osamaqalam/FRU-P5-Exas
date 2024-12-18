using UnityEngine;
using System.Collections;


public class SpawnWaves : MonoBehaviour
{
    public GameObject objectToSpawn; // The prefab to spawn
    public Transform exasParent; // The object to spawn parallel to
    public Vector3 offset = new Vector3(1, 0, 0); // Offset from the reference object
    public float timeBetweenObjects = 0.5f; // Delay between objects in the same set
    public float timeBetweenSets = 5f; // Delay between starting new sets
    private int SETS_NUM = 3;

    public void StartSpawning()
    {
        StartCoroutine(StartExaSets());
    }

    private IEnumerator StartExaSets()
    {
        Transform[] childrenArray = new Transform[2];

        for (int i=0; i< SETS_NUM; i++)
        {
            childrenArray[0] = exasParent.GetChild(i * 2);
            childrenArray[1] = exasParent.GetChild(i * 2 + 1);

            StartCoroutine(SpawnSet(childrenArray)); // Start spawning a new set
            yield return new WaitForSeconds(timeBetweenSets); // Wait to start the next set
        }
    }

    public IEnumerator SpawnSet(Transform[] Exas)
    {
        if (objectToSpawn != null && exasParent != null)
        {
            // Destroy all instances of the object to spawn
            //DestroyAllInstances();

            // If we are in reset mode then don't spawn waves
            //if (!exasParent.activeSelf) 
            //   return;

            // Loop through each child of the parent
            foreach (Transform child in Exas)
            {
                // Calculate the offset relative to the reference object's local rotation
                Vector3 adjustedOffset = child.TransformDirection(offset);

                // Calculate spawn position relative to the child
                Vector3 spawnPosition = child.position + adjustedOffset;

                // Spawn the object at the calculated position with the child's rotation
                Instantiate(objectToSpawn, spawnPosition, child.rotation);

            }
            
        }
        else
        {
            Debug.LogWarning("Object to spawn or reference object is not assigned!");
        }
        yield return null;
    }

    public void DestroyAllInstances()
    {
        if (objectToSpawn == null)
        {
            Debug.LogWarning("Prefab is not assigned!");
            return;
        }

        // Find all GameObjects in the scene that are instances of the prefab
        GameObject[] objects = GameObject.FindGameObjectsWithTag(objectToSpawn.tag);


        foreach (GameObject obj in objects)
        {
                Destroy(obj); // Destroy the object
        }

        Debug.Log("All instances of the prefab have been deleted.");
    }

    
}
