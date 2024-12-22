using UnityEngine;
using System.Collections;


public class SpawnWaves : MonoBehaviour
{
    public GameObject objectToSpawn; // The prefab to spawn
    public Transform exasParent; // The object to spawn parallel to
    public Vector3 offset = new Vector3(1, 0, 0); // Offset from the reference object
    public float timeBetweenWaves = 0.5f; // Delay between objects in the same set
    public float timeBetweenSets = 5f; // Delay between starting new sets
    public float timeToSpawnSet1 = 2f;
    public float timeToSpawnSet2 = 4f;
    public float timeToSpawnSet3 = 6f;
    public int wavesPerSet = 4;
    private int waveIndex1 = 0;
    private int waveIndex2 = 0;
    private int waveIndex3 = 0;

    private bool isUpdateEnabled = false; // Flag to control Update logic
    private float waveWidth; // Width in the local x-axis
    private int SETS_NUM = 3;

    void Start ()
    {
        Renderer planeRenderer = objectToSpawn.GetComponent<Renderer>();
        waveWidth = planeRenderer.bounds.size.x;
    }

    void Update ()
    {
        if (!isUpdateEnabled) return;

        CheckSpawnTime(ref timeToSpawnSet1, 0, ref waveIndex1);

        CheckSpawnTime(ref timeToSpawnSet2, 1, ref waveIndex2);

        CheckSpawnTime(ref timeToSpawnSet3, 2, ref waveIndex3);
    }

    public void CheckSpawnTime(ref float currentTimeToSpawnSet, int setIndex, ref int waveIndex)
    {
        if (currentTimeToSpawnSet > 0)
            currentTimeToSpawnSet -= Time.deltaTime;
        else if (waveIndex < wavesPerSet)
        {
            SpawnSet(setIndex, waveIndex++);
            currentTimeToSpawnSet = timeBetweenWaves;
        }
    }

    public void SpawnSet(int setIndex, int waveIndex)
    {
        if (objectToSpawn != null && exasParent != null)
        {
            Transform[] setExas = new Transform[2];

            setExas[0] = exasParent.GetChild(setIndex * 2);
            setExas[1] = exasParent.GetChild(setIndex * 2 + 1);

            // Loop through each child of the parent
            foreach (Transform child in setExas)
            {
                // Calculate the offset relative to the reference object's local rotation
                Vector3 adjustedOffset = child.TransformDirection(offset + new Vector3(waveIndex * waveWidth, 0, 0));

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
    }


    // Method to enable Update (assign this to your button's OnClick)
    public void EnableUpdate()
    {
        isUpdateEnabled = true;
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
