using UnityEngine;
using System.Collections;

public class FlashScript : MonoBehaviour {
	private SpriteRenderer spr;
	private bool run;

	private float counter;
	private bool waiting;

	// Use this for initialization
	void Awake () {
		spr = this.GetComponent<SpriteRenderer> ();
		spr.color = new Color (1f, 1f, 1f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (run) {
			counter += 7.5f * Time.deltaTime;

			spr.color = Color.Lerp(new Color(1f, 1f, 1f, 0f), new Color(1f, 1f, 1f, 1f), counter);

			if (1f - spr.color.a < 0.01f && !waiting) {
				StartCoroutine(Wait ());
				waiting = true;
			}
		}
	}

	public void Flash() {
		if (!run)
			run = true;
	}

	public IEnumerator Wait() {
		yield return new WaitForSeconds(0.5f);
		spr.color = new Color (1f, 1f, 1f, 0f);
		counter = 0f;
		run = false;
		waiting = false;
	}
}
