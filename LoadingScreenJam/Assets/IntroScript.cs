using UnityEngine;
using System.Collections;

public class IntroScript : MonoBehaviour {
	public bool playIntro = true;
	public CharController charContr;
	public PlayerAnimControllerScript playAnim;
	public HouScript hou;
	public SherScript sher;
	public MayScript may;
	public Transform cam;
	public FlashScript flash;

	public DialogueTrigger introTextbox, endingA1TextBox, endingB1TextBox, endingC1TextBox;
	private bool introInitialized;
	private bool introComplete;

	public Vector3 sherAfterIntroPos, houAfterIntroPos, mayAfterIntroPos;

	public bool endingA, endingB, endingC, playEndingA, playEndingB, playEndingC;

	private bool part1, part2;

	// Use this for initialization
	void Start () {
		InitVars ();

		if (playIntro) {
			InitIntro ();
		} else {
			charContr.transform.position = new Vector3(0f, -2f, 0f);

			hou.Run(false, 0f);
			sher.Run (false, 0f);
			may.Run(false, 1f);
			hou.transform.position = houAfterIntroPos;
			sher.transform.position = sherAfterIntroPos;
			may.transform.position = mayAfterIntroPos;
			introComplete = true;
		}

		//change
		PlayerPrefs.SetString ("ShopKeeper", "False");
		PlayerPrefs.SetString ("BusTicketGiven", "False");
	}
	
	// Update is called once per frame
	void Update () {
		if (charContr.transform.position.x >= -1f && playIntro && !introComplete) {
			BeginIntroScene();
		}

		//After Bus ticket is given, before ending begins.
		if (!endingA && PlayerPrefs.GetString ("BusTicketGiven") == "True") {
			endingA = true;
			hou.EnableDialogueTrigger(1);
			sher.EnableDialogueTrigger(1);
			may.EnableDialogueTrigger(1);
		}

		//EndingA1 begun, initialization
		if (PlayerPrefs.GetString ("BusTicketGiven") == "Play" && endingA && !playEndingA) {
			playEndingA = true;

			sher.transform.position = hou.transform.position + new Vector3(12f, 0f, 0f);
			may.transform.position = hou.transform.position + new Vector3(16f, -1f, 0f);
			charContr.disableInput = true;

			Transform busBoy = GameObject.Find("BusBoy").transform;
			busBoy.GetComponent<SpriteRenderer>().enabled = false;
			busBoy.GetComponent<BoxCollider2D>().enabled = false;
			busBoy.GetChild(0).gameObject.SetActive(false);

			part1 = true;
			part2 = false;

			GameObject.Find("Car").transform.position = hou.transform.position + new Vector3(28f, -2f, 0f);
		}

		//Play EndingA1
		if (playEndingA) {
			BeginEndingA1();
		}

		//After shopkeeper has left, before ending begins.
		if (!endingB && PlayerPrefs.GetString ("ShopKeeper") == "True") {
			endingB = true;
			hou.EnableDialogueTrigger(2);
			sher.EnableDialogueTrigger(3);
			may.EnableDialogueTrigger(3);
			hou.transform.position = new Vector3(32.5f, 0.75f, 0f);
			may.transform.position = new Vector3(29.75f, -1.25f, 0f);
			sher.transform.position = new Vector3(61f, -1f, 0f);
		}

		//EndingB1 begun, initialization
		if (PlayerPrefs.GetString ("ShopKeeper") == "Play" && endingB && !playEndingB) {
			playEndingB = true;
			charContr.disableInput = true;

			GameObject.Find("Shopkeeper son").GetComponent<BoxCollider2D>().enabled = false;

			part1 = true;
			part2 = false;

			sher.transform.position = hou.transform.position + new Vector3(14f, -2f, 0f);
		}

		//PlayEndingB1
		if (playEndingB) {
			BeginEndingB1();
		}

		//Player has failed both quests, before ending begins
		if (!endingC && PlayerPrefs.GetString ("ShopKeeper") == "False" && PlayerPrefs.GetString ("BusTicketGiven") == "False") {
			endingC = true;

			hou.transform.position = new Vector3(32.5f, 0.75f, 0f);

			hou.EnableDialogueTrigger(3);
			sher.EnableDialogueTrigger(4);
			may.EnableDialogueTrigger(4);
		}

		//Ending C initialization
		if (PlayerPrefs.GetString ("EndingC") == "Play" && endingC && !playEndingC) {
			playEndingC = true;
			charContr.disableInput = true;

			part1 = true;
			part2 = false;

			endingC1TextBox.PlayDialogue();
			endingC1TextBox.pause = true;

			hou.GetComponent<BoxCollider2D>().enabled = false;
		}

		//PlayEndingC1
		if (playEndingC) {
			BeginEndingC1();
		}
	}

	void InitVars() {
		//charContr = GameObject.Find ("Player").GetComponent<CharController> ();
	}

	void InitIntro() {
		hou.SetLR (0f);
	}

	private bool mayAtFinalPos = false;
	private bool sherAtFinalPos = false;
	private bool leaveScene = false;

	void BeginIntroScene() {
		if (!introInitialized) {
			charContr.disableInput = true;
			playAnim.Run (false, 1f);
			introTextbox.PlayDialogue();

			//Intro initial positions
			sher.transform.position = new Vector3(-12f, 0f, 0f);
			hou.transform.position = new Vector3(0f, 0f, 0f);
			may.transform.position = new Vector3(10f, -1f, 0f);

			introInitialized = true;
		}

		int ctr = introTextbox.counter;

		if (ctr == 3 || ctr == 17) {
			hou.Laugh(0f);
		}

		if (ctr == 5 && !mayAtFinalPos) {
			may.Run(true, 0f);
		}

		if (ctr >= 5 && !mayAtFinalPos) {
			Vector3 mayFinalPos = new Vector3(4f, -1f, 0f);
			may.transform.position = Vector3.MoveTowards(may.transform.position, mayFinalPos, 2f * Time.deltaTime);

			if (Vector3.Magnitude(may.transform.position - mayFinalPos) < 0.01f) {
				may.Run(false, 0f);
				mayAtFinalPos = true;
			}
		}

		if (mayAtFinalPos) {
			if (ctr == 8 || ctr == 9)
				may.Cry (true, 0f);
		}

		if (ctr == 10) {
			sher.Run(true, 1f);
		}

		if (ctr >= 10 && !sherAtFinalPos) {
			Vector3 sherFinalPos = new Vector3(-3f, 0f, 0f);
			sher.transform.position = Vector3.MoveTowards(sher.transform.position, sherFinalPos, 1.5f * Time.deltaTime);

			if (Vector3.Magnitude(sher.transform.position - sherFinalPos) < 0.01f) {
				sher.Run(false, 1f);
				sherAtFinalPos = true;
			}
		}

		if (ctr >= 11 && ctr <= 14) {
			may.Cry(false, 0f);
		}

		if (ctr >= 15 && ctr <= 18) {
			may.Cry(true, 0f);
		}

		if (ctr == 19) {
			may.Cry(false, 0f);
		}

		if (introTextbox.dialogueFinished) {
			leaveScene = true;
			hou.Run(true, 1f);
			sher.Run(true, 1f);
			may.Run(true, 1f);
		}

		if (leaveScene) {
			sher.transform.position = Vector3.MoveTowards(sher.transform.position, new Vector3(10f, 0f, 0f), 2f * Time.deltaTime);
			hou.transform.position = Vector3.MoveTowards(hou.transform.position, new Vector3(10f, 0f, 0f), 2.5f * Time.deltaTime);
			may.transform.position = Vector3.MoveTowards(may.transform.position, new Vector3(10f, -1f, 0f), 2f * Time.deltaTime);

			if (Vector3.Magnitude(sher.transform.position - new Vector3(9f, 0f, 0f)) < 0.01f) {
				hou.Run(false, 0f);
				sher.Run (false, 0f);
				may.Run(false, 1f);
				hou.transform.position = houAfterIntroPos;
				sher.transform.position = sherAfterIntroPos;
				may.transform.position = mayAfterIntroPos;
				introComplete = true;
			}
		}
	}

	void BeginEndingA1() {
		if (part1) {
			sher.Run (true, 0f);
			may.Run (true, 0f);
			sher.transform.position = Vector3.MoveTowards (sher.transform.position, hou.transform.position + new Vector3 (5f, 0f, 0f), 1.5f * Time.deltaTime);
			may.transform.position = Vector3.MoveTowards (may.transform.position, hou.transform.position + new Vector3 (7f, -1f, 0f), 2f * Time.deltaTime);
			hou.GetComponent<BoxCollider2D>().enabled = false;


			if (!endingA1TextBox.dialoguePlaying && sher.transform.position.x - hou.transform.position.x < 8f) {
				endingA1TextBox.PlayDialogue ();
			}

			if (sher.transform.position.x - hou.transform.position.x < 5.1f) {
				part1 = false;
				part2 = true;

				sher.Run (false, 0f);
				may.Run (false, 0f);
			}
		} 

		if (part2) {
			int ctr = endingA1TextBox.counter;

			if (ctr == 12) {
				hou.Laugh(1f);
			}

			if (ctr == 19) {
				endingA1TextBox.textAutoPlay = true;
			}

			if (ctr >= 18) {
				Transform car = GameObject.Find("Car").transform;
				hou.transform.position = Vector3.MoveTowards(hou.transform.position, sher.transform.position - new Vector3(5f, 0f, 0f) - new Vector3(0f, 3f, 0f), 5f * Time.deltaTime);
				car.GetComponent<SpriteRenderer>().enabled = true;
				car.position = Vector3.MoveTowards(car.position, sher.transform.position - new Vector3(5f, 0f, 0f) - new Vector3(0f, 3f, 0f), 8f * Time.deltaTime);
			}

			if (endingA1TextBox.dialogueFinished) {
				endingA1TextBox.gameObject.SetActive(false);
				GameObject.Find("Car").transform.GetComponent<SpriteRenderer>().enabled = false;

				hou.EnableDialogueTrigger(100);
				sher.EnableDialogueTrigger(2);
				may.EnableDialogueTrigger(2);

				may.Cry(true, 1f);

				flash.Flash();

				//Set Hou dead animation

				Transform busBoy = GameObject.Find("BusBoy").transform;
				busBoy.position = hou.transform.position - new Vector3(3f, 0f, 0f);
				busBoy.GetComponent<SpriteRenderer>().enabled = true;
				busBoy.GetComponent<BoxCollider2D>().enabled = true;
				busBoy.GetChild(1).GetComponent<BoxCollider2D>().enabled = true;
			}
		}
	}

	void BeginEndingB1() {
		if (part1) {
			sher.transform.position = Vector3.MoveTowards(sher.transform.position, hou.transform.position + new Vector3(6f, -2f, 0f), 1.5f * Time.deltaTime);
			sher.Run(true, 0f);


			if (sher.transform.position.x - hou.transform.position.x < 6.1f) {
				part2 = true;
				part1 = false;

				sher.Run(false, 0f);
				endingB1TextBox.PlayDialogue();
			}

		}

		if (part2) {
			int ctr = endingB1TextBox.counter;

			if (ctr == 3) {
				sher.Shrug(0f);
			}

			if (ctr >= 10) {
				Transform son = GameObject.Find("Shopkeeper son").transform;
				son.position = Vector3.MoveTowards(son.position, hou.transform.position + new Vector3(0f, 0.5f, 0f), 2.9f * Time.deltaTime);
			}

			if (ctr == 10) {
				endingB1TextBox.textAutoPlay = true;
			}

			if (endingB1TextBox.dialogueFinished) {
				endingB1TextBox.gameObject.SetActive(false);
				flash.Flash();

				hou.EnableDialogueTrigger(100);
				sher.EnableDialogueTrigger(2);
				may.EnableDialogueTrigger(2);

				charContr.disableInput = false;

				hou.GetComponent<BoxCollider2D>().enabled = false;

				Transform son = GameObject.Find ("Shopkeeper son").transform;
				son.position = hou.transform.position + new Vector3(-2f, 1f, 0f);
				son.GetComponent<BoxCollider2D>().enabled = true;
				son.GetChild(0).gameObject.SetActive(false);
				son.GetChild(1).gameObject.SetActive(false);
				son.GetChild(2).GetComponent<BoxCollider2D>().enabled = true;
				
				may.Cry(true, 1f);
			}
		}
	}

	void BeginEndingC1() {
		if (part1 && !part2) {
			if (endingC1TextBox.counter == 0 && endingC1TextBox.dialogueComplete) {
				Vector3 finalPos = charContr.transform.position + new Vector3 (-12f, 0f, 0f);
				finalPos = new Vector3 (finalPos.x, hou.transform.position.y, 0f);
				hou.transform.position = Vector3.MoveTowards (hou.transform.position, finalPos, 3f * Time.deltaTime);
				hou.Run(true, 0f);

				if (Mathf.Abs (hou.transform.position.x - finalPos.x) < 0.1f) {
					hou.Run(true, 1f);
					sher.Run(true, 1f);
					may.Run(true, 1f);

					sher.transform.position = hou.transform.position + new Vector3(-2f, -0.5f, 0f);
					may.transform.position = hou.transform.position + new Vector3(-1f, -1.5f, 0f);

					part1 = false;
					part2 = false;
				}
			}
		} 

		if (!part2 && !part1) {
			Vector3 originPos = new Vector3(32.5f, 0.75f, 0f);

			hou.transform.position = Vector3.MoveTowards(hou.transform.position, originPos, 3f * Time.deltaTime);
			may.transform.position = Vector3.MoveTowards(may.transform.position, originPos + new Vector3(-2f, -1.5f, 0f), 2f * Time.deltaTime);
			sher.transform.position = Vector3.MoveTowards(sher.transform.position, originPos + new Vector3(-4f, -0.5f, 0f), 1.5f * Time.deltaTime);

			bool houFinalPos = false;
			bool mayFinalPos = false;
			bool sherFinalPos = false;

			if (Mathf.Abs (hou.transform.position.x - originPos.x) < 0.1f) {
				houFinalPos = true;
				hou.Run(false, 1f);
			}

			if (Mathf.Abs (sher.transform.position.x - originPos.x) < 4.1f) {
				sherFinalPos = true;
				sher.Run(false, 1f);
			}

			if (Mathf.Abs (may.transform.position.x - originPos.x) < 2.1f) {
				mayFinalPos = true;
				may.Run(false, 1f);
			}

			if (houFinalPos && mayFinalPos && sherFinalPos) {
				part1 = false;
				part2 = true;
				endingC1TextBox.pause = false;
			}
		}

		if (part2 && !part1) {
			int ctr = endingC1TextBox.counter;

			if (ctr == 7) {
				endingC1TextBox.pause = true;

				Transform train = GameObject.Find ("Bus").transform;
				if (train.position.x >= 17f) {
					train.GetComponent<SpriteRenderer>().enabled = true;
					train.position = new Vector3(train.position.x - 5f * Time.deltaTime, train.position.y, 0f);
				} else {
					train.GetComponent<SpriteRenderer>().enabled = false;
					charContr.disableInput = false;
				}

				if (train.position.x <= 33f && train.position.x > 17f) {
					endingC1TextBox.EndDialogue();

					//Set sher worried animation
					//Play splash
					hou.EnableDialogueTrigger(100);
					sher.EnableDialogueTrigger(5);
					may.EnableDialogueTrigger(5);

					hou.GetComponent<SpriteRenderer>().enabled = false;
					hou.GetComponent<BoxCollider2D>().enabled = false;

					endingC1TextBox.gameObject.SetActive(false);
					charContr.disableInput = true;
				}


			}
		}
	}
}
