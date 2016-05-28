using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    //Animator animator;

    public GameObject player;
    Vector3 pos;
    Vector2 offset = Vector2.zero;

	void Start ()
    {
        //animator = GetComponent<Animator>();
	}
	
	void Update ()
    {
        pos.x = player.transform.position.x + offset.x;
        pos.y = 2 + offset.y;
        pos.z = -1;
        transform.position = pos;
    }

    public void Move(Vector2 direction)
    {
        offset = direction;
    }

    public void Zoom(float factor)
    {
        Camera.main.orthographicSize = factor;
    }
}
