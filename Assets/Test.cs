using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
public class Test : MonoBehaviour
{
    private GazeAware gazeAware;

    // Start is called before the first frame update
    void Start()
    {
        
    }   
    void Awake()
    {
        gazeAware = GetComponent<GazeAware>();	
    }

    // Update is called once per frame
    void Update()
    {
        if (gazeAware.HasGazeFocus) // && Vector3.Distance(transform.position, Camera.main.transform.position) < distanciaMaxima)
		{
            Debug.Log("XXXXXXX");
        }

    }
}
