using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpforce;
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;
    private bool powerup = false;
    public GameObject bulletPrefab;
    public Transform spawnLocation;
    private float firerate = 1f;
    private float nextfire = 0f;
    private bool facingRight;
    private float bulletSpeed = 5f;
    public GameObject gameOverScreen;
    private bool isGameOver;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        } else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            Jump();
        }

        if(Input.GetMouseButton(0) && powerup && (Time.time > nextfire))
        {
            Attack();
        }

        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpforce);
        anim.SetTrigger("jump");
        grounded = false;
    }

    private void Attack()
    {
        nextfire = Time.time + firerate;
        anim.SetTrigger("attack");
        float horizontalInput = Input.GetAxis("Horizontal");

        GameObject bullet = Instantiate(bulletPrefab, spawnLocation.position, Quaternion.identity);

        if (horizontalInput > 0.01f)
        {
            facingRight = true;
        } else if (horizontalInput < -0.01f)
        {
            facingRight = false;
        }

        if (facingRight == true)
        {
            bullet.transform.position += new Vector3(1f, 0f, 0f);
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (facingRight == false)
        {
            bullet.transform.position += new Vector3(-1f, 0f, 0f);
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }

        Rigidbody2D bulletRigidBody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidBody.velocity = bullet.transform.right * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }

        if(collision.gameObject.tag == "PowerUp")
        {
            powerup = true;
        }
        if(collision.gameObject.tag == "Death")
        {
            Destroy(gameObject);
            Debug.Log("You are dead.:");
        }
        if(collision.gameObject.tag == "Finish")
        {
            SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
        }
    }
}
