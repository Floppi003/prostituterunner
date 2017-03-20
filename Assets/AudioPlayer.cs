using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

	public AudioSource audioSource;

	public AudioClip cakedSound;
	public AudioClip cakeCollectedSound;
	public AudioClip moneyCollectedSound;
	public AudioClip womanGroaning1;
	public AudioClip womanGroaning2;
	public AudioClip womanGroaning3;
	public AudioClip speedboost;
	public AudioClip kiss1;
	public AudioClip kiss2;
	public AudioClip kiss3;
	public AudioClip notEnoughMoney;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void playCakedSound() {
		this.audioSource.PlayOneShot (this.cakedSound);
	}

	public void playCakeCollectedSound() {
		this.audioSource.PlayOneShot (this.cakeCollectedSound);
	}

	public void playMoneyCollectedSound() {
		this.audioSource.PlayOneShot (this.moneyCollectedSound);
	}

	public void playWomanGroaningSound() {
		int random = Random.Range (0, 3);
		switch (random) {
		case 0:
			this.audioSource.PlayOneShot (this.womanGroaning1);
			break;
		case 1:
			this.audioSource.PlayOneShot (this.womanGroaning2);
			break;
		case 2:
			this.audioSource.PlayOneShot (this.womanGroaning3);
			break;
		default:
			this.audioSource.PlayOneShot (this.womanGroaning1);
			break;
		}
	}

	public void playKissSound() {
		int random = Random.Range (0, 3);
		switch (random) {
		case 0:
			this.audioSource.PlayOneShot (this.kiss1);
			break;
		case 1:
			this.audioSource.PlayOneShot (this.kiss2);
			break;
		case 2:
			this.audioSource.PlayOneShot (this.kiss3);
			break;
		default:
			this.audioSource.PlayOneShot (this.kiss1);
			break;
		}
	}

	public void playSpeedboostSound() {
		this.audioSource.PlayOneShot(this.speedboost);
	}

	public void playNotEnoughMoney() {
		this.audioSource.PlayOneShot (this.notEnoughMoney);
	}
}
