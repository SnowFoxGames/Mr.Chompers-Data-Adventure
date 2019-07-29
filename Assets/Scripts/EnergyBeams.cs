using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBeams : MonoBehaviour {

    [SerializeField] EnergyParticle energy;
    [SerializeField] float spawnDelay = 1f;
    [SerializeField] GameObject target;
    bool spawning = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(CreateEnergy());
	}
    IEnumerator CreateEnergy()
    {
        if(spawning == true)
        {
            yield break;
        }
        spawning = true;
        yield return new WaitForSeconds(spawnDelay);
       
        EnergyParticle energyParticle = Instantiate(energy, transform.position, Quaternion.identity);
        energyParticle.target = target;

        spawning = false;

    }
}
