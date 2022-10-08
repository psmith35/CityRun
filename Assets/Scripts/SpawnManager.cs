using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Obstacles")]
    public GameObject[] obstaclePrefabs;

    [Header("Spawning")]
    public float startDelay = 2.0f;
    public float repeatMin = 2.0f;
    public float repeatMax = 3.0f;

    private Vector3 spawnPosition = new Vector3(25, 0, 0);
    private PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        if (obstaclePrefabs.Length > 0 && repeatMin <= repeatMax) Invoke("SpawnObstacle", startDelay);
        else Debug.Log("Error: Data has not been setup properly");
    }

    void SpawnObstacle()
    {
        if (player && !player.gameOver)
        {
            GameObject obstacle = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            Instantiate(obstacle, spawnPosition, obstacle.transform.rotation);
            float repeatRate = Random.Range(repeatMin, repeatMax);
            Invoke("SpawnObstacle", repeatRate);
        }
    }

}
