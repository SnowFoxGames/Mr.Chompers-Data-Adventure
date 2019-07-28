using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeChanger : MonoBehaviour {
    [SerializeField] float shrinkFactor = .01f;
    bool playerIsIn = false;
    Vector2 defaultScale;
    Player player;
	// Use this for initialization
	void Start () {
        player = (Player)FindObjectOfType(typeof(Player));
        defaultScale = player.gameObject.transform.localScale;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(playerIsIn == false)
        {
            playerIsIn = true;
            player.gameObject.transform.localScale = new Vector2(-shrinkFactor, shrinkFactor);
            Debug.Log("Shrink");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(playerIsIn == true)
        {
            playerIsIn = false;
            player.gameObject.transform.localScale = defaultScale;
            Debug.Log("Grow");
        }
    }
}
