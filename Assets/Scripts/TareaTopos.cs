using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class TareaTopos : Tarea
{
    private int puntuacion, aciertos, errores, omisiones;     
    private GazeAware gazeAware;
    // todo: establecer como privado y recurrir al 
    // componente Aplicacion 
    public NivelScriptable nivel;
    public ConfiguracionScriptable configuracion; 
    private NivelDificultadScriptable nivelDificultad;    

    // devuelve el tiempo que el topo es visible al salir
    public float TiempoPermanenciaDelEstimulo { get{ return nivelDificultad.tiempoPermanenciaDelEstimulo; } }
    // devuelve el nivel de dificultad
    public NivelDificultadScriptable NivelDificultad { get { return nivelDificultad;} }
    public ConfiguracionScriptable Configuracion { get { return configuracion;} }
    public NivelScriptable Nivel { get { return nivel;} }
    public int Aciertos { get { return aciertos;} }    
    public int Errores { get{return errores;} }
    public int Omisiones { get { return omisiones;} }

    // lista topos
    public Estimulo[] estimulos;
    
    [SerializeField] private RectTransform canvasRect; 

    public RectTransform CanvasRect 
    {
        get {return canvasRect;}
    }

    protected override void Inicio()
    {
        // crear referencia al nivel de dificultad
        nivelDificultad = nivel.nivelDeDificultad;
      

        // inciar cortuina de la partida 
        StartCoroutine(CorutinaPartida());
    }

    
    // se registra un acierto 
    public void Acierto()
    {        
        aciertos++;
        if(aciertos >= nivel.aciertosParaSuperarElNivel)
            PartidaGanada();
    }

    // se registra una omision 
    public void Omision()
    {
        omisiones++;
        ComprobarOmisionError();
    }    

    // se registra un error
    public void Error()
    {
        errores++;
        ComprobarOmisionError();
    }

    // se comprueba el estado de la partida despues de un error
    // o una omision 
    private void ComprobarOmisionError()
    {
        if(errores + omisiones >= nivel.omisionesOErroresParaPerder)
            PartidaPerdida();
    }
    
    private void PartidaGanada()
    {
        //Debug.Log("Partida ganada");
    }
    private void PartidaPerdida()
    {
        //Debug.Log("Partida perdida");
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
            yield return new WaitForSeconds(nivelDificultad.tiempoParaNuevoEstimulo);

        }
    }

    // aparece un topo nuevo 
    private void NuevoEstimulo()
    {
        
        // Debug.Log("Nuevo estimulo");

        // obtener topo al azar
        int indiceEstimulo = Random.Range(0, estimulos.Length);
        // TODO: 
        // obtener indices en la matriz al azar hasta que
        // se obtenga un estimulo que este escondido
        if(!estimulos[indiceEstimulo].EnUso)
            estimulos[indiceEstimulo].Nuevo();   
            
    }

   

}
