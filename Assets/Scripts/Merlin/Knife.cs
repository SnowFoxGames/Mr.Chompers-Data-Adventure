using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour {

    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] float stabSpeed = .1f;
    [SerializeField] ParticleSystem particleSystem;
    public bool attack;
    bool summoned;
    Player player;
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    Vector3 playerPosition;
    Vector2 direction;
	// Use this for initialization
	void Start () {
        player = (Player)FindObjectOfType(typeof(Player));
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        TrackPlayer();
        Attack();
	}

    void TrackPlayer()
    {
        if(summoned)
        {
            playerPosition = player.transform.position;
            direction = playerPosition - transform.position;
            Vector3 trackDirection = new Vector3(playerPosition.x - transform.position.x,
                                            playerPosition.y - transform.position.y,
                                            transform.position.z);
            if (transform.up != -trackDirection)
            {
                transform.up += -trackDirection * Time.deltaTime * rotationSpeed;
            }
        }

    }

    public void StartTracking()
    {
        summoned = true;
        myAnimator.enabled = false;

    }
    void Attack()
    {
        if(attack)
        {
            summoned = false;
           // transform.position = Vector2.MoveTowards(transform.position, playerPosition , Time.deltaTime * stabSpeed);
            myRigidBody.velocity = (direction) * (stabSpeed * Time.deltaTime);
               
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
        Debug.Log(collision);
    }


}
