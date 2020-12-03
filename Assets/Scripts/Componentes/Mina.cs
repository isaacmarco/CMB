using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mina : MonoBehaviour
{
    
    private float momento; 
    public void Iniciar(float momento)
    {
        this.momento = momento; 
    }
    void Update()
    {
        // si el jugador esta en un momento posterior
        // a la mina, se destruye
        if(FindObjectOfType<NaveJugador>().Tiempo > momento)
            Destroy(this.gameObject);
        
    }
}
