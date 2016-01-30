using UnityEngine;
using System.Collections;
using UnityEngine.Sprites;

public class AI_Walk : MonoBehaviour {

	Rigidbody2D rig2d;

	public Transform rayStart;

	bool _active = false;

	Vector3 aiScale;
	Vector3 dir = new Vector3(0,0,0);
	characterMovingScript playerScript = null;

	float speed = 3;

	Sprite aiSprite;

	// Use this for initialization
	void Start () {
		rig2d = GetComponent<Rigidbody2D> ();
		aiSprite = GetComponent<Sprite> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (_active) {
			checkEdges ();
			MoveForward ();
		}
	}

	void MoveForward(){
		transform.Translate (Vector2.right * aiScale.x * speed * Time.deltaTime);
	}

	void checkEdges(){
		aiScale = transform.localScale;
		Vector3 rayDir = new Vector3 (aiScale.x, -1f, 0);


		RaycastHit2D ray = Physics2D.Raycast(rayStart.position, Vector3.down, 2f);
		Debug.DrawLine (rayStart.position, rayStart.position + rayDir, Color.red);
		Debug.DrawRay(rayStart.position, Vector3.down, Color.blue);

		if (ray.collider != null) {
			switch (ray.collider.tag) {
			case "Player":
				hitPlayer (ray);
				break;
			default:
				break;
			}

		} else {
			aiScale.x *= -1;
		}


		transform.localScale = aiScale;
	}

	void hitPlayer(RaycastHit2D ray){
		playerScript = ray.collider.gameObject.GetComponent<characterMovingScript> ();
		Rigidbody2D playerRig = ray.collider.gameObject.GetComponent<Rigidbody2D> ();
		if (!playerScript._tookDamage) {
			playerRig.AddForce (new Vector2 (aiScale.x, 1) * 300);
			playerScript.StartCoroutine ("invincibility");
			StartCoroutine ("WaitLaugh");
			playerScript.hearts[playerScript.health].SetActive(false);
			playerScript.health--;
			playerScript._tookDamage = true;

		}
	}

	void OnBecameVisible(){
		_active = true;
	}

	void OnBecameInvisible(){
		if (transform.position.x < GameObject.FindGameObjectWithTag ("Player").transform.position.x) {
			Debug.Log ("Destroyed " + gameObject.name);
			Destroy (gameObject);
		}
	}

	IEnumerator WaitLaugh(){
		speed = 0;
		yield return new WaitForSeconds (2F);
		speed = 3;
	}
}
