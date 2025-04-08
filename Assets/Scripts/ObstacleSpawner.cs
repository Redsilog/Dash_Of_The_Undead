using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] objectToSpawn;
    public float timer;
    public float defaultTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = defaultTimer;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        SpawnObstacle();
    }

    public void SpawnObstacle()
    {
        if (timer <= 0)
        {
            int randomTimer = Random.Range(1, 3);
            int randomNumber = Random.Range(0, objectToSpawn.Length);
            if (objectToSpawn[randomNumber] != null)
            {
                Instantiate(objectToSpawn[randomNumber], transform.position, Quaternion.identity);
            }
            timer = randomTimer;
        }
    }
}
