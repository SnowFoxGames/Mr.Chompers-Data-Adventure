using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    [SerializeField] GameObject[] spawnPoints;
    [SerializeField] float spawnRate = 5;
    [SerializeField] GameObject data;
    [SerializeField] GameObject fireWall;

    [SerializeField] bool level2;
    [SerializeField] bool level3;
    public float dataSpeedIncrease = 0;
    bool isSpawned = false;
    GameSession gameSession;
	// Use this for initialization
	void Start () {
        gameSession = (GameSession)FindObjectOfType(typeof(GameSession));
		
	}
	
	// Update is called once per frame
	void Update () {
        if(gameSession.stopSpawning == false)
        {
            StartCoroutine(SpawnData());
        }

	}
    IEnumerator SpawnData()
    {
        if(isSpawned == false)
        {
            isSpawned = true;

            int chosenSpot = Random.Range(0, spawnPoints.Length);

            GameObject newData = Instantiate(data, spawnPoints[chosenSpot].transform.position, Quaternion.identity);

            if(level2)
            {
                int otherSpot;
                if (chosenSpot <= 0)
                {
                    otherSpot = Random.Range(1, spawnPoints.Length);
                }
                else if (chosenSpot >= spawnPoints.Length - 1)
                {
                    otherSpot = Random.Range(0, spawnPoints.Length - 1);
                }
                else
                {
                    int randomAddon = (int)Mathf.Sign(Random.Range(-1, 1));
                    otherSpot = chosenSpot + randomAddon;
                }
                GameObject newData2 = Instantiate(fireWall, spawnPoints[otherSpot].transform.position, Quaternion.identity);
            }
            else if(level3)
            {
                if(chosenSpot == 0)
                {
                    GameObject newData1 = Instantiate(fireWall, spawnPoints[1].transform.position, Quaternion.identity); 
                    GameObject newData2 = Instantiate(fireWall, spawnPoints[2].transform.position, Quaternion.identity); 
                }
                else if(chosenSpot == 1)
                {
                    GameObject newData1 = Instantiate(fireWall, spawnPoints[0].transform.position, Quaternion.identity);
                    GameObject newData2 = Instantiate(fireWall, spawnPoints[2].transform.position, Quaternion.identity);
                }
                else if(chosenSpot == 2)
                {
                    GameObject newData1 = Instantiate(fireWall, spawnPoints[0].transform.position, Quaternion.identity);
                    GameObject newData2 = Instantiate(fireWall, spawnPoints[1].transform.position, Quaternion.identity);
                }
            }

            yield return new WaitForSeconds(spawnRate);
            isSpawned = false;
        }
       


    }
}
