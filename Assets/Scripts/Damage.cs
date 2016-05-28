using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour {

    public int damage;
    PlayerController playerCtrl;

    void Start()
    {
        playerCtrl = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {

    }

	void OnCollisionEnter2D(Collision2D col)
    {
        if (gameObject.layer != 8 && gameObject.layer != 10 && col.gameObject.layer == 8)
            playerCtrl.ApplyDamage(damage);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (gameObject.layer != 8 && gameObject.layer != 10 && col.gameObject.layer == 8)
            playerCtrl.ApplyDamage(damage);

        if (gameObject.layer == 10 && col.gameObject.layer == 9)
        {
            col.gameObject.GetComponent<EnemyController>().ApplyDamage(damage);
            col.gameObject.transform.root.gameObject.GetComponent<EnemyController>().ApplyDamage(damage);
        }
    }
}
