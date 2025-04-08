using UnityEngine;

public class PowerUp : MonoBehaviour
{
    Rigidbody2D rb;
    LevelManager levelManager;
    [SerializeField] public PowerUpType powerUpType;
    public enum PowerUpType
    {
        Medkit,
        Adrenaline,
        Ammo
    }
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
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            if (powerUpType == PowerUpType.Adrenaline)
            {
                playerMovement.AdrenalineFunction();
            }
            else if (powerUpType == PowerUpType.Medkit)
            {
                playerMovement.HealthFunction();
            }

            else if (powerUpType == PowerUpType.Ammo)
            {
                playerMovement.AmmoFunction();
            }
            Destroy(gameObject);

        }
    }
}
