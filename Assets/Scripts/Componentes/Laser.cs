using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{   
    private Vector3 objetivo; 

    public void Disparar(Vector3 objetivo)
    {
        // inicialmente se orienta al objetivo y
        // se programa su destruccion 
        gameObject.transform.LookAt(objetivo);
        StartCoroutine(Destruir());
    }
    
    void Update()
    {
        // el laser va siempre hacia delante, ya que
        // inicialmente se oriento hacia el objetivo
        gameObject.transform.Translate(Vector3.forward);    
    }
    
    private IEnumerator Destruir()
    {
        float tiempoAntesDeDestruir = 10f; 
        yield return new WaitForSeconds(tiempoAntesDeDestruir);
        Destroy(this.gameObject);
    }
}
