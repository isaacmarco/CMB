using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveEnemiga : MonoBehaviour
{
    private int vida = 100;

    void Update()
    {
        IA(); 
    }

    private void RecibirImpacto()
    {
        vida -= 50; 
        if(vida <= 0)
            Destruir();
    }

    private void Disparar()
    {

    }

    private void Destruir()
    {

    }

    private void IA()
    {

    }
}
