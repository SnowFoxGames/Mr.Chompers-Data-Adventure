using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {
    [SerializeField] float moveSpeed = 500f;
    [SerializeField] float dropSpeed = .001f;
    [SerializeField] AudioClip smashSound;
    Player player;
    Vector2 playerPositionX;
    Vector2 playerPositionY;

    bool goingToAttack = false;
    bool playerHit = false;
	// Use this for initialization
	void Start () {
        player = (Player)FindObjectOfType(typeof(Player));
	}
	
	// Update is called once per frame
	void Update () {
        if(player != null)
        {
            playerPositionX = new Vector2(player.transform.position.x, transform.position.y);
            playerPositionY = new Vector2(transform.position.x, player.transform.position.y);
            FollowPlayer();
        }

	}

    private void FollowPlayer()
    {
        if(player != null && goingToAttack == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerPositionX, moveSpeed * Time.deltaTime);
        }
        if(transform.position.x == player.transform.position.x)
        {
            if(goingToAttack == false)
            {
                StartCoroutine(Attack());
            }

        }
    }

  

    IEnumerator Attack()
    {
        
        goingToAttack = true;
        bool wait = false;
        Vector2 storedPosition = transform.position;


        yield return new WaitForSeconds(.6f);

        Debug.Log("Attack!");
        while (player != null && transform.position.y > player.transform.position.y)
        {
            if(wait == false  )
            {
                wait = true;
                transform.position = Vector2.MoveTowards(transform.position, playerPositionY, moveSpeed * Time.deltaTime * dropSpeed);
                yield return new WaitForSeconds(.001f);
                wait = false;
            }


        }
        AudioSource.PlayClipAtPoint(smashSound, Camera.main.transform.position, 1f);
        yield return new WaitForSeconds(4f);
        while (transform.position.y < storedPosition.y)
        {
            if (wait == false)
            {
                wait = true;
                transform.position = Vector2.MoveTowards(transform.position, storedPosition, moveSpeed * Time.deltaTime * dropSpeed);
                yield return new WaitForSeconds(.001f);
                wait = false;
            }

        }
        goingToAttack = false;
        playerHit = false;
        Debug.Log("Cycle Over");



         

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>()&& playerHit == false)
        {
            playerHit = true;
            FindObjectOfType<GameSession>().TakeLives();
        }
    }
}
