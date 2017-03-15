using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour {

	// public variables
	public GameObject worldPart;
	public GameObject speeder;
	public int speederProbability = 50;

	// private variables
	private float worldXProgress = 0.0f;
	private float buildNextWorldPartDelta = 20.0f;

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


			// make random speederf
			int randomNumber = Random.Range(0, 100);
			//Debug.Log ("randomNumber: " + randomNumber);
			if (randomNumber < this.speederProbability) {
				GameObject speeder = Instantiate (this.speeder);

				// make random lane
				int randomLane = Random.Range(1, 4);

				speeder.transform.position = new Vector3 (this.worldXProgress, this.speeder.transform.position.y, (randomLane - 2) * (3.0f));
			}
		}
	}
}
