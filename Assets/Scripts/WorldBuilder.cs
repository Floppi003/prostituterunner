using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour {

	// public variables
	public GameObject worldPart;
	public GameObject speeder;
	public GameObject prostitute;
	public GameObject granny;
	public GameObject fatty;
	public GameObject money;
	public GameObject cake;
	public GameObject trashbagLeft;
	public GameObject trashbagRight;
	public GameObject trashcanLeft;
	public GameObject trashcanRight;

	public int numberOfWorldParts = 100;
	public int speederProbability = 50;
	public int womanProbability = 70;
	public int moneyProbability = 30;
	public int trashcanProbability = 100;
	public int trashbagProbability = 100;

	// private variables
	private float worldXProgress = 0.0f;
	private float buildNextWorldPartDelta = 60.0f;
	private int numberOfWorldPartsCreated = 0;

	/*
	private ArrayList leftWomens = new ArrayList();
	private ArrayList middleWomens = new ArrayList();
	private ArrayList rightWomens = new ArrayList();
	*/
	
	// Use this for initialization
	void Start () {
		this.worldXProgress = this.worldPart.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		// get the position of both players and check if new world parts should be created

		if (this.numberOfWorldPartsCreated >= (this.numberOfWorldParts + 4)) {
			return;
		}

		GameObject player1 = GameObject.Find("player1");
		GameObject player2 = GameObject.Find ("player2");

		// x-Axis is the axis where the player moves forward
		if (player1.transform.position.x > (this.worldXProgress - this.buildNextWorldPartDelta) ||
			player2.transform.position.x > (this.worldXProgress - this.buildNextWorldPartDelta)) {
		
			// create a new world-part
			GameObject newWorldPart = Instantiate(this.worldPart);

			// try to load new texture on the image
			int leftTextureID = Random.Range(0, 2);
			int rightTextureID = Random.Range(0, 2);

			Texture2D leftTexture;
			Texture2D rightTexture;

			if (leftTextureID == 0) {
				leftTexture = Resources.Load ("house_left_01") as Texture2D;
			} else {
				leftTexture = Resources.Load ("house_left_02") as Texture2D;
			}

			if (rightTextureID == 0) {
				rightTexture = Resources.Load ("house_right_01") as Texture2D;
			} else {
				rightTexture = Resources.Load ("house_right_02") as Texture2D;
			}

			Material leftMaterial = new Material (Shader.Find("Standard"));
			leftMaterial.mainTexture = leftTexture;

			Material rightMaterial = new Material (Shader.Find ("Standard"));
			rightMaterial.mainTexture = rightTexture;

			//Material testMaterial = new Material (Shader.Find("Standard"));
			//testMaterial.mainTexture = testTexture;
			GameObject wallLeft = newWorldPart.transform.Find ("Houses_L").gameObject;
			GameObject wallRight = newWorldPart.transform.Find ("Houses_R").gameObject;
			//Debug.Log ("wallleft: " + wallLeft);
			wallRight.GetComponentInParent<MeshRenderer> ().material = rightMaterial;
			wallLeft.GetComponent<MeshRenderer> ().material = leftMaterial;

			newWorldPart.transform.position = new Vector3 (this.worldXProgress + 16.0f, this.worldPart.transform.position.y, this.worldPart.transform.position.z);
			this.worldXProgress += 16.0f;
			this.numberOfWorldPartsCreated++;

			// no items or women anymore after reaching goal
			if (this.numberOfWorldPartsCreated >= (this.numberOfWorldParts + 2)) {
				return;	
			}


			// make random speeder
			int randomNumber = Random.Range(0, 100);
			int speederLane = 0;
			int speederX;
			if (randomNumber < this.speederProbability) {
				GameObject speeder = Instantiate (this.speeder);

				// make random lane
				speederLane = Random.Range(1, 4);

				// make random x variance
				speederX = Random.Range(0, 15) - 7;

				speeder.transform.position = new Vector3 (this.worldXProgress + speederX, this.speeder.transform.position.y, (speederLane - 2) * (3.5f));
				speeder.name = "Speeder";
			}

			// make random money
			int randomMoney = Random.Range (0, 100);
			if (randomMoney < this.moneyProbability) {
				GameObject money = Instantiate (this.money);

				// make random lane
				int moneyLane = Random.Range(1, 4);
				while (moneyLane == speederLane) {
					moneyLane = Random.Range(1, 4); // money should not be on same lane as speeder
				}

				// make random x variance
				int xVariance = Random.Range(0, 15) - 7;

				money.transform.position = new Vector3 (this.worldXProgress + xVariance, this.money.transform.position.y, (moneyLane - 2) * 3.5f);
			}


			// make random women
			int createWoman = Random.Range (0, 100);
			if (createWoman < this.womanProbability) {
				// create random woman type
				GameObject woman;
				int womanType = Random.Range(0, 3);
				if (womanType == 0) {
					// make prostitute
					woman = Instantiate(this.prostitute);
					woman.GetComponentInChildren<Prostitute>(true).enabled = true;

				} else if (womanType == 1) {
					// make granny
					woman = Instantiate(this.granny);
					woman.GetComponentInChildren<Granny>(true).enabled = true;

				} else {
					// make fatty
					woman = Instantiate(this.fatty);
					woman.GetComponentInChildren<Fatty> (true).enabled = true;
				}

				// place woman on random lane
				// make random x variance
				int xVariance = Random.Range(0, 15) - 7;

				int lane = Random.Range(0, 3);
				if (lane == 0) {
					// left lane
					woman.transform.position = new Vector3(this.worldXProgress + xVariance, this.prostitute.transform.position.y, -3.5f);
				} else if (lane == 1) {
					// middle lane
					woman.transform.position = new Vector3(this.worldXProgress + xVariance, this.prostitute.transform.position.y, 0.0f);

				} else {
					// right lane
					woman.transform.position = new Vector3(this.worldXProgress + xVariance, this.prostitute.transform.position.y, 3.5f);
				}
			}



			float xLeftTaken = 0.0f;
			float xRightTaken = 0.0f;
			// make random trashbags on left side
			int createTrashbag = Random.Range(0, 100);
			if (createTrashbag < this.trashbagProbability) {
				GameObject trashbag = Instantiate (this.trashbagLeft);

				// make random x variance and place the trashbag
				int xVariance = Random.Range(0, 15) - 7;
				trashbag.transform.position = new Vector3(this.worldXProgress + xVariance, trashbag.transform.position.y, trashbag.transform.position.z);
				xLeftTaken = this.worldXProgress + xVariance;
			}

			// make random trashbags on right side
			createTrashbag = Random.Range(0, 100);
			if (createTrashbag < this.trashbagProbability) {
				GameObject trashbag = Instantiate (this.trashbagRight);

				// make random x variance and place the trashbag
				int xVariance = Random.Range(0, 15) - 7;
				trashbag.transform.position = new Vector3(this.worldXProgress + xVariance, trashbag.transform.position.y, trashbag.transform.position.z);
				xRightTaken = this.worldXProgress + xVariance;
			}

			// make random trashcan on left side
			int createTrashcan = Random.Range(0, 100);
			if (createTrashcan < this.trashcanProbability) {
				GameObject trashcan = Instantiate (this.trashcanLeft);

				// make random x variance and place the trashcan
				int xVariance = Random.Range(0, 15) - 7;
				while (this.worldXProgress + xVariance > xLeftTaken - 1.0f &&
				       this.worldXProgress + xVariance < xLeftTaken + 1.0f) {
					xVariance = Random.Range(0, 15) - 7;
				}

				trashcan.transform.position = new Vector3(this.worldXProgress + xVariance, trashcan.transform.position.y, trashcan.transform.position.z);
			}

			// make random trashcan on right side
			createTrashcan = Random.Range(0, 100);
			if (createTrashcan < this.trashcanProbability) {
				GameObject trashcan = Instantiate (this.trashcanRight);

				// make random x variance and place the trashcan
				int xVariance = Random.Range(0, 15) - 7;
				while (this.worldXProgress + xVariance > xRightTaken - 1.0f &&
				       this.worldXProgress + xVariance < xRightTaken + 1.0f) {
					xVariance = Random.Range(0, 15) - 7;
				}
				trashcan.transform.position = new Vector3(this.worldXProgress + xVariance, trashcan.transform.position.y, trashcan.transform.position.z);
			}
		}
	}



// PRAGMA MARK: Custom Functions

	public void createCakeAtLocation(Vector3 location, Player.PlayerNumber spawnedByPlayer) {
		GameObject cake = Instantiate(this.cake);
		cake.transform.position = location;

		Debug.Log ("creating cake spawned by player " + spawnedByPlayer);

		if (spawnedByPlayer == Player.PlayerNumber.Player1) { 
			cake.tag = "CakeByP1";
		} else if (spawnedByPlayer == Player.PlayerNumber.Player2) {
			cake.tag = "CakeByP2";
		}
	}

	public float getWorldXLength() {
		return this.numberOfWorldParts * 16.0f;
	}
}
