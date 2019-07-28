using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLayers : MonoBehaviour {

    [SerializeField] string SortingLayerName = "Default";
    [SerializeField] int SortingOrder = 0;

    [SerializeField] bool particles = true;


    private void Awake()
    {
        if(particles)
        {
            
        }
    }
}
