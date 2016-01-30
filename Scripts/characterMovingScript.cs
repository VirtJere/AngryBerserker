using UnityEngine;
using System.Collections;

public class characterMovingScript : MonoBehaviour {
    Vector3 movement = new Vector3(0, 0);
    float horizontal;
    float vertical;
    Rigidbody2D rig2d;
    bool _isJumping = false;
    public GameObject rayStart;
    float seconds = 0;
    // Use this for initialization
    void Start () {
        rig2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        
        horizontal = Input.GetAxis("Horizontal");
        RaycastHit2D ray = Physics2D.Raycast(rayStart.transform.position, rayStart.transform.position + Vector3.up * -0.05f);

        if (horizontal != 0)
        {
            movement.x = 5 * horizontal;
            
        } else
        {
            movement.x = 0;
        }
        

        if (Input.GetButtonDown("Jump") && !_isJumping)
        {
            movement.y = 5;
            _isJumping = true;
        }

        if (ray.collider == null)
        {
            _isJumping = true;
        }

        if (_isJumping){
            movement.y -= Time.deltaTime * 5 * 1.8f;
            


            if (ray.collider != null) 
            {
                if(ray.collider.tag != "Player" && movement.y <= 0)
                {
                    movement.y = 0;
                    _isJumping = false;
                }
            }
        }
        rig2d.velocity = movement;

    }
}
