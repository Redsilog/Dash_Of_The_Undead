using UnityEngine;

public class CeilingObstacle : MonoBehaviour
{
    [SerializeField] private GameObject ceiling;
    private float timer;
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
            int randomTimer = Random.Range(1, 4);

            if (randomTimer == 1)
            {
                if (gameObject.CompareTag("LeftRoof"))
                {
                    Instantiate(ceiling, transform.position, Quaternion.identity);
                    Debug.Log("Left Spawn");
                }
            }
            else if (randomTimer == 2)
            {
                if (gameObject.CompareTag("MiddleRoof"))
                {
                    Instantiate(ceiling, transform.position, Quaternion.identity);
                    Debug.Log("Middle Spawn");
                }
            }
            else if (randomTimer == 3)
            {
                if (gameObject.CompareTag("RightRoof"))
                {
                    Instantiate(ceiling, transform.position, Quaternion.identity);
                    Debug.Log("Right Spawn");
                }
            }

            timer = randomTimer;
        }
    }
}