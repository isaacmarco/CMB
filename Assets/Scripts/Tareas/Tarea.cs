using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
using System.IO;

public class Tarea : MonoBehaviour
{
    private ArrayList listaRegistrosOculares; 

    protected GazeAware gazeAware;
    [SerializeField] private ConfiguracionScriptable configuracion;     
    [SerializeField] private RectTransform canvasRect; 
    [SerializeField] private Mensaje mensaje; 

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

    protected IEnumerator MostrarMensaje(string mensaje, int duracion = 0)
    {
        // si la duracion no se especifica se usa la duracion
        // configurada en el scriptable 
        if(duracion == 0)
            duracion = Configuracion.duracionDeMensajes; 
        Mensaje.Mostrar(mensaje);        
        yield return new WaitForSeconds(duracion); 
        Mensaje.Ocultar();
    }
    
    void Awake()
    {
        Mensaje.Ocultar();
        
        // inicio de la tarea (virtual)
        Inicio();
        // iniciar la corrutina del diario
        if(Configuracion.registrarMovimientoOcularEnDiario)
            StartCoroutine(RegistroDiario());
    }

    private IEnumerator TestRegistro()
    {
        Debug.LogError("Activo test de registro a disco");
        yield return new WaitForSeconds(5f);
        EscribirDiarioEnDisco();        
        yield return null; 
    }

    protected virtual void Inicio()
    {
        // cada tarea implementa su propio metodo Inicio()
    }   
    
    private void EscribirDiarioEnDisco()
    {
        Debug.Log("Escribiendo diario en disco");

        string nombreFichero = @"diario.txt";

        using (StreamWriter sw = new StreamWriter(nombreFichero))
        {
            // cabecera
            sw.WriteLine("codigo del paciente");
            sw.WriteLine("nombre de la tarea"); 
            sw.WriteLine("datos de fechas");

            sw.WriteLine(ObtenerCabeceraTarea());


            // comienzo de datos, escribimos cada registro en una nueva linea
            for(int i=0; i<listaRegistrosOculares.Count; i++)
            {
                // obtenemos el registro 
                RegistroPosicionOcular registro = (RegistroPosicionOcular) listaRegistrosOculares[i];
                sw.WriteLine(registro.RegistroFormateadoParaEscribirEnDisco());
            }
           
           
            sw.Flush();
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

        StartCoroutine(TestRegistro());

        while(true)
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
