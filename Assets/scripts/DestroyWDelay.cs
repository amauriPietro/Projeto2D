using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWDelay : MonoBehaviour {

    public float delay = 3f;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, delay);		
	}
}
