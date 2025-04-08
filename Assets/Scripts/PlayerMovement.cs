using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public int health = 5;
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    Vector2 input;
    [SerializeField] private float moveSpeed, jumpHeight;
    public bool grounded, enemyHit, attacked;
    public float jumpForce = 0f;
    public Transform groundCheck, attackCheck;
    public LayerMask groundLayer;
    [SerializeField] private float attackSpeed;
    [SerializeField] private GameObject scalpel, scalpelSpawn;
    public bool slipping = false;
    public float jumpTimer = 0.2f;
    [SerializeField] Slider healthSlider;

    [SerializeField] public int ammoCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GameObject.FindGameObjectWithTag("Player Sprite").GetComponent<Animator>();
        sr = anim.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        HealthInterfaceFunction();
        MoveFunction();
        DeathFunction();
        if (health <= 0) return;
        JumpFunction();
        AttackFunction();
        ThrowFunction();
    }

    void JumpFunction()
    {
        Debug.DrawRay(groundCheck.position, Vector2.down * 0.1f, grounded ? Color.green : Color.red);
        grounded = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, groundLayer);

        if (grounded)
        {
            jumpTimer = 0.3f;
        }
        else
        {
            jumpTimer -= Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (jumpTimer > 0 && !slipping)
            {
                rb.linearVelocityY = jumpHeight * jumpForce;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (jumpTimer > 0 && !slipping)
            {
                jumpTimer = 0;
            }
        }

        if (rb.linearVelocity.y < 0 && !grounded)
        {
            rb.linearVelocity += Vector2.down * 0.3f;
        }
    }

    void MoveFunction()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

        if (!slipping)
        {
            rb.linearVelocityX = input.x * moveSpeed;
            if (input.x == 0)
            {
                rb.linearVelocityX = -2f;
            }
        }
        else
        {
            rb.linearVelocityX = 3;
        }

        if (health <= 0)
        {
            rb.linearVelocityX = 0;
        }
    }

    void AttackFunction()
    {
        if (Input.GetMouseButtonDown(0) && !attacked)
        {
            attacked = true;
            SFXManager.PlaySfx("slice");
            anim.SetTrigger("Attack");
        }
        Debug.DrawRay(attackCheck.position, Vector2.right * 1.5f, enemyHit ? Color.green : Color.red);
        if (attacked)
        {
            if (attackSpeed > 0)
            {
                attackSpeed -= Time.deltaTime;
            }
            else
            {
                enemyHit = true;
                attacked = false;

                RaycastHit2D hit = Physics2D.Raycast(attackCheck.position, Vector2.right, 1.5f);
                if (hit.collider != null)
                {
                    ObstacleScript obstacleScript = hit.collider.GetComponent<ObstacleScript>();
                    obstacleScript.Killed();
                }
                attackSpeed = 0.2f;


            }
        }
        else
        {
            enemyHit = false;
        }
    }

    public void AttackedFunction()
    {
        if (health <= 0) return;
        SFXManager.PlaySfx("scream");
        health--;
        StartCoroutine(AttackedDelay());
    }

    IEnumerator AttackedDelay()
    {
        sr.color = Color.red;
        anim.SetTrigger("Hurt");
        yield return new WaitForSeconds(0.5f);
        sr.color = Color.white;
    }

    public void ThrowFunction()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (ammoCount > 0)
            {
                Instantiate(scalpel, scalpelSpawn.transform.position, Quaternion.identity);
                ammoCount--;
            }
        }
    }

    public void SplippingFunction()
    {
        slipping = true;
        SFXManager.PlaySfx("blood");
        anim.SetTrigger("Idle");
        StartCoroutine(SlippingDelay());
    }
    public IEnumerator SlippingDelay()
    {
        yield return new WaitForSeconds(2);
        anim.SetTrigger("Running");
        slipping = false;
    }

    void HealthInterfaceFunction()
    {
        healthSlider.value = health;
    }
    void DeathFunction()
    {
        if (health <= 0)
        {
            anim.SetTrigger("Dead");
        }
    }

    public void AmmoFunction()
    {
        ammoCount += 3;
    }

    public void HealthFunction()
    {
        if (health < 5)
        {
            health += 1;
        }
    }

    public void AdrenalineFunction()
    {

        moveSpeed += 3;
        jumpHeight += 1;
        StartCoroutine(AdrenalineDelay());
    }

    IEnumerator AdrenalineDelay()
    {
        yield return new WaitForSeconds(5);
        moveSpeed = 5;
        jumpHeight = 5;
    }
}