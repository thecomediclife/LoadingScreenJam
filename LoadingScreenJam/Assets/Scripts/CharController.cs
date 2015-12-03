using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour {
	public float speed = 5.0f;
	public bool disableInput = false;
	private Rigidbody2D rb;

	public Vector3 forwardDir;

	public bool resetPlayerPrefs;

	void Awake () {
		if (resetPlayerPrefs) {
			PlayerPrefs.DeleteAll();
		}
	}

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody2D> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (!disableInput) {
			rb.velocity = new Vector2 (Input.GetAxis ("Horizontal") * speed, Input.GetAxis ("Vertical") * speed);
		} else {
			rb.velocity = Vector2.zero;
		}

		if (Mathf.Abs (Input.GetAxis ("Horizontal")) > 0.01f || Mathf.Abs (Input.GetAxis ("Vertical")) > 0.01f) {
			if (Mathf.Abs (Mathf.Abs(Input.GetAxis("Horizontal")) - Mathf.Abs(Input.GetAxis("Vertical"))) > 0.01f) {
				if (Mathf.Abs(Input.GetAxis("Horizontal")) > Mathf.Abs (Input.GetAxis("Vertical"))) {
					forwardDir = new Vector3(Input.GetAxis("Horizontal"), 0f, 0f);
					forwardDir.Normalize();
				} else {
					forwardDir = new Vector3(0f, Input.GetAxis("Vertical"), 0f);
					forwardDir.Normalize();
				}
			}
		}
	}
}
