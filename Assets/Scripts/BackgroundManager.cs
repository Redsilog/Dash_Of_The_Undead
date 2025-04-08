using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public Rigidbody2D rb;
    LevelManager levelManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        levelManager = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocityX = -levelManager.gameSpeed;

        if (transform.position.x < -18.54f)
        {
            transform.position = Vector3.zero;
        }
    }
}
