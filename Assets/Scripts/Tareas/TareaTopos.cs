using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class TareaTopos : Tarea
{
    private int puntuacion, aciertos, errores, omisiones;       
    private Coroutine corrutinaJuego;     
    private Coroutine corrutinaCambioEstimuloObjetivo; 
    // contador de estimulos mostrados
    private int contadorEstimulosMostrados = 0; 
  
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
        corrutinaJuego = StartCoroutine(CorrutinaPartida());       
    }

    
    
    // se registra un acierto 
    public override void Acierto()
    {        
        FindObjectOfType<Audio>().FeedbackAcierto();
        aciertos++;
        if(aciertos >= Nivel.aciertosParaSuperarElNivel)
            JuegoGanado();
    }

    // se registra una omision 
    public override void Omision()
    {
        FindObjectOfType<Audio>().FeedbackOmision();
        omisiones++;
        ComprobarOmisionError();
    }    

    // se registra un error
    public override void Error()
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
            JuegoPerdido();
    }
    
    /*
    protected override IEnumerator TerminarJuego(bool juegoGanado)
    {
        
        // mostrar feedback
        if(juegoGanado)
        {
            yield return StartCoroutine(MostrarMensaje("Partida Ganada"));
        } else {
            yield return StartCoroutine(MostrarMensaje("Partida perdida"));
        }

        // en este punto se vuelve al menu 
        Debug.LogError("Juego finalizado");

        yield return null;
    }*/


    private IEnumerator CorrutinaPartida()
    {
        Debug.Log("Inicio de tarea");

        yield return StartCoroutine(MostrarMensaje("Comienza la partida", 1));

        // tiempo de espera inicial
        yield return new WaitForSeconds(1f);

        Debug.Log("Comienzo de juego");
       

        // comienzo del game loop
        while(true)
        {            
            // generar un nuevo topo
            NuevoEstimulo();
            contadorEstimulosMostrados++;

            // comprobar si el nivel de dificultad exige cambios
            // de estimulo objetivo
            if(Nivel.similitudEntreEstimulos == SimilitudEstimulos.EstimuloObjetivoCambiante)  
                yield return StartCoroutine(CorrutinaCambioEstimuloObjetivo());

            
            // esperar un tiempo antes de mostrar otro estimulo 
            yield return new WaitForSeconds(Nivel.tiempoParaNuevoEstimulo);
        }
    }

    private IEnumerator CorrutinaCambioEstimuloObjetivo()
    {        
        // comprobamos si ya han aparecido suficientes estimulos
        // objetivos del tipo actual                 
        if(contadorEstimulosMostrados >= Nivel.aparicionesAntesDeCambiarEstimuloObjetivo)
        {                     

            // se han mostrado suficientes estimulos para hacer un cambio, 
            // pero primero hay que esperar a que no se este 
            // mostrando ningun estimulo al jugador
            while(HayEstimulosVisibles())
            {
                // esperamos
                yield return null; 
            }
            
            // proseguimos con la corrutina ya que en este punto
            // todos los estimulos estan ocultos

            // recordamos el estimulo actual 
            EstimulosTareaTopos estimuloObjetivoActual = Nivel.estimuloObjetivo; 

            // reiniciamos el contador de estimulos mostrados
            contadorEstimulosMostrados = 0; 
            EstimulosTareaTopos nuevoEstimuloObjetivo = (EstimulosTareaTopos) Random.Range(0, 4);
            // OJO: estoy cambiando el estimulo del scriptable!,
            // esto no deberia importar en la practica
            Nivel.estimuloObjetivo = nuevoEstimuloObjetivo; 
            
            // comprobar si ha cambiado
            if(Nivel.estimuloObjetivo != estimuloObjetivoActual)
            {
                Debug.Log("Cambiando estimulo objetivo, ahora es " + Nivel.estimuloObjetivo);
                
                // feedback
                yield return StartCoroutine(MostrarMensaje("El objetivo ha cambiado"));

            } else {
                // seguimos con el mismo estimulo objetivo que antes porque ha
                // vuelto a ser seleccionado al azar
            }
        }                    

        yield return null; 
    }

    

    // devuelve verdadero si alguno de los estimulos
    // esta mostrandose al jugador
    private bool HayEstimulosVisibles()
    {
        foreach(EstimuloTareaTopo estimulo in estimulos)
            if(estimulo.EnUso)
                return true; 
        return false; 
    }

    private bool ComprobarCambioEstimuloObjetivo()
    {        
        EstimulosTareaTopos estimuloObjetivoActual = Nivel.estimuloObjetivo; 

        // comprobamos si ya han aparecido suficientes estimulos
        // objetivos del tipo actual                 
        if(contadorEstimulosMostrados >= Nivel.aparicionesAntesDeCambiarEstimuloObjetivo)
        {                    
            // reiniciamos el contador de estimulos mostrados
            contadorEstimulosMostrados = 0; 
            EstimulosTareaTopos nuevoEstimuloObjetivo = (EstimulosTareaTopos) Random.Range(0, 4);
            // OJO: estoy cambiando el estimulo del scriptable!
            Nivel.estimuloObjetivo = nuevoEstimuloObjetivo; 
            // comprobar si ha cambiado
            if(Nivel.estimuloObjetivo != estimuloObjetivoActual)
                Debug.Log("Cambiando estimulo objetivo, ahora es " + Nivel.estimuloObjetivo);
        }                    

        // devuelve verdadero si el estimulo objetivo ha cambiado 
        return Nivel.estimuloObjetivo != estimuloObjetivoActual;
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
