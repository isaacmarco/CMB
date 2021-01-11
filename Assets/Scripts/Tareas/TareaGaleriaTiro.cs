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
  
    // posicion de dianas y camaras para este nivel instanciado
    private BloqueTareaDisparo bloqueTareaDisparo; 

    public NivelDisparoScriptable Nivel { 
        get { return (NivelDisparoScriptable) Configuracion.nivelActual;} 
    }
   

    // estado del juego
    private int errores; 
    private int aciertos; 
    private int omisiones; 
    private float tiempoEntreCambiosCamara = 5f;
    private float tiempoDelUltimoCambioCamara; 

    protected override void Inicio()
    {          
        // crear el escenario
        InstanciarEscenario();
        // comenzar la tarea        
        DesbloquearTarea();
        StartCoroutine(CorrutinaTareaDisparo());       
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
        
        return false; //  dianas != null && dianas.Length > 0;
    }

    private int bloqueActual = 0; 
    private bool camaraMoviendose = false; 

    private IEnumerator CorrutinaTareaDisparo()
    {
   

        // hay varios bloques de disparos, entre bloque
        // y bloque hay una animacion de camara
        while(true)
        {
            Debug.Log("Bloque actual es " + bloqueActual);

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

    private IEnumerator CorrutinaBloqueDeDianas()
    {                   
        Debug.Log("Nuevo bloque de dianas");              
        float tiempoFinBloque = Time.time + Nivel.duracionDeCadaBloqueDeDianas;     
        // permanecemos en el bloque de dianas hasta cuando se haya
        // pasado el tiempo y haya dianas visibles           
        while(Time.time < tiempoFinBloque) // && HayEstimulosVisibles())
        {              
            // instanciar un nuevo estimulo despues de esperar el tiempo                       
            yield return new WaitForSeconds(1f); // Nivel.tiempoParaNuevoEstimuloEntrenamiento);           
            MostrarNuevoObjetivo();            
            yield return null; 
        }  

        while(HayEstimulosVisibles())
        {
            // esperamos a que se oculten todas las dianas para salir del bloque
            yield return null; 
        }

       
        yield return new WaitForSeconds(1f);
        Debug.Log("Bloque de dianas terminado");
    }
 

    private void MostrarNuevoObjetivo()
    {
        ArrayList bloques = new ArrayList() {
            dianasPrimerBloque, dianasSegundoBloque, dianasTercerBloque
        };
        GameObject[] dianasBloque = (GameObject[]) bloques[bloqueActual];
        if(dianasBloque == null || dianasBloque.Length == 0)
        {
            Debug.LogError("Faltan dianas para los bloques");
            dianasBloque = dianasPrimerBloque;
        }

        // seleccionar posicion al azar         
        int indice = Random.Range(0, dianasBloque.Length);
        GameObject diana = dianasBloque[indice];

        ObjetivoTareaDisparo objetivo = diana.GetComponent<ObjetivoTareaDisparo>();
        // comprobar si ya esta en uso 
        if(objetivo.EnUso)
            return; 
        
        
        switch(Nivel.estimulos)
        {
            case EstimulosTareaDisparoEntrenamiento.SoloDianaObjetivo:
                // la diana es objetivio                
                objetivo.esObjetivo = true; 
                objetivo.Mostrar();
            break;

            case EstimulosTareaDisparoEntrenamiento.VariosTiposDiana:
                // las dianas pueden ser no objetivos
                objetivo.esObjetivo = Random.value < Nivel.probabilidadAparicionEstimuloErroneo;
                objetivo.Mostrar();
            break;

        }

        FindObjectOfType<Audio>().FeedbackAparicionEstimulo();
        
    }   
  
    public override void Acierto()
    {
        aciertos++;
        FindObjectOfType<Audio>().FeedbackAcierto();
        aciertosUI.text = "Aciertos " + aciertos.ToString();
        
    }

    public override void Error()
    {
        errores++;
        FindObjectOfType<Audio>().FeedbackError();
        erroresUI.text = "Errores " + errores.ToString();
        
    } 

    public override void Omision()
    {
        omisiones++;
        FindObjectOfType<Audio>().FeedbackOmision();
        omisionesUI.text = "Omisiones " + omisiones.ToString();
        
    }

}
