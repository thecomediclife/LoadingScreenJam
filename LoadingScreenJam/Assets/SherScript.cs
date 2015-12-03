using UnityEngine;
using System.Collections;

public class SherScript : MonoBehaviour {
	private float timer;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (this.GetComponent<Animator> ().GetBool ("Shrug") && Time.time > timer) {
			this.GetComponent<Animator>().SetBool("Shrug", false);
		}

	}

	public void Shrug() {
		this.GetComponent<Animator>().SetBool("Shrug", true);
		timer = Time.time + 0.5f;
	}
}
