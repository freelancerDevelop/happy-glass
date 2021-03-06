﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour {
    public PolygonCollider2D PolygonInsideCup;
    public ContactFilter2D LiquidFilter;
    Collider2D[] res = new Collider2D[1000];
	void Update () {
        if (GameManager.GameStatus==GameStatus.PLAYING)
            GetComponent<Rigidbody2D>().isKinematic = false;

        int count= Physics2D.OverlapCollider(PolygonInsideCup, LiquidFilter, res);        
        if (count > 0 && transform.GetChild(0).gameObject.active)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        if (count >= 30 && transform.GetChild(1).gameObject.active)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().DayNuoc(transform.position);
        }
    }
}
