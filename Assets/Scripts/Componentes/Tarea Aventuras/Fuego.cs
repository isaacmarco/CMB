using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuego : MonoBehaviour
{
    
    
    void OnTriggerEnter2D(Collider2D col)
    {
        FindObjectOfType<TareaAventuras>().RecibirImpacto();
    }


}
