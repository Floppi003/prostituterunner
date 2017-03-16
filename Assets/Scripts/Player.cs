using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
	public float womanMultiplier = 0.65f;
	public float speedTime = 2.5f;

	// private variables
	private float speedTimeRemaining = 0.0f;
	private Queue<Woman> women = new Queue<Woman>();
	private int money;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// decrease times
		this.speedTimeRemaining -= Time.deltaTime;
	

		// set which keys should move the player left/right
		string leftKey = "j";
		string rightKey = "l";
		string prostituteKey = "u";
		if (this.playerNumber == PlayerNumber.Player1) {
			leftKey = "a";
			rightKey = "d";
			prostituteKey = "q";
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

		if (Input.GetKeyDown (prostituteKey)) {
			// check if the current woman is a prostitute
			if (this.women.Count > 0) {
				Woman woman = this.women.Peek ();
				if (woman is ProstituteWoman) {
					ProstituteWoman prostitute = (ProstituteWoman) woman;
					if (this.money >= prostitute.moneyNeeded) {
						this.money -= prostitute.moneyNeeded; // subtract money
						this.women.Dequeue (); // get rid of prostitute
						this.updateMoneyText(); // update GUI
						this.updateWomenText ();
					}
				}
			}
		}


		// update the position based on the current lane
		this.setPositionBasedOnLane();

		// move the player forward
		float realSpeed = this.unitsPerSecond;

		if (this.women.Count > 0) {
			// there is a woman! slow player down
			realSpeed = realSpeed * this.womanMultiplier;
		}

		if (this.speedTimeRemaining > 0) { // make player faster if it was on a speeder
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

	private void updateMoneyText() {
		// get correct player UI
		GameObject moneyTextUI = null;

		if (this.playerNumber == PlayerNumber.Player1) {
			moneyTextUI = GameObject.Find ("MoneyTextP1");
		} else if (this.playerNumber == PlayerNumber.Player2) {
			moneyTextUI = GameObject.Find ("MoneyTextP2");
		}

		Text moneyTF = moneyTextUI.GetComponent<Text> ();
		moneyTF.text = "" + this.money;
	}

	private void updateWomenText() {
		// get correct player UI
		GameObject womenTextUI = null;

		if (this.playerNumber == PlayerNumber.Player1) {
			womenTextUI = GameObject.Find ("WomenTextP1");
		} else if (this.playerNumber == PlayerNumber.Player2) {
			womenTextUI = GameObject.Find ("WomenTextP2");
		}

		Text womenTF = womenTextUI.GetComponent<Text> ();
		womenTF.text = "" + this.women.Count;
	}




// PRAGMA MARK: - Trigger

	void OnTriggerEnter(Collider other) {
		Debug.Log ("OnTriggerEnter: " + other.name);

		// check if it collided with speeder
		if (other.CompareTag ("Speeder")) {
			this.speedTimeRemaining = this.speedTime;

			// check if it collided with prostitute
		} else if (other.CompareTag ("Prostitute")) {
			// make player slow again
			this.speedTimeRemaining = 0.0f;
			Destroy(other.gameObject);

			// Add Prostitute to woman queue
			Woman prostitute = new ProstituteWoman();
			this.women.Enqueue (prostitute);
			this.updateWomenText ();

		} else if (other.CompareTag ("Money")) {
			// make random money value
			int moneyValue = Random.Range(0, 6) * 10 + 70; // range: 70 - 120
			this.money += moneyValue;

			this.updateMoneyText();

			// destroy money object
			Destroy (other.gameObject);
		}
	}

	void OnTriggerExit(Collider other) {
		Debug.Log ("OnTriggerExit");
	} 

	void OnTriggerStay(Collider other) {
		Debug.Log ("OnTriggerStay");
	}
}
