using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Files : MonoBehaviour {

    [SerializeField] AudioClip biteSounds;


    BoxCollider2D textBoxCollider;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<Player>())
        {
            AudioSource.PlayClipAtPoint(biteSounds, Camera.main.transform.position, .7f);
            Destroy(gameObject); 
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Mouth")
        {
            AudioSource.PlayClipAtPoint(biteSounds, Camera.main.transform.position, .7f);
            Destroy(gameObject);
        }

    }

}
