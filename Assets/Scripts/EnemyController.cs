using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    
    private Rigidbody2D rigbody;
    private Animator animator;

    public int maxHealth = 100;
    public int health = 100;
    
    void Start ()
    {
        rigbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
	
	void Update ()
    {
	    
	}

    public void ApplyDamage(int damage)
    {
        if (health - damage > 0)
        {
            health -= damage;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
