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

	public int speederProbability = 50;
	public int womanProbability = 70;
	public int moneyProbability = 30;

	// private variables
	private float worldXProgress = 0.0f;
	private float buildNextWorldPartDelta = 20.0f;
	private ArrayList leftWomens = new ArrayList();
	private ArrayList middleWomens = new ArrayList();
	private ArrayList rightWomens = new ArrayList();

	// Use this for initialization
	void Start () {
		this.worldXProgress = this.worldPart.transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		// get the position of both players and check if new world parts should be created
		GameObject player1 = GameObject.Find("player1");
		GameObject player2 = GameObject.Find ("player2");

		// x-Axis is the axis where the player moves forward
		if (player1.transform.position.x > (this.worldXProgress - this.buildNextWorldPartDelta) ||
			player2.transform.position.x > (this.worldXProgress - this.buildNextWorldPartDelta)) {
		
			// create a new world-part
			GameObject newWorldPart = Instantiate(this.worldPart);

			// try to load new texture on the image
			Texture2D testTexture = Resources.Load("regularshow") as Texture2D;
			Material testMaterial = new Material (Shader.Find("Standard"));
			testMaterial.mainTexture = testTexture;
			GameObject wallLeft = newWorldPart.transform.Find ("wall_left").gameObject;
			GameObject wallRight = newWorldPart.transform.Find ("wall_right").gameObject;
			//Debug.Log ("wallleft: " + wallLeft);
			wallRight.GetComponentInParent<MeshRenderer> ().material = testMaterial;
			wallLeft.GetComponent<MeshRenderer> ().material = testMaterial;

			newWorldPart.transform.position = new Vector3 (this.worldXProgress + 10.0f, this.worldPart.transform.position.y, this.worldPart.transform.position.z);
			this.worldXProgress += 10.0f;


			// make random speeder
			int randomNumber = Random.Range(0, 100);
			int speederLane = 0;
			int speederX;
			if (randomNumber < this.speederProbability) {
				GameObject speeder = Instantiate (this.speeder);

				// make random lane
				speederLane = Random.Range(1, 4);

				// make random x variance
				speederX = Random.Range(0, 9) - 4;

				speeder.transform.position = new Vector3 (this.worldXProgress + speederX, this.speeder.transform.position.y, (speederLane - 2) * (3.0f));
				speeder.name = "Speeder";
			}

			int randomMoney = Random.Range (0, 100);
			if (randomMoney < this.moneyProbability) {
				GameObject money = Instantiate (this.money);

				// make random lane
				int moneyLane = Random.Range(1, 4);
				while (moneyLane == speederLane) {
					moneyLane = Random.Range(1, 4); // money should not be on same lane as speeder
				}

				// make random x variance
				int xVariance = Random.Range(0, 9) - 4;

				money.transform.position = new Vector3 (this.worldXProgress + xVariance, this.money.transform.position.y, (moneyLane - 2) * 3.0f);
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

				} else if (womanType == 1) {
					// make granny
					woman = Instantiate(this.granny);

				} else {
					// make fatty
					woman = Instantiate(this.fatty);
				}

				// place woman on random lane
				int lane = Random.Range(0, 3);
				if (lane == 0) {
					// left lane
					woman.transform.position = new Vector3(this.worldXProgress, this.prostitute.transform.position.y, -3.0f);
				} else if (lane == 1) {
					// middle lane
					woman.transform.position = new Vector3(this.worldXProgress, this.prostitute.transform.position.y, 0.0f);

				} else {
					// right lane
					woman.transform.position = new Vector3(this.worldXProgress, this.prostitute.transform.position.y, 3.0f);
				}
			}
		}
	}
}
