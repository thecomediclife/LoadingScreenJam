using UnityEngine;
using System.Collections;

public class MayScript : MonoBehaviour {
	private Animator animtr;

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

	}

	public void Cry(bool startCry, float LR) {
		animtr.SetBool ("Cry", startCry);
		SetLR (LR);
	}

	public void SetLR(float LR){
		animtr.SetFloat ("LR", LR);
	}

	public void Run(bool startRun, float LR) {
		animtr.SetBool ("Run", startRun);
		SetLR (LR);
	}

	public void Walk(bool startWalk, float LR) {
		animtr.SetBool ("Walk", startWalk);
		SetLR (LR);
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
