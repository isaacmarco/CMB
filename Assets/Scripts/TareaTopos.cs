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
    //public NivelScriptable nivel;   
    private ArrayList listaRegistrosOculares; 
    public ConfiguracionScriptable configuracion;     

    // devuelve el tiempo que el topo es visible al salir
    public float TiempoPermanenciaDelEstimulo { get{ return Nivel.tiempoPermanenciaDelEstimulo; } }
    // devuelve el nivel de dificultad
    //public NivelDificultadScriptable NivelDificultad { get { return nivelDificultad;} }
    public ConfiguracionScriptable Configuracion { get { return configuracion;} }
    public NivelToposScriptable Nivel { 
        get { return (NivelToposScriptable) Configuracion.nivelActual;} 
    }
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
        // inciar corrutina de la partida 
        StartCoroutine(CorutinaPartida());
        // iniciar la corrutina del diario
        StartCoroutine(RegistroDiario());
    }

    private IEnumerator RegistroDiario()
    {
        float tiempoEspera = 1 / Configuracion.intervaloRegistroOcularEnHZ;
        listaRegistrosOculares = new ArrayList();
        int tiempoActualRegistro = 0; 

        while(true)
        {           

            // obtenemos la posicion a la que se mira para registrarla
            GazePoint gazePoint = TobiiAPI.GetGazePoint();

		    if (gazePoint.IsValid)
		    {
			    Vector2 posicionGaze = gazePoint.Screen;	            			        
                // creamos el nuevo registro y lo introducimos en la lista
                listaRegistrosOculares.Add( new RegistroCocular(
                    (int) posicionGaze.x, 
                    (int) posicionGaze.y, 
                    tiempoActualRegistro)
                );
                

            } else {
                
                // si el punto no es valido lo indicamos con un valor especial
                // x,y negativo
                listaRegistrosOculares.Add( new RegistroCocular(
                    -1, -1, tiempoActualRegistro
                ));
            }      
            
            tiempoActualRegistro++;
            yield return new WaitForSeconds(tiempoEspera);      

            
		}   
        
    }

    private class RegistroCocular
    {
        // momento
        int tiempo;
        // punto al que se mira
        int x, y;
        public RegistroCocular(int x, int y, int tiempo)
        {
            this.x = x; 
            this.y = y; 
            this.tiempo = tiempo; 
        }
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
