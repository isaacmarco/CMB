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
         /*     
        Vector3 error = new Vector3(
            Random.Range(-0.3f, 0.4f),
            Random.Range(-0.3f, 0.4f),
            Random.Range(-0.3f, 0.4f)
        );*/
        float f = 0.1f;
        Vector3 error = new Vector3(
            Random.Range(-f, f),
            Random.Range(-f, f),
            Random.Range(-f, f)
        );
        gameObject.transform.LookAt(objetivo + error);
        StartCoroutine(Destruir());
    }
    
    void Update()
    {
        // el laser va siempre hacia delante, ya que
        // inicialmente se oriento hacia el objetivo
        float velocidad = 50f; 
        gameObject.transform.Translate(Vector3.forward * velocidad * Time.deltaTime);    
    }
    
    
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Diana")
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator Destruir()
    {
        float tiempoAntesDeDestruir = 1f; 
        yield return new WaitForSeconds(tiempoAntesDeDestruir);
        Destroy(this.gameObject);
    }
}
