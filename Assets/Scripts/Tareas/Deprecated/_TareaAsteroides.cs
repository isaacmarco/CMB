using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TareaAsteroides : MonoBehaviour
{
   
    public GameObject prefabAsteroide; 
    public GameObject prefabLaser; 
    public Transform[] posicionesOrigenDestinoAsteroides; 
 
    void Start()
    {
        StartCoroutine(CorutinaGeneracionAsteroides());

    }
    
    private IEnumerator CorutinaGeneracionAsteroides()
    {
        while(true)
        {
         
            float tiempoParaNuevaAsteroide = 2f; 

            yield return new WaitForSeconds(tiempoParaNuevaAsteroide);            
            
            // InstanciarAsteroide();
        }
    }
    
    private void InstanciarAsteroide()
    {                
        
        Transform[] t = posicionesOrigenDestinoAsteroides;
        Transform[] _0 = {t[0],t[1]};
        Transform[] _1 = {t[3],t[4],t[5]};
        Transform[] _2 = {t[3],t[4]};
        Transform[] _3 = {t[1],t[2]};
        Transform[] _4 = {t[0],t[1],t[2]};
        Transform[] _5 = {t[0],t[1]};
        
        GameObject asteroide = (GameObject) Instantiate(prefabAsteroide);
        float escala = 2.5f; 
        asteroide.transform.localScale = new Vector3(escala, escala, escala);
        asteroide.name = "Asteroide";
    }

}