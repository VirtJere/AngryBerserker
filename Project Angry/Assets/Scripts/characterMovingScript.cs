using UnityEngine;
using System.Collections;

public class characterMovingScript : MonoBehaviour {
    public Vector3 movement = new Vector3(0, 0); //Helps too see what direction should we go
	Vector3 faceDir = new Vector3(1, 1, 1); //Reference to save sprite lookdirection

    public float horizontal; //For input to left and right
    float vertical;
    Rigidbody2D rig2d;
	public LayerMask environment;

    public bool _isJumping = false; // Check if character is falling
	bool _canMove = true;
	public bool _tookDamage = false; // Did someone do damage to this character

    public GameObject rayStart; // Empty gameObject from where we cast a raycast
	public GameObject rayFront;

    void Start () {
        rig2d = GetComponent<Rigidbody2D>(); // get the rigidbody2d from gameobject
	}
	

	void Update () {
        
		if (_canMove) {
			moveCharacter ();
		} else {
			horizontal = 0;
		}
		checkFalling (); // Continuosly check are we falling?
    }

	void moveCharacter(){
		horizontal = Input.GetAxis("Horizontal"); // Get horizontal input

		if (horizontal != 0) // If we are pressing left or right assign it to movement
		{
			transform.Translate(Vector2.right * 5 * horizontal * Time.deltaTime);

		} 

		if (horizontal < 0) { // check what direction we should be going left == <0 and right == >0 
			faceDir = new Vector3 (-1, 1, 1);
		} else if (horizontal > 0) {
			faceDir =  new Vector3 (1, 1, 1);
		}

		transform.localScale = faceDir; //Set lookdirection
	}

	void checkFalling(){
		Vector3 rayDir = new Vector3 (0, -1f, 0); //Create vector with little angle

		RaycastHit2D ray = Physics2D.Raycast(rayStart.transform.position, rayDir, 0.5f); //create a ray to check if there is ground under the feet
		Debug.DrawRay(rayStart.transform.position, rayDir, Color.blue); //Debugging, remove when done
		if (Input.GetButtonDown("Jump") && !_isJumping) //Did we press spacebar and are we jumping or not?
		{
			rig2d.AddForce (Vector2.up * 525);
			_isJumping = true; //Declare that we jumped
		}

		if (ray.collider == null) //If we fall etc, we have to say this to not jump in air
		{
			_isJumping = true;
		}

		if (_isJumping){ // lets stick in here while we are falling/jumping

			if (ray.collider != null)  //Ray detects something
			{
				switch (ray.collider.tag) {
				case "Ground":
					_isJumping = false;
					break;
				case "Enemy":
					rig2d.AddForce (Vector2.up * 125);
					break;
				}
			}
		}

	}

	void checkFront(){
		Vector3 forwardRay = new Vector3 (faceDir.x, 0, 0);
		RaycastHit2D ray = Physics2D.Raycast(rayFront.transform.position, forwardRay, 1f); //scan front
		Debug.DrawRay(rayStart.transform.position, forwardRay, Color.blue); //Debugging, remove when done
	}

	public IEnumerator invincibility(){
		float time = 2f;
		_canMove = false;
		Debug.Log ("DO");
		yield return new WaitForSeconds (time);
		_tookDamage = false;
		_canMove = true;
	}
}
