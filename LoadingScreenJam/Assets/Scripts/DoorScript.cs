using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {
	public Transform player;
	public Transform exit;

	public bool playerEntered;
	public bool teleported;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (teleported) {
			teleported = false;
		}

		if (Input.GetButtonDown ("Fire1") && playerEntered) {
			player.position = exit.position;
			player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			player.GetComponent<CharController>().disableInput = true;
			StartCoroutine(Loading ());
			teleported = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.transform == player) {
			playerEntered = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.transform == player) {
			playerEntered = false;
		}

	}
	
	public IEnumerator Loading() {
		yield return new WaitForSeconds (0.5f);
		player.GetComponent<CharController> ().disableInput = false;
	}
}
