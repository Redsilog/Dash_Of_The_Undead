using System.Collections;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    Rigidbody2D rb;
    LevelManager levelManager;
    Animator anim;
    bool dead;
    [SerializeField] public ObstacleType obstacleType;

    [SerializeField] private float obstacleSpeed;
    public enum ObstacleType
    {
        Wheelchair,
        Zombie,
        Blood,
        Ceiling
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        levelManager = GameObject.FindGameObjectWithTag("Level Manager").GetComponent<LevelManager>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (obstacleType == ObstacleType.Zombie)
        {
            if (!dead)
                rb.linearVelocityX = -levelManager.gameSpeed - obstacleSpeed;
        }
        else if (obstacleType == ObstacleType.Wheelchair)
        {
            rb.linearVelocityX = -levelManager.gameSpeed - obstacleSpeed;

            float wheelchairSpeedMultiplier = 2f;
            rb.linearVelocityX *= wheelchairSpeedMultiplier;
        }
        else if (obstacleType == ObstacleType.Blood)
        {
            rb.linearVelocityX = -levelManager.gameSpeed;
        }
        else if (obstacleType == ObstacleType.Ceiling)
        {
            rb.linearVelocity = new Vector2(-levelManager.gameSpeed, -15);

        }


        if (transform.position.x <= -18.59f)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    public void Killed()
    {
        if (anim == null) return; //if not zombie, return
        SFXManager.PlaySfx("zombie-death");
        anim.SetTrigger("Dead");
        levelManager.kills++;
        Debug.Log("Dead");
        dead = true;
        CapsuleCollider2D capsule = GetComponent<CapsuleCollider2D>();
        capsule.enabled = false;
        rb.linearVelocityX = -levelManager.gameSpeed;

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            if (!dead)
            {
                if (obstacleType != ObstacleType.Blood)
                {
                    player.AttackedFunction();
                    if (anim != null)
                    {
                        SFXManager.PlaySfx("bite");
                        anim.SetTrigger("Attack");
                    }

                    if (gameObject.tag == "Wheelchair")
                    {
                        SFXManager.PlaySfx("crash");
                    }
                }
                else
                {
                    player.SplippingFunction();
                }
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (anim == null) return;

        if (!dead && collision.CompareTag("Player"))
        {
            anim.SetTrigger("Run");
        }
    }
}