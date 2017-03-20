using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
	public GameObject grabonGranny;
	public GameObject grabonFatty;
	public GameObject grabonProstitute;
	public GameObject camera;

	public Lane lane = Lane.Middle; 
	public PlayerNumber playerNumber = PlayerNumber.Player1;
	public float unitsPerSecond = 1.0f;
	public float speedMultiplier = 1.7f;
	//public float womanMultiplier = 0.65f;
	public float prostituteMultiplier = 0.85f;
	public float grannyMultiplier = 0.55f;
	public float fattyMultiplier = 0.7f;
	public float speedTime = 2.5f;
	public float cakeTime = 2.0f;
	public float startTime = 4.0f;
	public float preMovementTime = 1.0f;

	// private variables
	private float speedTimeRemaining = 0.0f;
	private Queue<Woman> women = new Queue<Woman>();
	private int money = 0;
	private int cake = 0;
	private float cakeTimeRemaining = 0.0f;
	private float startXPosition;
	private float goalXPosition;
	private float timeTillStart;

	private Vector3 initialCameraPosition;
	private Vector3 cameraStartLerpPosition;
	private float timePassed = 0.0f;
	private float preMovementTimePassed = 0.0f;
	private float cameraInitialAngle;
	private float cameraStartLerpAngle;
	private bool adjustedCameraOnce = false;

	private AudioPlayer audioPlayer;


	// Use this for initialization
	void Start () {
		this.startXPosition = this.transform.position.x;
		float worldLength = GameObject.Find ("Logic").GetComponent<WorldBuilder> ().getWorldXLength();
		this.goalXPosition = this.startXPosition + worldLength;
		this.timeTillStart = this.startTime;
		this.preMovementTimePassed = this.preMovementTime;
		this.initialCameraPosition = this.camera.transform.position;
		this.cameraInitialAngle = this.camera.transform.localRotation.eulerAngles.x;

		// move camera to end of line and rotate it
		this.camera.transform.position = new Vector3(GameObject.Find ("Logic").GetComponent<WorldBuilder> ().getWorldXLength() +6.0f, this.initialCameraPosition.y, this.initialCameraPosition.z);
		this.cameraStartLerpPosition = this.camera.transform.position;
		this.camera.transform.rotation = Quaternion.Euler(5.0f, this.camera.transform.rotation.eulerAngles.y, this.camera.transform.rotation.eulerAngles.z);

		this.audioPlayer = GameObject.Find ("AudioPlayer").GetComponent<AudioPlayer> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (this.preMovementTimePassed > 0.0f) {
			this.preMovementTimePassed -= Time.deltaTime;
			return;
		}

		if (this.timeTillStart > 0.0f) {
			this.timeTillStart -= Time.deltaTime;

			// make camera lerp
			float cameraXPosition = Mathf.SmoothStep(this.cameraStartLerpPosition.x, this.initialCameraPosition.x, this.timePassed / this.startTime);
			Vector3 cameraPosition = new Vector3 (cameraXPosition, this.camera.transform.position.y, this.camera.transform.position.z);
			//Vector3 cameraPosition = Vector3.Lerp (this.cameraStartLerpPosition, this.initialCameraPosition, this.timePassed / this.startTime);
			this.camera.transform.position = cameraPosition;

			//float cameraAngle = Mathf.Lerp (0.0f, this.cameraInitialAngle, this.timePassed / this.startTime);
			float cameraAngle = Mathf.SmoothStep (0.0f, this.cameraInitialAngle, this.timePassed / this.startTime);
		
			this.camera.transform.rotation = Quaternion.Euler (cameraAngle, this.camera.transform.rotation.eulerAngles.y, this.camera.transform.rotation.eulerAngles.z);


			this.timePassed += Time.deltaTime;

			return;
		} else {
			if (this.adjustedCameraOnce == false) {
				// do only once
				this.camera.transform.position = this.initialCameraPosition;
				this.camera.transform.rotation = Quaternion.Euler (this.cameraInitialAngle, this.camera.transform.rotation.eulerAngles.y, this.camera.transform.rotation.eulerAngles.z);
				this.adjustedCameraOnce = true;
			}
		}


		// decrease times
		this.speedTimeRemaining -= Time.deltaTime;
		this.cakeTimeRemaining -= Time.deltaTime;
	

		// set which keys should move the player left/right
		string leftKey = "j";
		string rightKey = "l";
		string prostituteKey = "u";
		string fatKey = "o";
		string cakeKey = "i";

		if (this.playerNumber == PlayerNumber.Player1) {
			leftKey = "a";
			rightKey = "d";
			prostituteKey = "q";
			fatKey = "e";
			cakeKey = "w";
		}

		//Debug.DrawRay(this.transform.position, this.transform.forward * 5.0f, Color.magenta, 5.0f);
		//Debug.DrawRay(this.transform.position, this.transform.forward * (-1), Color.magenta, 20, true);

		// check for keyboard events to update current lane
		if (Input.GetKeyDown(leftKey)) {
			
			// check if the left side is free
			RaycastHit hit;
			bool allowLaneChange = true;
			if (Physics.Raycast (this.transform.position, this.transform.forward * (-1), out hit, 5.0f)) {
				if (hit.collider.CompareTag ("Player")) {
					// player was hit, so don't allow change of lane
					allowLaneChange = false;
				}
			} 

			if (allowLaneChange) {
				// left arrow key was pressed
				if (this.lane == Lane.Middle) this.lane = Lane.Left;
				if (this.lane == Lane.Right) this.lane = Lane.Middle;
			}
		}

		if (Input.GetKeyDown(rightKey)) {

			// check if the right side is free
			RaycastHit hit;
			bool allowLaneChange = true;
			if (Physics.Raycast (this.transform.position, this.transform.forward * (1), out hit, 5.0f)) {
				if (hit.collider.CompareTag ("Player")) {
					allowLaneChange = false;
				}

			} 

			if (allowLaneChange) {
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

						// play audio
						this.audioPlayer.playKissSound();
					}
				}
			}
		}

		if (Input.GetKeyDown (fatKey)) {
			// check if the current woman is a fatty
			if (this.women.Count > 0) {
				Woman woman = this.women.Peek ();
				if (woman is FatWoman) {
					FatWoman fatty = (FatWoman)woman;
					fatty.loveNeeded--;
					this.audioPlayer.playWomanGroaningSound ();
					if (fatty.loveNeeded <= 0) {
						// she got enough love, get rid of fatty
						this.women.Dequeue();
						this.updateWomenText();
					}
				}
			}
		}

		// check if the cake button was touched
		if (Input.GetKeyDown(cakeKey)) {
			if (this.cake > 0) {
				// "cake" the other player
				GameObject otherPlayer = null;
				if (this.playerNumber == PlayerNumber.Player1) {
					// cake player 2
					otherPlayer = GameObject.Find("player2");
				} else {
					// cake player 2
					otherPlayer = GameObject.Find("player1");
				}

				otherPlayer.GetComponent<Player>().cakePlayer();
				this.cake--;
				this.updateCakeText();

				// play audio
				this.audioPlayer.playCakedSound();
			}
		}

		// check if the next women is a granny
		if (this.women.Count > 0) {
			Woman woman = this.women.Peek();
			if (woman is GrandmaWoman) {
				GrandmaWoman granny = (GrandmaWoman)woman;
				granny.timeNeeded -= Time.deltaTime;

				Debug.Log ("grandmaWoman time left: " + granny.timeNeeded);

				// check if grandma time left is <= 0
				if (granny.timeNeeded <= 0) {
					this.women.Dequeue();
					this.updateWomenText();
				}
			}
		}


		this.updateWomanHUD();
		this.updateCakePlane();


		// update the position based on the current lane
		this.setPositionBasedOnLane();

		// move the player forward
		float realSpeed = this.unitsPerSecond;

		if (this.women.Count > 0) {
			// there is a woman! slow player down
			Woman woman = this.women.Peek();
			realSpeed = realSpeed * woman.womanMultiplier;
		}

		if (this.speedTimeRemaining > 0) { // make player faster if it was on a speeder
			realSpeed = realSpeed * this.speedMultiplier;
		} 

		Debug.Log ("realSpeed: " + realSpeed);
		if (this.transform.position.x < this.goalXPosition) { // only move forward if not at the goal yet
			this.transform.Translate (realSpeed * Time.deltaTime * (-1), 0.0f, 0.0f);
		}

		// check if both players are in the goal
		GameObject theOtherPlayer = null;
		if (this.playerNumber == PlayerNumber.Player1) {
			theOtherPlayer = GameObject.Find("player2");
		} else {
			theOtherPlayer = GameObject.Find("player1");
		}

		if (this.transform.position.x >= this.goalXPosition &&
			theOtherPlayer.transform.position.x >= this.goalXPosition) {

			GameObject canvas = null;
			if (this.playerNumber == PlayerNumber.Player1) {
				canvas = GameObject.Find("CanvasP2");
			} else {
				canvas = GameObject.Find("CanvasP1");
			}

			GameObject playAgainTextUI = canvas.transform.FindChild("PlayAgainText").gameObject;
			playAgainTextUI.GetComponent<Text> ().enabled = true;

			// already at goal! reload scene when pressing space bar
			if (Input.GetKeyDown(KeyCode.Space)) {
				SceneManager.LoadScene("Menu");
			}
		}

		// update position of minimap
		// get percentage of progress
		float totalDistance = this.goalXPosition - this.startXPosition; // distance
		float distanceRun = this.transform.position.x - this.startXPosition; // current progress
		float progress = distanceRun / totalDistance;
		if (this.playerNumber == PlayerNumber.Player1) {
			GameObject.Find ("Logic").GetComponent<MiniMapManager> ().setProgressPlayer1(progress);
		} else {
			GameObject.Find ("Logic").GetComponent<MiniMapManager> ().setProgressPlayer2(progress);
		}
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
			this.transform.position = new Vector3(startPosition.x, startPosition.y, 3.5f);
			break;

		case Lane.Middle:
			this.transform.position = new Vector3(startPosition.x, startPosition.y, 0.0f);
			break;

		case Lane.Right:
			this.transform.position = new Vector3(startPosition.x, startPosition.y, -3.5f);
			break;
		}
	}

	public void cakePlayer() {
		this.cakeTimeRemaining = this.cakeTime;


	}

	private void updateCakePlane() {
		// get the cakePanel GameObject
		GameObject canvas = null;
		if (this.playerNumber == PlayerNumber.Player1) {
			canvas = GameObject.Find ("CanvasP1");
		} else {
			canvas = GameObject.Find ("CanvasP2");
		}

		GameObject cakePanel = canvas.transform.FindChild("CakePanel").gameObject;

		// show or hide the cake panel
		if (this.cakeTimeRemaining > 0.0f) {
			// show cake panel
			cakePanel.GetComponent<Image>().enabled = true;

		} else {
			// hide cake panel
			cakePanel.GetComponent<Image>().enabled = false;
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

	private void updateWomanHUD() {

		GameObject canvas = null;
		if (this.playerNumber == PlayerNumber.Player1) {
			canvas = GameObject.Find ("CanvasP1");
		} else {
			canvas = GameObject.Find ("CanvasP2");
		}

		bool showFatty = false;
		bool showProstitution = false;
		bool showGranny = false;

		if (this.women.Count > 0) {
			Woman woman = this.women.Peek();
			if (woman is ProstituteWoman) {
				showProstitution = true;
				showFatty = false;
				showGranny = false;
			}

			if (woman is FatWoman) {
				showFatty = true;
				showProstitution = false;
				showGranny = false;
			} 

			if (woman is GrandmaWoman) {
				showGranny = true;
				showProstitution = false;
				showFatty = false;
			}
		}

		// show/hide grab ons
		this.grabonFatty.GetComponent<MeshRenderer>().enabled = showFatty;
		this.grabonGranny.GetComponent<MeshRenderer>().enabled = showGranny;
		this.grabonProstitute.GetComponent<MeshRenderer>().enabled = showProstitution;

		if (showFatty == true) {
			// show fat UI
			GameObject fatImageUI = canvas.transform.FindChild ("FattyImage").gameObject;
			Image fatImage = fatImageUI.GetComponent<Image> ();
			fatImage.enabled = true;

			GameObject fatButtonUI = canvas.transform.FindChild ("FattyButton").gameObject;
			Image fatButton = fatButtonUI.GetComponent<Image> ();
			fatButton.enabled = true;

			GameObject fatButtonFrameUI = canvas.transform.FindChild ("FattyButtonFrame").gameObject;
			Image fatButtonFrame = fatButtonFrameUI.GetComponent<Image> ();
			fatButtonFrame.enabled = true;
		
		} else {
			// show fat UI
			GameObject fatImageUI = canvas.transform.FindChild ("FattyImage").gameObject;
			Image fatImage = fatImageUI.GetComponent<Image> ();
			fatImage.enabled = false;

			GameObject fatButtonUI = canvas.transform.FindChild ("FattyButton").gameObject;
			Image fatButton = fatButtonUI.GetComponent<Image> ();
			fatButton.enabled = false;

			GameObject fatButtonFrameUI = canvas.transform.FindChild ("FattyButtonFrame").gameObject;
			Image fatButtonFrame = fatButtonFrameUI.GetComponent<Image> ();
			fatButtonFrame.enabled = false;
		}

		if (showProstitution == true) {
			// show prostitute UI
			GameObject prostituteImageUI = canvas.transform.FindChild("ProstituteImage").gameObject;
			Image prostituteImage = prostituteImageUI.GetComponent<Image>();
			prostituteImage.enabled = true;

			GameObject prostituteButtonUI = canvas.transform.FindChild ("ProstituteButton").gameObject;
			Image prostituteButton = prostituteButtonUI.GetComponent<Image>();
			prostituteButton.enabled = true;

			GameObject prostituteFrameUI = canvas.transform.FindChild ("ProstituteButtonFrame").gameObject;
			Image prostituteFrame = prostituteFrameUI.GetComponent<Image>();
			prostituteFrame.enabled = true;

		} else {
			// show prostitute UI
			GameObject prostituteImageUI = canvas.transform.FindChild("ProstituteImage").gameObject;
			Image prostituteImage = prostituteImageUI.GetComponent<Image>();
			prostituteImage.enabled = false;

			GameObject prostituteButtonUI = canvas.transform.FindChild ("ProstituteButton").gameObject;
			Image prostituteButton = prostituteButtonUI.GetComponent<Image>();
			prostituteButton.enabled = false;

			GameObject prostituteFrameUI = canvas.transform.FindChild ("ProstituteButtonFrame").gameObject;
			Image prostituteFrame = prostituteFrameUI.GetComponent<Image>();
			prostituteFrame.enabled = false;
		}
	}

	private void updateCakeText() {
		GameObject canvas = null;
		if (this.playerNumber == PlayerNumber.Player1) {
			canvas = GameObject.Find ("CanvasP1");
		} else {
			canvas = GameObject.Find ("CanvasP2");
		}

		GameObject cakeTextUI = canvas.transform.FindChild("CakeText").gameObject;

		cakeTextUI.GetComponent<Text> ().text = "" + this.cake;
	}




// PRAGMA MARK: - Trigger

	void OnTriggerEnter(Collider other) {

		// check if it collided with speeder
		if (other.CompareTag ("Speeder")) {
			this.speedTimeRemaining = this.speedTime;

			// Play audio
			this.audioPlayer.playSpeedboostSound ();

			// check if it collided with prostitute
		} else if (other.CompareTag ("Prostitute")) {
			// make player slow again
			this.speedTimeRemaining = 0.0f;
			Destroy (other.gameObject);

			// Add Prostitute to woman queue
			Woman prostitute = new ProstituteWoman (this.prostituteMultiplier);
			this.women.Enqueue (prostitute);
			this.updateWomenText ();

		} else if (other.CompareTag ("Money")) {
			// make random money value
			//int moneyValue = Random.Range (0, 6) * 10 + 70; // range: 70 - 120
			int moneyValue = 100;
			this.money += moneyValue;

			this.updateMoneyText ();

			// destroy money object
			Destroy (other.gameObject);

			// create a cake object where money was
			GameObject logic = GameObject.Find("Logic");
			logic.GetComponent<WorldBuilder> ().createCakeAtLocation (other.transform.position, this.playerNumber);

			// Play audio
			this.audioPlayer.playMoneyCollectedSound();

		} else if (other.CompareTag ("Fatty")) {
			// make player slow again
			this.speedTimeRemaining = 0.0f;
			Destroy (other.gameObject);

			// add Fatty to woman queue
			Woman fatty = new FatWoman (this.fattyMultiplier);
			this.women.Enqueue (fatty);
			this.updateWomenText ();

		} else if (other.CompareTag ("Granny")) {
			//Debug.Log ("collided with granny");
			// make player slow again
			this.speedTimeRemaining = 0.0f;
			Destroy (other.gameObject);

			// add granny to woman queue
			Woman granny = new GrandmaWoman(this.grannyMultiplier);
			this.women.Enqueue (granny);
			this.updateWomenText();
		}

		// cake comparison... first find out which cakes are important
		string importantCakeTag;
		if (this.playerNumber == PlayerNumber.Player1) {
			importantCakeTag = "CakeByP2";
		} else {
			importantCakeTag = "CakeByP1";
		}

		if (other.CompareTag(importantCakeTag)) {
			// collected a cake by the other player! --> instantly cake other player!
			Destroy(other.gameObject);

			// "cake" the other player
			GameObject otherPlayer = null;
			if (this.playerNumber == PlayerNumber.Player1) {
				// cake player 2
				otherPlayer = GameObject.Find("player2");
			} else {
				// cake player 2
				otherPlayer = GameObject.Find("player1");
			}

			otherPlayer.GetComponent<Player>().cakePlayer();
			this.audioPlayer.playCakedSound ();
			/*
			this.cake++;
			this.updateCakeText ();

			// play audio
			this.audioPlayer.playCakeCollectedSound();
			*/
		}
	}

	void OnTriggerExit(Collider other) {
		//Debug.Log ("OnTriggerExit");
	} 

	void OnTriggerStay(Collider other) {
		//Debug.Log ("OnTriggerStay");
	}
}
