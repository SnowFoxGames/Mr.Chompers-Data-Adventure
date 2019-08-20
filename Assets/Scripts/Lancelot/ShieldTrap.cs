using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldTrap : MonoBehaviour {

    [SerializeField] GameObject outerShield;
    [SerializeField] float buffer = .3f;
    [SerializeField] float trapDistance = 1f;
    public float moveSpeed;
    public float delaySeconds =5f;
    [SerializeField] GameObject trap;
    [SerializeField] GameObject bubbleBackground;
    [SerializeField] float speed = 2;
    [SerializeField] AudioClip bounce;

    bool trapped = false;
    bool moving = true;
    bool countingDown;
    Player player;
    float radius;
    float distance;
    Vector2 playerPosition;
    Vector2 playerRelativePosition;


	// Use this for initialization
	void Start () {
        player = (Player)FindObjectOfType(typeof(Player));
        radius = GetComponent<CircleCollider2D>().radius;

       GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(-10, 10)* speed, Random.Range(-10, 10)*speed);
       
	}

    private void FixedUpdate()
    {
        playerPosition = player.transform.position;
        playerRelativePosition = new Vector2(player.transform.position.x - transform.position.x,
                                                    player.transform.position.y - transform.position.y);
        distance = Mathf.Sqrt(Mathf.Pow(playerRelativePosition.x, 2) + Mathf.Pow(playerRelativePosition.y, 2));

        outerShield.SetActive(false);
        DetectPlayerPosition();
        RestrictMovement();
    }
    // Update is called once per frame
    void Update () 
    {
        //MoveTowardPlayer();
        FreeMove();
        Debug.Log(distance);
	}

    private void DetectPlayerPosition()
    {
        if(moving == false)
        {
            if (distance >= trapDistance &&
            trapped == false)
            {

                Debug.Log("Not Trapped");
                ToggleBarrier();
            }
            else
            {
                Debug.Log("Trapped");
                trapped = true;
            }
        }

    }

    void RestrictMovement()
    {
       

        if(trapped == true)
        {
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

           /* if (playerRelativePosition.x > (radius) || playerRelativePosition.x < (radius) * -1)
            {
                player.transform.position = new Vector2((transform.position.x) + ((radius) * (playerRelativePosition.x /
                                                         Mathf.Abs(playerRelativePosition.x))),
                                                        playerPosition.y);
            }

            if (playerRelativePosition.y > (radius)|| playerRelativePosition.y < (radius) * -1)
            {
                player.transform.position = new Vector2(playerPosition.x, transform.position.y + ((radius) * (playerRelativePosition.y /
                                                         Mathf.Abs(playerRelativePosition.y))));
            }*/
        }



    }
    void ToggleBarrier()
    {
        outerShield.SetActive(true);
    }

    void MoveTowardPlayer()
    {
        if(moving == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
            StartCoroutine(CountdownToBubble());
        }

    }

    void FreeMove()
    {
        if(moving == true)
        {
            
            StartCoroutine(CountdownToBubble());
        }
    }

    IEnumerator CountdownToBubble()
    {
        if(countingDown == false)
        {
            countingDown = true;
            yield return new WaitForSeconds(delaySeconds);
            Color temp = bubbleBackground.GetComponent<SpriteRenderer>().color;

            temp.a = 1;

            bubbleBackground.GetComponent<SpriteRenderer>().color = temp; 

            trap.SetActive(true);
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            moving = false;
        }


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(moving)
        {
            AudioSource.PlayClipAtPoint(bounce, Camera.main.transform.position, .7f);
        }

    }
}
