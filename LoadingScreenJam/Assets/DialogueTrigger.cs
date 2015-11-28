using UnityEngine;
using System.Collections;

public class DialogueTrigger : MonoBehaviour {
	public Transform image;
	public Transform textBox;
	public Transform player;

	public bool autoPlayDialogue = false;
	private CharController charContr;
	public bool dialoguePlaying = false;
	public string colliderTag = "Player";

	public string[] dialogueArray;

	// Use this for initialization
	void Start () {
		InitVars ();
	}

	void OnTriggerStay2D(Collider2D other) {
		if (autoPlayDialogue && other.tag == colliderTag) {

			if (!dialoguePlaying) {

				PlayDialogue ();

			} else {
				if (Input.GetButtonDown("Fire1")) {

					//ContinueDialogue();

				}
			}

		} else if (!autoPlayDialogue && other.tag == colliderTag) {

			if (!dialoguePlaying) {
				if (Input.GetButtonDown("Fire1")) {
					PlayDialogue();
				}
			} else {
				if (Input.GetButtonDown("Fire1")) {

				}

				if (Input.GetButtonDown("Fire2")) {

				}
			}

		}

	}

	void InitVars() {
		image = GameObject.Find ("Image").transform;
		textBox = GameObject.Find ("TextBox").transform;
		player = GameObject.Find ("Player").transform;
		charContr = player.GetComponent<CharController> ();
	}

	void PlayDialogue() {
		dialoguePlaying = true;
		charContr.disableInput = true;
	}
}
