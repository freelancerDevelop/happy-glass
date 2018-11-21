using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObstacle : MonoBehaviour {	
	// Update is called once per frame
	void Update () {
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().GameStatus == GameStatus.PLAYING)
            GetComponent<Rigidbody2D>().isKinematic = false;
	}
}
