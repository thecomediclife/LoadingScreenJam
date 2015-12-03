using UnityEngine;
using System.Collections;

public class RupeeScript : MonoBehaviour {
	public Transform player;


	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.transform == player) {
			PlayerPrefs.SetInt("Rupee", PlayerPrefs.GetInt("Rupee") + 1);

			DestroyObject(this.gameObject);
		}
	}
}
