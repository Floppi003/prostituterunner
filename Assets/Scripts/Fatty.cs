﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fatty : MonoBehaviour {

	public float unitsPerSecond = 4.5f;
	public float speedVariance = 2.0f;

	private float actualSpeed;

	// Use this for initialization
	void Start () {
		// create a random actual speed
		float randomVariance = Random.Range(0.0f, this.speedVariance);
		randomVariance = randomVariance - (this.speedVariance / 2.0f); // make range halfway negative, halfway positive
		this.actualSpeed = this.unitsPerSecond + randomVariance;
	}

	// Update is called once per frame
	void Update () {
		this.transform.Translate (this.transform.right * this.actualSpeed * Time.deltaTime);
	}
}
