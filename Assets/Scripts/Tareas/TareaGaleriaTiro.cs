using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TareaGaleriaTiro : Tarea
{   
    
    [Header("Marcador")]
    public Text aciertosUI;
    public Text erroresUI;
    public Text omisionesUI;
    public Text municionUI;
    public Text puntuacionUI;
    public GameObject interfazRecarga2D; 
    public GameObject avisoRecarga; 
   
    [Header("Arma")]
    public GameObject interfazRecarga;
    public GameObject arma; 
    

    // posicion de dianas y camaras para este nivel instanciado
    private BloqueTareaDisparo bloqueTareaDisparo; 

    public NivelGaleriaTiroScriptable Nivel { 
        get { return (NivelGaleriaTiroScriptable) Configuracion.nivelActual;} 
    }
   

    // estado del juego
    private int errores; 
    private int aciertos; 
    private int omisiones;   
    private float tiempoEntreCambiosCamara = 5f;
    private float tiempoDelUltimoCambioCamara; 
    private bool avisoMunicionEscuchado = false; 

    public void MostrarInterfazRecarga()
    {
        if(!avisoMunicionEscuchado)
        {
            FindObjectOfType<Audio>().FeedbackCargadorVacio();
            avisoMunicionEscuchado = true; 
        }
        interfazRecarga.SetActive(true);
        avisoRecarga.SetActive(true);
    }
    public void Recargar()
    {   
        FindObjectOfType<Audio>().FeedbackClickDefecto();
        StartCoroutine(CorrutinaRecarga());        
    }

    private bool recargando = false; 
    public bool Recargando{
        get{ return this.recargando;}
    }
    private IEnumerator CorrutinaRecarga()
    {
        
        // ocultar la interfaz de recarga
        interfazRecarga.SetActive(false);
        avisoRecarga.SetActive(false);
        // reponer municion
        FindObjectOfType<JugadorTareaGaleriaTiro>().Recargar();
        avisoMunicionEscuchado = false; // permite volver a escucharlo 
        recargando = true; 
        // animaciones de recarga
        arma.SetActive(true);
        // esperamos a que termina la animacion 
        FindObjectOfType<Audio>().FeedbackRecarga();
        yield return new WaitForSeconds(1.33f);
        
        recargando = false; 
        // ocultamos el arma
        arma.SetActive(false);
        interfazRecarga.SetActive(true);
    }

    private IEnumerator CorrutinaMunicion()
    {
        JugadorTareaGaleriaTiro jugador = FindObjectOfType<JugadorTareaGaleriaTiro>();
        while(true)
        {           
            // actualizamos aqui la puntuacion
            puntuacionUI.text = puntuacion.ToString();

            // actualizamos el marcador de municion 
            int municion = jugador.Municion;
            string ceros = string.Empty; 

            if(municion <= 0)            
            {
                municionUI.text = "000";
            } else {
                if(municion < 100)
                    ceros += "0";            
                if(municion < 10)
                    ceros += "0";
                municionUI.text = ceros + jugador.Municion.ToString();
            }
            
            

            if(!jugador.HayMunicion())
            {
                MostrarInterfazRecarga();
            }
            yield return null; 
        }
    }

    protected override void Inicio()
    {          
        // crear el escenario
        InstanciarEscenario();
        avisoRecarga.SetActive(false);
        Recargar();
        
        // comenzar la tarea        
        DesbloquearTarea();
        StartCoroutine(CorrutinaTareaDisparo());  
        StartCoroutine(CorrutinaMunicion());
    }

    private GameObject[] dianasPrimerBloque; 
    private GameObject[] dianasSegundoBloque;
    private GameObject[] dianasTercerBloque; 

    private void InstanciarEscenario()
    {
        // instanciar el prefab del nivel        
        string ruta = "Escenarios/Escenario" + Nivel.numeroDelNivel;
        GameObject escenarioPrefab = (GameObject) Resources.Load(ruta);
        GameObject escenario = (GameObject) Instantiate(escenarioPrefab);
        escenario.transform.position = Vector3.zero;
        // crear referencias despues de instanciar el escenario
        bloqueTareaDisparo = FindObjectOfType<BloqueTareaDisparo>();
        dianasPrimerBloque = GameObject.FindGameObjectsWithTag("DianaPrimerBloque");
        dianasSegundoBloque = GameObject.FindGameObjectsWithTag("DianaSegundoBloque");
        dianasTercerBloque = GameObject.FindGameObjectsWithTag("DianaTercerBloque");

    }

    private bool HayEstimulosVisibles()
    {
        ObjetivoTareaDisparo[] dianas = FindObjectsOfType<ObjetivoTareaDisparo>();
        foreach(ObjetivoTareaDisparo diana in dianas)
            if(diana.EnUso)
                return true;
        
        return false; 
    }

    private int bloqueActual = 0; 
    private bool camaraMoviendose = false; 

    private IEnumerator CorrutinaTareaDisparo()
    {
   

        // hay varios bloques de disparos, entre bloque
        // y bloque hay una animacion de camara
        while(true)
        {
            //Debug.Log("Bloque actual es " + bloqueActual);

            // movemos la camara al siguiente bloque                            
            Vector3 posicion = bloqueTareaDisparo.posicionesCamara[bloqueActual].position;
            GameObject jugador = Camera.main.gameObject; 
            float duracionAnimacionCamara = 3f; 
            iTween.MoveTo(jugador, posicion, duracionAnimacionCamara);
            //camaraMoviendose = true; 
            // esperamos a que la camara llegue al nuevo bloque
            yield return new WaitForSeconds(duracionAnimacionCamara);
            //camaraMoviendose = false; 
            // comenzamos el juego de disparos de este bloque
            
            yield return StartCoroutine(CorrutinaBloqueDeDianas());
           

            // cambiamos al siguiente bloque
            bloqueActual++;
            if(bloqueActual >= bloqueTareaDisparo.posicionesCamara.Length)
                bloqueActual = 0; 
        }

    }

    private bool haciendoMovimientosAleatorios = false; 

    private IEnumerator CorrutinaMovimientosAleatorios()
    {
        while(true)
        {

            // espera entre movimientos
            yield return new WaitForSeconds(1f);
            //if(!haciendoMovimientosAleatorios)        
            //{
                haciendoMovimientosAleatorios = true; 

                float cantidad = 0.4f;
                Vector3 posicionAgachado = new Vector3(0f, -cantidad, 0f);
                Vector3 posicionDerecha = new Vector3(cantidad, 0f, 0f);
                Vector3 posicionIzquierda = new Vector3(-cantidad, 0f, 0f);
                Vector3 posicionAdelantada = new Vector3(0f, 0f, cantidad);
                Vector3 posicionAtrasada = new Vector3(0f, 0f, -cantidad);

                Vector3[] posiciones = {
                    posicionAgachado, posicionDerecha, posicionIzquierda, 
                    posicionAdelantada, posicionAtrasada
                };

                Vector3 pR = new Vector3(
                    Random.Range(-0.1f, 0.2f), Random.Range(-0.3f, 0f), Random.Range(-0.1f, 0.2f)
                );

                GameObject jugador = Camera.main.gameObject; 
                Vector3 posicionOriginal = jugador.transform.position;
                Vector3 posicionDestino = posicionOriginal + posiciones[Random.Range(0, posiciones.Length)] + pR;

                float duracionAnimacionCamara = 2f;
                iTween.MoveTo(jugador, posicionDestino, duracionAnimacionCamara);
                yield return new WaitForSeconds(duracionAnimacionCamara);
                iTween.MoveTo(jugador, posicionOriginal, duracionAnimacionCamara);
                yield return new WaitForSeconds(duracionAnimacionCamara);

                haciendoMovimientosAleatorios = false; 

            //}
            yield return null; 
        }
    }

    private IEnumerator CorrutinaBloqueDeDianas()
    {                   
        Debug.Log("Nuevo bloque de dianas");              

        // iniciamos la corrutina de movimientos aleatorios
        // solo son para dar mas variedad al juego 
        Coroutine tareaMoverCamaraAleatoriamente = StartCoroutine(CorrutinaMovimientosAleatorios());

        // calculamos el tiempo de permanencia del bloque 
        float tiempoFinBloque = Time.time + Nivel.duracionDeCadaBloqueDeDianas;     
        // permanecemos en el bloque de dianas hasta cuando se haya
        // pasado el tiempo y haya dianas visibles           
        while(Time.time < tiempoFinBloque)
        {              
            // instanciar un nuevo estimulo despues de esperar el tiempo                       
            yield return new WaitForSeconds(Nivel.tiempoParaNuevaDiana);
            MostrarNuevoObjetivo();            
            yield return null; 
        }  

        // detenmos los movimientos de camara
        StopCoroutine(tareaMoverCamaraAleatoriamente);

        while(HayEstimulosVisibles())
        {
            // esperamos a que se oculten todas las dianas para salir del bloque
            yield return null; 
        }

       
        yield return new WaitForSeconds(1f);
        //Debug.Log("Bloque de dianas terminado");
    }
 

    private int ContadorDianasVisibles()
    {
        int contador = 0; 
        ObjetivoTareaDisparo[] dianas = FindObjectsOfType<ObjetivoTareaDisparo>();
        foreach(ObjetivoTareaDisparo diana in dianas)
            if(diana.EnUso)
                contador++;        

        return contador; 
    }

    private void MostrarNuevoObjetivo()
    {
        // mantenemos un maximo de 2 dianas visibles
        if(ContadorDianasVisibles() >= 2)
        {
            Debug.Log("No se pueden generar mas dianas por ahora");
            return; 
        }

        ArrayList bloques = new ArrayList() {
            dianasPrimerBloque, dianasSegundoBloque, dianasTercerBloque
        };
        GameObject[] dianasBloque = (GameObject[]) bloques[bloqueActual];
        if(dianasBloque == null || dianasBloque.Length == 0)
        {
            // no hay suficinetes dianas, usamos las del primer
            // bloque. Esto solo es valido si no avanzamos
            // mucho por el decorado
            dianasBloque = dianasPrimerBloque;
        }

        // seleccionar posicion al azar         
        int indice = Random.Range(0, dianasBloque.Length);
        GameObject diana = dianasBloque[indice];

        ObjetivoTareaDisparo objetivo = diana.GetComponent<ObjetivoTareaDisparo>();
        // comprobar si ya esta en uso 
        if(objetivo.EnUso)
            return; 
        
        
        switch(Nivel.dianas)
        {
            case EstimulosTareaGaleriaTiro.SoloDianaObjetivo:
                // la diana es objetivio                
                objetivo.esObjetivo = true; 
                
                
            break;

            case EstimulosTareaGaleriaTiro.VariosTiposDiana:
                // las dianas pueden ser no objetivos
                objetivo.esObjetivo = Random.value > Nivel.probabilidadAparicionDianaErronea;
                
            break;

        }

        // comprobamos si es una gema de bonus
        if(objetivo.esObjetivo)
        {
            bool esGema = Random.value < Nivel.probabilidadAparicionGema;
            objetivo.esGema = esGema;
        }

        objetivo.Mostrar();

        FindObjectOfType<Audio>().FeedbackAparicionEstimulo();
        
    }   

   
    public void Bonus()
    {
        // cuando se alcanza una gema
        aciertos++;
        FindObjectOfType<Audio>().FeedbackAciertoBonus();
        aciertosUI.text = "Aciertos " + aciertos.ToString();
        AgregarPuntuacion(Configuracion.puntuacionGemaGaleriaTiro);
    }
  
    public override void Acierto()
    {
        aciertos++;
        FindObjectOfType<Audio>().FeedbackAcierto();
        aciertosUI.text = "Aciertos " + aciertos.ToString();
        AgregarPuntuacion(Configuracion.puntuacionAciertoGaleriaTiro);
       
    }

    public override void Error()
    {
        errores++;
        FindObjectOfType<Audio>().FeedbackError();
        erroresUI.text = "Errores " + errores.ToString();
        AgregarPuntuacion(-Configuracion.penalizacionErrorGaleriaTiro);
       
        
    } 

    public override void Omision()
    {
        omisiones++;
        FindObjectOfType<Audio>().FeedbackOmision();
        omisionesUI.text = "Omisiones " + omisiones.ToString();
        AgregarPuntuacion(-Configuracion.penalizacionOmisionGaleriaTiro);
       
        
    }

}
