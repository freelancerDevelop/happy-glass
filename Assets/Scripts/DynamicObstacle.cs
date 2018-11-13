using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObstacle : MonoBehaviour {	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0))
            GetComponent<Rigidbody2D>().isKinematic = false;
	}
}
