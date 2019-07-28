using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour {
    
    [SerializeField] Laser laserBody;
    [SerializeField] GameObject laserImpact;
    [SerializeField] float impactBuffer = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.gameObject.tag == "Player")
        {
            Debug.Log("HitPlayer");
            FindObjectOfType<GameSession>().TakeLives();
        }
        else
        {
            Debug.Log("Stop Laser" +other.GetComponent<BoxCollider2D>().transform.position.y);

            laserBody.isOn = false;
            StopCoroutine(laserBody.LaserLength());
            laserImpact.transform.position = new Vector2(transform.position.x, other.transform.position.y + 
                                                         (laserImpact.GetComponent<Renderer>().bounds.size.y /2)
                                                         + (other.GetComponent<BoxCollider2D>().bounds.size.y / 2));
            laserImpact.SetActive(true);
            laserBody.ending = true;
        }


       

    }
}
