using UnityEngine;
using System.Collections;

public class RupeeScript : MonoBehaviour {
	public Transform player;
	public DoorScript door;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player").transform;
		door = GameObject.Find ("DoorGrassExit").GetComponent<DoorScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (door.teleported) {
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.transform == player) {
			PlayerPrefs.SetInt("Rupee", PlayerPrefs.GetInt("Rupee") + 1);

			DestroyObject(this.gameObject);
		}
	}
}
