using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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
        GameObject spawnedWave;

        if (objectToSpawn != null && exasParent != null)
        {
            if (waveIndex>0 && waveIndex<wavesPerSet)
                DestroyWaves(setIndex, waveIndex - 1);

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
                spawnedWave = Instantiate(objectToSpawn, spawnPosition, child.rotation);
                
                spawnedWave.name = $"YWave{setIndex}{waveIndex}";
            }
            
        }
        else
        {
            Debug.LogWarning("Object to spawn or reference object is not assigned!");
        }
    }

    public void ToggleUpdate()
    {
        isUpdateEnabled = !isUpdateEnabled;
    }

    public void DestroyWaves(int setIndex, int waveIndex)
    {
        // Find all GameObjects in the scene that have specified name
        List<GameObject> waves = SearchByName($"YWave{setIndex}{waveIndex}");

        foreach (GameObject wave in waves)
            Destroy(wave);
    }

    public List<GameObject> SearchByName(string targetName)
    {
        GameObject[] allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None); // Get all active GameObjects

        List<GameObject> matchingObjects = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == targetName)
            {
                matchingObjects.Add(obj);
            }
        }

        return matchingObjects;
    }
}
