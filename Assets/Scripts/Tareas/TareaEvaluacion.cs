using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TareaEvaluacion : Tarea
{
    [Header("Estimulos")]
    public GameObject estimulo;
    public GameObject estimuloFijacion;
    
    public NivelEvaluacion Nivel { 
        get { return (NivelEvaluacion) Configuracion.nivelActual;} 
    }

    private float intervaloActualizacion = 0.016f; // 60HZ
    // contador de ticks
    private int nTimerTicks = 0;     
    // constantes arbitrarias de cristian
    private double b0 = 2;
    private double a1 = -4;
    private double b1 = 3;
    private double a2 = -4.9;
    private double b2 = -3.6;
    private double a3 = 3.9;
    private double b3 = 4.5;
    private double a4 = 0;
    private double b4 = 1;
    private double a5 = -3.8;
    private double b5 = -0.5;
    private double a6 = 1; 
    private double b6 = 2.5;
    // amplitud del movimiento
    private int amplitud = 9; 
    private float tiempoInicioTarea;

    protected override void Inicio()
    {           
        StartCoroutine(CorrutinaEvaluacion());
    }
    
    private IEnumerator CorrutinaEvaluacion()
    {
        Debug.Log("Inicio de la evaluacion");
        OcultarEstimulos();

        tiempoInicioTarea = Time.time; 

        for(int i=0; i<Configuracion.numberoDeBloquesDeEvaluacion; i++)
        {
            Debug.Log("Nuevo bloque");
            // para cada bloque
            float tiempoInicioBloque = Time.time;
            // mostrar fijacion
            MostrarEstimuloFijacion();
            // esperar
            yield return new WaitForSeconds(1f);
            OcultarEstimuloFijacion();
            // bucle de tarea            
            while(Time.time < tiempoInicioBloque + Configuracion.duracionDelBloqueDeEvaluacion)
            {
                MoverEstimulo();
                yield return new WaitForSeconds(intervaloActualizacion);
            }
         
        }

        Debug.Log("Final de la evaluacion");

        OcultarEstimulos();
    }

    
    private void MoverEstimulo()
    {
        /*
        TODO: REINICIAR EL TIEMPO DE LA ANIMACION
        DEL ESTIMULO CADA VEZ QUE SE CAMBIA
        DE BLOQUE
        */

        //while(true)
        //{
            
            nTimerTicks++;
            float VI = Time.time;

            // obtener el centro
            int centroX = Screen.width / 2;
            int centroY = Screen.height / 2;

            int xTarget =  (int) (amplitud *  // amplitud es 9 por defecto
                (
                b0 + a1 * Mathf.Sin(VI) + 
                b1 * Mathf.Cos(VI) + 
                a2 * Mathf.Sin(2 * VI) + 
                b2 * Mathf.Cos(2 * VI) + 
                a3 * Mathf.Sin(3 * VI) + 
                b3 * Mathf.Cos(3 * VI) + 
                a4 * Mathf.Sin(4 * VI) + 
                b4 * Mathf.Cos(4 * VI) + 
                a5 * Mathf.Sin(5 * VI) + 
                b5 * Mathf.Cos(5 * VI) + 
                a6 * Mathf.Sin(6 * VI) + 
                b6 * Mathf.Cos(6 * VI)
                )
            );
         
            estimulo.GetComponent<RectTransform>().anchoredPosition = new Vector2
            (
                xTarget + centroX, centroY
            );

            //yield return new WaitForSeconds(intervaloActualizacion);
        //}
    }

    private void MostrarEstimuloFijacion()
    {
        estimulo.SetActive(false);
        estimuloFijacion.SetActive(true);
    }

    private void OcultarEstimuloFijacion()
    {
        estimulo.SetActive(true);
        estimuloFijacion.SetActive(false);
    }

    private void OcultarEstimulos()
    {
        estimulo.SetActive(false);
        estimuloFijacion.SetActive(false);
    }

    
    public override string ObtenerNombreTarea()
    {
        return "Tarea evaluacion";
    }
    
    protected override string ObtenerCabeceraTarea()
    {
        string cabecera = string.Empty;
        // datos de la tarea
        cabecera += "Tarea de evaluacion\n";
        cabecera += "Leyenda: tiempo; mirando x; mirando y; objetivo x; objetivo y";
        return cabecera;
    }
    
    protected override RegistroPosicionOcular NuevoRegistro(float tiempo, int x, int y)
    {
        // obtener posicion del estimulo
        Vector2 posicionEstimulo = estimulo.GetComponent<RectTransform>().anchoredPosition;        
        return new RegistroPosicionOcularTareaEvaluacion(
            tiempo, x, y, (int) posicionEstimulo.x, (int) posicionEstimulo.y
        );
    } 
    
    
}
