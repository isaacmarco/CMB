using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class Tarea : MonoBehaviour
{
    protected bool tareaBloqueada;
    private ArrayList listaRegistrosOculares; 
    private bool registrarEnDiario = true; 
    
    // fecha para registrar los datosw 
    private string fechaRegistro = string.Empty; 

    private bool escrituraFallidaEnDisco = false; 
    
    protected int puntuacion; 
    protected string nombreTarea; 
    protected GazeAware gazeAware;
    [SerializeField] private ConfiguracionScriptable configuracion;     
    [SerializeField] private RectTransform canvasRect; 
    [SerializeField] private Mensaje mensaje; 
    
    public int Puntuacion
    {
        get { return puntuacion; }
    }

    public bool TareaBloqueada {
        get { return tareaBloqueada;}
    }

    public Mensaje Mensaje {
        get { return mensaje;}
    } 

    public ConfiguracionScriptable Configuracion 
    { 
        get { return configuracion;} 
    }   
  
    public RectTransform CanvasRect 
    {
        get {return canvasRect;}
    }

    public virtual string ObtenerNombreTarea()
    {
        return string.Empty; 
    }

    public virtual void Acierto(){}
    public virtual void Error(){}
    public virtual void Omision(){}
    public virtual void TiempoExcedido(){}
    
    protected virtual bool GuardarProgreso(bool partidaGanada)
    {
        return false; 
    }

    public virtual void AgregarPuntuacion(int puntuacion)
    {
        // no modificar la puntuacion si el juego ya ha terminado 
        if(TareaBloqueada)
            return; 
                    
        this.puntuacion += puntuacion;         
        if(this.puntuacion < 0 )
            this.puntuacion = 0; 
    }

    protected virtual void JuegoGanado(){
        BloquearTarea();
        FindObjectOfType<Audio>().FeedbackPartidaGanada();
        FinalizarRegistro();
        StopAllCoroutines();
        StartCoroutine(TerminarJuego(true)); 
    }

    protected void JuegoPerdido(){
       
        BloquearTarea();
        FindObjectOfType<Audio>().FeedbackPartidaPerdida();
        // reiniciamos el flag de nivel de bonus
        //Configuracion.pacienteActual.jugandoNivelDeBonus = false; 
        FinalizarRegistro();
        StopAllCoroutines();        
        StartCoroutine(TerminarJuego(false)); 
    }

    
    protected void FinalizarRegistro()
    {
        registrarEnDiario = false; 
        EscribirDiarioEnDisco();
    }

    
    protected virtual void Actualizacion()
    {
        // cada tarea ejecuta aqui su propio update()
    }

    void Update()
    {
        
            // para el debug 
            if(Input.GetKeyDown(KeyCode.W))
                JuegoGanado();
            if(Input.GetKeyDown(KeyCode.L))
                JuegoPerdido();
        
        Actualizacion();
    }

    protected virtual bool TodosLosNivelesCompletados()
    {
        return false; 
    }

    protected virtual IEnumerator TareaCompletada()
    {
        Debug.Log("Tarea finalizada completamente");
        yield return null; 
    }

    protected virtual IEnumerator ComprobarNivelBonusCompletado(bool partidaGanada)
    {
        yield return null; 
    }

    protected virtual IEnumerator TerminarJuego(bool partidaGanada){
        
        // en este punto se vuelve al menu 
        Debug.Log("Juego finalizado");

        // detener el tiempo
        Reloj reloj = FindObjectOfType<Reloj>();
        if(reloj != null)
            reloj.Detener();

        // mostrar feedback dependiendo del resultado 
        if(partidaGanada)
        {
            yield return StartCoroutine(MostrarMensaje("¡Muy bien!", 0, null, Mensaje.TipoMensaje.Exito));
        } else {
            yield return StartCoroutine(MostrarMensaje("Has perdido", 0, null, Mensaje.TipoMensaje.Fallo));
        }

        
        // feedback para cuando se completa una tarea de bonus
        // de memory, comprobamos esto ANTES del metodo
        // GuardarProgreso, porque ese metodo cambia el valor
        // de la variable paciente.jugandoNivelBobus! 
        yield return ComprobarNivelBonusCompletado(partidaGanada);


        // calcula el progreso del paciente, puntos, niveles, etc
        // y los guarda
        bool premioExtra = GuardarProgreso(partidaGanada);
        if(premioExtra)
            yield return StartCoroutine(
                MostrarMensaje("¡Nuevo record!", 0, null, Mensaje.TipoMensaje.Record)
            );

        // comprobar el final de partida
        if(TodosLosNivelesCompletados())
            yield return StartCoroutine(TareaCompletada());

        
      


        // esperar 1 seg antes de lanzar el menu
        yield return new WaitForSeconds(1f);
        AbandonarTarea();
    }

    protected void AbandonarTarea()
    {       
        SceneManager.LoadScene("Menu");
    }
    
    protected IEnumerator MostrarMensaje(
        string mensaje, int duracion = 0, Sprite image = null, 
        Mensaje.TipoMensaje tipoMensaje = Mensaje.TipoMensaje.Aviso
    )
    {
        // si la duracion no se especifica se usa la duracion
        // configurada en el scriptable 
        if(duracion == 0)
            duracion = Configuracion.duracionDeMensajes; 
        Mensaje.Mostrar(mensaje, image, tipoMensaje);        
        yield return new WaitForSeconds(duracion); 
        Mensaje.Ocultar();
    }
    
    void Awake()
    {
        Mensaje.Ocultar();
        // bloquear la tarea siempre al incio
        BloquearTarea();
        // inicio de la tarea (virtual)
        Inicio();
        // iniciar la corrutina del diario
        if(Configuracion.registrarMovimientoOcularEnDiario)
            StartCoroutine(RegistroDiario());
            
    }

    protected void BloquearTarea()
    {
        tareaBloqueada = true; 
    }

    protected void DesbloquearTarea()
    {
        tareaBloqueada = false; 
    }

    protected virtual void Inicio()
    {
        // cada tarea implementa su propio metodo Inicio()
    }   

    private string ObtenerNombreFichero()
    {
        // rutas 
        string rutaEscritorio = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        string directorioBaseDatos = "Registros";
        string codigoPaciente = Configuracion.pacienteActual.codigo; 
        string nombreTarea = ObtenerNombreTarea();        
        string rutaDirectorioRegistros = rutaEscritorio + "\\" + directorioBaseDatos + "\\paciente" +
        codigoPaciente + "\\" + nombreTarea;
        
        // crear el directorio para los datos si no existen 
        Directory.CreateDirectory(rutaDirectorioRegistros);

        // obtener fecha
        fechaRegistro = System.DateTime.Now.ToString("dddd, dd MMMM yyyy HH-mm-ss");

        // si estamos en la escritura de emergencia cambiamos el nombre del fichero 
        if(escrituraFallidaEnDisco)
            fechaRegistro += "_grabado de emergencia_";

        // devolver la ruta para el fichero
        return rutaDirectorioRegistros + "\\" + codigoPaciente + "-" + nombreTarea + "-" + 
        fechaRegistro + ".txt";

        //return rutaDirectorioRegistros + "\\" + codigoPaciente + ".txt";
    }


    private void EscrituraEmergencia()
    {
        Debug.LogError("Realizando escritura de emergencia");
        escrituraFallidaEnDisco = true; 
        EscribirDiarioEnDisco();
    }

    private void EscribirDiarioEnDisco()
    {
        Debug.Log("Escribiendo diario en disco");
        string nombreFichero = ObtenerNombreFichero();
            
        try
        {

        
            using (StreamWriter sw = new StreamWriter(nombreFichero))
            {
                // cabecera general 
                sw.WriteLine("codigo del paciente: " + configuracion.pacienteActual.codigo);            
                sw.WriteLine("fecha de registro: " + fechaRegistro);
                // cabcera propia de la tarea
                sw.WriteLine(ObtenerCabeceraTarea());
                // comienzo de datos, escribimos cada registro en una nueva linea
                for(int i=0; i<listaRegistrosOculares.Count; i++)
                {
                    // obtenemos el registro 
                    RegistroPosicionOcular registro = (RegistroPosicionOcular) listaRegistrosOculares[i];
                    sw.WriteLine(registro.RegistroFormateadoParaEscribirEnDisco());
                }
           
                // sw.Flush(); // ??
            }

        } catch (Exception e)
        {            
            if (e is IOException)
            {

            }

            if(e is IOException)
            {

            }

            Debug.LogError("Error escribiendo el fichero, imprimiendo la informacion");
            Debug.LogError(e.ToString());            
            Debug.LogError("Repetimos la operacion de guardado de datos en otro fichero diferente");   
            // repetimos la escritura, pero solo una vez mas
            if(!escrituraFallidaEnDisco)         
                EscrituraEmergencia();
        }
    }

    /*
    Metodos virtuales
    */

    protected virtual string ObtenerCabeceraTarea()
    {
        return "Cabecera por defecto";
    }

    protected virtual RegistroPosicionOcular NuevoRegistro(float tiempo, int x, int y)
    {
        return new RegistroPosicionOcular(tiempo, x, y);        
    } 
    
    // corrutina para registrar a donde mira el paciente
    // en cada momento
    private IEnumerator RegistroDiario()
    {
        float tiempoEspera = 1 / Configuracion.intervaloRegistroOcularEnHZ;
        listaRegistrosOculares = new ArrayList();
        float tiempoInicio = Time.time;

        while(registrarEnDiario)
        {           

            // obtenemos la posicion a la que se mira para registrarla
            GazePoint gazePoint = TobiiAPI.GetGazePoint();
            // calculamos el tiempo actual 
            float tiempoActual = Time.time - tiempoInicio; 

		    if (gazePoint.IsValid)
		    {
			    Vector2 posicionGaze = gazePoint.Screen;	            		
                	        
                // creamos el nuevo registro y lo introducimos en la lista
                RegistroPosicionOcular r = NuevoRegistro(
                    tiempoActual, (int) posicionGaze.x, (int) posicionGaze.y
                );
                listaRegistrosOculares.Add(r); 
                

            } else {
                
                // si el punto no es valido lo indicamos con un valor especial
                // x,y negativo
                RegistroPosicionOcular r = NuevoRegistro(
                    tiempoActual, -1, -1
                );
                listaRegistrosOculares.Add(r);
                //listaRegistrosOculares.Add( new RegistroPosicionOcular(
                    //tiempoActual, -1, -1  
                //));
            }      
            
            //tiempoInicio++;
            yield return new WaitForSeconds(tiempoEspera); 
            
		}   
        
    }


}
