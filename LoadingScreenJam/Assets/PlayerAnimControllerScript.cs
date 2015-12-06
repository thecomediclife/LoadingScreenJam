using UnityEngine;
using System.Collections;

public class PlayerAnimControllerScript : MonoBehaviour {
	public CharController charContr;
	public Animator animtr;

	private bool lastDisableInput = false;

	// Use this for initialization
	void Start () {
		charContr = this.GetComponent<CharController> ();
		animtr = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!lastDisableInput && charContr.disableInput) {
			animtr.SetBool("Run", false);
		}

		lastDisableInput = charContr.disableInput;

		if (!charContr.disableInput) {
			if (Mathf.Abs (Input.GetAxis ("Horizontal")) > 0.1f || Mathf.Abs (Input.GetAxis ("Vertical")) > 0.1f) {
				animtr.SetBool ("Run", true);
			} else {
				animtr.SetBool ("Run", false);
			}

			if (charContr.forwardDir.x == -1f) {
				SetLR(0f);
			} else if (charContr.forwardDir.x == 1f) {
				SetLR(1f);
			}
		}
	}

	public void SetLR(float LR) {
		animtr.SetFloat ("LR", LR);
	}

	public void Run(bool startRun, float LR) {
		animtr.SetBool ("Run", startRun);
		SetLR (LR);
	}
}
