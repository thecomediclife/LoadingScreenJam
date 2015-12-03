using UnityEngine;
using System.Collections;

public class GrassScript : MonoBehaviour {

	private bool hit;
	public DoorScript door;
	public Transform rupee;

	public float spawnChance = 0.8f;

	// Use this for initialization
	void Start () {
		this.GetComponent<Animator> ().speed = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (door.teleported) {
			ResetGrass();
		}
	}


	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Sword" && !hit) {
			hit = true;

			this.GetComponent<Animator> ().speed = 0.5f;
			StartCoroutine(SpawnRupee());
		}
	}

	void ResetGrass() {
		this.GetComponent<Animator>().Play ("Play", -1, 0f);
		this.GetComponent<Animator> ().speed = 0f;
		hit = false;
	}

	public IEnumerator SpawnRupee() {
		yield return new WaitForSeconds(0.75f);
		if (Random.Range(0f,1f) < spawnChance) {
			Instantiate(rupee, transform.position, Quaternion.identity);
		}
	}
}
