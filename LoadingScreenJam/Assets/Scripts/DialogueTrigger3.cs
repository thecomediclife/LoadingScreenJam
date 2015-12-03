using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueTrigger3 : MonoBehaviour {
	public Transform image, cursorImage;
	public Text textBox, answerbox1, answerbox2, answerbox3, answerbox4;
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
	public string[] questionArray1 = new string[1];
	
	public string[] answerArray = new string[1];
	public string[] response1, response2;
	public string[] dialogueArray2;
	public int answer1 = 0;
	
	private IEnumerator coroutine;

	private string[] currentArray;

	// Use this for initialization
	void Start () {
		InitVars ();
	}
	
	void Update() {
		if (dialoguePlaying) {
			
			if (Input.GetButtonDown("Fire1") && dialogueComplete) {
				PlayNextLine(dialogueArray);
			}
			
			if (Input.GetButtonDown("Fire2")) {
				if (!dialogueComplete) {
					StopCoroutine(coroutine);
					textBox.text = dialogueArray[counter];
					dialogueComplete = true;
				} else {
					PlayNextLine(dialogueArray);
				}
				
			}
		}
	}

	/////////////////////This is where the script first begins the dialogue. The first dialogue array should be inputted here.
	void OnTriggerStay2D(Collider2D other) {
		if (autoPlayDialogue && other.tag == colliderTag) {
			if (!dialoguePlaying) {
				PlayDialogue (dialogueArray);
			}
		} else if (!autoPlayDialogue && other.tag == colliderTag) {
			if (!dialoguePlaying) {
				if (Input.GetButtonDown("Fire1")) {
					PlayDialogue(dialogueArray);
				}
			}
		}
		
	}
	
	void InitVars() {
		image = GameObject.Find ("Canvas").transform.GetChild (0);
		cursorImage = GameObject.Find ("Canvas").transform.GetChild (6);
		textBox = GameObject.Find ("Canvas").transform.GetChild (1).GetComponent<Text> ();
		answerbox1 = GameObject.Find ("Canvas").transform.GetChild (2).GetComponent<Text> ();
		answerbox2 = GameObject.Find ("Canvas").transform.GetChild (3).GetComponent<Text> ();
		answerbox3 = GameObject.Find ("Canvas").transform.GetChild (4).GetComponent<Text> ();
		answerbox4 = GameObject.Find ("Canvas").transform.GetChild (5).GetComponent<Text> ();
		player = GameObject.Find ("Player").transform;
		charContr = player.GetComponent<CharController> ();
		letterPause = letterPauseDefault;
	}
	
	void PlayDialogue(string[] stringArray) {
		dialoguePlaying = true;
		charContr.disableInput = true;
		
		textBox.gameObject.SetActive (true);
		image.gameObject.SetActive (true);

		dialogueComplete = false;
		textBox.GetComponent<Text>().text = "";
		coroutine = TypeDialogue (stringArray [counter]);
		StartCoroutine (coroutine);
	}
	
	void EndDialogue() {
		charContr.disableInput = false;
		
		textBox.gameObject.SetActive (false);
		image.gameObject.SetActive (false);
		this.gameObject.SetActive (false);
	}
	
	void PlayNextLine(string[] stringArray) {
		counter++;
		if (counter >= stringArray.Length) {
			EndDialogue();
		} else {
			dialogueComplete = false;
			textBox.GetComponent<Text>().text = "";
			coroutine = TypeDialogue (stringArray [counter]);
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
