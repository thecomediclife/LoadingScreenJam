using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopQuestionTrigger : MonoBehaviour {
	//Play first dialogue array begun, next, figure out how to end with a question.
	
	public Transform image, cursorImage;
	public Text textBox, answerbox1, answerbox2, answerbox3, answerbox4;
	public Transform player;

	public Transform shield, shroom, shopkeeper;
	
	public bool autoPlayDialogue = false;
	private CharController charContr;
	private bool dialoguePlaying = false;
	public string colliderTag = "Player";
	
	public float letterPauseDefault = 0.05f;
	private float letterPause;
	private int counter;
	private bool lineComplete;
	
	private IEnumerator coroutine;
	
	private string[] currentTextArray;
	public string[] firstDialogueArray;
	public string[] answerArray = new string[4];
	public string[] answerArray2 = new string[4];
	public string[] response1Array;
	public string[] response2Array;
	public string[] response3Array, response4Array;
	public string[] shopSuccessArray;
	public string[] shopFailArray;
	
	private bool question;
	private bool makeChoice;
	public int chosenAnswer = 0;
	
	public string playerPrefVarName;

	private bool success;

	public int value = 11;

	// Use this for initialization
	void Start () {
		InitVars ();
		
		//Set first block of dialogue.
		currentTextArray = firstDialogueArray;
		question = true;
		
//		if (PlayerPrefs.GetString (playerPrefVarName) == "") {
//			//No data to retrieve
//		} else if (PlayerPrefs.GetString (playerPrefVarName) == "True_0") {
//			currentTextArray = response1Array;
//			question = false;
//		} else if (PlayerPrefs.GetString (playerPrefVarName) == "True_1") {
//			currentTextArray = response2Array;
//			question = false;
//		}

		if (PlayerPrefs.GetString (playerPrefVarName) == "True") {
			shroom.GetComponent<SpriteRenderer> ().enabled = false;
			shield.GetComponent<SpriteRenderer> ().enabled = false;
			shopkeeper.GetComponent<SpriteRenderer> ().enabled = false;

			this.gameObject.SetActive (false);
		} else if (PlayerPrefs.GetString (playerPrefVarName) == "False") {
			currentTextArray = response4Array;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (dialoguePlaying) {
			if (Input.GetButtonDown("Fire1") && lineComplete) {
				PlayNextLine();
				
				if (makeChoice && currentTextArray == firstDialogueArray) {
					//question = false;
					makeChoice = false;
					
					if (chosenAnswer == 0) {
						currentTextArray = response1Array;
						InitNewStringArray();
						PlayNextLine();
						
						//question = false;
						
//						PlayerPrefs.SetString(playerPrefVarName, "True_0");
					} else if (chosenAnswer == 1) {
						currentTextArray = response2Array;
						InitNewStringArray();
						PlayNextLine();
						
//						PlayerPrefs.SetString(playerPrefVarName, "True_1");
						//question = false;
					}

					chosenAnswer = 0;
					AdjustCursor(chosenAnswer);
				} else if (makeChoice && (currentTextArray == response1Array || currentTextArray == response2Array)) {
					question = false;
					makeChoice = false;

					if (chosenAnswer == 0) {

						if (PlayerPrefs.GetInt("Rupee") >= value) {
							currentTextArray = shopSuccessArray;
							shield.GetComponent<SpriteRenderer>().enabled = false;
							shroom.GetComponent<SpriteRenderer>().enabled = false;
						} else {
							currentTextArray = shopFailArray;
						}

						InitNewStringArray();
						PlayNextLine();
					} else if (chosenAnswer == 1) {
						currentTextArray = response4Array;
						InitNewStringArray();
						PlayNextLine();

						PlayerPrefs.SetString(playerPrefVarName, "False");
					}
				}
			}
			
			if (Input.GetButtonDown("Fire2")) {
				if (!lineComplete) {
					StopCoroutine(coroutine);
					textBox.text = currentTextArray[counter];
					lineComplete = true;
					
					if (question && counter == currentTextArray.Length - 1) {
						if (currentTextArray == firstDialogueArray) {
							BeginQuestion(2, answerArray[0], answerArray[1], answerArray[2], answerArray[3]);
						} else if (currentTextArray == response1Array || currentTextArray == response2Array) {
							BeginQuestion(2, answerArray2[0], answerArray2[1], answerArray2[2], answerArray2[3]);
						}
					}
				} else {
					
					if (!makeChoice) {
						PlayNextLine();
					}
				}
				
			}
			
			if (question && makeChoice) {
				if (Input.GetButtonDown("Left")) {
					chosenAnswer--;
					if (chosenAnswer < 0) {
						chosenAnswer = 0;
					}
					
					AdjustCursor(chosenAnswer);
				}
				
				if (Input.GetButtonDown("Right")) {
					chosenAnswer++;
					if (chosenAnswer > 1) {
						chosenAnswer = 1;
					}
					
					AdjustCursor(chosenAnswer);
				}
			}
		}

		if (success) {
			Vector3 goal = new Vector3(32f,0f,0f);
			shopkeeper.position = Vector3.MoveTowards(shopkeeper.position, goal, 2f * Time.deltaTime);

			if (Vector3.Magnitude(shopkeeper.position - goal) < 0.01f) {
				shopkeeper.GetComponent<SpriteRenderer>().enabled = false;
				EndDialogue();

				this.gameObject.SetActive(false);
				PlayerPrefs.SetString(playerPrefVarName, "True");
			}
		}
	}
	
	void OnTriggerStay2D(Collider2D other) {
		if (other.tag == colliderTag && !dialoguePlaying) {
			if (autoPlayDialogue) {
				BeginDialogue();
			} else {
				if (Input.GetButtonDown("Fire1")) {
					BeginDialogue();
				}
			}
		}
	}
	
	void BeginDialogue() {
		charContr.disableInput = true;
		
		textBox.gameObject.SetActive (true);
		image.gameObject.SetActive (true);
		
		InitNewStringArray ();
		
		PlayNextLine ();

		AdjustCursor (0);
	}
	
	void InitNewStringArray() {
		counter = -1;
		dialoguePlaying = true;
		
		cursorImage.gameObject.SetActive (false);
		answerbox1.gameObject.SetActive (false);
		answerbox2.gameObject.SetActive (false);
		answerbox3.gameObject.SetActive (false);
		answerbox4.gameObject.SetActive (false);
	}
	
	void PlayNextLine() {
		counter++;
		lineComplete = false;

		if (counter < currentTextArray.Length) {
			textBox.GetComponent<Text> ().text = "";
			coroutine = TypeDialogue (currentTextArray [counter]);
			StartCoroutine (coroutine);

			if (currentTextArray == shopSuccessArray && counter == currentTextArray.Length - 1) {
				
				success = true;
			}

		} else {
			if (!question && currentTextArray != response1Array && !success && currentTextArray != shopSuccessArray) {
				EndDialogue();
			}



//			if ((currentTextArray == response1Array || currentTextArray == response2Array) && !success) {
//				if (PlayerPrefs.GetInt("Rupee") == 0) {
//					//Success
//					currentTextArray = shopSuccessArray;
//					InitNewStringArray();
//					PlayNextLine();
//
//					shield.GetComponent<SpriteRenderer>().enabled = false;
//					shroom.GetComponent<SpriteRenderer>().enabled = false;
//
//				} else {
//					//fail
//					currentTextArray = shopFailArray;
//					InitNewStringArray();
//					PlayNextLine();
//				}
//			}


		}
	}
	
	void BeginQuestion(int numberOfAnswers, string answer1, string answer2, string answer3, string answer4) {
		switch (numberOfAnswers) {
		case 0:
			Debug.Log ("0 answers in begin question function");
			break;
		case 1:
			Debug.Log ("1 answer only in begin question function");
			break;
		case 2:
			answerbox1.gameObject.SetActive (true);
			answerbox2.gameObject.SetActive (true);
			break;
		case 3:
			answerbox1.gameObject.SetActive (true);
			answerbox2.gameObject.SetActive (true);
			answerbox3.gameObject.SetActive (true);
			break;
		case 4:
			answerbox1.gameObject.SetActive (true);
			answerbox2.gameObject.SetActive (true);
			answerbox3.gameObject.SetActive (true);
			answerbox4.gameObject.SetActive (true);
			break;
		}
		
		if (numberOfAnswers >= 2) {
			answerbox1.text = answer1;
			answerbox2.text = answer2;
		}
		if (numberOfAnswers >= 3) {
			answerbox3.text = answer3;
		}
		if (numberOfAnswers >= 4) {
			answerbox4.text = answer4;
		}
		cursorImage.gameObject.SetActive (true);
		makeChoice = true;
		
	}
	
	void AdjustCursor(int answerIndex) {
		if (answerIndex == 0) {
			cursorImage.GetComponent<RectTransform>().anchoredPosition =  new Vector2(-118f, 60f);
		} else if (answerIndex == 1) {
			cursorImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(27f, 60f);
		}
	}
	
	void EndDialogue() {
		charContr.disableInput = false;
		
		textBox.gameObject.SetActive (false);
		image.gameObject.SetActive (false);
		
		dialoguePlaying = false;

		if (PlayerPrefs.GetString (playerPrefVarName) != "False") {
			currentTextArray = firstDialogueArray;
			question = true;
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

		shield = this.transform.parent.transform.GetChild (2);
		shroom = this.transform.parent.transform.GetChild (3);
		shopkeeper = this.transform.parent.transform.GetChild (0);


	}
	
	public IEnumerator TypeDialogue(string dialogue) {
		foreach (char letter in dialogue.ToCharArray()) {
			textBox.text += letter;
			
			yield return new WaitForSeconds(letterPause);
		}
		lineComplete = true;
		if (question && counter == currentTextArray.Length - 1) {
			BeginQuestion(2, answerArray[0], answerArray[1], answerArray[2], answerArray[3]);
		}
	}
}
