using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapManager : MonoBehaviour {

	private float miniMapStartXPosition;
	private float miniMapEndXPosition;

	public GameObject positionP1;
	public GameObject positionP2;

	// Use this for initialization
	void Start () {
		this.miniMapStartXPosition = this.positionP1.transform.position.z;
		this.miniMapEndXPosition = this.miniMapStartXPosition + 4.8f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setProgressPlayer1(float progress) {
		float distance = this.miniMapEndXPosition - this.miniMapStartXPosition;
		float newPosition = this.miniMapStartXPosition + (distance * progress);
		Debug.Log ("new Position: " + newPosition);
		this.positionP1.transform.position = new Vector3(this.positionP1.transform.position.x, this.positionP1.transform.position.y, newPosition);
	}

	public void setProgressPlayer2(float progress) {
		float distance = this.miniMapEndXPosition - this.miniMapStartXPosition;
		float newPosition = this.miniMapStartXPosition + (distance * progress);
		this.positionP2.transform.position = new Vector3(this.positionP2.transform.position.x, this.positionP2.transform.position.y, newPosition);
	}
}
