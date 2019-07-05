using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour {


    [SerializeField] float moveSpeed = 5f;
    [SerializeField] GameObject dataExplodeEffect;
    [SerializeField] bool isDeadly = false;
    [SerializeField] bool isBoss = false;
    [SerializeField] AudioClip biteSounds;
    GameSession gameSession;
    Rigidbody2D myRigidBody;
    Spawner spawner;
	// Use this for initialization
	void Start () {
       
        gameSession = (GameSession)FindObjectOfType(typeof(GameSession));
        myRigidBody = GetComponent<Rigidbody2D>();
        spawner = FindObjectOfType<Spawner>();
	}
	
	// Update is called once per frame
	void Update () {
        if(isBoss == false)
        {
            myRigidBody.velocity = Vector2.left * Time.deltaTime * (moveSpeed + spawner.dataSpeedIncrease);
        }

	}

    public void CauseEffectAndDestroy()
    {
       
        if(isDeadly == false)
        {
            GameObject xplode = Instantiate(dataExplodeEffect, transform.position, Quaternion.identity) as GameObject;
            gameSession.IncreasePercentage();
        }
        else
        {
            TakeLives();
        }
        AudioSource.PlayClipAtPoint(biteSounds, Camera.main.transform.position, .7f);
        Destroy(gameObject);
    }

    public void TakeLives()
    {
        int remainingLives = gameSession.lives.Count;
        if (remainingLives <= 1)
        {
            DestroyHearts();
            gameSession.stopSpawning = true;
            FindObjectOfType<Player>().Die();
            Debug.Log("GameOver");
        }
        else
        {
            DestroyHearts();

        }
    }

    private void DestroyHearts()
    {
        var liveNumber = gameSession.lives.Count - 1;
        Destroy(gameSession.lives[liveNumber].gameObject);
        gameSession.lives.Remove(gameSession.lives[liveNumber]);

    }

   
}
