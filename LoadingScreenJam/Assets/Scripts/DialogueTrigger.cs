using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueTrigger : MonoBehaviour {
	public Transform image;
	public Text textBox;
	public Image headIcon;
	public bool useHeadIcon;
	public Sprite icon;
	public Transform player;

	public bool autoPlayDialogue = false;
	public bool textAutoPlay = false;
	private CharController charContr;
	public bool dialoguePlaying = false;
	public string colliderTag = "Player";

	public float letterPauseDefault = 0.05f;
	private float letterPause;
	public bool dialogueComplete;
	public int counter = 0;
	public string[] dialogueArray = new string[1];

	private IEnumerator coroutine;
	
	public bool dialogueFinished;

	private float autoPlayTimer;

	public bool pause;

	// Use this for initialization
	void Start () {
		InitVars ();
	}

	void Update() {
		if (dialogueFinished) {
			dialogueFinished = false;
		}

		if (dialoguePlaying) {
			
			if (Input.GetButtonDown("Fire1") && dialogueComplete && !textAutoPlay && !pause) {
				PlayNextLine();
			}
			
			if (Input.GetButtonDown("Fire2") && !textAutoPlay && !pause) {
				if (!dialogueComplete) {
					StopCoroutine(coroutine);
					textBox.text = dialogueArray[counter];
					dialogueComplete = true;
				} else {
					PlayNextLine();
				}
				
			}

			if (textAutoPlay && dialogueComplete && Time.time > autoPlayTimer) {
				PlayNextLine();
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
		headIcon = GameObject.Find ("Canvas").transform.GetChild (7).GetComponent<Image> ();
		player = GameObject.Find ("Player").transform;
		charContr = player.GetComponent<CharController> ();
		letterPause = letterPauseDefault;
	}

	public void PlayDialogue() {
		counter = 0;
		dialoguePlaying = true;
		charContr.disableInput = true;

		textBox.gameObject.SetActive (true);
		image.gameObject.SetActive (true);

		if (useHeadIcon) {
			headIcon.gameObject.SetActive(true);
			headIcon.sprite = icon;
		}

		dialogueComplete = false;
		textBox.GetComponent<Text>().text = "";
		coroutine = TypeDialogue (dialogueArray [counter]);
		StartCoroutine (coroutine);

	}

	public void EndDialogue() {
		charContr.disableInput = false;
		dialoguePlaying = false;
		
		textBox.gameObject.SetActive (false);
		image.gameObject.SetActive (false);

		if (useHeadIcon) {
			headIcon.gameObject.SetActive(false);
		}

		dialogueFinished = true;
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

			if (!textAutoPlay) {
				yield return new WaitForSeconds(letterPause);
			} else {
				yield return new WaitForSeconds(letterPause * 3f);
			}
		}
		dialogueComplete = true;

		autoPlayTimer = Time.time + 1f;
	}
}
