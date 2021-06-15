using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstimuloTareaTopo : MonoBehaviour
{    
    public GameObject modeloTopo, modeloPato, modeloOveja, 
    modeloPinguino, modeloGato; 

    public bool Escondido {
        get{ return escondido; }
    }

    public bool EsVisible {
        get { return false; }
    }

    public bool EnUso {
        get { return enUso; }
    }

    public bool Golpeado {
        get { return golpeado;}
    }
    
    public EstimulosTareaTopos Estimulo
    {
        get { return estimulo;}
    }
    // el estimulo esta bajo tierra
    private bool escondido = true; 
    // se ha golpeado el estimulo 
    private bool golpeado = false; 
    // el estimulo esta siendo usado por la tarea    
    private bool enUso = false;     
    public bool VisibleParaRegistrar
    {
        get {
            return transform.position.y > 0.3f; 
        }
    }
    // tipo de estimulo 
    private EstimulosTareaTopos estimulo = EstimulosTareaTopos.Ninguno; 
    // referencia a la tarea 
    private TareaTopos tarea; 
    // corutina para mostrar el estimulo 
    private Coroutine corrutinaSalida; 
    
    private void GenerarDiferentesEstimulos()
    {
        NivelToposScriptable nivel = tarea.Nivel; 
        float probabilidad = tarea.Configuracion.probabilidadAparicionEstimuloObjetio; 
        if(Random.value < probabilidad)
        {
            // aparece el estimulo objetivo
            estimulo = nivel.estimuloObjetivo;
        } else {
            // aparecera un estimulo que no es el objetivo, 
            // lo eliminamos de la lista de posibles estimulos y elegimos
            // uno al azar
            ArrayList opcionesEstimulos = new ArrayList() {
                EstimulosTareaTopos.Topo, EstimulosTareaTopos.Pato, 
                EstimulosTareaTopos.Oveja, EstimulosTareaTopos.Pinguino, 
                EstimulosTareaTopos.Gato
            };
            // lo quitamos
            opcionesEstimulos.Remove(nivel.estimuloObjetivo);
            // elegimos otro al azar
            estimulo = (EstimulosTareaTopos) opcionesEstimulos[Random.Range(0, opcionesEstimulos.Count)];

        }
    }

    // generamos un nuevo estimulo siguiendo la configuracion
    // del nivel de dificultad 
    public void Nuevo()
    {
        enUso = true; 
        
        // reiniciamos el estimulo
        estimulo = EstimulosTareaTopos.Ninguno; 

        // generar el estimulo dependiendo de la configuracion 
        // de dificultad de la tarea
        NivelToposScriptable nivel = tarea.Nivel; 

        // comprobamos el tipo de similitud entre estimulos configurada
        switch( nivel.similitudEntreEstimulos)
        {         

            case SimilitudEstimulos.SoloEstimuloObjetivo:
            // solo aparecen estimulos objetivo
            estimulo = nivel.estimuloObjetivo;
            break;

            case SimilitudEstimulos.EstimuloObjetivoCambiante:
            // el estimulo objetivo cambia cada cierto tiempo, por lo que
            // entramos en el codigo del case 'Diferentes estimulos'            
            GenerarDiferentesEstimulos();
            break;

            case SimilitudEstimulos.DiferentesEstimulos:
            // puede aparecer cualquier estimulo, el estimulo objetivo
            // aparece con cierta probabilidad configurada
            GenerarDiferentesEstimulos();
            break;

        }

        // iniciar la tarea de mostrar el estimulo 
        corrutinaSalida = StartCoroutine(CorrutinaSalirExterior(estimulo));
    }
  
    public void Golpedo()
    {
        // si esta escondido abandonamos el metodo 
        if(escondido)
            return; 
            
        if(!golpeado)
        {         
            golpeado = true; 
            // comprobamos si es un acierto o un error comparando
            // el estimulo golpeado con el estimulo objetivo configurado
            // en la tarea 
            if(tarea.Nivel.estimuloObjetivo == estimulo)
            {
                FindObjectOfType<TareaTopos>().Acierto();
            } else {
                FindObjectOfType<TareaTopos>().Error();
            }                                  

            // detenemos la corrutina de salida
            StopCoroutine(corrutinaSalida);

            // iniciamos una secuencia nueva      
            StartCoroutine(CorrutinaGolpeo());
        }
    }

    private IEnumerator CorrutinaGolpeo()
    {
        
        // feedback visual del golpe
        iTween.ShakeScale(gameObject, new Vector3(0.4f, 0.4f, 0.4f), 0.6f);     
        yield return new WaitForSeconds(0.6f); 

        // empezamos la animacion de vuelta a la tierra
        Vector3 posicion = gameObject.transform.position; 
        float tiempoAnimacion = 0.5f; 
        iTween.MoveTo(gameObject, new Vector3(posicion.x, -1, posicion.z), tiempoAnimacion);
        
        // esperamos un tiempo antes de dejar este estimulo
        // disponible
        yield return new WaitForSeconds(1f); 

        // permitimos su nuevo uso 
        enUso = false; 

        yield return null; 

    }

    void Awake()
    {
        // crear referencias
        tarea = FindObjectOfType<TareaTopos>();
    }

    // oculta todos los modelos 3D que puede usar el estimulo 
    private void OcultarModelos()
    {
        GameObject[] modelos = {
            modeloTopo, modeloPato, modeloOveja, modeloPinguino, modeloGato
        };

        foreach(GameObject modelo in modelos)
            modelo.SetActive(false);
    }    

    private IEnumerator CorrutinaSalirExterior(EstimulosTareaTopos tipo)
    {
        // ocultar los modelos
        OcultarModelos();

        // lista de estimulos posibles 
        GameObject[] modelos = {
            modeloTopo, modeloPato, modeloOveja, modeloPinguino, modeloGato
        };
        // mostrar el modelo 3D adecuado 
        modelos[ (int) tipo].SetActive(true);

        // variables para la animacion de aparicion del estimulo
        Vector3 posicion = gameObject.transform.position; 
        float tiempoAnimacion = 0.5f; 
        float alturaObjetivo = 0.5f; 
 
        // animacion de salida al exterior        
        iTween.MoveTo(gameObject, new Vector3(posicion.x, alturaObjetivo, posicion.z), tiempoAnimacion);

        // actualizamos el estado del topo
        escondido = false; 
        golpeado = false; 

        // esperamos el tiempo de permanencia configurado en el 
        // scriptable del nivel de dificultad 
        // añadimos aqui el multiplicador de velocidad
        yield return new WaitForSeconds(
            tarea.Nivel.tiempoPermanenciaDelEstimulo *
            tarea.Configuracion.multiplicadorVelocidad
        );

        // empezamos la animacion de vuelta a la tierra
        iTween.MoveTo(gameObject, new Vector3(posicion.x, -1, posicion.z), tiempoAnimacion);

        // pasado el tiempo de permanencia del estimulo podemos
        // computar un error o una omision si no hemos golpeado el estimulo.
        // Comparamos el estimulo con el estimulo objetivo configurado en el nivel 
        if (!golpeado)
        {
            
            //Debug.Log(estimulo + " vs " + tarea.Nivel.estimuloObjetivo);

            if(estimulo == tarea.Nivel.estimuloObjetivo)
            {
                // era un estimulo objetivo que no hemos golpeado 
                FindObjectOfType<TareaTopos>().Omision();

            } else{

                // hemos dejado de golpear un estimulo que no 
                // era objetivo
            }
        }
        
        // actualizamos el estado del topo
        escondido = true; 

        // esperamos un tiempo antes de dejar este estimulo
        // disponible
        yield return new WaitForSeconds(1f); 

        // permitimos su nuevo uso 
        enUso = false; 

        yield return null; 
    }


    
}
