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
    
    

    public override void Mostrar(PuntoAparicionDiana posicion, MovimientoDiana movimientoDiana, bool enMovimiento = false)
    {
      
        gameObject.transform.LookAt(Camera.main.gameObject.transform.position);

        OcultarModelos();
        
        base.Mostrar(posicion, movimientoDiana);

         //this.puntoAparicion = puntoAparicionDiana.gameObject.transform.position; 
        gameObject.transform.position = posicion.gameObject.transform.position; 

 
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
        {
            if(corrutinaMovimiento!=null)
                StopCoroutine(corrutinaMovimiento);

            corrutinaMovimiento = StartCoroutine(CorrutinaMovimiento());
        }
            // TareaMovimientoDiana();

    }    
    private Coroutine corrutinaMovimiento; 
   

    private void TareaMovimientoDiana()
    {
        iTween.Stop(gameObject);
        posicionInicial = gameObject.transform.position; 

        float x = posicionInicial.x; 
        float z = posicionInicial.z; 
        float y = posicionInicial.y; 


        int cantidadDesplazamiento = 2; 
        float velocidad = 0.8f; 
        int destinoX = 0; 
        int destinoY = 0; 

        switch(movimientoDianas)
        {
            case MovimientoDiana.HorizontalIzquierda:
                destinoX = -cantidadDesplazamiento; 
            break;
            case MovimientoDiana.HorizontalDerecha:
                destinoX = cantidadDesplazamiento; 
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
                //"x", x + destinoX,
                "y", y + destinoY, 
                "z", z + destinoX,
            "looptype", iTween.LoopType.pingPong,
            "easetype", iTween.EaseType.linear, 
            "speed", velocidad,
            "islocal", true)
        );

    }

    private IEnumerator CorrutinaMovimiento()
    {
         posicionInicial = gameObject.transform.position; 

        float x = posicionInicial.x; 
        float z = posicionInicial.z; 
        float y = posicionInicial.y; 


        int cantidadDesplazamiento = 2;        
        int destinoX = 0; 
        int destinoY = 0; 

        switch(movimientoDianas)
        {
            case MovimientoDiana.HorizontalIzquierda:
                destinoX = -cantidadDesplazamiento; 
            break;
            case MovimientoDiana.HorizontalDerecha:
                destinoX = cantidadDesplazamiento; 
            break;
            case MovimientoDiana.VerticalAbajo:
                destinoY = -cantidadDesplazamiento;
            break;
            case MovimientoDiana.VerticalArriba:
                destinoY = cantidadDesplazamiento; 
            break;
        }
        

        Vector3 positionDisplacement;
        Vector3 positionOrigin;
        float _timePassed = 0f;         
        positionDisplacement = new Vector3(destinoX, destinoY, 0);
        positionOrigin = transform.position;
     
   

        while(true)
        {

            
            _timePassed += Time.deltaTime;
            transform.position = Vector3.Lerp(positionOrigin, positionOrigin + positionDisplacement,
            Mathf.PingPong(_timePassed, 1));
    
            /*

            float distanciaRecorrida = 0f; 
            float velocidad_ = Time.deltaTime * 2f; 
            distanciaRecorrida += velocidad_; 
            // desplazar en ping-pong las dianas
            gameObject.transform.Translate(
                new Vector3(destinoX, destinoY, 0) * velocidad_ , Space.Self
            );
            if(distanciaRecorrida > 5)*/

            yield return null; 
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
