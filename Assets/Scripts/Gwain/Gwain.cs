using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gwain : MonoBehaviour {
    
    [SerializeField] GameObject text;

    [Header("Speeds")]
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float stabSpeed = 4f;
    [SerializeField] float rotationSpeed = 4f;
    [Header("Sword Mode Stuff")]
    [SerializeField] GameObject flash;
    [SerializeField] float flashInterval = .25f;
    bool becameSword;
    bool trackPlayer;

    [Header("Laser Mode Stuff")]
    [SerializeField] GameObject laser;
    [SerializeField] float yLaserPos;
    [SerializeField] float xLaserPos;
    [SerializeField] int laserQuantity = 3;
    [SerializeField] GameObject magicParticle;
    bool spawningLaser;

    [Header("Times")]
    [SerializeField] float idleTime = 5f;
    [SerializeField] float aimTime = 5f;
    [SerializeField] float returnDelay = 2f;

    [Header("Sounds")]
    [SerializeField] AudioClip chestBeep;
    [SerializeField] AudioClip stabCharge;
    [SerializeField] AudioClip laserCharge;
    [SerializeField] AudioClip swordCharge;

    [Header("IdlePoints")]
    [SerializeField] GameObject topPoint;
    [SerializeField] GameObject bottomPoint;
    [SerializeField] float idleFloatSpeed = 1;
    bool becameIdle;
    bool goUp;

    enum State{Idle, Stab,Fire, Return}


   

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
        goUp = false;
        currentState = State.Idle;
        myAnimator = GetComponent<Animator>();
        pointOfOrigin = transform.position;
        player = (Player)FindObjectOfType(typeof(Player));
	}

    // Update is called once per frame

    private void FixedUpdate()
    {
        text.transform.rotation = Quaternion.identity;
    }
    void Update () {
       
        TrackPlayer();
        switch (currentState)
        {
            case State.Idle:
                
                myAnimator.SetBool("isSword", false);
                myAnimator.SetBool("raiseArm", false);
                IdleMovement();
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

    void IdleMovement()
    {
        if(goUp == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, topPoint.transform.position, idleFloatSpeed * Time.deltaTime);
            if (transform.position.y >= topPoint.transform.position.y) 
            {
                goUp = false;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, bottomPoint.transform.position, idleFloatSpeed * Time.deltaTime);
            if (transform.position.y <= bottomPoint.transform.position.y)
            {
                goUp = true;
            }
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
            pointOfOrigin = transform.position;
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
            AudioSource.PlayClipAtPoint(swordCharge, Camera.main.transform.position, .7f);
            yield return new WaitForSeconds(1);
            trackPlayer = true;

            Debug.Log("Sword");
            yield return new WaitForSeconds(aimTime);
            Debug.Log("Attack");

            flash.gameObject.SetActive(true);
            AudioSource.PlayClipAtPoint(chestBeep, Camera.main.transform.position, .7f);
            yield return new WaitForSeconds(flashInterval);
            flash.gameObject.SetActive(false);
            yield return new WaitForSeconds(flashInterval);
            flash.gameObject.SetActive(true);
            AudioSource.PlayClipAtPoint(chestBeep, Camera.main.transform.position, .7f);
            yield return new WaitForSeconds(flashInterval);
            flash.gameObject.SetActive(false);
            Vector3 playerPosition = player.transform.position;
            trackPlayer = false;
            yield return new WaitForSeconds(flashInterval);

            AudioSource.PlayClipAtPoint(stabCharge, Camera.main.transform.position, .4f);
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
            Vector3 playerPosition = player.transform.position;
            Vector3 direction = new Vector3(playerPosition.x - transform.position.x,
                                            playerPosition.y - transform.position.y,
                                            transform.position.z);
            if(transform.up != -direction)
            {
                transform.up += -direction*Time.deltaTime*rotationSpeed;
            }


        }

    }
    void ResetPosition()
    {
        Vector3 rotationReset = new Vector3 (0, 0, 0);
        Vector3 direction = new Vector3(transform.position.x, -10 - transform.position.y, transform.position.z); 
       
        transform.position = Vector2.MoveTowards(transform.position, pointOfOrigin, moveSpeed * Time.deltaTime);
        if(transform.position != pointOfOrigin)
        {
            if(transform.up != -direction)
            {
                transform.up += -direction * Time.deltaTime * rotationSpeed;
            }
        }
        else
        {
            transform.up = rotationReset;
            currentState = State.Idle;
        }
    }

    IEnumerator CreateLasers()
    {
        if(spawningLaser == false)
        {
            
            spawningLaser = true;
            yield return new WaitForSeconds(.5f);

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
            AudioSource.PlayClipAtPoint(laserCharge, Camera.main.transform.position, .7f);
        }

    }
  

}
