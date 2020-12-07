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
        Vector3 error = new Vector3(
            Random.Range(-0.3f, 0.4f),
            Random.Range(-0.3f, 0.4f),
            Random.Range(-0.3f, 0.4f)
        ); 
        gameObject.transform.LookAt(objetivo + error);
        StartCoroutine(Destruir());
    }
    
    void Update()
    {
        // el laser va siempre hacia delante, ya que
        // inicialmente se oriento hacia el objetivo
        float velocidad = 150f; 
        gameObject.transform.Translate(Vector3.forward * velocidad * Time.deltaTime);    
    }
    
    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "Mina")
            Destroy(this.gameObject);
    }

    private IEnumerator Destruir()
    {
        float tiempoAntesDeDestruir = 3f; 
        yield return new WaitForSeconds(tiempoAntesDeDestruir);
        Destroy(this.gameObject);
    }
}
