using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merlin : MonoBehaviour {

    [SerializeField] GameObject text;
    [SerializeField] float idleTime =3f;

    [Header("KnifeStuff")]
    [SerializeField] GameObject knife;
    [SerializeField] int knifeQuantity = 3;
    [SerializeField] float spawnDelay = .1f;
    [SerializeField] float releaseDelay = 1f;
    [SerializeField] float radiusFromOrigin = 3f;  enum State { Idle, Charge, SummonKnives, SummonLasers }
    bool summoningKnives;
    [Header("EnergyBeam Stuff")]
    [SerializeField] EnergyBall energyBall;
    [SerializeField] float laserDuration = 3f;
    [SerializeField] float rotationSpeed;
    bool summoningBeam;


    int firstAttack = (int)State.SummonKnives;
    int lastAttack = (int)State.SummonLasers;//Need to update if adding more attacks


    bool becameIdle;

    Vector3 pointOfOrigin;

    Player player;
    Animator myAnimator;
    State currentState;
	// Use this for initialization
	void Start () {
        player = (Player)FindObjectOfType(typeof(Player));
        myAnimator = GetComponent<Animator>();
        currentState = State.Idle;
        energyBall.rotationSpeed = rotationSpeed;
       
    }

    // Update is called once per frame
	void Update () {

        switch (currentState)
        {
            case State.Idle:
                FlipSprite();
                if (player.isActiveAndEnabled)
                {
                    StartCoroutine(ChangeFromIdle());
                }
                break;

            case State.Charge:
                myAnimator.SetBool("isCharging", true);
                break;

            case State.SummonKnives:
                StartCoroutine(SummonKnives());
                break;
            case State.SummonLasers:
                StartCoroutine(SummonEnergyBeam());
                break;

            
        }

	}
    private void FlipSprite()
    {

        float ratio = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector2(Mathf.Sign(transform.position.x - player.transform.position.x) * ratio, 1f * ratio);
        text.transform.localScale = new Vector2(transform.localScale.x / Mathf.Abs(transform.localScale.x), 1);

    }
    IEnumerator ChangeFromIdle()
    {
        if (becameIdle == false)
        {
            becameIdle = true;
            Debug.Log("Idle");
            yield return new WaitForSeconds(idleTime);

            currentState = (State)UnityEngine.Random.Range(firstAttack, lastAttack);//Change later;
            pointOfOrigin = transform.position;
            Debug.Log("Switching to: " + currentState);
            becameIdle = false;
        }

    }

    IEnumerator SummonKnives()
    {
        if(summoningKnives == false)
        {
            summoningKnives = true;
            bool summonComplete = false;

            myAnimator.SetBool("isCharging", true);
            yield return new WaitForSeconds(1f);
            while(summonComplete == false)
            {
                for (int i = 0; i < knifeQuantity; i++)
                {
                    float degree = (Mathf.PI) / (knifeQuantity - 1);
                    float xCoord = radiusFromOrigin * Mathf.Cos(degree * i);
                    float yCoord = radiusFromOrigin * Mathf.Sin(degree * i);
                    Vector2 position = new Vector2(xCoord, yCoord);
                   // Debug.Log(degree * i);
                    GameObject newKnife = Instantiate(knife, position, Quaternion.identity);
                    yield return new WaitForSeconds(spawnDelay);
                }
                summonComplete = true;
            }

            myAnimator.SetBool("isCharging", false);
            yield return new WaitForSeconds(releaseDelay);
            myAnimator.SetBool("isUnleashing", true);
            var knives = FindObjectsOfType<Knife>();
            foreach (var singleKnife in knives)
            {
                singleKnife.attack = true;
                yield return new WaitForSeconds(.5f);
            }
            myAnimator.SetBool("isUnleashing", false);
            currentState = State.Idle;
            summoningKnives = false;

        }
    }

    IEnumerator SummonEnergyBeam()
    {
        if(summoningBeam == false)
        {
            summoningBeam = true;
            bool laserSpawnComplete = false;

            myAnimator.SetBool("isSummoning", true);
            yield return new WaitForSeconds(.5f);
            energyBall.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);

            while (laserSpawnComplete == false)
            {
                for (int i = 0; i < energyBall.laserBeams.Length; i++)
                {
                    energyBall.laserBeams[i].grow = true;
                    yield return new WaitForSeconds(.2f);
                }
                laserSpawnComplete = true;
            }
            yield return new WaitForSeconds(laserDuration);

            while(laserSpawnComplete == true)
            {
                for (int i = 0; i < energyBall.laserBeams.Length; i++)
                {
                    energyBall.laserBeams[i].grow = false;
                    yield return new WaitForSeconds(.2f);
                }
                while(energyBall.laserBeams[energyBall.laserBeams.Length-1].transform.localScale.y>0)
                {
                    yield return null;
                }
                laserSpawnComplete = false;
            }
            yield return new WaitForSeconds(.2f);
            energyBall.gameObject.SetActive(false);

            myAnimator.SetBool("isSummoning", false);
            currentState = State.Idle;
            summoningBeam = false;

        }

    }

}
