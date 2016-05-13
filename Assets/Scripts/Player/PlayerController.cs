using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float maxSpeed = 4;
    public float jumpForce = 550;

    [HideInInspector]
    public bool lookingRight = true;

    public LayerMask groundLayers;
    public Transform groundCheck;
    private bool isGrounded;

    private bool jump = false;

    private Rigidbody2D rigbody;
    private Animator animator;

    public static int health = 100;

	void Start ()
    {
        rigbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
	}
	
	void Update ()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
            jump = true;
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        animator.SetFloat("speed", Mathf.Abs(horizontal));
        Camera.main.GetComponent<Animator>().SetFloat("Direction", horizontal);
        rigbody.velocity = new Vector2(horizontal * maxSpeed, rigbody.velocity.y);
        if ((horizontal > 0 && !lookingRight) || (horizontal < 0 && lookingRight))
            Flip();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundLayers);
        animator.SetBool("isGrounded", isGrounded);

        if (jump)
        {
            rigbody.AddForce(new Vector2(0, jumpForce));
            jump = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Death")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if(col.tag == "CameraEvent")
        {
            Camera.main.GetComponent<CameraController>().Move(new Vector2(0, 1));
        }
    }

    public void Flip()
    {
        lookingRight = !lookingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void ApplyDamage(int damage)
    {
        if (health - damage > 0)
        {
            health -= damage;
        }
        else
        {
            health = 100;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
