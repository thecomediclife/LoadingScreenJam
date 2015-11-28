using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueTrigger : MonoBehaviour {
	public Transform image;
	public Text textBox;
	public Transform player;

	public bool autoPlayDialogue = false;
	private CharController charContr;
	private bool dialoguePlaying = false;
	public string colliderTag = "Player";

	public float letterPauseDefault = 0.05f;
	private float letterPause;
	private bool dialogueComplete;
	private int counter = 0;
	public string[] dialogueArray = new string[1];

	private IEnumerator coroutine;

	// Use this for initialization
	void Start () {
		InitVars ();
	}

	void Update() {
		if (dialoguePlaying) {
			
			if (Input.GetButtonDown("Fire1") && dialogueComplete) {
				PlayNextLine();
			}
			
			if (Input.GetButtonDown("Fire2")) {
				if (!dialogueComplete) {
					StopCoroutine(coroutine);
					textBox.text = dialogueArray[counter];
					dialogueComplete = true;
				} else {
					PlayNextLine();
				}
				
			}
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (autoPlayDialogue && other.tag == colliderTag) {
			if (!dialoguePlaying) {
				PlayDialogue ();
			}
		} else if (!autoPlayDialogue && other.tag == colliderTag) {
			if (!dialoguePlaying) {
				if (Input.GetButtonDown("Fire1")) {
					PlayDialogue();
				}
			}
		}

	}

	void InitVars() {
		image = GameObject.Find ("Canvas").transform.GetChild (0);
		textBox = GameObject.Find ("Canvas").transform.GetChild (1).GetComponent<Text> ();
		player = GameObject.Find ("Player").transform;
		charContr = player.GetComponent<CharController> ();
		letterPause = letterPauseDefault;
	}

	void PlayDialogue() {
		dialoguePlaying = true;
		charContr.disableInput = true;

		textBox.gameObject.SetActive (true);
		image.gameObject.SetActive (true);

		dialogueComplete = false;
		textBox.GetComponent<Text>().text = "";
		coroutine = TypeDialogue (dialogueArray [counter]);
		StartCoroutine (coroutine);
	}

	void EndDialogue() {
		charContr.disableInput = false;
		
		textBox.gameObject.SetActive (false);
		image.gameObject.SetActive (false);
		this.gameObject.SetActive (false);
	}

	void PlayNextLine() {
		counter++;
		if (counter >= dialogueArray.Length) {
			EndDialogue();
		} else {
			dialogueComplete = false;
			textBox.GetComponent<Text>().text = "";
			coroutine = TypeDialogue (dialogueArray [counter]);
			StartCoroutine (coroutine);
		}
	}

	public IEnumerator TypeDialogue(string dialogue) {
		foreach (char letter in dialogue.ToCharArray()) {
			textBox.text += letter;

			yield return new WaitForSeconds(letterPause);
		}
		dialogueComplete = true;
	}
}
