using UnityEngine;
using System.Collections;

public class BushScript : MonoBehaviour {
	public Transform player;
	public BushParentScript bushParent;
	public bool playerEntered;

	public bool catEntered;

	public int bushIndex;

	private bool shake;
	private float timer;
	private Vector3 originPos;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player").transform;
		bushParent = this.transform.parent.GetComponent<BushParentScript> ();
		originPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (playerEntered) {
			if (Input.GetButtonDown("Fire1") && !bushParent.activated) {
				bushParent.CheckBush(bushIndex);
			}
		}

		if (shake && Time.time < timer) {
			Vector2 randoVec2 = Random.insideUnitCircle;
			Vector3 randoVec3 = new Vector3 (randoVec2.x, randoVec2.y, 0f);
			randoVec3 = randoVec3 * 0.05f;
			this.transform.position = originPos + randoVec3;
		} else {
			this.transform.position = originPos;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.transform == player) {
			playerEntered = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.transform == player) {
			playerEntered = false;
		}
	}

	public void ShakeBush() {
		shake = true;
		timer = Time.time + 1.0f;
	}
}
