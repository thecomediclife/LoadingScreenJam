using UnityEngine;
using System.Collections;

public class CharController : MonoBehaviour {
	public float speed = 5.0f;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = this.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {

		rb.velocity = new Vector2 (Input.GetAxis ("Horizontal") * speed, Input.GetAxis ("Vertical") * speed);
	}
}
