using UnityEngine;
using System.Collections;

public class TitleScreenScript : MonoBehaviour {
	public Transform cursor;
	public Transform cam;
	public TextMesh titleText;
	public TextMesh startButton;
	public TextMesh quitButton;
	public TextMesh deleteButton;

//	private bool start = true;
	private int choice = 0;

	private bool beginLoading;
	private bool completeCamMove;

	public float counter1, counter2;
	public int index;
	public float speed = 0.5f;

	private SpriteRenderer[] squares;

	private int playthrough;

	private bool deleteAvailable;

	void Awake() {
		squares = new SpriteRenderer[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) {
			squares[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
		}
	}

	// Use this for initialization
	void Start () {

		counter1 = Random.Range (0f, 2f);
		counter2 = Random.Range (0f, 4f);
		index = Mathf.FloorToInt(counter2)* Mathf.FloorToInt(counter1);

		if (PlayerPrefs.GetString ("Complete") == "Ongoing") {
			startButton.text = "Continue?";
			deleteButton.GetComponent<MeshRenderer> ().enabled = true;
			deleteAvailable = true;
		} else {
			deleteButton.GetComponent<MeshRenderer>().enabled = false;
		}

		if (PlayerPrefs.GetInt ("Playthrough") > 0) {
			titleText.text = "What if she could be saved?";

			if (PlayerPrefs.GetString("Complete") != "Ongoing") {
				startButton.text = "Try again?";
			}

			string quitt = "Quit";

			for (int i = 0; i < PlayerPrefs.GetInt("Playthrough"); i++) {
				quitt += "!";
			}
			quitButton.text = quitt;
		}

		playthrough = PlayerPrefs.GetInt ("Playthrough");
	}
	
	// Update is called once per frame
	void Update () {
//		if (Input.GetKey (KeyCode.Q) && Input.GetKey(KeyCode.P)) {
//			PlayerPrefs.DeleteAll();
//		}

		if (!beginLoading) {
			if (Input.GetButtonDown ("Up")) {
				//start = true;
				cursor.position = new Vector3 (-1.5f, 1f, 0f);

				choice--;

				if (choice < 0) {
					choice = 0;
				}
			}

			if (Input.GetButtonDown ("Down")) {
				//start = false;
				cursor.position = new Vector3 (-1.5f, 0f, 0f);

				choice ++;

				if (deleteAvailable) {
					if (choice > 2) {
						choice = 2;
					}
				} else {
					if (choice > 1) {
						choice = 1;
					}
				}
			}

			if (choice == 0) {
				cursor.position = new Vector3 (-1.5f, 1f, 0f);
			} else if (choice == 1) {
				cursor.position = new Vector3 (-1.5f, 0f, 0f);
			} else if (choice == 2) {
				cursor.position = new Vector3 (-1.5f, -1.15f, 0f);
			}

			if (Input.GetButtonDown ("Fire1") || Input.GetButtonDown ("Enter")) {
				if (choice == 0) {
					beginLoading = true;

				} else if (choice == 1) {
					Application.Quit ();
				} else if (choice == 2) {
					PlayerPrefs.DeleteAll();

					deleteButton.GetComponent<MeshRenderer>().enabled = false;
					deleteAvailable = false;

					startButton.text = "Start?";
					quitButton.text = "Quit";
					titleText.text = "Paradigm";

					choice = 0;
					cursor.position = new Vector3 (-1.5f, 1f, 0f);
				}
			}
		}

		if (beginLoading) {
			if (!completeCamMove) {
				cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3( 0f, -35f, -10f), 55f * Time.deltaTime);

				if (cam.transform.position.y < -34.9f) {
					cam.transform.position = new Vector3(0f, -35f, -100f);
					completeCamMove = true;

					StartCoroutine(LoadNext());
				}
			} else {
				if (Input.GetButton("Up")) {
					counter1 -= speed * Time.deltaTime;

					if (counter1 < 0f)
						counter1 = 0f;

					index = Mathf.FloorToInt(counter2)* Mathf.FloorToInt(counter1);
					squares[index].enabled = false;
				}

				if (Input.GetButton("Down")) {
					counter1 += speed * Time.deltaTime;

					if (counter1 > 2f)
						counter1 = 2f;

					index = Mathf.FloorToInt(counter2)* Mathf.FloorToInt(counter1);
					squares[index].enabled = false;
				}

				if (Input.GetButton("Left")) {
					counter2 -= speed * Time.deltaTime;

					if (counter2 < 0f)
						counter2 = 0f;

					index = Mathf.FloorToInt(counter2)* Mathf.FloorToInt(counter1);
					squares[index].enabled = false;
				}

				if (Input.GetButton("Right")) {
					counter2 += speed * Time.deltaTime;

					if (counter2 > 4f)
						counter2 = 4f;

					index = Mathf.FloorToInt(counter2)* Mathf.FloorToInt(counter1);
					squares[index].enabled = false;
				}
			}
		}

	}

	public IEnumerator LoadNext() {
		yield return new WaitForSeconds (Random.Range (4f, 5f));
		Application.LoadLevel (1);

		if (PlayerPrefs.GetString("Complete") == "True") {
			PlayerPrefs.DeleteAll();
			PlayerPrefs.SetInt("Playthrough", playthrough);
		}
	}
}
