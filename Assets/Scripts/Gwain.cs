﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gwain : MonoBehaviour {

    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float stabSpeed = 4f;
    [SerializeField] GameObject flash;
    [SerializeField] float flashInterval = .25f;
    [SerializeField] GameObject laser;

    [SerializeField] float yLaserPos;
    [SerializeField] float xLaserPos;
    [SerializeField] int laserQuantity = 3;

    [SerializeField] GameObject text;
    [SerializeField] GameObject magicParticle;

    [Header("Times")]
    [SerializeField] float idleTime = 5f;
    [SerializeField] float aimTime = 5f;
    [SerializeField] float returnDelay = 2f;

    enum State{Idle, Stab,Fire, Return}

    bool becameIdle;
    bool becameSword;
    bool trackPlayer;
    bool spawningLaser;
    Vector3 pointOfOrigin;

    Animator myAnimator;
    State currentState;
    Player player;
	// Use this for initialization
	void Start () {
        becameIdle = false;
        becameSword = false;
        trackPlayer = false;
        spawningLaser = false;
        currentState = State.Idle;
        myAnimator = GetComponent<Animator>();
        pointOfOrigin = transform.position;
        player = (Player)FindObjectOfType(typeof(Player));
	}
	
	// Update is called once per frame
	void Update () {
        text.transform.rotation = Quaternion.identity;
        TrackPlayer();
        switch (currentState)
        {
            case State.Idle:
                
                myAnimator.SetBool("isSword", false);
                myAnimator.SetBool("raiseArm", false);
                FlipSprite();
                if(player.isActiveAndEnabled)
                {
                    StartCoroutine(ChangeFromIdle());  
                }

                break;
            case State.Stab:
                myAnimator.SetBool("isSword", true); 

                StartCoroutine(AttackPlayer());

                break;
            case State.Fire:
                myAnimator.SetBool("raiseArm", true);

                StartCoroutine(CreateLasers());
                break;
            case State.Return:
                
                ResetPosition();
                break;

        }
	}

  

    IEnumerator ChangeFromIdle()
    {
        if(becameIdle == false)
        {
            becameIdle = true;
            Debug.Log("Idle");
            yield return new WaitForSeconds(idleTime);

            currentState = (State)Random.Range(1, 3);
            Debug.Log("Switching to: "+ currentState);
            becameIdle = false;
        }


    }
    IEnumerator AttackPlayer()
    {
        if (becameSword == false)
        {
            bool wait = false;

            becameSword = true;
            yield return new WaitForSeconds(1);
            trackPlayer = true;

            Debug.Log("Sword");
            yield return new WaitForSeconds(aimTime);
            Debug.Log("Attack");
            Vector3 playerPosition = player.transform.position;
            trackPlayer = false;
            flash.gameObject.SetActive(true);
            yield return new WaitForSeconds(flashInterval);
            flash.gameObject.SetActive(false);
            yield return new WaitForSeconds(flashInterval);
            flash.gameObject.SetActive(true);
            yield return new WaitForSeconds(flashInterval);
            flash.gameObject.SetActive(false);
            yield return new WaitForSeconds(flashInterval);

            while (transform.position != playerPosition)
            {
                if (wait == false)
                {
                    wait = true;
                    transform.position = Vector2.MoveTowards(transform.position, playerPosition, moveSpeed * Time.deltaTime * stabSpeed);
                    yield return new WaitForSeconds(.001f);
                    wait = false;
                }

            }

            yield return new WaitForSeconds(returnDelay);

            currentState = State.Return;
            becameSword = false;
        }
    }
    void TrackPlayer()
    {
        if (trackPlayer == true) 
        {
            Vector2 playerPosition = player.transform.position;
            Vector2 direction = new Vector2(playerPosition.x - transform.position.x,
                                            playerPosition.y - transform.position.y);
            transform.up = -direction;
        }

    }
    void ResetPosition()
    {
        transform.up = new Vector3 (0, 0, 0);
       
        transform.position = Vector2.MoveTowards(transform.position, pointOfOrigin, moveSpeed * Time.deltaTime);
        if(transform.position != pointOfOrigin)
        {
            
        }
        else
        {
            currentState = State.Idle;
        }
    }

    IEnumerator CreateLasers()
    {
        if(spawningLaser == false)
        {
            spawningLaser = true;

            for (int i = 0; i < laserQuantity; i++)
            {
                yield return new WaitForSeconds(0.5f);

                Vector2 laserPos = new Vector2(Mathf.Round(Random.Range(-xLaserPos, xLaserPos)), yLaserPos);
                //Debug.Log(laserPos);
                GameObject newLaser = Instantiate(laser, laserPos, Quaternion.identity);
            }



            yield return new WaitForSeconds(1);
            currentState = State.Return;
            spawningLaser = false;
        }

    }
    private void FlipSprite()
    {
       
        float ratio = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector2(Mathf.Sign(transform.position.x - player.transform.position.x) * ratio, 1f * ratio);
        text.transform.localScale = new Vector2 (transform.localScale.x / Mathf.Abs(transform.localScale.x), 1);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            
            FindObjectOfType<GameSession>().TakeLives();
        }
    }
    public void ToggleEffect()
    {
        if(magicParticle.gameObject.activeInHierarchy)
        {
            magicParticle.SetActive(false);
        }
        else
        {
            magicParticle.SetActive(true);
        }

    }
  

}