using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
    [SerializeField] GameObject laserBeam;
    [SerializeField] GameObject laserImpact;
    [SerializeField] float growRate = .1f;
    [SerializeField] float growSpeed = .01f;
    [SerializeField] float laserDelay = 2f;
    [SerializeField] float laserDuration = 2f;

    [Header("Audio")]
    [SerializeField] AudioClip laserSpawn;
    [SerializeField] AudioClip laserFire;
    [SerializeField] AudioClip laserBurning;
    
    



    Animator myAnimator;

    public bool isOn;

    bool firing;
    bool loading;
    bool playingLaserSound = false;
    public bool ending;
	// Use this for initialization
	void Start () {
        firing = false;
        isOn = false;
        laserBeam.SetActive(false);
        loading = false;
        myAnimator = GetComponent<Animator>();
        AudioSource.PlayClipAtPoint(laserSpawn, Camera.main.transform.position);
        StartCoroutine(CountDown(laserDelay));
	}
	
	// Update is called once per frame
	void Update () {
        FireLaser();
        DestroyLaser();

	}
    public void FireLaser()
    {
        if(isOn == true)
        {
            StartCoroutine(LaserLength());
            if(playingLaserSound == false)
            {
                playingLaserSound = true;
                AudioSource.PlayClipAtPoint(laserBurning, Camera.main.transform.position, .05f);
            }
        }

    }
    public IEnumerator LaserLength()
    {
        if (firing == false)
        {
            firing = true;

            laserBeam.gameObject.transform.localScale = new Vector2(laserBeam.transform.localScale.x, laserBeam.transform.localScale.y + growRate);
            yield return new WaitForSeconds(growSpeed);
            firing = false;
        }
    }
    public void TurnOnLaser()
    {
        isOn = true;
        AudioSource.PlayClipAtPoint(laserFire, Camera.main.transform.position, .4f);
        myAnimator.SetBool("firing", false);
    }
    IEnumerator CountDown(float delayTime)
    {
        if(loading == false)
        {
            loading = true;
            Debug.Log("Firing in: " + delayTime);
            yield return new WaitForSeconds(delayTime);

            laserBeam.SetActive(true);
            Debug.Log("Fire");
            myAnimator.SetBool("firing", true);

        }
       
    }
    void DestroyLaser()
    {
        if(ending == true)
        {
            Debug.Log("begin destruction count");
            StartCoroutine(CountdownToDestroy(laserDuration));
        }
    }

    IEnumerator CountdownToDestroy(float seconds)
    {
        if(ending == true)
        {
            ending = false;

            yield return new WaitForSeconds(seconds);
            laserBeam.SetActive(false);
            laserImpact.SetActive(false);

            myAnimator.SetBool("destroy", true);
            yield return new WaitForSeconds(1);
            Destroy(gameObject);

        }


    }
   






}
