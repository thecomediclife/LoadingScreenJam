using UnityEngine;
using System.Collections;

public class HouScript : MonoBehaviour {
	private Animator animtr;
	private float timer;

	public Transform[] dialogueTriggers;

	void Awake() {
		animtr = this.GetComponent<Animator> ();
	}

	// Use this for initialization
	void Start () {

		dialogueTriggers = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) {
			dialogueTriggers[i] = transform.GetChild(i);
		}
	}
	
	// Update is called once per frame
	void Update () {
//		if (animtr.GetBool("Laugh") && Time.time > timer) {
//			animtr.SetBool("Laugh", false);
//		}
	}

	public void Laugh(bool tf, float LR) {
		animtr.SetBool("Laugh", tf);
		SetLR (LR);
		//timer = Time.time + 0.5f;
	}

	public void Run(bool startRun, float LR) {
		animtr.SetBool("Run", startRun);
		SetLR (LR);
	}

	public void Pose(bool tf, float LR) {
		animtr.SetBool ("Pose", tf);
		SetLR (LR);
	}

	public void Dead(bool tf, float LR) {
		animtr.SetBool ("Dead", tf);
		SetLR (LR);
	}

	public void SetLR(float LR) {
		animtr.SetFloat ("LR", LR);
	}

	public void SetLayer(int layer) {
		this.GetComponent<SpriteRenderer> ().sortingOrder = layer;
	}

	public void EnableDialogueTrigger(int index) {
		for (int i = 0; i < dialogueTriggers.Length; i++) {
			if (i != index) {
				dialogueTriggers[i].GetComponent<BoxCollider2D>().enabled = false;
			} else {
				dialogueTriggers[i].GetComponent<BoxCollider2D>().enabled = true;
			}
		}
	}
}
