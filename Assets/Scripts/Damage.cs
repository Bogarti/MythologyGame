using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour {

    public int damage;

	void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 8)
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().ApplyDamage(damage);
    }
}
