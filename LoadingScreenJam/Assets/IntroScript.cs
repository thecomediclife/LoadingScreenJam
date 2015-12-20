using UnityEngine;
using UnityEngine.UI;
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
	public Image headIcon;
	public Sprite houIcon, sherIcon, mayIcon;

	public DialogueTrigger introTextbox, endingA1TextBox, endingB1TextBox, endingC1TextBox;
	private bool introInitialized;
	private bool introComplete;

	public Vector3 sherAfterIntroPos, houAfterIntroPos, mayAfterIntroPos;

	public bool endingA, endingB, endingC, playEndingA, playEndingB, playEndingC;

	private bool part1, part2;

	private bool checkPoint1, checkPoint2, endingComplete;

	private int pt;

	// Use this for initialization
	void Start () {
		//change
//		PlayerPrefs.SetString ("ShopKeeper", "False");
//		PlayerPrefs.SetString ("BusTicketGiven", "False");
//		PlayerPrefs.SetInt ("Playthrough", 2);

		pt = PlayerPrefs.GetInt ("Playthrough");

		InitVars (pt);

		if (pt > 0) {
			playIntro = false;
		}

		if (playIntro) {
			InitIntro ();
		} else {
			//charContr.transform.position = new Vector3(0f, -2f, 0f);

			hou.Run(false, 0f);
			sher.Run (false, 0f);
			may.Run(false, 1f);
			hou.transform.position = houAfterIntroPos;
			sher.transform.position = sherAfterIntroPos;
			may.transform.position = mayAfterIntroPos;
			introComplete = true;


		}



		if (PlayerPrefs.GetString ("BusTicketGiven") == "Play") {
			PlayerPrefs.SetString("BusTicketGiven", "True");
		}

		if (PlayerPrefs.GetString("ShopKeeper") == "Play") {
			PlayerPrefs.SetString("ShopKeeper", "True");
		}

		if (PlayerPrefs.GetString ("EndingC") == "Play") {
			PlayerPrefs.SetString("EndingC", "");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (charContr.transform.position.x >= -1f && playIntro && !introComplete) {
			BeginIntroScene();
		}

		//After Bus ticket is given, before ending begins.
		if (!endingA && PlayerPrefs.GetString ("BusTicketGiven") == "True") {
			endingA = true;

			hou.transform.position = new Vector3(30f, 6f, 0f);

			if (pt == 0) {
				hou.EnableDialogueTrigger(1);
				sher.EnableDialogueTrigger(1);
				may.EnableDialogueTrigger(1);
			} else if (pt == 1) {
				hou.EnableDialogueTrigger(4);
				sher.EnableDialogueTrigger(6);
				may.EnableDialogueTrigger(6);
			} else if (pt >= 2) {
				hou.EnableDialogueTrigger(7);
				sher.EnableDialogueTrigger(7);
				may.EnableDialogueTrigger(7);
			}
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

			GameObject.Find("Car").transform.position = hou.transform.position + new Vector3(22f, -2f, 0f);

			checkPoint1 = false;
			checkPoint2 = false;
			endingComplete = false;
		}

		//Play EndingA1
		if (playEndingA) {

			if (pt == 0) {
				BeginEndingA1();
			} else if (pt == 1) {
				BeginEndingA2();
			} else if (pt >= 2) {
				BeginEndingA3();
			}
		}

		//After shopkeeper has left, before ending begins.
		if (!endingB && PlayerPrefs.GetString ("ShopKeeper") == "True") {
			endingB = true;

			if (pt == 0) {
				hou.EnableDialogueTrigger(2);
				sher.EnableDialogueTrigger(3);
				may.EnableDialogueTrigger(3);
			} else if (pt == 1) {
				hou.EnableDialogueTrigger(5);
				sher.EnableDialogueTrigger(8);
				may.EnableDialogueTrigger(8);
			} else if (pt >= 2) {
				hou.EnableDialogueTrigger(8);
				sher.EnableDialogueTrigger(9);
				may.EnableDialogueTrigger(9);
			}
			
			hou.transform.position = new Vector3(30f, 6f, 0f);
			may.transform.position = hou.transform.position + new Vector3(-4f, -0.5f, 0f);
			//sher.transform.position = new Vector3(61f, -1f, 0f);
		}

		//EndingB1 begun, initialization
		if (PlayerPrefs.GetString ("ShopKeeper") == "Play" && endingB && !playEndingB) {
			playEndingB = true;
			charContr.disableInput = true;

			GameObject.Find("Shopkeeper son").GetComponent<BoxCollider2D>().enabled = false;

			part1 = true;
			part2 = false;

			sher.transform.position = hou.transform.position + new Vector3(14f, 0f, 0f);

			checkPoint1 = false;
			checkPoint2 = false;
			endingComplete = false;
		}

		//PlayEndingB1
		if (playEndingB) {

			if (pt == 0) {
				BeginEndingB1();
			} else if (pt == 1) {
				BeginEndingB2();
			} else if (pt >= 2) {
				BeginEndingB3();
			}
		}

		//Player has failed both quests, before ending C begins
		if (!endingC && PlayerPrefs.GetString ("ShopKeeper") == "False" && PlayerPrefs.GetString ("BusTicketGiven") == "False") {
			endingC = true;

			hou.transform.position = new Vector3(30f, 6f, 0f);

			if (pt == 0) {
				hou.EnableDialogueTrigger(3);
				sher.EnableDialogueTrigger(4);
				may.EnableDialogueTrigger(4);
			} else if (pt == 1) {
				hou.EnableDialogueTrigger(6);
				sher.EnableDialogueTrigger(10);
				may.EnableDialogueTrigger(10);
			} else if (pt >= 2) {
				hou.EnableDialogueTrigger(9);
				sher.EnableDialogueTrigger(11);
				may.EnableDialogueTrigger(11);
			}
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

			checkPoint1 = false;
			checkPoint2 = false;
			endingComplete = false;
		}

		//PlayEndingC1
		if (playEndingC) {
			if (pt == 0) {
				BeginEndingC1();
			} else if (pt == 1) {
				BeginEndingC2();
			} else if (pt >= 2) {
				BeginEndingC3();
			}
		}
	}

	void InitVars(int playthru) {
		if (PlayerPrefs.GetString ("Complete") == "") {
			PlayerPrefs.SetString("Complete", "Ongoing");
		}

		if (pt == 0) {
			hou.EnableDialogueTrigger(0);
			may.EnableDialogueTrigger(0);
			sher.EnableDialogueTrigger(0);
		} else if (pt == 1) {
			endingA1TextBox = transform.GetChild(4).GetComponent<DialogueTrigger>();
			endingB1TextBox = transform.GetChild(5).GetComponent<DialogueTrigger>();
			endingC1TextBox = transform.GetChild(6).GetComponent<DialogueTrigger>();

			hou.EnableDialogueTrigger(10);
			sher.EnableDialogueTrigger(14);
			may.EnableDialogueTrigger(14);
		} else if (pt >= 2) {
			endingA1TextBox = transform.GetChild(7).GetComponent<DialogueTrigger>();
			endingB1TextBox = transform.GetChild(8).GetComponent<DialogueTrigger>();
			endingC1TextBox = transform.GetChild(7).GetComponent<DialogueTrigger>();

			hou.EnableDialogueTrigger(11);
			sher.EnableDialogueTrigger(15);
			may.EnableDialogueTrigger(15);
		}
	}

	void InitIntro() {
		hou.SetLR (0f);
		hou.transform.position = new Vector3 (2f, 6.5f, 0f);
		GameObject.Find("IntroColl").GetComponent<BoxCollider2D>().enabled = true;
		GameObject.Find("House1TextBox").GetComponent<BoxCollider2D>().enabled = false;
	}

	void SetHeadIcon(bool enable, Sprite icon) {
		headIcon.gameObject.SetActive (enable);
		headIcon.sprite = icon;
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
			sher.transform.position = new Vector3(-12f, 5f, 0f);
//			hou.transform.position = new Vector3(0f, 0f, 0f);
			may.transform.position = new Vector3(10f, 6f, 0f);

			introInitialized = true;

			hou.GetComponent<BoxCollider2D>().enabled = false;
			may.GetComponent<BoxCollider2D>().enabled = false;
			sher.GetComponent<BoxCollider2D>().enabled = false;

			GameObject.Find("IntroColl").GetComponent<BoxCollider2D>().enabled = false;
		}

		int ctr = introTextbox.counter;

		if (ctr == 2 || ctr == 22 || ctr == 28) {
			hou.Pose(true, 0f);
		}

		if (ctr == 3 || ctr == 24) {
			hou.Pose(false, 0f);
		}

		if (ctr == 3 || ctr == 17) {
			hou.Laugh(true, 0f);
		}

		if (ctr == 4 || ctr == 18) {
			hou.Laugh(false, 0f);
		}

		if (ctr == 5 && !mayAtFinalPos) {
			may.Run(true, 0f);
		}

		if (ctr >= 5 && !mayAtFinalPos) {
			Vector3 mayFinalPos = new Vector3(4f, 6f, 0f);
			may.transform.position = Vector3.MoveTowards(may.transform.position, mayFinalPos, 2.5f * Time.deltaTime);

			if (Mathf.Abs(may.transform.position.x - mayFinalPos.x) < 0.01f) {
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
			Vector3 sherFinalPos = new Vector3(-3f, 5f, 0f);
			sher.transform.position = Vector3.MoveTowards(sher.transform.position, sherFinalPos, 1.5f * Time.deltaTime);

			if (Mathf.Abs(sher.transform.position.x - sherFinalPos.x) < 0.1f) {
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
			hou.Pose(false, 1f);
			hou.Run(true, 1f);
			sher.Run(true, 1f);
			may.Walk(true, 1f);
			SetHeadIcon(false, houIcon);

			charContr.disableInput = true;
		}

		if (leaveScene) {
			sher.transform.position = Vector3.MoveTowards(sher.transform.position, new Vector3(10f, 0f, 0f), 2f * Time.deltaTime);
			hou.transform.position = Vector3.MoveTowards(hou.transform.position, new Vector3(10f, 0f, 0f), 2.5f * Time.deltaTime);
			may.transform.position = Vector3.MoveTowards(may.transform.position, new Vector3(10f, -1f, 0f), 2f * Time.deltaTime);

			if (Mathf.Abs(sher.transform.position.x - 10f) < 0.05f) {
				hou.Run(false, 0f);
				sher.Run (false, 0f);
				may.Walk(false, 1f);
				hou.transform.position = houAfterIntroPos;
				sher.transform.position = sherAfterIntroPos;
				may.transform.position = mayAfterIntroPos;
				introComplete = true;

				hou.EnableDialogueTrigger(0);
				may.EnableDialogueTrigger(0);
				sher.EnableDialogueTrigger(0);

				hou.GetComponent<BoxCollider2D>().enabled = true;
				may.GetComponent<BoxCollider2D>().enabled = true;
				sher.GetComponent<BoxCollider2D>().enabled = true;

				charContr.disableInput = false;

				GameObject.Find("House1TextBox").GetComponent<BoxCollider2D>().enabled = true;
			}
		}

		if (ctr == 0 || ctr == 6 || ctr == 11 || ctr == 17 || ctr == 19) {
			SetHeadIcon(true, houIcon);
		}

		if (ctr == 5 || ctr == 8 || ctr == 15 || ctr == 18) {
			SetHeadIcon(true, mayIcon);
		}

		if (ctr == 10 || ctr == 13 || ctr == 16) {
			SetHeadIcon(true, sherIcon);
		}
	}

	void BeginEndingA1() {
		if (part1) {
			sher.Run (true, 0f);
			may.Run (true, 0f);
			sher.transform.position = Vector3.MoveTowards (sher.transform.position, hou.transform.position + new Vector3 (5f, 0f, 0f), 1.5f * Time.deltaTime);
			may.transform.position = Vector3.MoveTowards (may.transform.position, hou.transform.position + new Vector3 (7f, -1f, 0f), 2f * Time.deltaTime);
			hou.GetComponent<BoxCollider2D>().enabled = false;

			GameObject.Find("Car").GetComponent<SpriteRenderer>().enabled = true;


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

			if (ctr == 11) {
				hou.Laugh(true, 1f);
			}

			if (ctr == 12) {
				hou.Laugh(false, 1f);
			}

			if (ctr == 17) {
				hou.Pose(true, 1f);
			}

			if (ctr == 18) {
				endingA1TextBox.textAutoPlay = true;
			}

			if (ctr >= 17) {
				hou.transform.position = Vector3.MoveTowards(hou.transform.position, sher.transform.position - new Vector3(5f, 0f, 0f) - new Vector3(0f, 3f, 0f), 5f * Time.deltaTime);
			}

			if (ctr >= 18) {
				Transform car = GameObject.Find("Car").transform;
				car.position = Vector3.MoveTowards(car.position, sher.transform.position - new Vector3(5f, 0f, 0f) - new Vector3(0f, 3f, 0f), 8f * Time.deltaTime);
			}

			if (endingA1TextBox.dialogueFinished) {
				endingComplete = true;

				endingA1TextBox.gameObject.SetActive(false);
				GameObject.Find("Car").transform.GetComponent<SpriteRenderer>().enabled = false;

				hou.EnableDialogueTrigger(100);
				sher.EnableDialogueTrigger(2);
				may.EnableDialogueTrigger(2);

				may.Cry(true, 1f);

				flash.Flash();

				hou.Pose(false, 0f);
				hou.Dead(true, 0f);
				sher.Sad(true, 0f);

				Transform busBoy = GameObject.Find("BusBoy").transform;
				busBoy.position = hou.transform.position - new Vector3(3f, 0f, 0f);
				busBoy.GetComponent<SpriteRenderer>().enabled = true;
				busBoy.GetComponent<BoxCollider2D>().enabled = true;
				busBoy.GetChild(1).GetComponent<BoxCollider2D>().enabled = true;
			}
		}

		//Setting head icons in the dialogue boxes
		if (endingA1TextBox.dialoguePlaying) {
			int ctr = endingA1TextBox.counter;

			if (ctr == 0 || ctr == 18) {
				SetHeadIcon(true, sherIcon);
			}

			if (ctr == 1 || ctr == 5) {
				SetHeadIcon(true, mayIcon);
			}

			if (ctr == 4 || ctr == 8) {
				SetHeadIcon(true, houIcon);
			}
		}

		if (endingA1TextBox.dialogueFinished) {
			SetHeadIcon(false, houIcon);
		}

		if (endingComplete) {
			endingA1TextBox.dialogueFinished = false;

			if (sher.transform.GetChild(2).GetComponent<DialogueTrigger>().dialogueFinished) {
				checkPoint1 = true;
			}

			if (may.transform.GetChild(2).GetComponent<DialogueTrigger>().dialogueFinished) {
				checkPoint2 = true;
			}

			if (checkPoint1 && checkPoint2) {
				flash.Fade();

				if (flash.fadeComplete) {
					PlayerPrefs.SetInt("Playthrough", PlayerPrefs.GetInt("Playthrough") + 1);
					PlayerPrefs.SetString("Complete", "True");
					Application.LoadLevel(0);
				}
			}
		}

	}

	void BeginEndingA2() {
		if (part1) {
			sher.Run (true, 0f);
			may.Run (true, 0f);
			sher.transform.position = Vector3.MoveTowards (sher.transform.position, hou.transform.position + new Vector3 (5f, 0f, 0f), 1.5f * Time.deltaTime);
			may.transform.position = Vector3.MoveTowards (may.transform.position, hou.transform.position + new Vector3 (7f, -1f, 0f), 2f * Time.deltaTime);
			hou.GetComponent<BoxCollider2D>().enabled = false;
			
			GameObject.Find("Car").GetComponent<SpriteRenderer>().enabled = true;
			
			
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
			
			if (ctr == 7) {
				hou.Laugh(true, 1f);
			}

			if (ctr == 8) {
				hou.Laugh(false, 1f);
			}

			if (ctr == 12) {
				hou.Pose(true, 1f);
			}
			
			if (ctr == 13) {
				endingA1TextBox.textAutoPlay = true;
			}

			if (ctr >= 12) {
				hou.transform.position = Vector3.MoveTowards(hou.transform.position, sher.transform.position - new Vector3(5f, 0f, 0f) - new Vector3(0f, 3f, 0f), 5f * Time.deltaTime);
			}
			
			if (ctr >= 13) {
				Transform car = GameObject.Find("Car").transform;
				car.position = Vector3.MoveTowards(car.position, sher.transform.position - new Vector3(5f, 0f, 0f) - new Vector3(0f, 3f, 0f), 8f * Time.deltaTime);
			}
			
			if (endingA1TextBox.dialogueFinished) {
				endingComplete = true;
				
				endingA1TextBox.gameObject.SetActive(false);
				GameObject.Find("Car").transform.GetComponent<SpriteRenderer>().enabled = false;
				
				hou.EnableDialogueTrigger(100);
				sher.EnableDialogueTrigger(12);
				may.EnableDialogueTrigger(12);
				
				may.Cry(false, 1f);
				
				flash.Flash();

				hou.Pose(false, 0f);
				hou.Dead(true, 0f);
				sher.Sad(true, 0f);
				
				Transform busBoy = GameObject.Find("BusBoy").transform;
				busBoy.position = hou.transform.position - new Vector3(3f, 0f, 0f);
				busBoy.GetComponent<SpriteRenderer>().enabled = true;
				busBoy.GetComponent<BoxCollider2D>().enabled = true;
				busBoy.GetChild(1).GetComponent<BoxCollider2D>().enabled = true;
			}
		}
		
		//Setting head icons in the dialogue boxes
		if (endingA1TextBox.dialoguePlaying) {
			int ctr = endingA1TextBox.counter;
			
			if (ctr == 0 || ctr == 13) {
				SetHeadIcon(true, sherIcon);
			}
			
			if (ctr == 1) {
				SetHeadIcon(true, mayIcon);
			}
			
			if (ctr == 5) {
				SetHeadIcon(true, houIcon);
			}
		}
		
		if (endingA1TextBox.dialogueFinished) {
			SetHeadIcon(false, houIcon);
		}
		
		if (endingComplete) {
			endingA1TextBox.dialogueFinished = false;
			
			if (sher.transform.GetChild(12).GetComponent<DialogueTrigger>().dialogueFinished) {
				checkPoint1 = true;
			}
			
			if (may.transform.GetChild(12).GetComponent<DialogueTrigger>().dialogueFinished) {
				checkPoint2 = true;
			}
			
			if (checkPoint1 && checkPoint2) {
				flash.Fade();
				
				if (flash.fadeComplete) {
					PlayerPrefs.SetInt("Playthrough", PlayerPrefs.GetInt("Playthrough") + 1);
					PlayerPrefs.SetString("Complete", "True");
					Application.LoadLevel(0);
				}
			}
		}
	}

	void BeginEndingA3() {
		if (part1) {
			sher.Run (true, 0f);
			may.Walk (true, 0f);
			sher.transform.position = Vector3.MoveTowards (sher.transform.position, hou.transform.position + new Vector3 (5f, 0f, 0f), 1.5f * Time.deltaTime);
			may.transform.position = Vector3.MoveTowards (may.transform.position, hou.transform.position + new Vector3 (7f, -1f, 0f), 2f * Time.deltaTime);
			hou.GetComponent<BoxCollider2D>().enabled = false;
			
			GameObject.Find("Car").GetComponent<SpriteRenderer>().enabled = true;
			
			
//			if (!endingA1TextBox.dialoguePlaying && sher.transform.position.x - hou.transform.position.x < 8f) {
//				endingA1TextBox.PlayDialogue ();
//			}
			
			if (sher.transform.position.x - hou.transform.position.x < 5.1f) {
				part1 = false;
				part2 = true;
				
				sher.Run (false, 0f);
				may.Walk (false, 0f);

				endingA1TextBox.PlayDialogue ();
			}
		} 
		
		if (part2) {
			int ctr = endingA1TextBox.counter;
			
//			if (ctr == 12) {
//				hou.Laugh(1f);
//			}

			if (ctr == 56) {
				hou.Pose(true, 0f);
			}
			
			if (ctr == 57) {
				endingA1TextBox.textAutoPlay = true;
			}

			if (ctr >= 56) {
				hou.transform.position = Vector3.MoveTowards(hou.transform.position, sher.transform.position - new Vector3(5f, 0f, 0f) - new Vector3(0f, 3f, 0f), 5f * Time.deltaTime);
			}
			
			if (ctr >= 57) {
				Transform car = GameObject.Find("Car").transform;
				car.position = Vector3.MoveTowards(car.position, sher.transform.position - new Vector3(5f, 0f, 0f) - new Vector3(0f, 3f, 0f), 8f * Time.deltaTime);
			}
			
			if (endingA1TextBox.dialogueFinished) {
				endingComplete = true;
				
				endingA1TextBox.gameObject.SetActive(false);
				GameObject.Find("Car").transform.GetComponent<SpriteRenderer>().enabled = false;
				
				hou.EnableDialogueTrigger(100);
				sher.EnableDialogueTrigger(13);
				may.EnableDialogueTrigger(13);
				
				may.Cry(true, 1f);
				
				flash.Flash();

				hou.Pose(false, 0f);
				hou.Dead(true, 0f);
				sher.Sad(true, 0f);
				
				Transform busBoy = GameObject.Find("BusBoy").transform;
				busBoy.position = hou.transform.position - new Vector3(3f, 0f, 0f);
				busBoy.GetComponent<SpriteRenderer>().enabled = true;
				busBoy.GetComponent<BoxCollider2D>().enabled = true;
				busBoy.GetChild(1).GetComponent<BoxCollider2D>().enabled = true;
			}
		}
		
		//Setting head icons in the dialogue boxes
		if (endingA1TextBox.dialoguePlaying) {
			int ctr = endingA1TextBox.counter;
			
			if (ctr == 4 || ctr == 6 || ctr == 15 || ctr == 21 || ctr == 26 || ctr == 38) {
				SetHeadIcon(true, sherIcon);
			}
			
			if (ctr == 35 || ctr == 45) {
				SetHeadIcon(true, mayIcon);
			}
			
			if (ctr == 0 || ctr == 5 || ctr == 14 || ctr == 20 || ctr == 25 || ctr == 48) {
				SetHeadIcon(true, houIcon);
			}

			if (ctr == 57 || ctr == 58) {
				SetHeadIcon(false, houIcon);
			}



		}
		
		if (endingA1TextBox.dialogueFinished) {
			SetHeadIcon(false, houIcon);
		}
		
		if (endingComplete) {
			endingA1TextBox.dialogueFinished = false;
			
			if (sher.transform.GetChild(13).GetComponent<DialogueTrigger>().dialogueFinished) {
				checkPoint1 = true;
			}
			
			if (may.transform.GetChild(13).GetComponent<DialogueTrigger>().dialogueFinished) {
				checkPoint2 = true;
			}
			
			if (checkPoint1 && checkPoint2) {
				flash.Fade();
				
				if (flash.fadeComplete) {
					PlayerPrefs.SetInt("Playthrough", PlayerPrefs.GetInt("Playthrough") + 1);
					PlayerPrefs.SetString("Complete", "True");
					Application.LoadLevel(0);
				}
			}
		}
	}

	void BeginEndingB1() {
		if (part1) {
			sher.transform.position = Vector3.MoveTowards(sher.transform.position, hou.transform.position + new Vector3(6f, 0f, 0f), 1.5f * Time.deltaTime);
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
				sher.Shrug(true, 0f);
				GameObject.Find("Shopkeeper son").GetComponent<Animator>().SetBool("Run", true);
			}

			if (ctr == 4) {
				sher.Shrug(false, 0f);
			}

			if (ctr == 8) {
				hou.Pose(true, 0f);
			}

			if (ctr == 9) {
				hou.Pose(false, 0f);
			}

			if (ctr >= 10 && !endingComplete) {
				Transform son = GameObject.Find("Shopkeeper son").transform;
				son.position = Vector3.MoveTowards(son.position, hou.transform.position + new Vector3(0f, 0.5f, 0f), 2.8f * Time.deltaTime);
			}

			if (ctr == 10) {
				endingB1TextBox.textAutoPlay = true;
			}

			if (ctr == 11) {
				GameObject.Find ("Shopkeeper son").GetComponent<Animator>().SetBool("Trip", true);
				//GameObject.Find ("Shopkeeper son").GetComponent<Animator>().SetBool("Run", false);
			}

			if (endingB1TextBox.dialogueFinished) {
				endingComplete = true;

				endingB1TextBox.gameObject.SetActive(false);
				flash.Flash();

				hou.EnableDialogueTrigger(100);
				sher.EnableDialogueTrigger(2);
				may.EnableDialogueTrigger(2);

				charContr.disableInput = false;

				hou.GetComponent<BoxCollider2D>().enabled = false;

				Transform son = GameObject.Find ("Shopkeeper son").transform;
				//son.position = hou.transform.position + new Vector3(-2f, 1f, 0f);
				son.GetComponent<BoxCollider2D>().enabled = true;
				son.GetChild(0).gameObject.SetActive(false);
				son.GetChild(1).gameObject.SetActive(false);
				son.GetChild(2).GetComponent<BoxCollider2D>().enabled = true;
				son.GetComponent<Animator>().SetBool("Run", false);
				son.GetComponent<Animator>().SetBool("Trip", false);

				hou.Dead(true, 1f);
				sher.Sad(true, 0f);

				hou.transform.position = son.position + new Vector3(2f, -1f, 0f);
				
				may.Cry(true, 1f);
			}
		}

		if (endingB1TextBox.dialoguePlaying) {
			int ctr = endingB1TextBox.counter;
			
			if (ctr == 1) {
				SetHeadIcon(true, sherIcon);
			}
			
//			if (ctr == 1 || ctr == 6) {
//				SetHeadIcon(true, mayIcon);
//			}
			
			if (ctr == 0 || ctr == 4 || ctr == 11) {
				SetHeadIcon(true, houIcon);
			}

			if (ctr == 10) {
				SetHeadIcon(false, houIcon);
			}
		}
		
		if (endingB1TextBox.dialogueFinished) {
			SetHeadIcon(false, houIcon);
		}

		if (endingComplete) {
			endingB1TextBox.dialogueFinished = false;

			if (sher.transform.GetChild(2).GetComponent<DialogueTrigger>().dialogueFinished) {
				checkPoint1 = true;
			}
			
			if (may.transform.GetChild(2).GetComponent<DialogueTrigger>().dialogueFinished) {
				checkPoint2 = true;
			}
			
			if (checkPoint1 && checkPoint2) {
				flash.Fade();
				
				if (flash.fadeComplete) {
					PlayerPrefs.SetInt("Playthrough", PlayerPrefs.GetInt("Playthrough") + 1);
					PlayerPrefs.SetString("Complete", "True");
					Application.LoadLevel(0);
				}
			}
		}
	}

	void BeginEndingB2() {
		if (part1) {
			sher.transform.position = Vector3.MoveTowards(sher.transform.position, hou.transform.position + new Vector3(6f, 0f, 0f), 1.5f * Time.deltaTime);
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

			if (ctr == 9 || ctr == 11) {
				sher.Shrug(true, 0f);
				GameObject.Find("Shopkeeper son").GetComponent<Animator>().SetBool("Run", true);
			}

			if (ctr == 10 || ctr == 12) {
				sher.Shrug(false, 0f);
			}

			if (ctr == 16) {
				hou.Pose(true, 0f);
			}

			if (ctr == 17) {
				hou.Pose(false, 0f);
			}

			if (ctr >= 18 && !endingComplete) {
				Transform son = GameObject.Find("Shopkeeper son").transform;
				son.position = Vector3.MoveTowards(son.position, hou.transform.position + new Vector3(0f, 0.5f, 0f), 2.8f * Time.deltaTime);
			}
			
			if (ctr == 18) {
				endingB1TextBox.textAutoPlay = true;
			}
			
			if (ctr == 19) {
				GameObject.Find ("Shopkeeper son").GetComponent<Animator>().SetBool("Trip", true);
				//GameObject.Find ("Shopkeeper son").GetComponent<Animator>().SetBool("Run", false);
			}

			if (endingB1TextBox.dialogueFinished) {
				endingComplete = true;
				
				endingB1TextBox.gameObject.SetActive(false);
				flash.Flash();
				
				hou.EnableDialogueTrigger(100);
				sher.EnableDialogueTrigger(12);
				may.EnableDialogueTrigger(12);
				
				charContr.disableInput = false;
				
				hou.GetComponent<BoxCollider2D>().enabled = false;
				
				Transform son = GameObject.Find ("Shopkeeper son").transform;
				//son.position = hou.transform.position + new Vector3(-2f, 1f, 0f);
				son.GetComponent<BoxCollider2D>().enabled = true;
				son.GetChild(0).gameObject.SetActive(false);
				son.GetChild(1).gameObject.SetActive(false);
				son.GetChild(2).GetComponent<BoxCollider2D>().enabled = true;
				son.GetComponent<Animator>().SetBool("Run", false);
				son.GetComponent<Animator>().SetBool("Trip", false);

				hou.Dead(true, 1f);
				sher.Sad(true, 0f);
				hou.transform.position = son.position + new Vector3(2f, -1f, 0f);
				
				may.Cry(false, 1f);
			}
		}
		
		if (endingB1TextBox.dialoguePlaying) {
			int ctr = endingB1TextBox.counter;
			
			if (ctr == 3 || ctr == 5) {
				SetHeadIcon(true, sherIcon);
			}
			
			if (ctr == 2) {
				SetHeadIcon(true, mayIcon);
			}
			
			if (ctr == 0 || ctr == 4 || ctr == 12 || ctr == 19) {
				SetHeadIcon(true, houIcon);
			}
			
			if (ctr == 18) {
				SetHeadIcon(false, houIcon);
			}
		}
		
		if (endingB1TextBox.dialogueFinished) {
			SetHeadIcon(false, houIcon);
		}
		
		if (endingComplete) {
			endingB1TextBox.dialogueFinished = false;

			if (sher.transform.GetChild(12).GetComponent<DialogueTrigger>().dialogueFinished) {
				checkPoint1 = true;
			}
			
			if (may.transform.GetChild(12).GetComponent<DialogueTrigger>().dialogueFinished) {
				checkPoint2 = true;
			}
			
			if (checkPoint1 && checkPoint2) {
				flash.Fade();
				
				if (flash.fadeComplete) {
					PlayerPrefs.SetInt("Playthrough", PlayerPrefs.GetInt("Playthrough") + 1);
					PlayerPrefs.SetString("Complete", "True");
					Application.LoadLevel(0);
				}
			}
		}
	}

	void BeginEndingB3() {
		if (part1) {
			sher.transform.position = Vector3.MoveTowards(sher.transform.position, hou.transform.position + new Vector3(6f, 0f, 0f), 1.5f * Time.deltaTime);
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
//				sher.Shrug(0f);
				GameObject.Find("Shopkeeper son").GetComponent<Animator>().SetBool("Run", true);
			}

			if (ctr == 56) {
				hou.Pose(true, 0f);
			}
			
			if (ctr >= 57 && !endingComplete) {
				Transform son = GameObject.Find("Shopkeeper son").transform;
				son.position = Vector3.MoveTowards(son.position, hou.transform.position + new Vector3(0f, 0.5f, 0f), 2.8f * Time.deltaTime);
			}
			
			if (ctr == 57) {
				endingB1TextBox.textAutoPlay = true;
			}
			
			if (ctr == 58) {
				GameObject.Find ("Shopkeeper son").GetComponent<Animator>().SetBool("Trip", true);
				//GameObject.Find ("Shopkeeper son").GetComponent<Animator>().SetBool("Run", false);
			}
			
			if (endingB1TextBox.dialogueFinished) {
				endingComplete = true;
				
				endingB1TextBox.gameObject.SetActive(false);
				flash.Flash();
				
				hou.EnableDialogueTrigger(100);
				sher.EnableDialogueTrigger(13);
				may.EnableDialogueTrigger(13);
				
				charContr.disableInput = false;
				
				hou.GetComponent<BoxCollider2D>().enabled = false;
				
				Transform son = GameObject.Find ("Shopkeeper son").transform;
				//son.position = hou.transform.position + new Vector3(-2f, 1f, 0f);
				son.GetComponent<BoxCollider2D>().enabled = true;
				son.GetChild(0).gameObject.SetActive(false);
				son.GetChild(1).gameObject.SetActive(false);
				son.GetChild(2).GetComponent<BoxCollider2D>().enabled = true;
				son.GetComponent<Animator>().SetBool("Run", false);
				son.GetComponent<Animator>().SetBool("Trip", false);
				
				hou.Pose(false, 0f);
				hou.Dead(true, 1f);
				hou.transform.position = son.position + new Vector3(2f, -1f, 0f);
				sher.Sad(true, 0f);
				
				may.Cry(true, 1f);
			}

		}
		
		if (endingB1TextBox.dialoguePlaying) {
			int ctr = endingB1TextBox.counter;
			
			if (ctr == 4 || ctr == 6 || ctr == 15 || ctr == 21 || ctr == 26 || ctr == 38) {
				SetHeadIcon(true, sherIcon);
			}
			
			if (ctr == 35 || ctr == 45) {
				SetHeadIcon(true, mayIcon);
			}
			
			if (ctr == 0 || ctr == 5 || ctr == 14 || ctr == 20 || ctr == 25 || ctr == 48) {
				SetHeadIcon(true, houIcon);
			}
			
			if (ctr == 57 || ctr == 58) {
				SetHeadIcon(false, houIcon);
			}

			if (ctr == 0) {
				sher.Sad(true, 0f);
			}
			
			if (ctr == 56) {
				hou.Pose(true, 0f);
			}
		}
		
		if (endingB1TextBox.dialogueFinished) {
			SetHeadIcon(false, houIcon);
		}
		
		if (endingComplete) {
			endingB1TextBox.dialogueFinished = false;

			if (sher.transform.GetChild(13).GetComponent<DialogueTrigger>().dialogueFinished) {
				checkPoint1 = true;
			}
			
			if (may.transform.GetChild(13).GetComponent<DialogueTrigger>().dialogueFinished) {
				checkPoint2 = true;
			}
			
			if (checkPoint1 && checkPoint2) {
				flash.Fade();
				
				if (flash.fadeComplete) {
					PlayerPrefs.SetInt("Playthrough", PlayerPrefs.GetInt("Playthrough") + 1);
					PlayerPrefs.SetString("Complete", "True");
					Application.LoadLevel(0);
				}
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
			Vector3 originPos = new Vector3(30f, 6f, 0f);

			hou.transform.position = Vector3.MoveTowards(hou.transform.position, originPos, 3f * Time.deltaTime);
			may.transform.position = Vector3.MoveTowards(may.transform.position, originPos + new Vector3(-2f, -1f, 0f), 2f * Time.deltaTime);
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

			if (ctr == 5) {
				hou.Pose(true, 1f);
			}

			if (ctr == 6) {
				hou.Pose(false, 1f);
			}

			if (ctr == 6) {
				hou.Laugh(true, 1f);
			}

			if (ctr == 7) {
				hou.Laugh(false, 1f);
			}

			if (ctr == 7) {
				endingC1TextBox.pause = true;

				Transform train = GameObject.Find ("Bus").transform;
				if (train.position.x >= 17f) {
					train.GetComponent<SpriteRenderer>().enabled = true;
					train.position = new Vector3(train.position.x - 10f * Time.deltaTime, train.position.y, 0f);
				} else {
					train.GetComponent<SpriteRenderer>().enabled = false;
					charContr.disableInput = false;
				}

				if (train.position.x <= 33f && train.position.x > 17f) {
					endingC1TextBox.EndDialogue();
					endingComplete = true;

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

		if (endingC1TextBox.dialoguePlaying) {
			int ctr = endingC1TextBox.counter;
			
//			if (ctr == 1) {
//				SetHeadIcon(true, sherIcon);
//			}
//			
//			if (ctr == 1 || ctr == 6) {
//				SetHeadIcon(true, mayIcon);
//			}
			
			if (ctr == 0) {
				SetHeadIcon(true, houIcon);
			}
			
//			if (ctr == 10) {
//				SetHeadIcon(false, houIcon);
//			}
		}
		
		if (endingC1TextBox.dialogueFinished) {
			SetHeadIcon(false, houIcon);
		}

		if (endingComplete) {
			if (sher.transform.GetChild(5).GetComponent<DialogueTrigger>().dialogueFinished) {
				checkPoint1 = true;
			}
			
			if (may.transform.GetChild(5).GetComponent<DialogueTrigger>().dialogueFinished) {
				checkPoint2 = true;
			}
			
			if (checkPoint1 && checkPoint2) {
				flash.Fade();
				
				if (flash.fadeComplete) {
					PlayerPrefs.SetInt("Playthrough", PlayerPrefs.GetInt("Playthrough") + 1);
					PlayerPrefs.SetString("Complete", "True");
					Application.LoadLevel(0);
				}
			}
		}
	}

	void BeginEndingC2() {
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
			Vector3 originPos = new Vector3(30f, 6f, 0f);
			
			hou.transform.position = Vector3.MoveTowards(hou.transform.position, originPos, 3f * Time.deltaTime);
			may.transform.position = Vector3.MoveTowards(may.transform.position, originPos + new Vector3(-2f, -1f, 0f), 2f * Time.deltaTime);
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

			if (ctr == 6) {
				hou.Pose(true, 1f);
			}

			if (ctr == 7) {
				hou.Pose(false, 1f);
			}

			if (ctr == 7) {
				hou.Laugh(true, 1f);
			}

			if (ctr == 8) {
				hou.Laugh(false, 1f);
			}
			
			if (ctr == 9) {
				endingC1TextBox.pause = true;
				
				Transform train = GameObject.Find ("Bus").transform;
				if (train.position.x >= 17f) {
					train.GetComponent<SpriteRenderer>().enabled = true;
					train.position = new Vector3(train.position.x - 10f * Time.deltaTime, train.position.y, 0f);
				} else {
					train.GetComponent<SpriteRenderer>().enabled = false;
					charContr.disableInput = false;
				}
				
				if (train.position.x <= 33f && train.position.x > 17f) {
					endingC1TextBox.EndDialogue();
					endingComplete = true;
					
					//Set sher worried animation
					//Play splash
					hou.EnableDialogueTrigger(100);
					sher.EnableDialogueTrigger(12);
					may.EnableDialogueTrigger(12);
					
					hou.GetComponent<SpriteRenderer>().enabled = false;
					hou.GetComponent<BoxCollider2D>().enabled = false;
					
					endingC1TextBox.gameObject.SetActive(false);
					charContr.disableInput = true;
				}
				
				
			}
		}
		
		if (endingC1TextBox.dialoguePlaying) {
			int ctr = endingC1TextBox.counter;
			
//			if (ctr == 1) {
//				SetHeadIcon(true, sherIcon);
//			}
//			
//			if (ctr == 1 || ctr == 6) {
//				SetHeadIcon(true, mayIcon);
//			}
			
			if (ctr == 0) {
				SetHeadIcon(true, houIcon);
			}
			
//			if (ctr == 10) {
//				SetHeadIcon(false, houIcon);
//			}
		}
		
		if (endingC1TextBox.dialogueFinished) {
			SetHeadIcon(false, houIcon);
		}
		
		if (endingComplete) {
			if (sher.transform.GetChild(12).GetComponent<DialogueTrigger>().dialogueFinished) {
				checkPoint1 = true;
			}
			
			if (may.transform.GetChild(12).GetComponent<DialogueTrigger>().dialogueFinished) {
				checkPoint2 = true;
			}
			
			if (checkPoint1 && checkPoint2) {
				flash.Fade();
				
				if (flash.fadeComplete) {
					PlayerPrefs.SetInt("Playthrough", PlayerPrefs.GetInt("Playthrough") + 1);
					PlayerPrefs.SetString("Complete", "True");
					Application.LoadLevel(0);
				}
			}
		}
	}

	void BeginEndingC3() {
		if (part1 && !part2) {
			if (endingC1TextBox.counter == 0 && endingC1TextBox.dialogueComplete) {
				Vector3 finalPos = charContr.transform.position + new Vector3 (-12f, 0f, 0f);
				finalPos = new Vector3 (finalPos.x, hou.transform.position.y, 0f);
				hou.transform.position = Vector3.MoveTowards (hou.transform.position, finalPos, 3f * Time.deltaTime);
				hou.Run(true, 0f);
				
				if (Mathf.Abs (hou.transform.position.x - finalPos.x) < 0.1f) {
					hou.Run(true, 1f);
					sher.Run(true, 1f);
					may.Walk(true, 1f);
					
					sher.transform.position = hou.transform.position + new Vector3(-2f, -0.5f, 0f);
					may.transform.position = hou.transform.position + new Vector3(-1f, -1.5f, 0f);
					
					part1 = false;
					part2 = false;
				}
			}
		} 
		
		if (!part2 && !part1) {
			Vector3 originPos = new Vector3(30f, 6f, 0f);
			
			hou.transform.position = Vector3.MoveTowards(hou.transform.position, originPos, 3f * Time.deltaTime);
			may.transform.position = Vector3.MoveTowards(may.transform.position, originPos + new Vector3(-2f, -1f, 0f), 2f * Time.deltaTime);
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
				may.Walk(false, 1f);
			}
			
			if (houFinalPos && mayFinalPos && sherFinalPos) {
				part1 = false;
				part2 = true;
				endingC1TextBox.pause = false;
			}
		}
		
		if (part2 && !part1) {
			int ctr = endingC1TextBox.counter;
			
//			if (ctr == 6) {
//				hou.Laugh(1f);
//			}

			if (ctr == 56) {
				hou.Pose(true, 0f);
			}
			
			if (ctr == 48) {
				endingC1TextBox.pause = true;
				
				Transform train = GameObject.Find ("Bus").transform;
				if (train.position.x >= 17f) {
					train.GetComponent<SpriteRenderer>().enabled = true;
					train.position = new Vector3(train.position.x - 10f * Time.deltaTime, train.position.y, 0f);
				} else {
					train.GetComponent<SpriteRenderer>().enabled = false;
					charContr.disableInput = false;
				}
				
				if (train.position.x <= 33f && train.position.x > 17f) {
					endingC1TextBox.EndDialogue();
					endingComplete = true;
					
					//Set sher worried animation
					//Play splash
					hou.EnableDialogueTrigger(100);
					sher.EnableDialogueTrigger(13);
					may.EnableDialogueTrigger(13);
					
					hou.GetComponent<SpriteRenderer>().enabled = false;
					hou.GetComponent<BoxCollider2D>().enabled = false;
					
					endingC1TextBox.gameObject.SetActive(false);
					charContr.disableInput = true;
				}
				
				
			}
		}
		
		if (endingC1TextBox.dialoguePlaying) {
			int ctr = endingC1TextBox.counter;
			
			if (ctr == 4 || ctr == 6 || ctr == 15 || ctr == 21 || ctr == 26 || ctr == 38) {
				SetHeadIcon(true, sherIcon);
			}
			
			if (ctr == 35 || ctr == 45) {
				SetHeadIcon(true, mayIcon);
			}
			
			if (ctr == 0 || ctr == 5 || ctr == 14 || ctr == 20 || ctr == 25 || ctr == 48) {
				SetHeadIcon(true, houIcon);
			}
			
			if (ctr == 57 || ctr == 58) {
				SetHeadIcon(false, houIcon);
			}

		}
		
		if (endingC1TextBox.dialogueFinished) {
			SetHeadIcon(false, houIcon);
		}
		
		if (endingComplete) {
			if (sher.transform.GetChild(13).GetComponent<DialogueTrigger>().dialogueFinished) {
				checkPoint1 = true;
			}
			
			if (may.transform.GetChild(13).GetComponent<DialogueTrigger>().dialogueFinished) {
				checkPoint2 = true;
			}
			
			if (checkPoint1 && checkPoint2) {
				flash.Fade();
				
				if (flash.fadeComplete) {
					PlayerPrefs.SetInt("Playthrough", PlayerPrefs.GetInt("Playthrough") + 1);
					PlayerPrefs.SetString("Complete", "True");
					Application.LoadLevel(0);
				}
			}
		}
	}
}
