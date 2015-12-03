using UnityEngine;
using System.Collections;

public class HouScript : MonoBehaviour {
	private Animator animt;
	private float timer;
	// Use this for initialization
	void Start () {
		animt = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (animt.GetBool("Laugh") && Time.time > timer) {
			animt.SetBool("Laugh", false);
		}

	}

	public void Laugh() {
		animt.SetBool("Laugh", true);
		timer = Time.time + 0.5f;
	}

	public void StartRun() {
		animt.SetBool("Run", true);
	}

	public void StopRun() {
		animt.SetBool("Run", false);
	}
}
