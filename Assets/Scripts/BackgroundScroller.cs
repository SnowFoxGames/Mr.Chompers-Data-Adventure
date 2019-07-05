using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {

    [SerializeField] float backgroundScrollSpeed = 0.5f;
    [SerializeField] float transparency = 1f;
    [SerializeField] bool isHorizontal = true;
    Material myMaterial;

    Vector2 offSet;


	// Use this for initialization
	void Start () 
    {
        
        myMaterial = GetComponent<Renderer>().material;
        if(isHorizontal)
        {
            offSet = new Vector2(backgroundScrollSpeed, 0);
        }
        else
        {
            offSet = new Vector2(0,backgroundScrollSpeed);
        }


	}
	
	// Update is called once per frame
	void Update () 
    {
        myMaterial.mainTextureOffset += offSet * Time.deltaTime;

       
       
	}
}
