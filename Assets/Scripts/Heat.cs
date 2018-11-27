using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heat : MonoBehaviour {
    public GameObject Smoke;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Liquid"))
        {
            Destroy(Instantiate(Smoke, collision.transform.position, Quaternion.identity),3);
            Destroy(collision.gameObject);

        }
    }
}
