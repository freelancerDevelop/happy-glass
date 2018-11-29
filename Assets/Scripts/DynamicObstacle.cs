using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObstacle : MonoBehaviour {	
	// Update is called once per frame
	void Update () {
        if (GameManager.GameStatus == GameStatus.PLAYING)
            GetComponent<Rigidbody2D>().isKinematic = false;
	}
}
