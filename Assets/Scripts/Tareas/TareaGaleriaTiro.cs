using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TareaGaleriaTiro : Tarea
{   
    
    [Header("Marcador")]
    public Text aciertosUI;
    public Text erroresUI;
    public Text omisionesUI;
    public Text municionUI;
    public Text puntuacionUI;
    public GameObject interfazRecarga2D; 
    public GameObject avisoRecarga; 
   
    [Header("Arma")]
    public GameObject interfazRecarga;
    public GameObject arma; 
    

    // posicion de dianas y camaras para este nivel instanciado
    private BloqueTareaDisparo bloqueTareaDisparo; 

    public NivelGaleriaTiroScriptable Nivel { 
        get { return (NivelGaleriaTiroScriptable) Configuracion.nivelActual;} 
    }
   

    // estado del juego
    private int errores; 
    private int aciertos; 
    private int omisiones;   
    private float tiempoEntreCambiosCamara = 5f;
    private float tiempoDelUltimoCambioCamara; 
    private bool avisoMunicionEscuchado = false; 

    public void MostrarInterfazRecarga()
    {
        if(!avisoMunicionEscuchado)
        {
            FindObjectOfType<Audio>().FeedbackCargadorVacio();
            avisoMunicionEscuchado = true; 
        }
        interfazRecarga.SetActive(true);
        avisoRecarga.SetActive(true);
    }
    public void Recargar()
    {   
        FindObjectOfType<Audio>().FeedbackClickDefecto();
        StartCoroutine(CorrutinaRecarga());        
    }

    private bool recargando = false; 
    public bool Recargando{
        get{ return this.recargando;}
    }
    private IEnumerator CorrutinaRecarga()
    {
        
        // ocultar la interfaz de recarga
        interfazRecarga.SetActive(false);
        avisoRecarga.SetActive(false);
        // reponer municion
        FindObjectOfType<JugadorTareaGaleriaTiro>().Recargar();
        avisoMunicionEscuchado = false; // permite volver a escucharlo 
        recargando = true; 
        // animaciones de recarga
        arma.SetActive(true);
        // esperamos a que termina la animacion 
        FindObjectOfType<Audio>().FeedbackRecarga();
        yield return new WaitForSeconds(1.33f);
        
        recargando = false; 
        // ocultamos el arma
        arma.SetActive(false);
        interfazRecarga.SetActive(true);
    }

    private IEnumerator CorrutinaMunicion()
    {
        JugadorTareaGaleriaTiro jugador = FindObjectOfType<JugadorTareaGaleriaTiro>();
        while(true)
        {           
            // actualizamos aqui la puntuacion
            puntuacionUI.text = puntuacion.ToString();

            // actualizamos el marcador de municion 
            int municion = jugador.Municion;
            string ceros = string.Empty; 

            if(municion <= 0)            
            {
                municionUI.text = "000";
            } else {
                if(municion < 100)
                    ceros += "0";            
                if(municion < 10)
                    ceros += "0";
                municionUI.text = ceros + jugador.Municion.ToString();
            }
            
            

            if(!jugador.HayMunicion())
            {
                MostrarInterfazRecarga();
            }
            yield return null; 
        }
    }

    protected override void Inicio()
    {          
        // crear el escenario
        InstanciarEscenario();
        avisoRecarga.SetActive(false);
        Recargar();
        
        // comenzar la tarea        
        DesbloquearTarea();
        StartCoroutine(CorrutinaTareaDisparo());  
        StartCoroutine(CorrutinaMunicion());
    }

    private GameObject[] dianasPrimerBloque; 
    private GameObject[] dianasSegundoBloque;
    private GameObject[] dianasTercerBloque; 

    private void InstanciarEscenario()
    {
        // instanciar el prefab del nivel        
        string ruta = "Escenarios/Escenario" + Nivel.numeroDelNivel;
        GameObject escenarioPrefab = (GameObject) Resources.Load(ruta);
        GameObject escenario = (GameObject) Instantiate(escenarioPrefab);
        escenario.transform.position = Vector3.zero;
        // crear referencias despues de instanciar el escenario
        bloqueTareaDisparo = FindObjectOfType<BloqueTareaDisparo>();
        dianasPrimerBloque = GameObject.FindGameObjectsWithTag("DianaPrimerBloque");
        dianasSegundoBloque = GameObject.FindGameObjectsWithTag("DianaSegundoBloque");
        dianasTercerBloque = GameObject.FindGameObjectsWithTag("DianaTercerBloque");

    }

    private bool HayEstimulosVisibles()
    {
        ObjetivoTareaDisparo[] dianas = FindObjectsOfType<ObjetivoTareaDisparo>();
        foreach(ObjetivoTareaDisparo diana in dianas)
            if(diana.EnUso)
                return true;
        
        return false; 
    }

    private int bloqueActual = 0; 
    private bool camaraMoviendose = false; 

    private IEnumerator CorrutinaTareaDisparo()
    {
   

        // hay varios bloques de disparos, entre bloque
        // y bloque hay una animacion de camara
        while(true)
        {
            //Debug.Log("Bloque actual es " + bloqueActual);

            // movemos la camara al siguiente bloque                            
            Vector3 posicion = bloqueTareaDisparo.posicionesCamara[bloqueActual].position;
            GameObject jugador = Camera.main.gameObject; 
            float duracionAnimacionCamara = 3f; 
            iTween.MoveTo(jugador, posicion, duracionAnimacionCamara);
            //camaraMoviendose = true; 
            // esperamos a que la camara llegue al nuevo bloque
            yield return new WaitForSeconds(duracionAnimacionCamara);
            //camaraMoviendose = false; 
            // comenzamos el juego de disparos de este bloque
            
            yield return StartCoroutine(CorrutinaBloqueDeDianas());
           

            // cambiamos al siguiente bloque
            bloqueActual++;
            if(bloqueActual >= bloqueTareaDisparo.posicionesCamara.Length)
                bloqueActual = 0; 
        }

    }

    private bool haciendoMovimientosAleatorios = false; 

    private IEnumerator CorrutinaMovimientosAleatorios()
    {
        while(true)
        {

            // espera entre movimientos
            yield return new WaitForSeconds(1f);
            //if(!haciendoMovimientosAleatorios)        
            //{
                haciendoMovimientosAleatorios = true; 

                float cantidad = 0.4f;
                Vector3 posicionAgachado = new Vector3(0f, -cantidad, 0f);
                Vector3 posicionDerecha = new Vector3(cantidad, 0f, 0f);
                Vector3 posicionIzquierda = new Vector3(-cantidad, 0f, 0f);
                Vector3 posicionAdelantada = new Vector3(0f, 0f, cantidad);
                Vector3 posicionAtrasada = new Vector3(0f, 0f, -cantidad);

                Vector3[] posiciones = {
                    posicionAgachado, posicionDerecha, posicionIzquierda, 
                    posicionAdelantada, posicionAtrasada
                };

                Vector3 pR = new Vector3(
                    Random.Range(-0.1f, 0.2f), Random.Range(-0.3f, 0f), Random.Range(-0.1f, 0.2f)
                );

                GameObject jugador = Camera.main.gameObject; 
                Vector3 posicionOriginal = jugador.transform.position;
                Vector3 posicionDestino = posicionOriginal + posiciones[Random.Range(0, posiciones.Length)] + pR;

                float duracionAnimacionCamara = 2f;
                iTween.MoveTo(jugador, posicionDestino, duracionAnimacionCamara);
                yield return new WaitForSeconds(duracionAnimacionCamara);
                iTween.MoveTo(jugador, posicionOriginal, duracionAnimacionCamara);
                yield return new WaitForSeconds(duracionAnimacionCamara);

                haciendoMovimientosAleatorios = false; 

            //}
            yield return null; 
        }
    }

    private IEnumerator CorrutinaBloqueDeDianas()
    {                   
        Debug.Log("Nuevo bloque de dianas");              

        // iniciamos la corrutina de movimientos aleatorios
        // solo son para dar mas variedad al juego 
        Coroutine tareaMoverCamaraAleatoriamente = StartCoroutine(CorrutinaMovimientosAleatorios());

        // calculamos el tiempo de permanencia del bloque 
        float tiempoFinBloque = Time.time + Nivel.duracionDeCadaBloqueDeDianas;     
        // permanecemos en el bloque de dianas hasta cuando se haya
        // pasado el tiempo y haya dianas visibles           
        while(Time.time < tiempoFinBloque)
        {              
            // instanciar un nuevo estimulo despues de esperar el tiempo                       
            yield return new WaitForSeconds(Nivel.tiempoParaNuevaDiana);
            MostrarNuevoObjetivo();            
            yield return null; 
        }  

        // detenmos los movimientos de camara
        StopCoroutine(tareaMoverCamaraAleatoriamente);

        while(HayEstimulosVisibles())
        {
            // esperamos a que se oculten todas las dianas para salir del bloque
            yield return null; 
        }

       
        yield return new WaitForSeconds(1f);
        //Debug.Log("Bloque de dianas terminado");
    }
 

    private int ContadorDianasVisibles()
    {
        int contador = 0; 
        ObjetivoTareaDisparo[] dianas = FindObjectsOfType<ObjetivoTareaDisparo>();
        foreach(ObjetivoTareaDisparo diana in dianas)
            if(diana.EnUso)
                contador++;        

        return contador; 
    }

    private void MostrarNuevoObjetivo()
    {
        // mantenemos un maximo de 2 dianas visibles
        if(ContadorDianasVisibles() >= 2)
        {
            Debug.Log("No se pueden generar mas dianas por ahora");
            return; 
        }

        ArrayList bloques = new ArrayList() {
            dianasPrimerBloque, dianasSegundoBloque, dianasTercerBloque
        };
        GameObject[] dianasBloque = (GameObject[]) bloques[bloqueActual];
        if(dianasBloque == null || dianasBloque.Length == 0)
        {
            // no hay suficinetes dianas, usamos las del primer
            // bloque. Esto solo es valido si no avanzamos
            // mucho por el decorado
            dianasBloque = dianasPrimerBloque;
        }

        // seleccionar posicion al azar         
        int indice = Random.Range(0, dianasBloque.Length);
        GameObject diana = dianasBloque[indice];

        ObjetivoTareaDisparo objetivo = diana.GetComponent<ObjetivoTareaDisparo>();
        // comprobar si ya esta en uso 
        if(objetivo.EnUso)
            return; 
        
        
        switch(Nivel.dianas)
        {
            case DificultadTareaGaleriaTiro.SoloDianaObjetivo:
                // la diana es objetivio                
                objetivo.esObjetivo = true; 
                objetivo.estimulo = EstimuloTareaGaleriaTiro.DianaObjetivo;
                
            break;

            case DificultadTareaGaleriaTiro.VariosTiposDiana:
                // las dianas pueden ser no objetivos
                objetivo.esObjetivo = Random.value > Nivel.probabilidadAparicionDianaErronea;
                if(!objetivo.esObjetivo)
                    objetivo.estimulo = EstimuloTareaGaleriaTiro.DianaErroena; 
                
            break;

        }

        // comprobamos si es una gema de bonus
        if(objetivo.esObjetivo)
        {
            bool esGema = Random.value < Nivel.probabilidadAparicionGema;
            objetivo.esGema = esGema;
            if(objetivo.esGema)
                objetivo.estimulo = EstimuloTareaGaleriaTiro.Gema;
        }

        objetivo.Mostrar();

        FindObjectOfType<Audio>().FeedbackAparicionEstimulo();
        
    }   

    
    // se comprueba el estado de la partida despues de un error
    // o una omision 
    private void ComprobarOmisionError()
    {
        if(errores + omisiones >= Nivel.omisionesOErroresParaPerder)
            JuegoPerdido();
    }

    
    protected override bool TodosLosNivelesCompletados()
    {
        int numeroNiveles = 15;             
        return Configuracion.pacienteActual.ultimoNivelDesbloqueadoTareaGaleriaTiro >= numeroNiveles;
    }

    protected override bool GuardarProgreso(bool partidaGanada)
    {        
        Debug.Log("Guardando progreso de partida de galeria de tiro");

        if(partidaGanada)
        {
            // guardar la puntuacion
            if(puntuacion > 0)
                Configuracion.pacienteActual.puntuacionTareaGaleriaTiro += puntuacion; 

            // progresar
            Configuracion.pacienteActual.ultimoNivelDesbloqueadoTareaGaleriaTiro++;
            
            // comprobar si hemos terminado todos los niveles            
            if(TodosLosNivelesCompletados())
            {
                Debug.Log("Todos los niveles de la tarea completos");
                // El juego se ha terminado, no hay mas niveles
                int numeroNiveles = 15; 
                Configuracion.pacienteActual.ultimoNivelDesbloqueadoTareaGaleriaTiro = numeroNiveles; 
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
    public void Bonus()
    {
        // cuando se alcanza una gema no voy a contabilizar
        // como un acierto, los aciertos seran las dianas
        // azules
        //aciertos++;
        //aciertosUI.text = "Aciertos " + aciertos.ToString();
        FindObjectOfType<Audio>().FeedbackAciertoBonus();        
        AgregarPuntuacion(Configuracion.puntuacionGemaGaleriaTiro);
    }
  
    public override void Acierto()
    {
        aciertos++;
        FindObjectOfType<Audio>().FeedbackAcierto();
        aciertosUI.text = "Aciertos " + aciertos.ToString();
        AgregarPuntuacion(Configuracion.puntuacionAciertoGaleriaTiro);
        if(aciertos >= Nivel.aciertosParaSuperarElNivel)
            JuegoGanado();
       
    }

    public override void Error()
    {
        errores++;
        FindObjectOfType<Audio>().FeedbackError();
        erroresUI.text = "Errores " + errores.ToString();
        AgregarPuntuacion(-Configuracion.penalizacionErrorGaleriaTiro);
        ComprobarOmisionError();
        
    } 

    public override void Omision()
    {
        omisiones++;
        FindObjectOfType<Audio>().FeedbackOmision();
        omisionesUI.text = "Omisiones " + omisiones.ToString();
        AgregarPuntuacion(-Configuracion.penalizacionOmisionGaleriaTiro);       
        ComprobarOmisionError();
    }

     
    public override string ObtenerNombreTarea()
    {
        return "Tarea galeria de tiro";
    }
    
    protected override string ObtenerCabeceraTarea()
    {
        string cabecera = string.Empty;
        string posicionesMatriz = string.Empty; 

        // datos de la tarea
        cabecera += "Tarea de galeria de tiro\n";
        cabecera += "Nivel actual: " + Configuracion.nivelActual.numeroDelNivel + "\n";
        // resultados
        cabecera += "Aciertos: " + aciertos + "\n";
        cabecera += "Errores: " + errores + "\n";
        cabecera += "Omision: " + omisiones + "\n";
        cabecera += "Puntos:" + puntuacion + "\n";

        // posicion de la recarga        
        Vector3 posicionInterfaz = interfazRecarga.transform.position;       
        // transformar las coordeandas
        Vector2 posicionViewport = Camera.main.WorldToViewportPoint(posicionInterfaz);
        Vector2 posicionEnPantalla = new Vector2(
            ((posicionViewport.x * CanvasRect.sizeDelta.x)-(CanvasRect.sizeDelta.x * 0.5f)),
            ((posicionViewport.y * CanvasRect.sizeDelta.y)-(CanvasRect.sizeDelta.y * 0.5f))
        );
        cabecera += "Posicion interfaz de recarga: " + posicionEnPantalla.x + ", " + posicionEnPantalla.y +  "\n";

        cabecera += "Leyenda: tiempo; vision x ; vision y ; tipo estimulo A; tipo estimulo B; AX; AY; BX; BY; recargando?; municion";
        return cabecera;
    }

    protected override RegistroPosicionOcular NuevoRegistro(float tiempo, int x, int y)
    {
        // obtenemos la municion actual
        int municion = -1;
        JugadorTareaGaleriaTiro jugador = FindObjectOfType<JugadorTareaGaleriaTiro>();
        if(jugador != null)
            municion = jugador.Municion; 
        
        // comprobar los estimulos activos
        ObjetivoTareaDisparo[] objetivos = FindObjectsOfType<ObjetivoTareaDisparo>();
        EstimuloTareaGaleriaTiro estimuloA = EstimuloTareaGaleriaTiro.Ninguno;
        EstimuloTareaGaleriaTiro estimuloB = EstimuloTareaGaleriaTiro.Ninguno;
        int AX = -1;
        int AY = -1; 
        int BX = -1;
        int BY = -1; 
        // solo puede haber dos dianas activas en 
        //cada momento como maximo 
        foreach(ObjetivoTareaDisparo objetivo in objetivos)
        {
            // si el objetivo es visible
            if(objetivo.EnUso)
            {
                // si para el primer estimulo no hemos asignado 
                // le asignamos este objetivo
                if(estimuloA == EstimuloTareaGaleriaTiro.Ninguno)
                {
                    estimuloA = objetivo.estimulo; 
                    /*
                        TODO: CALCULAR POSICION EN EL ESPACIO 2d
                    */
                    continue; 
                }
                // si para el segundo estimulo no hemos asignado
                // le asignamos este objetivo 
                if(estimuloB == EstimuloTareaGaleriaTiro.Ninguno)
                {
                    estimuloB = objetivo.estimulo;
                     /*
                        TODO: CALCULAR POSICION EN EL ESPACIO 2d
                    */
                    continue; 
                }
            }
        }
               
               
            
        // devolvemos el nuevo registro
        return new RegistroPosicionOcularTareaGaleriaTiro(tiempo, x, y, estimuloA, estimuloB, 
        AX, AY, BX, BY, recargando, municion
        );
        
    } 


}
