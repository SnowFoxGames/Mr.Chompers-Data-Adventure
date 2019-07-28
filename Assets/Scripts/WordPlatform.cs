using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordPlatform : MonoBehaviour {

    [SerializeField]GameObject playerFeet;
    [SerializeField] GameObject platformTop;
    BoxCollider2D myBoxCollider;
    Player player;

	// Use this for initialization
	void Start () {
        
        myBoxCollider = GetComponent<BoxCollider2D>();
        player = (Player)FindObjectOfType(typeof(Player));
        playerFeet = player.transform.Find("Feet").gameObject;
        platformTop.transform.position = new Vector2(transform.position.x,myBoxCollider.transform.position.y + (myBoxCollider.size.y));
        myBoxCollider.enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
       
       
	}
    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("Go Down");
            myBoxCollider.enabled = false;
        }
        else if (playerFeet.transform.position.y >= platformTop.transform.position.y && playerFeet !=null)
        {
            myBoxCollider.enabled = true;
        }
        else
        {
            myBoxCollider.enabled = false;
        }

    }
}
