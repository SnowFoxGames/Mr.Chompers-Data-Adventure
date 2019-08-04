using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lancelot : MonoBehaviour {

    [SerializeField]bool shieldsOn = false;
    [SerializeField] List<Shield> shields = new List<Shield>();
    [SerializeField] List<EnergyBeams> beams = new List<EnergyBeams>();

    [SerializeField] bool tempDeactivate;
    [SerializeField] float gravityScale = 8f;
    [SerializeField] float rotationSpeed = 4f;
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float chargeSpeed = 4f;
    [SerializeField] float rechargeTime = 3f;

    [Header("Times")]
    [SerializeField] float idleTime = 5f;
    [SerializeField] float aimTime = 5f;
    [SerializeField] float returnDelay = 2f;

    bool becameIdle;
    bool becameShield;
    bool recharging;
    bool impact;
    enum State { Idle, Contain, WallOff, Charge, Deactive, Return }


    State currentState;
    Vector3 pointOfOrigin;
    Animator myAnimator;
    Rigidbody2D myRigidBody;
    Player player;
    PolygonCollider2D myCollider;

	// Use this for initialization
	void Start ()
    {
        myAnimator = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        player = (Player)FindObjectOfType(typeof(Player));
        myCollider = GetComponent<PolygonCollider2D>();
        currentState = State.Idle;
        pointOfOrigin = transform.position;
        FindShieldsAndBeams();
    }

   

    // Update is called once per frame
    void Update () {
        Deactivate();
        ToggleShieldsAndBeams();
        //TrackPlayer();

        switch(currentState)
        {
            case State.Idle:
                pointOfOrigin = transform.position;
                myAnimator.SetBool("isShield", false);
                FlipSprite();
                StartCoroutine(ChangeFromIdle());
                break;
            case State.Contain:
                FlipSprite();
                currentState = State.Charge; //Temp
                break;
            case State.WallOff:
                FlipSprite();
                currentState = State.Charge; //Temp
                break;
            case State.Charge:

                StartCoroutine(ChargeAtPlayer());
                break;
            case State.Deactive:
                
                myRigidBody.gravityScale = gravityScale;
                myCollider.enabled = true;
                tempDeactivate = true;
                StartCoroutine(Recharge());
                break;
            case State.Return:
                ReturnToPosition();
                break;
        }
		
	}

    IEnumerator ChangeFromIdle()
    {
        if (becameIdle == false)
        {
            becameIdle = true;
            Debug.Log("Idle");
            yield return new WaitForSeconds(idleTime);

            currentState = (State)Random.Range(1, 3);
            pointOfOrigin = transform.position;
            Debug.Log("Switching to: " + currentState);
            becameIdle = false;
        }
    }

    void ToggleShieldsAndBeams()
    {
        
        if(shieldsOn == true)
        {
            for (int i = 0; i < shields.Count; i++)
            {
                shields[i].gameObject.SetActive(true);
                if(beams[i])
                {
                    beams[i].spawning = false;
                    beams[i].delaying = true;
                    beams[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < shields.Count; i++)
            {
                shields[i].gameObject.SetActive(false);
                if (beams[i])
                {
                    beams[i].gameObject.SetActive(true);

                    beams[i].StartCoroutine(beams[i].DelayParticles(2f));
                }
            }
        }
    }
    private void FindShieldsAndBeams()
    {
        var shieldCount = FindObjectsOfType<Shield>();
        for (int i = 0; i < shieldCount.Length; i++)
        {
            shields.Add(shieldCount[i]);
        }
        var beamCount = FindObjectsOfType<EnergyBeams>();
        for (int i = 0; i < beamCount.Length; i++)
        {
            beams.Add(beamCount[i]);
        }
    }
    private void Deactivate()
    {
        if(tempDeactivate)
        {
            myAnimator.SetBool("isDeactive", true);
            shieldsOn = false;
        }
        else
        {
            myAnimator.SetBool("isDeactive", false);
            shieldsOn = true;
        }
    }
    void TrackPlayer()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 direction = new Vector3(playerPosition.x - transform.position.x,
                                        playerPosition.y - transform.position.y,
                                        transform.position.z);
        if (transform.right != -direction)
        {
            transform.right += -direction * Time.deltaTime * rotationSpeed;
        }
    }
    private void FlipSprite()
    {

        float ratio = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector2(Mathf.Sign(transform.position.x - player.transform.position.x) * ratio, 1f * ratio);
        //text.transform.localScale = new Vector2(transform.localScale.x / Mathf.Abs(transform.localScale.x), 1);

    }
    IEnumerator ChargeAtPlayer()
    {
        if(becameShield == false)
        {
            bool wait = false;
            becameShield = true;
           myCollider.enabled = false;
            myAnimator.SetBool("isShield", true);
            yield return new WaitForSeconds(1f);
            Vector3 playerPosition = player.transform.position;
            while (transform.position != playerPosition)
            {
                if (wait == false)
                {
                    wait = true;
                    transform.position = Vector2.MoveTowards(transform.position, playerPosition, moveSpeed * Time.deltaTime * chargeSpeed);
                    yield return new WaitForSeconds(.001f);
                    wait = false;
                }

            }

            currentState = State.Deactive;
            becameShield = false;
        }

    }
    IEnumerator Recharge()
    {
        if(recharging == false)
        {
            recharging = true;
            yield return new WaitForSeconds(rechargeTime);
            currentState = State.Return;
            recharging = false;

        }

    }
    void ReturnToPosition()
    {
        myRigidBody.gravityScale = 0;
        tempDeactivate = false;
        myCollider.enabled = false;
        Vector3 rotationReset = new Vector3(0, 0, 0);
        Vector3 direction = new Vector3(transform.position.x, -10 - transform.position.y, transform.position.z);

        transform.position = Vector2.MoveTowards(transform.position, pointOfOrigin, moveSpeed * Time.deltaTime);
        if (transform.position != pointOfOrigin)
        {
            if (transform.up != -direction)
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


}
