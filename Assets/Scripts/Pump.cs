using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pump : MonoBehaviour {

    public GameObject Droplet;
    public int AmountOfWater;
    // Use this for initialization
	IEnumerator xaNuocTuTu()
    {
        for (int i = 0; i < AmountOfWater; i++)
        {
            GameObject obj=Instantiate(Droplet, transform.position + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f)), Quaternion.identity, transform);
            
            obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Sin(transform.eulerAngles.z*Mathf.PI/180), -Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180))*1.2f);
            yield return new WaitForSeconds(0.02f);
        }
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().CountDown();
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0) && transform.childCount == 0)
        {
            StartCoroutine(xaNuocTuTu());
        }
    }
}
