using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTrap : MonoBehaviour {

    [SerializeField] GameObject outerShield;
    [SerializeField] float buffer = .3f;

    bool trapped = false;
    Player player;
    float radius;
    Vector2 playerPosition;
    Vector2 playerRelativePosition;


	// Use this for initialization
	void Start () {
        player = (Player)FindObjectOfType(typeof(Player));
        radius = GetComponent<CircleCollider2D>().radius;
       
	}

    private void FixedUpdate()
    {
        playerPosition = player.transform.position;
        playerRelativePosition = new Vector2(player.transform.position.x - transform.position.x,
                                                    player.transform.position.y - transform.position.y);
        outerShield.SetActive(false);
        DetectPlayerPosition();
        RestrictMovement();
    }
    // Update is called once per frame
    void Update () 
    {
        


	}

    private void DetectPlayerPosition()
    {
        if (playerRelativePosition.x > (radius - buffer)|| playerRelativePosition.x < (radius - buffer) * -1 ||
            playerRelativePosition.y > (radius - buffer) || playerRelativePosition.y < (radius - buffer) * -1 &&
            trapped == false)
        {
            
            Debug.Log("Not Trapped");
           // ToggleBarrier();
        }
        else
        {
            Debug.Log("Trapped");
            trapped = true;
        }
    }

    void RestrictMovement()
    {
       

        if(trapped == true)
        {
            if (playerRelativePosition.x > (radius + buffer) || playerRelativePosition.x < (radius + buffer) * -1)
            {
                player.transform.position = new Vector2((transform.position.x) + ((radius + buffer) * (playerRelativePosition.x /
                                                         Mathf.Abs(playerRelativePosition.x))),
                                                        playerPosition.y);
            }

            if (playerRelativePosition.y > (radius + buffer)|| playerRelativePosition.y < (radius + buffer) * -1)
            {
                player.transform.position = new Vector2(playerPosition.x, transform.position.y + ((radius + buffer) * (playerRelativePosition.y /
                                                         Mathf.Abs(playerRelativePosition.y))));
            }
        }



    }
    void ToggleBarrier()
    {
        outerShield.SetActive(true);
    }
}
