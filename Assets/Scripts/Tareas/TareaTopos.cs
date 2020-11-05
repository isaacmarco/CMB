using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
using System.IO;
public class TareaTopos : Tarea
{
    private int aciertos, errores, omisiones;       
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
    
    
    public override string ObtenerNombreTarea()
    {
        return "Tarea topos";
    }
    
    protected override string ObtenerCabeceraTarea()
    {
        string cabecera = string.Empty;
        string posicionesMatriz = string.Empty; 

        // datos de la tarea
        cabecera += "Tarea de topos\n";
        cabecera += "Nivel actual: " + Configuracion.nivelActual.numeroDelNivel + "\n";
        // resultados
        cabecera += "Aciertos: " + aciertos + "\n";
        cabecera += "Errores: " + errores + "\n";
        cabecera += "Omision: " + omisiones + "\n";

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
        // creamos la matriz que define el estado actual del tablero 
        EstimulosTareaTopos[] matriz = new EstimulosTareaTopos[estimulos.Length];
        for(int i=0; i<estimulos.Length;i++)
        {
            // comprobamos que el estimulo este visible
            if(!estimulos[i].Escondido)
            {   
                // si es visible anotamos el estimulo en la matriz
                matriz[i] = estimulos[i].Estimulo;
            } else {
                // en caso contrario lo marcamos como 'ninguno' en la matriz
                matriz[i] = EstimulosTareaTopos.Ninguno;
            }
        }
        // devolvemos el nuevo registro
        return new RegistroPosicionOcultarTareaTopos(tiempo, x, y, Nivel.estimuloObjetivo, matriz);        
    } 

    protected override void Inicio()
    {        
       
        // inciar corrutina de la partida 
        corrutinaJuego = StartCoroutine(CorrutinaPartida());       
    }

    protected override bool GuardarProgreso(bool partidaGanada)
    {        
        Debug.Log("Guardando progreso de partida de topos");

        if(partidaGanada)
        {
            // guardar la puntuacion
            if(puntuacion > 0)
                Configuracion.pacienteActual.puntuacionTareaTopos += puntuacion; 

            // progresar
            Configuracion.pacienteActual.ultimoNivelDesbloqueadoTareaTopos++;
            
            // comprobar si hemos terminado todos los niveles
            int numeroNiveles = 61;             
            if(Configuracion.pacienteActual.ultimoNivelDesbloqueadoTareaTopos >= numeroNiveles)
            {
                Debug.Log("Todos los niveles de la tarea completos");
                // El juego se ha terminado, no hay mas niveles
                Configuracion.pacienteActual.ultimoNivelDesbloqueadoTareaTopos = numeroNiveles; 
            }        

            // serializar los datos en disco 
            Aplicacion.instancia.GuardarDatosPaciente(Configuracion.pacienteActual);

        } else {
            
            // no cambiamos el progreso ni guardamos datos
            Debug.Log("La partida se ha perdido, no se guarda el progreso");
        }

        // devolvemos falso porque no se conceden premios adicionales
        return false; 

    }
    
    
    // se registra un acierto 
    public override void Acierto()
    {        
        FindObjectOfType<Audio>().FeedbackAcierto();
        aciertos++;
        AgregarPuntuacion(Configuracion.puntuacionAciertoTopo);
        if(aciertos >= Nivel.aciertosParaSuperarElNivel)
            JuegoGanado();
    }

    // se registra una omision 
    public override void Omision()
    {
        FindObjectOfType<Audio>().FeedbackOmision();
        omisiones++;
        AgregarPuntuacion(Configuracion.penalizacionOmisionTopo);
        ComprobarOmisionError();
    }    

    // se registra un error
    public override void Error()
    {
        FindObjectOfType<Audio>().FeedbackError();
        errores++;
        AgregarPuntuacion(Configuracion.penalizacionErrorTopo);
        ComprobarOmisionError();
    }

    // se comprueba el estado de la partida despues de un error
    // o una omision 
    private void ComprobarOmisionError()
    {
        if(errores + omisiones >= Nivel.omisionesOErroresParaPerder)
            JuegoPerdido();
    }
    
    
    private IEnumerator CorrutinaPartida()
    {
        Debug.Log("Inicio de tarea");

        // mensaje de explicacion de los diferentes niveles clave
      
        int nivel = Configuracion.nivelActual.numeroDelNivel;
        if(nivel == 0)
            yield return StartCoroutine(MostrarMensaje("Mira al topo",0,null,Mensaje.TipoMensaje.Topos));
        //if(nivel == 4)
          //  yield return StartCoroutine(MostrarMensaje("Ahora debes ser más rápido"));
        if(nivel > 6 && nivel < 12)
            yield return StartCoroutine(MostrarMensaje("¡Atención! Aparecen otros animales"));
        if(nivel > 12 && nivel < 37)
        {
            string[] frases = 
            {
                "Mira al topo",
                "Mira al pato", 
                "Mira a la oveja",
                "Mira al pingüino",
                "Mira al gato"
            };
            string frase = frases[(int) Nivel.estimuloObjetivo];            
            yield return StartCoroutine(MostrarMensaje(frase));
        }
        if(nivel > 36)
            yield return StartCoroutine(MostrarMensaje("¡Ahora vamos a ir cambiando!"));

       
        
        // mensaje normal de inicia
        yield return StartCoroutine(MostrarMensaje("Comenzamos", 0, null, Mensaje.TipoMensaje.Comienzo));

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
        if(contadorEstimulosMostrados >= 5) // Nivel.aparicionesAntesDeCambiarEstimuloObjetivo)
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
                
                // feedback, mostramos un sprite y un mensaje para indicar
                // el nuevo estimulo objetivo
                Sprite spriteEstimuloObjetivo = FindObjectOfType<IndicadorEstimuloObjetivo>().SpriteEstimuloObjetivo;
                yield return StartCoroutine(MostrarMensaje(
                    "Tu nuevo objetivo", 4, spriteEstimuloObjetivo, Mensaje.TipoMensaje.Ojo));

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
