using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBeam : MonoBehaviour {

    public float growthRate = .5f;
    [SerializeField] float maxLength = 75f;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Grow();

    }

    private void Grow()
    {
        if (transform.localScale.y <= maxLength)
        {
            transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y + growthRate);
        }
    }
}
