using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;

public class ExasManager : MonoBehaviour
{
    public GameObject yellowWave; // The prefab to spawn
    public GameObject purpleWave; // The prefab to spawn
    public Transform exasParent; 
    public Vector3 offset = new Vector3(0.55f, 0, 0); // Offset from the reference object
    public float timeBetweenWaves = 0.5f; // Delay between objects in the same set
    public float timeBetweenSets = 5f; // Delay between starting new sets
    public float timeToSpawnSet1 = 2f;
    public float timeToSpawnSet2 = 4f;
    public float timeToSpawnSet3 = 6f;
    public int wavesPerSet = 4;
    public float glowSpeed = 2f; // Speed of the pulse
    public float minGlow = 0.5f; // Minimum intensity
    public float maxGlow = 2f;  // Maximum intensity
    private float _timeToSpawnSet1;
    private float _timeToSpawnSet2;
    private float _timeToSpawnSet3;
    private int waveIndex1 = 0;
    private int waveIndex2 = 0;
    private int waveIndex3 = 0;
    private bool isUpdateEnabled = false; // Flag to control Update logic
    private float waveWidth; // Width in the local x-axis

    /// <summary>
    /// 3rd from Dank Tank @14:29
    /// 4th from Dank Tank @16:24
    /// 5th from Dank Tank @17:54
    /// </summary>
    private Vector3[] positions = { new Vector3(0.95f, 0.01f, -1.29f),
                                    new Vector3(2.57f, 0.01f, -3.1f),
                                    new Vector3(0.735f, 0.01f, -1.558f),
                                    new Vector3(-4.15f, 0.01f, -1.4f),
                                    new Vector3(-0.55f, 0.01f, 0.41f)};
    private float[] yEulerAngles = { 0, 0, -45f, 90f, 135f };

    void Start ()
    {
        Renderer planeRenderer = yellowWave.GetComponent<Renderer>();
        waveWidth = planeRenderer.bounds.size.x;

        int randomNumber = Random.Range(0, 2); // Upper bound is exclusive for integers

        // Use the random number to determine the order of the sets
        if (randomNumber == 0)
        {
            _timeToSpawnSet1 = timeToSpawnSet1;
            _timeToSpawnSet2 = timeToSpawnSet2;
            _timeToSpawnSet3 = timeToSpawnSet3;
        }
        else
        {
            _timeToSpawnSet1 = timeToSpawnSet3;
            _timeToSpawnSet2 = timeToSpawnSet2;
            _timeToSpawnSet3 = timeToSpawnSet1;
        }
    }

    void Update ()
    {
        if (!isUpdateEnabled) return;

        CheckSpawnTime(ref _timeToSpawnSet1, 0, ref waveIndex1);

        CheckSpawnTime(ref _timeToSpawnSet2, 1, ref waveIndex2);

        CheckSpawnTime(ref _timeToSpawnSet3, 2, ref waveIndex3);
    }

    public void CheckSpawnTime(ref float currentTimeToSpawnSet, int setIndex, ref int waveIndex)
    {
        if (currentTimeToSpawnSet <= 3)
            GlowExas(setIndex);

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
        GameObject spawnedYWave;
        GameObject spawnedPWave;

        if (yellowWave != null && purpleWave != null && exasParent != null)
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
                Vector3 adjustedYOffset = child.TransformDirection(offset + new Vector3(waveIndex * waveWidth, 0, 0));
                Vector3 adjustedPOffset = child.TransformDirection(-offset - new Vector3(waveIndex * waveWidth, 0, 0));


                // Calculate spawn position relative to the child
                Vector3 spawnYPosition = child.position + adjustedYOffset;
                Vector3 spawnPPosition = child.position + adjustedPOffset;

                // Spawn the object at the calculated position with the child's rotation
                spawnedYWave = Instantiate(yellowWave, spawnYPosition, child.rotation);
                spawnedPWave = Instantiate(purpleWave, spawnPPosition, child.rotation);

                spawnedYWave.name = $"YWave{setIndex}{waveIndex}";
                spawnedPWave.name = $"PWave{setIndex}{waveIndex}";

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
        List<GameObject> waves = SearchByName(@$"^.Wave{setIndex}{waveIndex}");

        foreach (GameObject wave in waves)
            Destroy(wave);
    }

    public List<GameObject> SearchByName(string targetName)
    {
        GameObject[] allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None); // Get all active GameObjects

        List<GameObject> matchingObjects = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            //if (obj.name == targetName)
            if(Regex.IsMatch(obj.name, targetName))
            {
                matchingObjects.Add(obj);
            }
        }

        return matchingObjects;
    }

    public void RandomizeExas()
    {
        int randomNumber = Random.Range(0, 5); // Upper bound is exclusive for integers

        exasParent.transform.position = positions[randomNumber];
        exasParent.transform.rotation = Quaternion.Euler(0, yEulerAngles[randomNumber], 0);
    }

    public void ToggleObjectVisibility()
    {
        if (exasParent != null)
        {
            // Toggle the object's active state
            bool isActive = exasParent.gameObject.activeSelf;
            exasParent.gameObject.SetActive(!isActive);
        }
        else
        {
            Debug.LogWarning("Target object is not assigned!");
        }
    }

    public void GlowExas(int setIndex)
    {
        Transform[] setExas = new Transform[2];
        setExas[0] = exasParent.GetChild(setIndex * 2);
        setExas[1] = exasParent.GetChild(setIndex * 2 + 1);

        foreach (Transform exa in setExas)
        {
            Renderer exaRenderer = exa.GetComponent<Renderer>();

            if (exaRenderer != null)
            {
                Material originalMaterial = exaRenderer.material;
                Material glowMaterial = new Material(originalMaterial); // Create a new instance of the material

                Color glowColor = Color.yellow; // Base glow color (change to your desired color)
                
                // Calculate the pulsating intensity
                float intensity = Mathf.Lerp(minGlow, maxGlow, Mathf.PingPong(Time.time * glowSpeed, 1));

                // Assign the emission color with the calculated intensity
                glowMaterial.SetColor("_EmissionColor", glowColor * intensity);
                exaRenderer.material = glowMaterial; // Apply the new material to the renderer

                Debug.Log($"Emission Intensity: {intensity}");
            }
            else
            {
                Debug.LogWarning("Renderer component not found on the target object!");
            }
        }
    }
}
