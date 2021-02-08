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
    
    [Header("Configuracion")]
    public MovimientoDiana movimientoDianas; 

    private Vector3 posicionInicial; 


    public bool EsVisiblePlenamente {
        get {
            return gameObject.transform.localScale == Vector3.one;
        }
    }
    public bool Visible {
        get {
            return gameObject.transform.localScale.x > 0.3f;
        }
    }

    protected override void Iniciar()
    {
        base.Iniciar();
        // ocultar y mostrar solo el modelo 3D correspondiente
        OcultarModelos();               
    }
    
    

    public override void Mostrar(PuntoAparicionDiana puntoAparicionDiana, MovimientoDiana movimientoDiana, bool enMovimiento = false)
    {
      

        OcultarModelos();
        
        base.Mostrar(puntoAparicionDiana, movimientoDiana);

        this.movimientoDianas = movimientoDiana;
        posicionInicial = gameObject.transform.position; 
      
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

        if(enMovimiento)
            StartCoroutine(TareaMovimientoDiana());

    }    

    

    private IEnumerator TareaMovimientoDiana()
    {
        float x = posicionInicial.x; 
        float z = posicionInicial.z; 
        float y = posicionInicial.y; 

        /*
            TODO: DESPLAZAMIENTO VERTICAL U HORIZONTAL
            DEPENDIENDO DEL ENUM
        */

        int cantidadDesplazamiento = 1; 
        float velocidad = 0.5f; 
        int destinoX = 0; 
        int destinoY = 0; 

        switch(movimientoDianas)
        {
            case MovimientoDiana.HorizontalIzquierda:
                destinoX = cantidadDesplazamiento; 
            break;
            case MovimientoDiana.HorizontalDerecha:
                destinoX = -cantidadDesplazamiento; 
            break;
            case MovimientoDiana.VerticalAbajo:
                destinoY = -cantidadDesplazamiento;
            break;
            case MovimientoDiana.VerticalArriba:
                destinoY = cantidadDesplazamiento; 
            break;
        }
        

        iTween.MoveTo(gameObject, 
            iTween.Hash(
                "x", x + destinoX,
                "y", y + destinoY, 
                "z", z,
            "looptype", iTween.LoopType.pingPong,
            "easetype", iTween.EaseType.linear, "speed", velocidad)
        );

        yield return null; 
        
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
