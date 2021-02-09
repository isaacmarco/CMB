using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navegacion : MonoBehaviour
{
    private GameObject[] posiciones; 

    void Start()
    {
        GenerarPuntosNavegacion();
        StartCoroutine(CorrutinaTareaDisparo());

    }

    private void GenerarPuntosNavegacion()
    {        
        int numeroPosiciones = gameObject.transform.childCount; 
        posiciones = new GameObject[numeroPosiciones];
        for(int i=0; i<numeroPosiciones; i++)
        {
            // obtenemos el hijo y anotamos su posicion 
            Transform hijo = gameObject.transform.GetChild(i);
            hijo.gameObject.GetComponent<Renderer>().enabled = false; 
            hijo.GetChild(0).gameObject.GetComponent<Renderer>().enabled  = false; 
            posiciones[i] = hijo.gameObject; 
        }
    }
    int bloqueActual = 0; 

    GameObject dom; 
    void Update()
    {  
         if(dom==null)
            return;
        Vector3 v = new Vector3(0, 0.7f, 0);
        GameObject jugador = Camera.main.gameObject; 
        jugador.transform.rotation = Quaternion.Slerp(
            jugador.transform.rotation, (
                Quaternion.LookRotation(
                    (dom.transform.position + v) - jugador.transform.position)), Time.deltaTime * 10f);
    }

    public GameObject target; 
    
    void FixedUpdate()
    {
       
    }

    private IEnumerator CorrutinaTareaDisparo()
    {
        while(true)
        {
            // movemos la camara al siguiente bloque                            
            Vector3 posicion = posiciones[bloqueActual].transform.position + Vector3.up;
            GameObject jugador = Camera.main.gameObject; 
            float duracionAnimacionCamara = 3f; 
            
            //iTween.MoveTo(jugador, posicion, duracionAnimacionCamara);

            

            // orientamos la camara 
            GameObject dummyOrientacion = posiciones[bloqueActual].transform.GetChild(0).gameObject;
            dom = dummyOrientacion; 
            
          
            iTween.MoveTo(jugador, 
                iTween.Hash(
                "x", posicion.x, 
                "y", posicion.y, 
                "z", posicion.z,
                "easetype", iTween.EaseType.linear, 
                "time", duracionAnimacionCamara
                )
            );
        
            
            // esperamos a que la camara llegue al nuevo bloque
            yield return new WaitForSeconds(duracionAnimacionCamara);

           
            yield return new WaitForSeconds(3); // permanencia en bloque
        

            // cambiamos al siguiente bloque
            bloqueActual++;
            if(bloqueActual >= posiciones.Length)
                bloqueActual = 0; 
            yield return null; 
        }
    }
   

    
}
