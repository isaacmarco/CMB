using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class TareaAventuras : Tarea
{
    [Header("Inventario")]
    public ItemInventario[] inventario; 
    public ArrayList objetosRecogidos = new ArrayList();     
    public ArrayList objetosConsumidos = new ArrayList();
    public ArrayList objetosUsados = new ArrayList();
    [Header("Interfaz")]
    public GameObject[] corazones; 
    public Text puntos; 
    
    public NivelAventurasScriptable Nivel { 
        get { return (NivelAventurasScriptable) Configuracion.nivelActual;} 
    }

    // vida del jugador
    private int vida = 3; 
    private int vidaMaxima = 3; 
    private IsometricPlayerMovementController jugador; 
    
    protected override void Actualizacion()
    {

    }

    public override void TiempoExcedido()
    {
        Debug.Log("Tiempo excedido, se pierde la partida");
        JuegoPerdido();
    }

    protected override void JuegoGanado()
    {    
        BloquearTarea();
        // FindObjectOfType<Audio>().FeedbackPartidaGanada();
        FinalizarRegistro();
        StopAllCoroutines();
        StartCoroutine(TerminarJuego(true)); 
    }
    
    
    protected override bool TodosLosNivelesCompletados()
    {
        int numeroNiveles = 15;             
        return Configuracion.pacienteActual.ultimoNivelDesbloqueadoTareaAventuras >= numeroNiveles;
    }

    protected override IEnumerator TareaCompletada()
    {
        base.TareaCompletada();    
        yield return StartCoroutine(
            MostrarMensaje("¡Has completado todos los niveles del juego!",0, null, Mensaje.TipoMensaje.Record)
        );
    }

    protected override bool GuardarProgreso(bool partidaGanada)
    {        
        Debug.Log("Guardando progreso de partida de aventuras");

        if(partidaGanada)
        {
            // guardar la puntuacion
            if(puntuacion > 0)
                Configuracion.pacienteActual.puntuacionTareaAventuras += puntuacion; 

            // progresar
            Configuracion.pacienteActual.ultimoNivelDesbloqueadoTareaAventuras++;
            
            // comprobar si hemos terminado todos los niveles   
            if(TodosLosNivelesCompletados())
            {
                Debug.Log("Todos los niveles de la tarea completos");
                // El juego se ha terminado, no hay mas niveles
                int numeroNiveles = 15; 
                Configuracion.pacienteActual.ultimoNivelDesbloqueadoTareaAventuras = numeroNiveles; 
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

    public void RecibirImpacto()
    {             
        PerderVida();
        // parpadeo del jugador 
        jugador.RecibirImpacto();
    }

    public void GanarPartida()
    {              
        if(tareaBloqueada)      
            return; 

        JuegoGanado();
    }

    private bool juegoPerdido = false; 

    private void PerderVida()
    {
        if(juegoPerdido)
            return; 
            
        if(tareaBloqueada)      
            return; 

        vida--;
        if(vida <= 0)
        {
            vida = 0; 
            juegoPerdido = true; 
            JuegoPerdido();
        }        
        ActualizarMarcadorVida();
    }

    private void GanarVida()
    {        
        vida++;
        if(vida > vidaMaxima)
            vida = vidaMaxima; 
        ActualizarMarcadorVida();
    }

    private void ActualizarMarcadorVida()
    {
        // actualizar marcador de corazones
        for(int i=0; i<corazones.Length; i++)
            corazones[i].SetActive(vida >= i + 1);
    }

    protected override void Inicio()
    {        
        vida = 3; 
        ActualizarMarcadorVida();
        InstanciarNivel();
    }

    private void InstanciarNivel()
    {
        // cargamos el nivel completo y referenciamos el jugador
        GameObject nivel = (GameObject) Instantiate (Nivel.prefabGuionParaCargar);
        jugador = FindObjectOfType<IsometricPlayerMovementController>();

    }

    // devuelve verdadero si hay espacio en el inventario
    private bool HayEspacioInventario()
    {        
        foreach(ItemInventario espacioInventario in inventario)
            if(espacioInventario.Libre)
                return true; 
        return false;
    }

    // devuelve un epacio libre en el inventario
    private ItemInventario ObtenerEspacioInventarioLibre()
    {
        // se debe comprobar que existe espacio libre antes
        // de usar esta funcion 
        foreach(ItemInventario espacioInventario in inventario)
        {
            // devolvemos el primer espacio libre
            if(espacioInventario.Libre)
                return espacioInventario; 
        }
        // No deberia
        return null; 
    }

    private void AgregarInventario(ObjetosAventuras item)
    {        
        Debug.Log(item + " recogido");
        // buscar un hueco libre        
        ItemInventario espacioLibre = ObtenerEspacioInventarioLibre();
        // añadirlo al inventario
        espacioLibre.Agregar(item);
    }

    public void ConsumirItem(ObjetosAventuras item)
    {
        Debug.Log("Consumiendo " + item);
        
        objetosRecogidos.Add(item); 
        objetosConsumidos.Add(item);

        // items que no van al inventario
        switch(item)
        {
            case ObjetosAventuras.Corazon:
                // vida directa
                GanarVida();
            break;
            case ObjetosAventuras.Cofre:
                // no se agrega, se suman puntos
            break;
        }

        
        // logica para puntos        

        switch(item)
        {   
            // tesoros        
            case ObjetosAventuras.Monedas:
            AgregarPuntuacion(50);
            break;
            case ObjetosAventuras.Oro:
            AgregarPuntuacion(100);                            
            break;
            case ObjetosAventuras.Lingote:
            AgregarPuntuacion(200); 
            break;
         
            
            // cosas...
            
        }

        // actualizamos los puntos
        puntos.text = Puntuacion.ToString();
    }

    public bool RecogerItem(ObjetosAventuras item)
    {        
        // comprobamos si hay espacio
        if(!HayEspacioInventario())
            return false; 
        
        objetosRecogidos.Add(item); 

        switch(item)
        {
               
            // gemas
            case ObjetosAventuras.Rubi:
            AgregarPuntuacion(500);
            break;
            case ObjetosAventuras.Diamante:
            AgregarPuntuacion(1000);
            break;
            case ObjetosAventuras.Esmeralda:
            AgregarPuntuacion(500);
            break;
        }

        AgregarInventario(item);

        // devolvemos true para que el objeto sea destruido
        return true; 
    }


            
    public void UsarItem(ObjetosAventuras item)
    {
        Debug.Log("Usando " + item);
        
        objetosUsados.Add(item); 

        switch(item)
        {
            case ObjetosAventuras.PocimaSalud:
                GanarVida();
            break;
        }
    }



    
    public override string ObtenerNombreTarea()
    {
        return "Tarea aventuras";
    }
    
    protected override string ObtenerCabeceraTarea()
    {
        string cabecera = string.Empty;
        // datos de la tarea
        cabecera += "Tarea de evaluacion\n";
        cabecera += "Numero de bloques de evaluacion: " + Configuracion.numberoDeBloquesDeEvaluacion + "\n";
        cabecera += "Leyenda: tiempo; estimulo fijacion visible; numero bloque actual; mirando x; mirando y; estimulo objetivo x; estimulo objetivo y";
        return "Cabecera por decidir";
    }
    
    protected override RegistroPosicionOcular NuevoRegistro(float tiempo, int x, int y)
    {        
        return new RegirstroPosicionOcultarTareaAventuras(
            tiempo, x, y
        );
    } 
    
}
