using UnityEngine;
using System.Collections;

public class FlashScript : MonoBehaviour {
	private SpriteRenderer spr;
	private bool run;
	private bool fade;

	private float counter;
	private bool waiting;

	public bool fadeComplete;

	// Use this for initialization
	void Awake () {
		spr = this.GetComponent<SpriteRenderer> ();
		spr.color = new Color (1f, 1f, 1f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (run) {
			counter += 10f * Time.deltaTime;

			spr.color = Color.Lerp(new Color(1f, 1f, 1f, 0f), new Color(1f, 1f, 1f, 1f), counter);

			if (1f - spr.color.a < 0.01f && !waiting) {
				StartCoroutine(Wait ());
				waiting = true;
			}
		}

		if (fade) {
			counter += 1f * Time.deltaTime;

			spr.color = Color.Lerp(new Color(0f, 0f, 0f, 0f), new Color(0f, 0f, 0f, 1f), counter);

			if (1f - spr.color.a < 0.01f ) {
				fadeComplete = true;
			}
		}

//		if (Input.GetKeyDown (KeyCode.Q)) {
//			Flash ();
//		}
	}

	public void Flash() {
		if (!run)
			run = true;
	}

	public IEnumerator Wait() {
		yield return new WaitForSeconds(1f);
		spr.color = new Color (1f, 1f, 1f, 0f);
		counter = 0f;
		run = false;
		waiting = false;
	}

	public void Fade() {
		if (!fade)
			fade = true;
	}
}
