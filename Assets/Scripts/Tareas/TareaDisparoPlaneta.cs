using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TareaDisparoPlaneta : Tarea
{
    [Header("Jerarquia de localizaciones")]
    [SerializeField] public GameObject baseEntrenamiento;
    [SerializeField] public GameObject espacio;
    [SerializeField] public GameObject planeta;

    [Header("Instanciado")]
    [SerializeField] private GameObject prefabNaveEnemiga; 
    [SerializeField] private GameObject prefabMina;  
    [SerializeField] private GameObject prefabDiana;
    [SerializeField] private GameObject prefabSilueta;

    [Header("Ruta")]
    [SerializeField] private Transform[] puntosRecorrido;   
    [SerializeField] private AnimationCurve curvaRecorridoX, curvaRecorridoZ; 
     
    public NivelDisparoScriptable Nivel { 
        get { return (NivelDisparoScriptable) Configuracion.nivelActual;} 

    }
    private NaveJugador jugador; 
    private Coroutine corutinaGeneracionEnemigos; 
    private int numeroMaximoDeEnemigos = 5; 
    private int numeroMaximoDeDianas = 2;
    private int errores; 
    private int aciertos; 

    /*
        en la base de entrenamiento:
        1. aparecen dianas
        2. aparecen siluetas
        3. aparecen dianas y siluetas

        pueden aparecer por diferentes puntos con una animacion, 
        y estan visibles una determinada cantidad de tiempo
    */

    /*
        espacio:
        1. aparecen asteroides
        2. aparecen naves
        3. minerales
    */

    /*
        nave:
        1. aparecen minas
        2. aparecen naves
        3. aparecen minerales
    */

    public Vector3 PosicionInicial
    {
        get { return puntosRecorrido[0].position;}
    }
    // devuelve la posicion en la curva para un determinado momento
    public Vector3 EvaluarCurvas(float tiempo)
    {
        return new Vector3(
            curvaRecorridoX.Evaluate(tiempo), 0, curvaRecorridoZ.Evaluate(tiempo)
        );
    }   
    
    public override void Acierto()
    {
        aciertos++;
    }
    public override void Error()
    {
        errores++;
    } 

    protected override void Inicio()
    {           

        switch(Nivel.localizacionDelNivel)
        {
            case LocalizacionTareaDisparo.Entrenamiento:
                StartCoroutine(CorrutinaBaseEntrenamiento());
            break;
            case LocalizacionTareaDisparo.Espacio:
            break;
            case LocalizacionTareaDisparo.Planeta:
            break;
        }

        // creamos el recorrido 
        GenerarCurvaRecorrido();
        // iniciamos la generacion de enemigos durante toda la partida 
        // corutinaGeneracionEnemigos = StartCoroutine(CorutinaGeneracionEnemigos());
        StartCoroutine(CorutinaGeneracionMinas());
    }

    private void InstanciarDiana()
    {               
        GameObject diana = (GameObject) Instantiate(prefabDiana);
        float escala = 1f; 
        diana.transform.localScale = new Vector3(escala, escala, escala);
        diana.name = "Diana";
    }
    private void InstanciarSilueta()
    {
        GameObject silueta = (GameObject) Instantiate(prefabSilueta);
        float escala = 1f; 
        silueta.transform.localScale = new Vector3(escala, escala, escala);
        silueta.name = "Silueta";
    }

    private IEnumerator CorrutinaBaseEntrenamiento()
    {
        // instanciar base

        // colocar camara

        // fades de camara aqui?

        // comenzar la partida
        while(true)
        {
            // instanciar diana o silueta dependiendo del nivel
            yield return new WaitForSeconds(1f);
        }        
    }


    private void GenerarCurvaRecorrido()
    {
        Debug.Log("Generando recorrido");
        // añadir los puntos a la curva X, Z
        curvaRecorridoX = new AnimationCurve();
        curvaRecorridoZ = new AnimationCurve();

        if(puntosRecorrido == null || puntosRecorrido.Length == 0)
            Debug.LogError("No hay puntos para crear el recorrido");

        // introducimos en la curva todos los puntos del recorrido 
        int indice = 0;
        for(int j=0; j<5; j++)
        {
            for(int i=0; i<puntosRecorrido.Length; i++)
            {
                Vector3 posicion = puntosRecorrido[i].position; 
                // añadimos el valor a cda curva, el tiempo de recorrido
                // coincide con el indice del punto                 
                curvaRecorridoX.AddKey(indice, posicion.x);
                curvaRecorridoZ.AddKey(indice, posicion.z);
                // configurar como se extrapola fuera de la curva
                curvaRecorridoX.postWrapMode = WrapMode.Loop; 
                curvaRecorridoZ.postWrapMode = WrapMode.Loop;
                // la altura en cada punto se calcula sobre el terreno
                // mediante raycasting ...
                indice++;
                //Debug.Log("Añadido punto " + posicion.x + ", " + posicion.z + " en momento t " + indice);
            }
        }
        Debug.Log(curvaRecorridoX.length + " puntos añadidos");

        // volvemos a insertar el primer punto al final
        // para loopear el recorrido        
        //Vector3 primerPunto = puntosRecorrido[0].position; 
        //curvaRecorridoX.AddKey(curvaRecorridoX.length, primerPunto.x);
        //curvaRecorridoZ.AddKey(curvaRecorridoX.length, primerPunto.z);
    }

    public void EnemigoDestruido()
    {        
    }

    private void InstanciarEnemigo()
    {
    }

    private void InstanciarMina(float momento)
    {
        // evalusmo la curva para obtener la posicion
        
        GameObject mina = (GameObject) Instantiate(prefabMina);
        float escala = 2.5f; 
        mina.transform.localScale = new Vector3(escala, escala, escala);
        mina.name = "Mina";
        //mina.transform.position = posicion; 
        mina.GetComponent<Mina>().Iniciar(momento);


    }

    private IEnumerator CorutinaGeneracionMinas()
    {
        while(true)
        {
            
            float velocidadNormalizada = Mathf.Lerp(
                0f, 1f, Configuracion.velocidadDeLaNave
            );
            
            float tiempoParaNuevaMina = Mathf.InverseLerp(
                15f, 5f, velocidadNormalizada
            );
            
            tiempoParaNuevaMina = 2f; 

            yield return new WaitForSeconds(tiempoParaNuevaMina);
            // obtenemos la posicion de la mina en t+1
            float incrementoT = 0.15f; 
            float t = FindObjectOfType<NaveJugador>().Tiempo + incrementoT; 

            if (GameObject.FindGameObjectsWithTag("Diana").Length < numeroMaximoDeDianas)
                InstanciarMina(t);
        }
    }

    private IEnumerator CorutinaGeneracionEnemigos()
    {
        while(true)
        {
            float tiempoDecision = 10f; 
            yield return new WaitForSeconds(tiempoDecision);
            // esperamos un tiempo y decidimos que hacer 

            
        }
    }


}
