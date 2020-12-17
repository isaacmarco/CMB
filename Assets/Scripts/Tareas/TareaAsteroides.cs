using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TareaAsteroides : MonoBehaviour
{
   
    public GameObject prefabAsteroide; 
    public GameObject prefabLaser; 

    void Start()
    {
        // StartCoroutine(CorutinaGeneracionAsteroides());
    }
    
    private IEnumerator CorutinaGeneracionAsteroides()
    {
        while(true)
        {
         
            float tiempoParaNuevaAsteroide = 2f; 

            yield return new WaitForSeconds(tiempoParaNuevaAsteroide);            
            
            InstanciarAsteroide();
        }
    }
    
    private void InstanciarAsteroide()
    {                
        GameObject asteroide = (GameObject) Instantiate(prefabAsteroide);
        float escala = 2.5f; 
        asteroide.transform.localScale = new Vector3(escala, escala, escala);
        asteroide.name = "Asteroide";
    }

}