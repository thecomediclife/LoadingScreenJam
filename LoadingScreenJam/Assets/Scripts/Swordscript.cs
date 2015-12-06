using UnityEngine;
using System.Collections;

public class Swordscript : MonoBehaviour {
	public Transform player;
	private BoxCollider2D collidr;
	private SpriteRenderer rendr;

	public bool attached;
	private bool swinging;
	private Vector3 finalPos;
	private float angle;
	private float finalAngle;
	public float rotationRate = 0.25f;

	public DoorScript door;
	public Vector3 originPos;
	public Quaternion originRot;


	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player").transform;
		rendr = this.GetComponent<SpriteRenderer> ();
		collidr = this.GetComponent<BoxCollider2D> ();

		originPos = this.transform.position;
		originRot = this.transform.rotation;

//		if (attached) {
//			rendr.enabled = false;
//			collidr.enabled = false;
//			this.transform.parent = player;
//		}
	}
	
	// Update is called once per frame
	void Update () {
		if (door.teleported) {
			this.transform.parent = null;
			transform.position = originPos;
			transform.rotation = originRot;
			rendr.enabled = true;
			collidr.enabled = true;
			swinging = false;
			attached = false;
		}

		if (attached) {
			if (Input.GetButtonDown("Fire1") && !swinging) {
				swinging = true;

				finalPos = player.GetComponent<CharController>().forwardDir;

				if (finalPos == new Vector3(1f,0f,0f) || finalPos == Vector3.zero) {
					finalPos = new Vector3(1f,0f,0f);

					this.transform.localPosition = new Vector3(0f,1f,0f);
					rendr.enabled = true;
					collidr.enabled = true;

					angle = 90f;
					finalAngle = 0f;
				} else if (finalPos == new Vector3(-1f, 0f, 0f)) {

					this.transform.localPosition = new Vector3(0f,-1f,0f);
					rendr.enabled = true;
					collidr.enabled = true;

					angle = 270f;
					finalAngle = 180f;
				} else if (finalPos == new Vector3(0f, 1f, 0f)) {

					this.transform.localPosition = new Vector3(-1f,0f,0f);
					rendr.enabled = true;
					collidr.enabled = true;

					angle = 180f;
					finalAngle = 90f;
				} else if (finalPos == new Vector3(0f, -1f, 0f)) {

					this.transform.localPosition = new Vector3(1f,0f,0f);
					rendr.enabled = true;
					collidr.enabled = true;

					angle = 0f;
					finalAngle = -90f;
				}

				//this.transform.localPosition = player.GetComponent<CharController>().forwardDir;
				//this.transform.right = player.GetComponent<CharController>().forwardDir;
			}

			if (swinging) {
				angle -= rotationRate;

				this.transform.localPosition = new Vector3(1.3f * Mathf.Cos (Mathf.Deg2Rad * angle), 1.3f * Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
				this.transform.right = Vector3.Normalize(transform.position - transform.parent.position);

				if (angle < finalAngle) {
					swinging = false;
					rendr.enabled = false;
					collidr.enabled = false;
				}
			}

		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.transform == player && !attached) {
			rendr.enabled = false;
			collidr.enabled = false;
			this.transform.parent = player.transform.GetChild(0);
			attached = true;
		}
	}
}
