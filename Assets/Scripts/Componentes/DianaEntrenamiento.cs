using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DianaEntrenamiento : ObjetivoTareaDisparo
{
    
    [Header("Modelos 3D")]
    public GameObject modeloDianaCorrecta;
    public GameObject modeloDianaIncorrecta;
    [Header("Gemas")]
    public GameObject gemaA; 
    public GameObject gemaB;
    public GameObject gemaC;
    
    public bool EsVisible {
        get {
            return gameObject.transform.localScale == Vector3.one;
        }
    }

    protected override void Iniciar()
    {
        base.Iniciar();
        // ocultar y mostrar solo el modelo 3D correspondiente
        OcultarModelos();               
    }
    
    public override void Mostrar()
    {
        
        OcultarModelos();
        
        base.Mostrar();

        // mostrar la diana correspondiente o gema
        if(esObjetivo)
        {
            if(esGema)
            {
                // es gema, elegir una al azar
                GameObject[] gemas = {
                    gemaA, gemaB, gemaC
                };

                GameObject gema = gemas[Random.Range(0, gemas.Length)];
                gema.SetActive(true);
               

            } else {

                // diana azul 
                modeloDianaCorrecta.SetActive(true);
               
            }
            
        } else {
            // diana roja
            modeloDianaIncorrecta.SetActive(true);
            
        }
    }    
    protected override void AnimacionMostrar()
    {
        // animacion de la diana al aparecer
        gameObject.transform.localScale = Vector3.zero;
        iTween.ScaleTo(gameObject, Vector3.one, 0.5f);
    }

    protected override void AnimacionOcultar()
    {
        // animacion de la diana al aparecer       
        iTween.ScaleTo(gameObject, Vector3.zero, 0.5f);        
    }
  
    private void OcultarModelos()
    {
        modeloDianaCorrecta.SetActive(false);
        modeloDianaIncorrecta.SetActive(false);        
        gemaA.SetActive(false);
        gemaB.SetActive(false);
        gemaC.SetActive(false);
      
    }




}
