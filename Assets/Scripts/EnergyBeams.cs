using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBeams : MonoBehaviour {

    [SerializeField] EnergyParticle energy;
    [SerializeField] float spawnDelay = 1f;
    [SerializeField] GameObject target;
    
    public bool spawning = false;
    public bool delaying = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(delaying == false)
        {
            Debug.Log("Spwan ENergy");
            StartCoroutine(CreateEnergy());
        }
        else
        {
            Debug.Log("Destroy Energy");
            var particle = FindObjectsOfType<EnergyParticle>();
            foreach (var item in particle)
            {
                if(item)
                {
                    Destroy(item.gameObject);
                }

            }
        }

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
    public IEnumerator DelayParticles(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        delaying = false;
    }
}
