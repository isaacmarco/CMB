using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalidaGuion : MonoBehaviour
{
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            // al encontrar la salida informamos al guion y
            // eliminamos la salida
            FindObjectOfType<GuionAventura>().SalidaAlcanzada();
            Destroy(gameObject);
        }
    }

}
