using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerlinLasers : MonoBehaviour {
    [SerializeField] float maxYScale = 2f;
    [SerializeField] float growSpeed = 2f;
    public bool grow;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(grow)
        {
            GrowLaser();
        }
        else
        {
            ShrinkLaser();
        }

	}
    void GrowLaser()
    {
        if (transform.localScale.y < maxYScale)
        {
            transform.localScale += new Vector3(0, growSpeed, 0) * Time.deltaTime * growSpeed;
        }
    }
    void ShrinkLaser()
    {
        if (transform.localScale.y > 0)
        {
            transform.localScale -= new Vector3(0, growSpeed, 0) * Time.deltaTime * growSpeed;
        }
    }
}
