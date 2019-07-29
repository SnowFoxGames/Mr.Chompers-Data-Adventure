using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyParticle : MonoBehaviour {
    public GameObject target;
    [SerializeField] float moveSpeed = 1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            Vector3 targetTransform = new Vector3(target.transform.position.x,
                                            target.transform.position.y,
                                            transform.position.z);


            if (transform.position != targetTransform)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                                                   targetTransform,
                                                   moveSpeed * Time.deltaTime);

            }
            else
            {
                Destroy(gameObject);
            }
        }
       
	}
}
