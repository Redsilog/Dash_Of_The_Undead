using UnityEngine;

public class ScalpelThrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 initialPos, targetPos, distance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        initialPos = transform.position;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        targetPos = worldPos;

        SetTrajectory(0.5f);

        SFXManager.PlaySfx("throw");

    }

    // Update is called once per frame
    void Update()
    {
        rb.AddTorque(-1);
    }

    public void SetTrajectory(float time)
    {
        rb.linearVelocity = Vector3.zero;

        distance = targetPos - initialPos;
        rb.linearVelocity = new Vector2(ObjectSpeedX(distance.x, time), ObjectSpeedY(distance.y, Physics.gravity.y, time));
    }

    public float ObjectSpeedX(float distance, float time)
    {
        return distance / time;
    }

    public float ObjectSpeedY(float distance, float acceleration, float time)
    {
        return distance - Displacement(0, acceleration, time) / time;
    }

    public float Displacement(float initialVel, float acceleration, float time)
    {
        return initialVel * time + 0.5f * acceleration * time * time;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Zombie"))
        {
            ObstacleScript obstacleScript = collision.GetComponent<ObstacleScript>();
            obstacleScript.Killed();
        }
        Destroy(gameObject);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
