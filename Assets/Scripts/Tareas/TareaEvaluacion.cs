using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TareaEvaluacion : Tarea
{
    [Header("Estimulos")]
    public GameObject estimulo;
    public GameObject estimuloFijacion;
    private ComunicacionPuertoSerie puertoSerie; 
    private PuntoVision puntoVision; 
    
    public NivelEvaluacion Nivel { 
        get { return (NivelEvaluacion) Configuracion.nivelActual;} 
    }

    // cada bloque tiene su lista de coeficientes
    // <int (indice bloque), double[] (lista de coeficienes)>
    ArrayList listaCoeficientes = new ArrayList{ 
        new double[] { 3.2, 2.8, 0.1, 0.3, 3.4, 1.1, 2.3, 2.6, 0.5,-3.5,-3.7, 4.4, 2.3},
        new double[] {-0.7, 4.4, 3.8,  -4, 0.6, 4.8, 0.8, 0.8, 1.3,  -3,-0.1,-4.1,-4.3},
        new double[] { 3.8,-1.7, 0.9,-3.8, 4.2, 0.3,-2.5, 4.2,-4.6,-4.5, 3.5,-3.9, 3.5},
        new double[] {-1.1, 1.7,-3.4,-3.6, 1.9,-0.2, 1.6, 0.8, 1.1, 1.3, 3.7,-3.5, 4.3},
        new double[] { 2.6,-0.6,-2.9, 1.8, 0.8,   3,-4.1,-4.7,-1.3,-2.1,-2.3,-3.3, 4.7},
        new double[] {  -1, 3.3,-0.9,   0, 3.1,-2.7, 1.2,-3.7,-4.4, 0.4,-2.9, 1.2, 3.5},
        new double[] {   3, 2.6, 2.4,  -3, 3.7,   0, 1.6, 3.6,-0.1, 1.9, 0.6, 0.7, 2.8},
        new double[] { 2.5,-3.3, 3.2,   0, 4.8, 3.9, 2.3,-0.2,  -3,   0, 1.4,-4.4, 0.1},
        new double[] {-1.2, 3.5, 2.8,-3.5,-4.9, 0.7, 3.8, 3.4,-3.7, 0.4,-0.8, 4.2,-3.2},
        new double[] {-2.8, 4.8,-1.8,-4.4, 3.6, 3.4, 4.7,-2.8,-2.9,-0.5,-2.9, 2.2,  -1}
    };

    /*
        TODO: TODAVIA NO SE ESTAN USANDO LOS COEFICIENTES
        PARA EL OTRO BRAZO
    */
    double[,] matrizCoeficientesBrazoIzquierdo = new double[10,13] {
        {-3.6, 0.6, 4.8,-3.2,   4, 3.9,-0.4, 1.3, 2.4, 1.9, 0.5, 1.8, 2.4},
        {-4.6, 3.5, 0.4,-1.4,-3.9,-4.2,-3.9,  -4,-4.8, 0.6,-0.1,-3.6,-2.6},
        { 4.3,-1.5,   2,-4.3, 2.4,-2.5, 4.9,-4.1,-4.4,  -1, 3.8, 2.2, 2.3},
        {-1.9,-0.5, 4.9, 0.2, 2.3,-4.4,-1.6, 2.7, 1.6,-4.3, 2.9,-3.8, 4.6},
        {  -2,-4.4,-2.1,-1.6, 0.6,-0.6,  -2,   4,   1, 2.7, 2.3,-3.7, 3.6},
        {-1.6,-3.2,-0.8,-3.2,-3.1,-4.8,-4.3, 0.3, 0.3,-1.6,-4.4, 1.4,-4.1},
        {-0.3, 1.6,-0.3,-2.9,   1, 3.9,  -2,-3.8, 2.3, 1.1,-4.2,-1.7,-1.3},
        { 1.5,-1.7, 2.6,   4,  -2,  -3,-4.4, 3.2,   2, 2.4,  -4, 1.5,-1.3},
        {-4.7, 3.9, 3.1, 1.7,-3.6,  -4, 0.1,-1.6, 2.8,-3.9, 2.9, 2.4, 1.8},
        { 3.4,-3.7,-3.9,-0.3,-2.8,-1.9, 2.6,  -2,-2.1,-3.6, 4.3, 0.8,   1}
    };




    private float tasaRefresco = 0.033f; // 0.016f; // 30Hz 0.033f; // 30Hz 0.016f; // 60HZ
    // contador de ticks
    private int contadorTicks = 0;       
  
    // amplitud del movimiento
    private int amplitud = 9; 
    private float tiempoInicioTarea;
    // estado actual de la tarea 
    private bool mostrandoEstimuloFijacion; 
    private int numeroBloqueDeEvaluacion; 

    protected override void Actualizacion()
    {
        // cada tarea ejecuta aqui su propio update()
        if(Input.GetKeyDown(KeyCode.Escape))
            if(Configuracion.condicionTareaEvaluaion == CondicionTareaEvaluacion.Entrenamiento)
                TerminarEntrenamiento();
    }
    private void FinalizarTareaEvaluacion()
    {
        // detenmos las corrutinas, registramos en disco los
        // datos y volvemos al menu de la aplicacion 
        StopAllCoroutines();
        FinalizarRegistro();
        AbandonarTarea();
    }

    private void TerminarEntrenamiento()
    {
        Debug.Log("Entrenamiento terminado");
        // no se registra nada, se abandona directamente la tarea 
        StopAllCoroutines();
        AbandonarTarea();
    }

    protected override void Inicio()
    {          

        // obtener componente de puerto series
        puertoSerie = GetComponent<ComunicacionPuertoSerie>();
        // referencia al punto vision 
        puntoVision = FindObjectOfType<PuntoVision>();

        // configurar la tarea 
        float gris = 0.7f; 
        Color fondoNegro = new Color(0, 0, 0, 1);
        Color fondoGris = new Color(gris, gris, gris, 1);
        
        Camera.main.backgroundColor = Configuracion.usarFondoGrisTareaEvaluacion ? 
            fondoGris : fondoNegro; 
        StartCoroutine(CorrutinaEvaluacion());
    }
    
    


    private IEnumerator CorrutinaEvaluacion()
    {
        Debug.Log("Inicio de la evaluacion");
        OcultarEstimulos();

       
        // espera por el arduino
         yield return new WaitForSeconds(3f);

        tiempoInicioTarea = Time.time; 

        // enviar msg comienzo tarea
        //puertoSerie.EnviarPorPuertoSerie(ComunicacionPuertoSerie.MensajesPuertoSerie.Comienzo);
        puertoSerie.EnviarPorPuertoSerie("C");
        yield return new WaitForSeconds(1f);


        for (int i=0; i<Configuracion.numberoDeBloquesDeEvaluacion; i++)
        {
           
            numeroBloqueDeEvaluacion = i + 1;
            Debug.Log("Nuevo bloque");
            
            contadorTicks = 0; 

            // para cada bloque
            float tiempoInicioBloque = Time.time;
            // mostrar fijacion
            MostrarEstimuloFijacion();

            // enviar msg estimulo fijacion 
            //puertoSerie.EnviarPorPuertoSerie(ComunicacionPuertoSerie.MensajesPuertoSerie.Fijacion);
            puertoSerie.EnviarPorPuertoSerie("F");

            // esperar y ocultar el estimulo
            yield return new WaitForSeconds(Configuracion.duracionEstimuloFijacionEvaluacion);
            OcultarEstimuloFijacion();
            
            // centrar el estimulo 
            CentrarEstimulo();    

            // enviar msg de comienzo del seguimiento
            //puertoSerie.EnviarPorPuertoSerie(ComunicacionPuertoSerie.MensajesPuertoSerie.Seguimiento);
            puertoSerie.EnviarPorPuertoSerie("S");

            // obtener coeficientes del bloque actual            
            double[] coeficientesDelBloque = (double[]) listaCoeficientes[i];

            // bucle de tarea                
            while(Time.time < tiempoInicioBloque + Configuracion.duracionDelBloqueDeEvaluacion)
            {
                Tick(coeficientesDelBloque);
                yield return new WaitForSeconds(tasaRefresco);
            }
         
        }

        // enviar msg de final de la tara
        //puertoSerie.EnviarPorPuertoSerie(ComunicacionPuertoSerie.MensajesPuertoSerie.Final);
        puertoSerie.EnviarPorPuertoSerie("E");

        // cerrar el puerto
        puertoSerie.CerrarPuertoSerie();
        
        Debug.Log("Final de la evaluacion");
        OcultarEstimulos();
        FinalizarTareaEvaluacion();
        
        
        
    }

    private void CentrarEstimulo()
    {
        Debug.Log("Centrando estimulo");
        // obtener el centro
        int centroX = Screen.width / 2;
        int centroY = Screen.height / 2;   
        estimulo.GetComponent<RectTransform>().anchoredPosition = new Vector2
            (
                centroX, centroY
        );   
    }


    private void Tick(double[] coeficientesDelBloque)
    {    
                   
          
        float VI = contadorTicks * 2.14f * Mathf.PI / (amplitud * 30); 
          
        // obtener el centro
        int centroX = Screen.width / 2;
        int centroY = Screen.height / 2;

        int posicionX =  (int) (amplitud *  
            (
                coeficientesDelBloque[00] + coeficientesDelBloque[01] * Mathf.Sin(VI) + 
                coeficientesDelBloque[02] * Mathf.Cos(VI) + 
                coeficientesDelBloque[03] * Mathf.Sin(2 * VI) + 
                coeficientesDelBloque[04] * Mathf.Cos(2 * VI) + 
                coeficientesDelBloque[05] * Mathf.Sin(3 * VI) + 
                coeficientesDelBloque[06] * Mathf.Cos(3 * VI) + 
                coeficientesDelBloque[07] * Mathf.Sin(4 * VI) + 
                coeficientesDelBloque[08] * Mathf.Cos(4 * VI) + 
                coeficientesDelBloque[09] * Mathf.Sin(5 * VI) + 
                coeficientesDelBloque[10] * Mathf.Cos(5 * VI) + 
                coeficientesDelBloque[11] * Mathf.Sin(6 * VI) + 
                coeficientesDelBloque[12] * Mathf.Cos(6 * VI)
            )
        );

        /*
        int posicionX =  (int) (amplitud *  
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
        */
         
        estimulo.GetComponent<RectTransform>().anchoredPosition = new Vector2
        (
            posicionX + centroX - 100, centroY // - 100 unidades para centrar el estimulo
        );

        contadorTicks++;
       
    }

    private void MostrarEstimuloFijacion()
    {
        puntoVision.Ocultar();
        mostrandoEstimuloFijacion = true; 
        estimulo.SetActive(false);
        estimuloFijacion.SetActive(true);
    }

    private void OcultarEstimuloFijacion()
    {
        puntoVision.Mostrar();
        mostrandoEstimuloFijacion = false; 
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
        cabecera += "Numero de bloques de evaluacion: " + Configuracion.numberoDeBloquesDeEvaluacion + "\n";
        cabecera += "Leyenda: tiempo; estimulo fijacion visible; numero bloque actual; mirando x; mirando y; estimulo objetivo x; estimulo objetivo y";
        return cabecera;
    }
    
    protected override RegistroPosicionOcular NuevoRegistro(float tiempo, int x, int y)
    {
        // obtener posicion del estimulo
        Vector2 posicionEstimulo = estimulo.GetComponent<RectTransform>().anchoredPosition;        
        return new RegistroPosicionOcularTareaEvaluacion(
            tiempo, x, y, (int) posicionEstimulo.x, (int) posicionEstimulo.y, numeroBloqueDeEvaluacion,
            mostrandoEstimuloFijacion
        );
    } 
    
    
}
