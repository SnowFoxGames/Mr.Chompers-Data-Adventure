using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class Player : MonoBehaviour {

    [SerializeField] GameObject[] movePoints;
    [SerializeField] bool freeRoam = false;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] GameObject deathParticle;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip moveSound;
    enum PlayerState{OnRails, Free}
    int positionNumber = 1;

    PlayerState currentPlayerState;
    Animator myAnimator;
    Rigidbody2D myRigidBody;
    CircleCollider2D myBody;
	// Use this for initialization
	void Start () {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<CircleCollider2D>();
        if(freeRoam == false)
        {
            currentPlayerState = PlayerState.OnRails;
            ChangePosition();
        }
        else
        {
            myAnimator.SetBool("isIdle", true);
            currentPlayerState = PlayerState.Free;
        }


	}
	
	// Update is called once per frame
	void Update () {
        CheckPlayerState();
	}

    void CheckPlayerState()
    {
        switch (currentPlayerState)
        {
            case PlayerState.OnRails:
                myRigidBody.gravityScale = 0;
                myRigidBody.velocity = new Vector2(0, 0);
                myRigidBody.bodyType = RigidbodyType2D.Kinematic;
                InputMove();
                break;
            case PlayerState.Free:
                FreeMove();
                Jump();
                break;
        }
    }

    private void InputMove()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if(positionNumber > 0)
            {
                positionNumber--;
            }
            ChangePosition();
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)|| Input.GetKeyDown(KeyCode.S))
        {
            if(positionNumber < movePoints.Length-1)
            {
                positionNumber++;
            }
            ChangePosition();
        }
    }
    private void ChangePosition()
    {
        AudioSource.PlayClipAtPoint(moveSound, Camera.main.transform.position, .5f);
        transform.position = movePoints[positionNumber].transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        myAnimator.SetBool("inRange", true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        myAnimator.SetBool("inRange", false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<Data>() !=null)
        {
            other.gameObject.GetComponent<Data>().CauseEffectAndDestroy();
            myAnimator.SetBool("inRange", false);
        }

    }

    private void FreeMove()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * moveSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        if(Mathf.Abs(myRigidBody.velocity.x) > 0)
        {
            myAnimator.SetBool("isIdle", false);
        }
        else
        {
            myAnimator.SetBool("isIdle", true);
        }
        FlipSprite(controlThrow);
    }
    private void Jump()
    {
       // Debug.Log(myRigidBody.velocity.y);
       /* if(myRigidBody.velocity.y > 0)
        {
            myBody.enabled = false;
        }
        else
        {
            myBody.enabled = true;
        }*/
        if(!myBody.IsTouchingLayers(LayerMask.GetMask("Platform")))
        {
            return;
        }

        if(CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
            AudioSource.PlayClipAtPoint(jumpSound, Camera.main.transform.position, .5f);
        }


    }

    private void FlipSprite(float direction)
    {
        if (direction == 0)
        { return; }
        float ratio = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector2(-Mathf.Sign(direction) * ratio, 1f *ratio);
        //Debug.Log("Check" + direction);
    }
    public void Die()
    {
        gameObject.SetActive(false);
        GameObject xplode = Instantiate(deathParticle, transform.position, Quaternion.identity) as GameObject;
        FindObjectOfType<GameSession>().StartCoroutine(FindObjectOfType<GameSession>().GameOver());
    }

}
