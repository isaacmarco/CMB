using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class TareaTopos : Tarea
{
    private int puntuacion, aciertos, errores, omisiones;           
  
    // devuelve el tiempo que el topo es visible al salir
    public float TiempoPermanenciaDelEstimulo { get{ return Nivel.tiempoPermanenciaDelEstimulo; } }    
    
   
    public NivelToposScriptable Nivel { 
        get { return (NivelToposScriptable) Configuracion.nivelActual;} 
    }

    public int Aciertos { get { return aciertos;} }    
    public int Errores { get{return errores;} }
    public int Omisiones { get { return omisiones;} }

    // lista topos
    public EstimuloTareaTopo[] estimulos;
    
    protected override string ObtenerCabeceraTarea()
    {
        string cabecera = string.Empty;
        string posicionesMatriz = string.Empty; 

        for(int i=0; i<estimulos.Length; i++)
        {
            // obtener posicion del elemento
            Vector3 posicionElementoMatriz = estimulos[i].transform.position; 
            // transformar las coordeandas
            Vector2 posicionViewport = Camera.main.WorldToViewportPoint(posicionElementoMatriz);
            Vector2 posicionEnPantalla = new Vector2(
                ((posicionViewport.x * CanvasRect.sizeDelta.x)-(CanvasRect.sizeDelta.x * 0.5f)),
                ((posicionViewport.y * CanvasRect.sizeDelta.y)-(CanvasRect.sizeDelta.y * 0.5f))
            );
            posicionesMatriz += "(" + (int)posicionEnPantalla.x + "," + (int)posicionEnPantalla.y + ") ";            
        }
        cabecera += "Posiciones fijas de los estimulos " + posicionesMatriz + "\n";

        cabecera += "Leyenda: tiempo; Estimulo objetivo; x ;y ; matriz que representa el estado de la tarea";
        return cabecera;
    }

    protected override RegistroPosicionOcular NuevoRegistro(float tiempo, int x, int y)
    {
        // creamos la matriz
        EstimulosTareaTopos[] matriz = new EstimulosTareaTopos[estimulos.Length];
        for(int i=0; i<estimulos.Length;i++)
            matriz[i] = estimulos[i].Estimulo;
        
        return new RegistroPosicionOcultarTareaTopos(tiempo, x, y, Nivel.estimuloObjetivo, matriz);        
    } 

    protected override void Inicio()
    {        
        // inciar corrutina de la partida 
        StartCoroutine(CorrutinaPartida());
       
    }

    
    
    // se registra un acierto 
    public void Acierto()
    {        
        FindObjectOfType<Audio>().FeedbackAcierto();
        aciertos++;
        if(aciertos >= Nivel.aciertosParaSuperarElNivel)
            PartidaGanada();
    }

    // se registra una omision 
    public void Omision()
    {
        FindObjectOfType<Audio>().FeedbackOmision();
        omisiones++;
        ComprobarOmisionError();
    }    

    // se registra un error
    public void Error()
    {
        FindObjectOfType<Audio>().FeedbackError();
        errores++;
        ComprobarOmisionError();
    }

    // se comprueba el estado de la partida despues de un error
    // o una omision 
    private void ComprobarOmisionError()
    {
        if(errores + omisiones >= Nivel.omisionesOErroresParaPerder)
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

    private IEnumerator CorrutinaPartida()
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
            yield return new WaitForSeconds(Nivel.tiempoParaNuevoEstimulo);

        }
    }

    // aparece un topo nuevo 
    private void NuevoEstimulo()
    {
        
        // Debug.Log("Nuevo estimulo");

        // obtener topo al azar
        int indiceMatriz = Random.Range(0, estimulos.Length);
        // TODO: 
        // obtener indices en la matriz al azar hasta que
        // se obtenga un estimulo que este escondido
        if(!estimulos[indiceMatriz].EnUso)
            estimulos[indiceMatriz].Nuevo();   
            
    }

   

}
