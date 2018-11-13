using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
        float size = GetComponent<Camera>().orthographicSize;
        transform.GetChild(0).GetComponent<Camera>().orthographicSize = size;
        transform.GetChild(1).localScale = new Vector3(size * 2, size * 2, size * 2);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
