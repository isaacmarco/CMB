using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
public class TareaTopos : Tarea
{
    private int puntuacion, aciertos, errores;     
    private GazeAware gazeAware;
    // todo: establecer como privado y recurrir al 
    // componente Aplicacion 
    public NivelScriptable nivel;
    private NivelDificultadScriptable nivelDificultad; 

    // devuelve el tiempo que el topo es visible al salir
    public float TiempoExposicionDelEstimulo
    {
        get{return this.tiempoExposicionDelEstimulo; }
    }
    // devuelve el nivel de dificultad
    public NivelDificultadScriptable NivelDificultad
    {
        get { return this.nivelDificultad;}
    }

    public void Acierto()
    {        
        aciertos++;
        if(aciertos >= nivel.aciertosParaSuperarElNivel)
            PartidaGanada();
    }

    public void Error()
    {
        errores++;
        if( errores >= nivel.erroresParaPerder)
            PartidaPerdida();
    }
    
    private void PartidaGanada()
    {
        Debug.Log("Partida ganada");
    }
    private void PartidaPerdida()
    {
        Debug.Log("Partida perdida");
    }


    // lista topos
    public Estimulo[] estimulos;
    // tiempo entre salidas del topo
    private float tiempoParaNuevoEstimulo = 3f; 
    // tiempo durante el que el topo es visible
    private float tiempoExposicionDelEstimulo = 3f;

    protected override void Inicio()
    {
        // crear referencia al nivel de dificultad
        nivelDificultad = nivel.nivelDeDificultad;
        // configurar tarea segun el nivel de dificultad
        tiempoExposicionDelEstimulo = nivelDificultad.tiempoPermanenciaDelEstimuloObjetivo;
        tiempoParaNuevoEstimulo = nivelDificultad.tiempoParaNuevoEstimuloObjetivo;

        // inciar cortuina de la partida 
        StartCoroutine(CorutinaPartida());
    }

    private IEnumerator CorutinaPartida()
    {
        Debug.Log("Partida en curso");

        // tiempo de espera inicial
        yield return new WaitForSeconds(1f);

        // comienzo del game loop
        while(true)
        {
            // generar un nuevo topo
            NuevoEstimulo();
            // esperar un tiempo antes de mostrar otro
            yield return new WaitForSeconds(tiempoParaNuevoEstimulo);

        }
    }

    // aparece un topo nuevo 
    private void NuevoEstimulo()
    {
        Debug.Log("Nuevo estimulo");

        // obtener topo al azar
        int indiceEstimulo = Random.Range(0, estimulos.Length);
        // TODO: 
        // obtener indices en la matriz al azar hasta que
        // se obtenga un estimulo que este escondido
        if(estimulos[indiceEstimulo].Escondido)
            estimulos[indiceEstimulo].Nuevo(nivel);   
            
    }

   

}
