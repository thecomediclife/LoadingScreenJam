using UnityEngine;
using System.Collections;

public class BushParentScript : MonoBehaviour {
	public DoorScript door;
	public SpriteRenderer cat;
	public SpriteRenderer crash;
	public Transform dialogueBox, dialogueBox2, dialogueBox3;
	public CharController charContr;
	public Transform finalPos;

	public BushScript[] bushes = new BushScript[4];

	public int chosenBush;

	private float timer;
	private bool spawnCat;
	public bool activated;
	private bool moveCatFail, moveCatSuccess;
	private bool success;

	public bool completed;

	// Use this for initialization
	void Start () {
		bushes[0] = transform.GetChild (0).GetComponent<BushScript> ();
		bushes[1] = transform.GetChild (1).GetComponent<BushScript> ();
		bushes[2] = transform.GetChild (2).GetComponent<BushScript> ();
		bushes[3] = transform.GetChild (3).GetComponent<BushScript> ();
		cat.enabled = false;
		crash.enabled = false;
		dialogueBox.GetComponent<BoxCollider2D> ().enabled = false;
		dialogueBox2.GetComponent<BoxCollider2D>().enabled = false;
		dialogueBox3.GetComponent<BoxCollider2D>().enabled = false;
		charContr = GameObject.Find ("Player").GetComponent<CharController> ();

		ResetRoom ();
	}
	
	// Update is called once per frame
	void Update () {
		if (door.teleported) {
			ResetRoom();
		}

		if (Time.time > timer && spawnCat) {
			spawnCat = false;
			cat.enabled = true;

			if (success) {
				moveCatFail = false;
				moveCatSuccess = true;
			} else {
				moveCatSuccess = false;
				moveCatFail = true;
			}
		}

		if (moveCatFail) {
			charContr.disableInput = true;
			cat.transform.position = Vector3.MoveTowards(cat.transform.position, crash.transform.position, 5f * Time.deltaTime);

			if (Vector3.Magnitude(cat.transform.position - crash.transform.position) < 0.01f) {
				cat.enabled = false;
				crash.enabled = true;
				moveCatFail = false;

				dialogueBox3.GetComponent<Collider2D>().enabled = true;
				charContr.disableInput = false;
			}
		}

		if (moveCatSuccess) {
			charContr.disableInput = true;
			cat.transform.position = Vector3.MoveTowards(cat.transform.position, finalPos.position, 1f * Time.deltaTime);

			if (Vector3.Magnitude(cat.transform.position - finalPos.position) < 0.01f) {
				moveCatSuccess = false;
				dialogueBox.GetComponent<BoxCollider2D> ().enabled = true;
				charContr.disableInput = false;
			}
		}

		if (dialogueBox.GetComponent<DialogueTrigger> ().dialogueFinished) {
			if (dialogueBox.gameObject.activeSelf == true)
				dialogueBox.gameObject.SetActive(false);
			dialogueBox2.GetComponent<BoxCollider2D>().enabled = true;
			PlayerPrefs.SetString("BusTicket", "True");
			completed = true;
		}
	}

	void ResetRoom() {
		activated = false;
		spawnCat = false;
		moveCatFail = false;
		moveCatSuccess = false;
		timer = 0.0f;
		cat.enabled = false;
		crash.enabled = false;

		chosenBush = Random.Range (0, 4);
		cat.transform.position = bushes [chosenBush].transform.position;

		dialogueBox3.GetComponent<BoxCollider2D>().enabled = false;

		if (PlayerPrefs.GetString ("BusTicket") == "True") {
			cat.transform.position = finalPos.position;
			cat.enabled = true;

			if (dialogueBox.gameObject.activeSelf == true)
				dialogueBox.gameObject.SetActive(false);
			dialogueBox.GetComponent<BoxCollider2D>().enabled = false;
			dialogueBox2.GetComponent<BoxCollider2D>().enabled = true;

			completed = true;
		}
	}

	public void CheckBush(int index) {
		if (!completed) {
			if (chosenBush == index) {
				bushes [index].ShakeBush ();
				success = true;
			} else {
				bushes [chosenBush].ShakeBush ();
				success = false;
			}

			timer = Time.time + 1f;
			spawnCat = true;
			activated = true;

			charContr.disableInput = true;
		}
	}
}
