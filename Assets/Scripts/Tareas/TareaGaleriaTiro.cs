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
    public Image tiempoUI;
    [Header("Recarga")]
    public GameObject avisoRecarga; 
    public GameObject interfaz2D; 
    public GameObject contadorMunicion; 
   
    [Header("Arma")]
    public GameObject interfazRecarga;
    public GameObject arma; 
    [Header("Dianas")]
    public DianaEntrenamiento dianaA;
    public DianaEntrenamiento dianaB;
    public DianaEntrenamiento dianaC; 
    [Header("Bomba")]
    public GameObject prefabBomba; 
    [Header("VFX")]
    public GameObject particulasDianaCorrecta; 
    public GameObject particulasDianaIncorrecta; 
    public GameObject particulasGema; 
    public GameObject puntuacionVFX; 


    //public Transform[] padresBloques; 
    public Transform padreBloquesDianas; 
    
    public GameObject[] dianas; 

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
    private bool bombaColocada = false; 
    private bool recargando = false; 
    public bool Recargando{
        get{ return this.recargando;}
    }
    private GameObject[] posiciones; 
    public GameObject dom; 
    public GameObject navegacion;
    
    private GameObject[] puntosAparicionDianasPrimerBloque; 
    private GameObject[] puntosAparicionDianasSegundoBloque;
    private GameObject[] puntosAparicionDianasTercerBloque; 
    private ArrayList listaBloquesDianas; // <Transform[]>
    int bloqueActual = 0; 
    private bool haciendoMovimientosAleatorios = false; 
    private Coroutine corrutinaTiempo; 
    
    public void InstanciarVFXDestruccion(Vector3 posicionDiana, bool esGema, bool dianaCorrecta)
    {
        
        
        GameObject prefabParticulas = particulasDianaCorrecta; 
        if(!dianaCorrecta)
            prefabParticulas = particulasDianaIncorrecta; 
        if( esGema)
            prefabParticulas = particulasGema; 
        
        GameObject vfx = (GameObject) Instantiate(prefabParticulas);
        vfx.transform.position = posicionDiana;
        float factoEscala = 0.1f;  
        vfx.transform.localScale = new Vector3(
            factoEscala, factoEscala, factoEscala
        );

        // instanciar puntuacion 
        GameObject points = (GameObject) Instantiate(puntuacionVFX);
        points.transform.position = posicionDiana; 
                
        string mensaje = string.Empty;
        if(dianaCorrecta)
            mensaje = "+" + Configuracion.puntuacionAciertoGaleriaTiro;
        if(!dianaCorrecta)
            mensaje = "-" + Configuracion.penalizacionErrorGaleriaTiro;
        if(esGema)
            mensaje = "+" + Configuracion.puntuacionGemaGaleriaTiro; 
        
        bool acierto = dianaCorrecta || esGema; 
        points.GetComponent<PuntosGaleriaTiroVFX>().Mostrar(acierto);
        points.GetComponent<TextMeshPro>().text = mensaje;

    }

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
        //arma.SetActive(true);
        arma.GetComponent<Animator>().SetTrigger("Recargar");
        // esperamos a que termina la animacion 
        FindObjectOfType<Audio>().FeedbackRecarga();
        yield return new WaitForSeconds(1.33f);
        
        recargando = false; 
        // ocultamos el arma
        //arma.SetActive(false);
        interfazRecarga.SetActive(true);
    }

    private IEnumerator CorrutinaMunicion()
    {
        JugadorTareaGaleriaTiro jugador = FindObjectOfType<JugadorTareaGaleriaTiro>();
        while(true)
        {           
           

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

    
    private void GenerarPuntosNavegacion()
    {        

        // instanciamos el trayecto en el objeto navegacion 
        string rutaPrefab = "Escenarios/NivelGaleriaTiro" + Nivel.rutaPorLaCiudad; // Nivel.numeroDelNivel;
        GameObject _rutaPrefab = (GameObject) Resources.Load(rutaPrefab);        
        GameObject rutaJugador = (GameObject) Instantiate(_rutaPrefab, navegacion.transform);
        rutaJugador.transform.position = Vector3.zero;
        
        // generamos los puntos 
        Transform ruta = navegacion.transform.GetChild(0);

        int numeroPosiciones = ruta.transform.childCount; // navegacion.transform.childCount; 
        posiciones = new GameObject[numeroPosiciones];
        for(int i=0; i<numeroPosiciones; i++)
        {
            // obtenemos el hijo y anotamos su posicion 
            Transform hijo = ruta.transform.GetChild(i); // navegacion.transform.GetChild(i);
            hijo.name = "PuntoNavegacion " + i;             
            hijo.gameObject.GetComponent<Renderer>().enabled = false; 
            if(hijo.childCount> 0)
                hijo.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false; 
            posiciones[i] = hijo.gameObject; 
        }
        Debug.Log(posiciones.Length + " posiciones de navegacion");
    }

    private bool noContabilizarTiempo = false; 

    private void GenerarBloquesDianas()
    {
    }
    
    protected override void Inicio()
    {          
        

        GenerarPuntosNavegacion();
        GenerarBloquesDianas();

        interfaz2D.SetActive(false); 

        // si hay municion en este nivel entonces
        // mostramos la UI y lanzamos la corrutina de la mecanica
        // de recarga 

        avisoRecarga.SetActive(false);

        if(Nivel.esNecesarioRecargar)
        {   
            Recargar();            
            StartCoroutine(CorrutinaMunicion());
        } else {
            // ocultamos la UI de recarga
            interfazRecarga.SetActive(false);
            contadorMunicion.SetActive(false);
        }
        
        // comenzar la tarea        
        DesbloquearTarea();
        StartCoroutine(CorrutinaTareaDisparo());  
        StartCoroutine(CorrutinaMirar());
    }

    

    private IEnumerator CorrutinaMirar()
    {
        while(true)
        {
            if(dom!=null)
            {
            
        Vector3 v = new Vector3(0, 0.7f, 0);
        GameObject jugador = Camera.main.gameObject; 
        jugador.transform.rotation = Quaternion.Slerp(
            jugador.transform.rotation, (
                Quaternion.LookRotation(
                    (dom.transform.position + v) - jugador.transform.position)), Time.deltaTime * 10f);

            }
            yield return null; 
        }
    }
    private void InstanciarEscenario()
    {
        // instanciar el prefab del nivel        
        string ruta = "Escenarios/Escenario" + Nivel.numeroDelNivel;
        GameObject escenarioPrefab = (GameObject) Resources.Load(ruta);
        GameObject escenario = (GameObject) Instantiate(escenarioPrefab);
        escenario.transform.position = Vector3.zero;
        // crear referencias despues de instanciar el escenario
        bloqueTareaDisparo = FindObjectOfType<BloqueTareaDisparo>();
        puntosAparicionDianasPrimerBloque = GameObject.FindGameObjectsWithTag("DianaPrimerBloque");
        puntosAparicionDianasSegundoBloque = GameObject.FindGameObjectsWithTag("DianaSegundoBloque");
        puntosAparicionDianasTercerBloque = GameObject.FindGameObjectsWithTag("DianaTercerBloque");

    }

    private bool HayEstimulosVisibles()
    {        
        return dianaA.EnUso || dianaB.EnUso || dianaC.EnUso; 
    }

    protected override void Actualizacion()
    {
        // actualizamos aqui la puntuacion
        puntuacionUI.text = puntuacion.ToString();
    }
    private IEnumerator CorrutinaTareaDisparo()
    {
        
        // posicionamos la camar en el primer punto 
        Vector3 _posicion = posiciones[bloqueActual].transform.position + Vector3.up;
        Camera.main.gameObject.transform.position = _posicion; 
        // miramos hacia el segundo punto
        Vector3 _posicionSegundoPunto = posiciones[bloqueActual+1].transform.position + Vector3.up; 
        Camera.main.gameObject.transform.LookAt(_posicionSegundoPunto);

        int nivel = Configuracion.nivelActual.numeroDelNivel;

        // mensaje por defecto
        int numeroDianas = Nivel.aciertosParaSuperarElNivel; 
        yield return StartCoroutine(MostrarMensaje(
            "Rompe " + numeroDianas + " dianas azules",0, null, Mensaje.TipoMensaje.DianaAzul));

        // mensajes segun el nivel 
        if(nivel == 0)
            yield return StartCoroutine(MostrarMensaje(
            "¡Hazlo antes de que se acabe el tiempo!",0, null, Mensaje.TipoMensaje.Tiempo));
       
        if(nivel == 1)
            yield return StartCoroutine(MostrarMensaje(
            "¡No te dejes ninguna diana azul!",0, null, Mensaje.TipoMensaje.DianaAzul));

        if(nivel == 2)
            yield return StartCoroutine(MostrarMensaje(
            "¡No rompas las dianas rojas!",0, null, Mensaje.TipoMensaje.DianaRoja));

        if(nivel == 3)         
        {
            yield return StartCoroutine(MostrarMensaje(
            "Ahora tu munición se gasta",0, null, Mensaje.TipoMensaje.Municion));

            yield return StartCoroutine(MostrarMensaje(
            "Mira al dibujo para recargar",0, null, Mensaje.TipoMensaje.Municion));
        }

        if(nivel == 4)
            yield return StartCoroutine(MostrarMensaje(
            "¡Las gemas te darán puntuación extra!",0, null, Mensaje.TipoMensaje.Bonus));

        if(nivel == 5)
            yield return StartCoroutine(MostrarMensaje(
            "¡Vamos a mover las dianas!",0, null, Mensaje.TipoMensaje.DianaAzul));
        
        if(nivel == 6 || nivel == 8 || nivel == 11)
            yield return StartCoroutine(MostrarMensaje(
            "¡Ahora tienes menos munición!",0, null, Mensaje.TipoMensaje.Municion));

        
        // mostrar toda la interfaz despues de los mensajes 
        interfaz2D.SetActive(true); 

        // iniciamos la tarea de ui del tiempo
        corrutinaTiempo = StartCoroutine(CorrutinaTiempo());

        // hay varios bloques de disparos, entre bloque
        // y bloque hay una animacion de camara
        while(true)
        {
            
         

            // movemos la camara al siguiente bloque                            
            Vector3 posicion = posiciones[bloqueActual].transform.position + Vector3.up;
            GameObject jugador = Camera.main.gameObject; 
            float duracionAnimacionCamara = 3f; 
            
            
            

            // orientamos la camara 
            GameObject dummyOrientacion = posiciones[bloqueActual].transform.GetChild(0).gameObject;
            dom = dummyOrientacion; 
            
          
            iTween.MoveTo(jugador, 
                iTween.Hash(
                "x", posicion.x, 
                "y", posicion.y, 
                "z", posicion.z,
                "easetype", iTween.EaseType.linear, 
                "time", duracionAnimacionCamara
                )
            );
        
            
            // esperamos a que la camara llegue al nuevo bloque
            yield return new WaitForSeconds(duracionAnimacionCamara);

           
            // comenzamos el juego de disparos de este bloque            
            yield return StartCoroutine(CorrutinaBloqueDeDianas());      

            // cambiamos al siguiente bloque
            bloqueActual++;
            if(bloqueActual >= posiciones.Length)
            {
                noContabilizarTiempo = true; 
                // aqui abandonamos la corutina pero la partida debe perderse 
                JuegoPerdido();                
                bloqueActual = 0; 
                yield break;
            }
            yield return null; 

        }

    }

    

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

   

    private void NuevaBomba()
    {
    }

    private IEnumerator CorrutinaTiempo()
    {
        while(true)
        {                           
            float progreso = 1 - ((float)bloqueActual / (float)(posiciones.Length));            
            tiempoUI.fillAmount =  progreso;  
            if(noContabilizarTiempo)           
                tiempoUI.fillAmount = 0; 

            //Debug.Log(bloqueActual + "/" + (posiciones.Length) + "-> " + progreso);
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
 

    private DianaEntrenamiento ObtenerDianaLibre()
    {
        if(!dianaA.EnUso)
            return dianaA; 
        if(!dianaB.EnUso)
            return dianaB; 
        if(!dianaC.EnUso)
            return dianaC; 
        return  null; 
    }
    private void MostrarNuevoObjetivo()
    {
        
        if(tareaBloqueada)
            return; 

        // si ya se estan mostrando las dos dianas no generamos mas 
        if(dianaA.EnUso && dianaB.EnUso && dianaC.EnUso)
            return; 

        // seleccionar posicion al azar         
        int indice = Random.Range(0, dianas.Length);
        GameObject puntoAparicionDiana = dianas[indice];
        MovimientoDiana movimientoDiana = puntoAparicionDiana.GetComponent<PuntoAparicionDiana>().direccionMovimientoDiana; 
        // comprobar si ya esta en uso 
        PuntoAparicionDiana puntoAparicion = puntoAparicionDiana.GetComponent<PuntoAparicionDiana>();
        if(puntoAparicion.EnUso)
            return; 

        // nuevo codigo

        // obtenemos una diana que no se este usando de las dos existentes
        // ya nos hemos aseguro de que haya una libre pero volvemos
        // a comprobar el null
        DianaEntrenamiento diana = ObtenerDianaLibre();
        if(diana==null)
            return; 
                
        
        switch(Nivel.dianas)
        {
            case DificultadTareaGaleriaTiro.SoloDianaObjetivo:
                // la diana es objetivio                
                diana.esObjetivo = true; 
                diana.estimulo = EstimuloTareaGaleriaTiro.DianaObjetivo;
                
            break;

            case DificultadTareaGaleriaTiro.VariosTiposDiana:
                // las dianas pueden ser no objetivos
                diana.esObjetivo = Random.value > Nivel.probabilidadAparicionDianaErronea;
                if(!diana.esObjetivo)
                {
                    diana.estimulo = EstimuloTareaGaleriaTiro.DianaErroena; 
                } else {
                    diana.estimulo = EstimuloTareaGaleriaTiro.DianaObjetivo;
                }
                
            break;

        }

        // comprobamos si es una gema de bonus
        if(diana.esObjetivo)
        {
            bool esGema = Random.value < Nivel.probabilidadAparicionGema;
            diana.esGema = esGema;
            if(diana.esGema)
                diana.estimulo = EstimuloTareaGaleriaTiro.Gema;
        }

        
        // comprobar si es un objetivo movil 
        bool esDianaEnMovimiento = Random.value < Nivel.probabilidadAparicionDianaMovil;

        // Debug.Log("Mostrando diana en " + puntoAparicion.gameObject.transform.position);

        diana.Mostrar(puntoAparicion, movimientoDiana, esDianaEnMovimiento);        
        puntoAparicion.Usar();

        FindObjectOfType<Audio>().FeedbackAparicionEstimulo();

        // la diana siempre mira al jugador
        diana.gameObject.transform.LookAt(Camera.main.transform.position);
        
    }   


    
    // se comprueba el estado de la partida despues de un error
    // o una omision 
    private void ComprobarOmisionError()
    {            
        if(errores + omisiones >= Nivel.omisionesOErroresParaPerder)
        {   
            /*
            // detenemos el tiempo
            if(corrutinaTiempo!=null)
                StopCoroutine(corrutinaTiempo);            
            tiempoUI.fillAmount =  0; */

            JuegoPerdido();
        }
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
        aciertosUI.text = aciertos.ToString(); // "Aciertos " + aciertos.ToString();
        AgregarPuntuacion(Configuracion.puntuacionAciertoGaleriaTiro);
        if(aciertos >= Nivel.aciertosParaSuperarElNivel)
            JuegoGanado();
       
    }

    public override void Error()
    {
        // si la tarea esta bloqueada no seguimos comprobando
        // errores u omisiones para evitar perder multiples veces
        if(tareaBloqueada)
            return; 

        errores++;
        FindObjectOfType<Audio>().FeedbackError();
        erroresUI.text = "Errores " + errores.ToString();
        AgregarPuntuacion(-Configuracion.penalizacionErrorGaleriaTiro);
        ComprobarOmisionError();
        
    } 

    public override void Omision()
    {
        // si la tarea esta bloqueada no seguimos comprobando
        // errores u omisiones para evitar perder multiples veces
        if(tareaBloqueada)
            return; 

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

        cabecera += "Leyenda: tiempo; vision x ; vision y ; tipo estimulo A; tipo estimulo B; tipo estimulo C; AX; AY; BX; BY; CX; CY; recargando?; municion";
        return cabecera;
    }

    protected override RegistroPosicionOcular NuevoRegistro(float tiempo, int x, int y)
    {

      
        // obtenemos la municion actual, evitamos el null
        // porque igual no existe todavia componente jugador
        // en el primer frame?
        int municion = -1;
        JugadorTareaGaleriaTiro jugador = FindObjectOfType<JugadorTareaGaleriaTiro>();
        if(jugador != null)
            municion = jugador.Municion; 
      

        int AX = -1; 
        int AY = -1; 
        int BX = -1; 
        int BY = -1; 
        int CX = -1; 
        int CY = -1; 

        EstimuloTareaGaleriaTiro estimuloA = EstimuloTareaGaleriaTiro.Ninguno; 
        EstimuloTareaGaleriaTiro estimuloB = EstimuloTareaGaleriaTiro.Ninguno; 
        EstimuloTareaGaleriaTiro estimuloC = EstimuloTareaGaleriaTiro.Ninguno; 

        if(dianaA.EnUso)
        {
            Vector2 posicionDianaA = ObtenerPosicionPantallaDelEstimulo(
                dianaA.gameObject.transform.position
            );
            AX = (int) posicionDianaA.x; 
            AY = (int) posicionDianaA.y;  
            estimuloA = dianaA.estimulo; 
        } 
        

        if(dianaB.EnUso)
        {
            Vector2 posicionDianaB = ObtenerPosicionPantallaDelEstimulo(
                dianaB.gameObject.transform.position
            );
            BX = (int) posicionDianaB.x; 
            BY = (int) posicionDianaB.y; 
            estimuloB = dianaB.estimulo; 
        }

        
        if(dianaC.EnUso)
        {
            Vector2 posicionDianaC = ObtenerPosicionPantallaDelEstimulo(
                dianaC.gameObject.transform.position
            );
            CX = (int) posicionDianaC.x; 
            CY = (int) posicionDianaC.y; 
            estimuloC = dianaC.estimulo; 
        }

            
        // devolvemos el nuevo registro
        return new RegistroPosicionOcularTareaGaleriaTiro(tiempo, x, y, 
            estimuloA, estimuloB, estimuloC, 
            AX, AY, BX, BY, CX, CY, recargando, municion
        );

        
    } 

    private Vector2 ObtenerPosicionPantallaDelEstimulo(Vector3 posicionDiana)
    {   
         
        // transformar las coordeandas
        Vector2 posicionViewport = Camera.main.WorldToViewportPoint(posicionDiana);
        Vector2 posicionEnPantalla = new Vector2(
            ((posicionViewport.x * CanvasRect.sizeDelta.x)-(CanvasRect.sizeDelta.x * 0.5f))+960,
            ((posicionViewport.y * CanvasRect.sizeDelta.y)-(CanvasRect.sizeDelta.y * 0.5f))+540
        );
        return posicionEnPantalla;
    }


}
