using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour {

	// public variables
	public GameObject worldPart;

	// private variables
	private float worldXProgress = 0.0f;
	private float buildNextWorldPartDelta = 5.0f;

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
		Debug.Log("worldProgress: " + this.worldXProgress + ", playerX: " + player1.transform.position.x);
		if (player1.transform.position.x > (this.worldXProgress - this.buildNextWorldPartDelta) ||
			player2.transform.position.x > (this.worldXProgress - this.buildNextWorldPartDelta)) {
		
			// create a new world-part
			GameObject newWorldPart = Instantiate(this.worldPart);

			/*
			Debug.Log ("localscale: " + this.worldPart.transform.localScale.x);
			Debug.Log ("forward: " + this.worldPart.transform.forward);
			Debug.Log ("right: " + this.worldPart.transform.right);
			Debug.Log ("up: " + this.worldPart.transform.up);
*/

			newWorldPart.transform.position = new Vector3 (this.worldXProgress + 10.0f, this.worldPart.transform.position.y, this.worldPart.transform.position.z);
			this.worldXProgress += 10.0f;
		}
	}
}
