using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float maxSpeed = 4;
    public float jumpForce = 550;

    [HideInInspector]
    public bool lookingRight = true;

    public LayerMask groundLayers;
    public Transform groundCheck;
    private bool isGrounded;

    private bool jump = false;

    private Rigidbody2D rigidbody;
    private Animator animator;

	void Start ()
    {
        rigidbody = GetComponent<Rigidbody2D>();
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
        rigidbody.velocity = new Vector2(horizontal * maxSpeed, rigidbody.velocity.y);
        if ((horizontal > 0 && !lookingRight) || (horizontal < 0 && lookingRight))
            Flip();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundLayers);
        animator.SetBool("isGrounded", isGrounded);

        if (jump)
        {
            rigidbody.AddForce(new Vector2(0, jumpForce));
            jump = false;
        }
    }

    public void Flip()
    {
        lookingRight = !lookingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
