using UnityEngine;
using System.Collections;

public class DialogueTrigger2 : MonoBehaviour {

	public bool autoPlayDialogue;
	public string colliderTag = "Player";
	public DialogueManagerScript dmScript;

	public bool dialoguePlaying = false;

	public int counter;
	public string[] dialogueArray = new string[1];

	// Use this for initialization
	void Start () {
		dmScript = GameObject.Find ("DialogueManager").GetComponent<DialogueManagerScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (dialoguePlaying && counter < dialogueArray.Length) {
			if (dmScript.PlayDialogue (dialogueArray [counter])) {
				counter++;
			}
		} else if (dialoguePlaying && counter >= dialogueArray.Length) {
			this.gameObject.SetActive(false);
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (autoPlayDialogue && other.tag == colliderTag) {
			if (!dialoguePlaying) {
				dialoguePlaying = true;
			}
		} else if (!autoPlayDialogue && other.tag == colliderTag) {
			if (!dialoguePlaying) {
				if (Input.GetButtonDown("Fire1")) {
					dialoguePlaying = true;
				}
			}
		}
		
	}
}
