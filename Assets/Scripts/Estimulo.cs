using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Estimulo : MonoBehaviour
{    
    public GameObject modeloTopo, modeloPato, modeloOveja, 
    modeloPinguino; 

    public bool Escondido {
        get{ return this.escondido; }
    }

    // un estimulo solo puede ser golpeado si no esta escondido 
    private bool escondido = true; 
    private bool golpeado = false; 
    private bool esObjetivo = false; 
    private Estimulos estimulo = Estimulos.Topo; 
    private TareaTopos tarea; 
    
    
    // generamos un nuevo estimulo siguiendo la configuracion
    // del nivel de dificultad 
    public void Nuevo(NivelScriptable nivel)
    {
        
        // generar el estimulo dependiendo de la configuracion 
        // de dificultad de la tarea

        Estimulos estimulo = nivel.estimuloObjetivo;
        NivelDificultadScriptable nivelDificultad = nivel.nivelDeDificultad;        
            

        // comprobamos el tipo de similitud entre estimulos configurada
        switch( nivelDificultad.similitudEntreEstimulos)
        {
            case SimilitudEstimulos.SoloEstimuloObjetivo:
            // solo se muestra el estimulo objetivo configurado en el nivel
            estimulo = nivel.estimuloObjetivo;
            break;

            case SimilitudEstimulos.DiferentesEstimulos:
            // estimulos aleatorios
            estimulo = (Estimulos) (Random.Range(0, 4));
            // si coincide el estimulo aleatorio con el objetivo entonces
            // es un estimulo objetivo 
            esObjetivo = estimulo == nivel.estimuloObjetivo; 
            break;

            case SimilitudEstimulos.DiferentesEstimulosConElColorDelObjetivo:
            // estimulos aleatorios que pueden tener el color
            // del estimulo objetivo 
            estimulo = (Estimulos) (Random.Range(0, 4));            
            esObjetivo = estimulo == nivel.estimuloObjetivo; 
            if(!esObjetivo)
            {
                // TODO
                // cambiar textura aleatoria
            }
            break;

            case SimilitudEstimulos.EstimuloObjetivoConColorCambiante:
            // estimulos aleatorios y el estimulo objetivo puede tener
            // color cambiante
            estimulo = (Estimulos) (Random.Range(0, 4));
            // TODO
            // cambiar textura aleatoria de cualquier tipo de estimulo
            break;

            case SimilitudEstimulos.EstimuloObjetivoConDetallesCambiantes:
            // estimulos aleatorios y el estimulo objetivo puede tener color
            // cambiante, ademas cambian detalles en el estimulo objetivo 
             // estimulos aleatorios y el estimulo objetivo puede tener
            // color cambiante
            estimulo = (Estimulos) (Random.Range(0, 4));
            // TODO
            // cambiar textura aleatoria de cualquier tipo de estimulo
            // TODO
            // cambiar detalles en el estimulo objetivo
            break;
        }


        //this.estimulo = estimulo;         
        // comprobar si el estimulo es objetivo 
        //esObjetivo = estimulo == tarea.nivel.estimuloObjetivo;
        // iniciar la tarea de mostrar el estimulo 
        StartCoroutine(CorutinaSalirExterior(estimulo));
    }

    /*
    public void SalirExterior()
    {
        Estimulos tipo = Estimulos.Topo; 
        if(Random.value > 0.5f)
            tipo = (Estimulos) Random.Range(0, 4);

        StartCoroutine(CorutinaSalirExterior( tipo) );
    }
    */


    public void Golpedo()
    {
        if(escondido)
            return; 
            
        // feedback al recibir el golpe
        if(!golpeado)
        {
            Debug.Log("topo golpeado");
            iTween.ShakeScale(gameObject, new Vector3(0.4f, 0.4f, 0.4f), 0.6f);
            golpeado = true; 
            FindObjectOfType<TareaTopos>().Acierto();
        }
    }

    void Awake()
    {
        // crear referencias
        tarea = FindObjectOfType<TareaTopos>();
        //nivelDificultad = tarea.NivelDificultad;
    }

    private void OcultarModelos()
    {
        GameObject[] modelos = {
            modeloTopo, modeloPato, modeloOveja, modeloPinguino
        };

        foreach(GameObject modelo in modelos)
            modelo.SetActive(false);
    }

    private IEnumerator CorutinaSalirExterior(Estimulos tipo)
    {
        // ocultar los modelos y mostrar el adecuado 
        OcultarModelos();
        GameObject[] modelos = {
            modeloTopo, modeloPato, modeloOveja, modeloPinguino
        };
        modelos[ (int) tipo].SetActive(true);

        Vector3 posicion = gameObject.transform.position; 
        float tiempoAnimacion = 0.5f; 
        float alturaObjetivo = 0.5f; 

        // animacion de salida al exterior        
        iTween.MoveTo(gameObject, new Vector3(posicion.x, alturaObjetivo, posicion.z), tiempoAnimacion);

        // actualizamos el estado del topo
        escondido = false; 
        golpeado = false; 

        // esperamos el tiempo de exposicion
        yield return new WaitForSeconds(3f); //tarea.TiempoExposicionTopo);


        // empezamos la animacion de vuelta a la tierra
        iTween.MoveTo(gameObject, new Vector3(posicion.x, -1, posicion.z), tiempoAnimacion);

        // contabilizar error
        if(!golpeado)
            FindObjectOfType<TareaTopos>().Error();
            
        // actualizamos el estado del topo
        escondido = true; 

        yield return null; 
    }


    
}
