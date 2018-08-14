using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feetCtrl : MonoBehaviour {

    GameObject player;

	void Start () {
        player = transform.parent.gameObject;
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("MovingPlatform"))
        {
            player.transform.parent = other.transform.parent.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("MovingPlatform"))
        {
            player.transform.parent = null;
        }
    }
}
