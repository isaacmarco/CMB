using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TareaNaves : Tarea
{
    [Header("Instanciado")]
    [SerializeField] private GameObject prefabNaveEnemiga; 
    [Header("Ruta")]
    [SerializeField] private Transform[] puntosRecorrido;

    private NaveJugador jugador; 
    private Coroutine corutinaGeneracionEnemigos; 
    private int numeroMaximoDeEnemigos = 5; 
    [SerializeField]  private AnimationCurve curvaRecorridoX, curvaRecorridoZ; 

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
    
    public override void Acierto(){}
    public override void Error(){} // cuando haya objetivos aliados o civiles?

    protected override void Inicio()
    {              
        // creamos el recorrido 
        GenerarCurvaRecorrido();
        // iniciamos la generacion de enemigos durante toda la partida 
        // corutinaGeneracionEnemigos = StartCoroutine(CorutinaGeneracionEnemigos());

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
                Debug.Log("Añadido punto " + posicion.x + ", " + posicion.z + " en momento t " + indice);
            }
        }
        Debug.Log(curvaRecorridoX.length + " puntos añadidos");

        // volvemos a insertar el primer punto al final
        // para loopear el recorrido
        
        //Vector3 primerPunto = puntosRecorrido[0].position; 
        //curvaRecorridoX.AddKey(curvaRecorridoX.length, primerPunto.x);
        //curvaRecorridoZ.AddKey(curvaRecorridoX.length, primerPunto.z);

        // suavizar la tangente para empatar correctamente la
        // curva en el fin-inicio
       
        //curvaRecorridoX.SmoothTangents(0, 0);
        //curvaRecorridoZ.SmoothTangents(0, 0); /*
       // curvaRecorridoX.SmoothTangents(puntosRecorrido.Length, 1);
        //curvaRecorridoZ.SmoothTangents(puntosRecorrido.Length, 1);

    }

    public void EnemigoDestruido()
    {        
    }

    private void InstanciarEnemigo()
    {
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
