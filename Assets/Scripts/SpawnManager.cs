using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _boxToSpawn;
    
    public Transform[] _boxTile;
    //private bool _isSpawned = false;
    [SerializeField]
    private List<Transform[]> baseTilesPerLevel = new List<Transform[]>();

    public HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

    
    public int maxSpawnPoints;

    public int spawnedTilesCount;

    public int currentLevel = 0;

    


    void Start()
    {
        
        InitializeSpawnPoints();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform boxTile in _boxTile)
        {
            if (boxTile != null)
            {
                Ray ray = new Ray(boxTile.position, boxTile.up);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Debug.DrawRay(boxTile.position, boxTile.up * hit.distance, Color.red);
                    // Add your logic for what happens when the ray hits something
                    //Debug.Log("Raycast hit: " + hit.collider.name + " from " + baseTile.name);
                }
                else
                {
                    Debug.DrawRay(boxTile.position, boxTile.forward * 10, Color.yellow);
                    //Debug.Log("Raycast did not hit anything from " + baseTile.name);
                    SpawnColorHexa();
                }
            }
            else
            {
                Debug.LogWarning("baseTile is null in the array");
            }
        }
    }

    void SpawnColorHexa()
    {
        List<Transform> availableSpawnPoints = new List<Transform>(_boxTile);

        while (availableSpawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform randomBaseTile = availableSpawnPoints[randomIndex];
            Vector3 spawnPosition = randomBaseTile.position + new Vector3(0, 0.204f, 0);

            // Check if the position is already occupied
            if (!occupiedPositions.Contains(spawnPosition))
            {
                Quaternion instantiateQuat = new Quaternion(0, 0, 0, 0);                     //Quaternion(-0.707106829f, 0, 0, 0.707106829f);
                GameObject go = Instantiate(_boxToSpawn, spawnPosition, instantiateQuat);
                go.name = randomBaseTile.ToString() + "New Tile";
                occupiedPositions.Add(spawnPosition);
                spawnedTilesCount++;
                break; // Exit the loop after spawning one object
            }

            availableSpawnPoints.RemoveAt(randomIndex);
        }
    }

    public void InitializeSpawnPoints()
    {
        List<Transform> allSpawnPoints = new List<Transform>(_boxTile);
        List<Transform> selectedSpawnPoints = new List<Transform>();

        // Randomly select 13 spawn points
        for (int i = 0; i < maxSpawnPoints && allSpawnPoints.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, allSpawnPoints.Count);
            selectedSpawnPoints.Add(allSpawnPoints[randomIndex]);
            allSpawnPoints.RemoveAt(randomIndex);
        }

        // Replace the original _baseTile array with the selected spawn points
        _boxTile = selectedSpawnPoints.ToArray();
    }



   

    public void TileDestroyed()
    {

        spawnedTilesCount--;
        if (spawnedTilesCount <= 0)
        {
            currentLevel++;
            ChangeScene();
            
        }

        //spawnedTilesCount--;
        //if (spawnedTilesCount <= 0)
        //{
        //    currentLevel++;
        //    if (currentLevel >= baseTilesPerLevel.Count)
        //    {
        //        Debug.Log("All levels completed!");
        //        // Handle game completion (e.g., show end screen, reset game, etc.)
        //    }
        //    else
        //    {
        //        ChangeScene();
        //    }
        //}
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        

    }
}
