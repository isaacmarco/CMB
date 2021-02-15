using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestruccionVFX : MonoBehaviour
{
    
    public float duracionVFX = 1f; 

    void Start()
    {
        Invoke("Destruir", duracionVFX);
    }

    private void Destruir()
    {
        Destroy(gameObject); 
    }
}
