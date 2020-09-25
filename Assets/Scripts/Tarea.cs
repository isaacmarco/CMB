using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;


public class Tarea : MonoBehaviour
{
    private ArrayList listaRegistrosOculares; 

    protected GazeAware gazeAware;
    [SerializeField] private ConfiguracionScriptable configuracion;     
    [SerializeField] private RectTransform canvasRect; 

    public ConfiguracionScriptable Configuracion 
    { 
        get { return configuracion;} 
    }   

    public RectTransform CanvasRect 
    {
        get {return canvasRect;}
    }


    void Awake()
    {
        // inicio de la tarea (virtual)
        Inicio();
        // iniciar la corrutina del diario
        if(Configuracion.registrarMovimientoOcularEnDiario)
            StartCoroutine(RegistroDiario());
    }

    protected virtual void Inicio()
    {
        // cada tarea implementa su propio metodo Inicio()
    }   

    private void EscribirDiarioEnDisco()
    {
        
    }
    
    // corrutina para registrar a donde mira el paciente
    // en cada momento
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
                listaRegistrosOculares.Add( new RegistroPosicionOcular(
                    (int) posicionGaze.x, 
                    (int) posicionGaze.y, 
                    tiempoActualRegistro)
                );
                

            } else {
                
                // si el punto no es valido lo indicamos con un valor especial
                // x,y negativo
                listaRegistrosOculares.Add( new RegistroPosicionOcular(
                    -1, -1, tiempoActualRegistro
                ));
            }      
            
            tiempoActualRegistro++;
            yield return new WaitForSeconds(tiempoEspera); 
            
		}   
        
    }


}
