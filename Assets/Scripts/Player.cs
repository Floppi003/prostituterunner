using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// enums 
	public enum Lane {
		Left = 0,
		Middle = 1,
		Right = 2
	}

	public enum PlayerNumber {
		Player1 = 0,
		Player2 = 1
	}




	// public variables
	public Lane lane = Lane.Middle; 
	public PlayerNumber playerNumber = PlayerNumber.Player1;
	public float unitsPerSecond = 1.0f;
	public float speedMultiplier = 1.7f;
	public float speedTime = 2.5f;

	// private variables
	private float speedTimeRemaining = 0.0f;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// decrease times
		this.speedTimeRemaining -= Time.deltaTime;
	

		// set which keys should move the player left/right
		string leftKey = "left";
		string rightKey = "right";
		if (this.playerNumber == PlayerNumber.Player1) {
			leftKey = "a";
			rightKey = "d";
		}

		//Debug.DrawRay(this.transform.position, this.transform.forward * 5.0f, Color.magenta, 5.0f);
		//Debug.DrawRay(this.transform.position, this.transform.forward * (-1), Color.magenta, 20, true);

		// check for keyboard events to update current lane
		if (Input.GetKeyDown(leftKey)) {
			
			// check if the left side is free
			RaycastHit hit;
			if (Physics.Raycast (this.transform.position, this.transform.forward * (-1), out hit, 5.0f)) {
				Debug.Log ("Left side is taken!");
			} else {
				// left arrow key was pressed
				if (this.lane == Lane.Middle) this.lane = Lane.Left;
				if (this.lane == Lane.Right) this.lane = Lane.Middle;
			}
		}

		if (Input.GetKeyDown(rightKey)) {
			
			// check if the right side is free
			RaycastHit hit;
			if (Physics.Raycast (this.transform.position, this.transform.forward * (1), out hit, 5.0f)) {
				Debug.Log ("Right side is taken!");

			} else {
				// right arrow key was pressed
				if (this.lane == Lane.Middle) this.lane = Lane.Right;
				if (this.lane == Lane.Left) this.lane = Lane.Middle;
			}
		}


		// update the position based on the current lane
		this.setPositionBasedOnLane();

		// move the player forward
		float realSpeed = this.unitsPerSecond;
		if (this.speedTimeRemaining > 0) {
			realSpeed = realSpeed * this.speedMultiplier;
		}
		this.transform.Translate(realSpeed * Time.deltaTime * (-1), 0.0f, 0.0f);
	}

	void Awake() {
		// allowed to talk to other objects here already
		this.setPositionBasedOnLane();
	}




// PRAGMA MARK: - Custom Functions

	private void setPositionBasedOnLane() {
		// set initial position of object
		Vector3 startPosition = this.transform.position;

		switch (this.lane) {
		case Lane.Left:
			this.transform.position = new Vector3(startPosition.x, startPosition.y, 3.0f);
			break;

		case Lane.Middle:
			this.transform.position = new Vector3(startPosition.x, startPosition.y, 0.0f);
			break;

		case Lane.Right:
			this.transform.position = new Vector3(startPosition.x, startPosition.y, -3.0f);
			break;
		}
	}




// PRAGMA MARK: - Trigger

	void OnTriggerEnter(Collider other) {
		Debug.Log ("OnTriggerEnter: " + other.name);
		if (other.name.Equals("Speeder(Clone)")) {
			this.speedTimeRemaining = this.speedTime;
		}
	}

	void OnTriggerExit(Collider other) {
		Debug.Log ("OnTriggerExit");
	} 

	void OnTriggerStay(Collider other) {
		Debug.Log ("OnTriggerStay");
	}
}
