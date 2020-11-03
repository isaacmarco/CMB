using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; 

public class Menu : MonoBehaviour
{
    private Tareas tareaActual; 
    public ConfiguracionScriptable configuracion; 
    public ConfiguracionScriptable Configuracion { get { return configuracion;} }
    [Header("Canvas del menu")]
    [SerializeField] private RectTransform canvasRect; 
    [Header("Jerarquia de menus")]
    [SerializeField] private Transform jerarquiaMenuPrincipal;
    [SerializeField] private Transform jerarquiaMenuTareaTopos;   
    [SerializeField] private Transform jerarquiaMenuTareaMemory;   
    [SerializeField] private Transform jerarquiaMenuPerfiles; 
    [Header("Debug")]
    [SerializeField] private Text debug; 

    private void TrazadoProgreso()
    {        
        string texto = Serializador.SerializarScriptable(
            Configuracion.pacienteActual
        );
        debug.text = texto; 
    }


    public RectTransform CanvasRect 
    {
        get {return canvasRect;}
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F12))
            Aplicacion.instancia.BorrarDatos();

        
    }
    void Start()
    {
       
        // cargamos todos los json de los perfiles disponibles
        Aplicacion.instancia.CargarPerfilesExistentes();        

        // dependiendo de si hay paciente actual se 
        // mostrara el menu de tareas o el de perfikes
        if(Configuracion.pacienteActual == null)
        {
            // se muestra la seleccion de perfiles, al seleccionar
            // el perfil se cargaran datos del paciente 
            Debug.Log("No hay paciente actual, mostrando menu de perfiles");
            MostrarMenu(jerarquiaMenuPerfiles);
        } else {
           
            // ya existe un paciente actual?
            Debug.Log("Paciente actual existente, mostrando menu correspondiente");

            // comprobamos si venimos de jugar una tarea o si acabamos
            // de entrar en el menu
            switch(configuracion.tareaActual)
            {
                case Tareas.Ninguna:
                    // mostrar el menu principal a continuacion             
                    MostrarMenu(jerarquiaMenuPrincipal);
                break;
                case Tareas.Topos:
                    // menu topos
                    MostrarMenu(jerarquiaMenuTareaTopos);
                break;
                case Tareas.Memory:
                    // menu memory
                    MostrarMenu(jerarquiaMenuTareaMemory);
                break;
            }
            
            // hacemos debug del perfil seleccionado en pantalla
            TrazadoProgreso();
        }

       
    }


    public void MostrarMenu(Transform jerarquia)
    {
        Debug.Log("Mostrando menu " + jerarquia.name);

        if(jerarquia == jerarquiaMenuTareaMemory || jerarquia == jerarquiaMenuTareaTopos)
        {
            // actualizar ui dependiendo el progreso
            FindObjectOfType<MenuTareaTopos>().Actualizar();
            FindObjectOfType<MenuTareaMemory>().Actualizar();
        }
        
        Transform[] jerarquias = {
            jerarquiaMenuPrincipal, jerarquiaMenuTareaTopos, jerarquiaMenuPerfiles,
            jerarquiaMenuTareaMemory
        };
        // desactivamos menus
        foreach(Transform j in jerarquias)
            j.gameObject.SetActive(false); 
        // activamos el correspondiente
        jerarquia.gameObject.SetActive(true); 

    }

    public void EjecutarOpcionMenu(OpcionesSeleccionablesMenu opcion, int nivelACargar = -1)
    {
      

        // reproducir el metodo apropiado
        switch(opcion)
        { 
            case OpcionesSeleccionablesMenu.VolverMenuPrincipal:
                // menu principal 
                MostrarMenu(jerarquiaMenuPrincipal);
            break;
            
            case OpcionesSeleccionablesMenu.MenuTareaTopos:
                // menu de topos
				MostrarMenu(jerarquiaMenuTareaTopos);
                //FindObjectOfType<MenuTareaTopos>().Actualizar();
            break;

            case OpcionesSeleccionablesMenu.MenuTareaMemory:
                // menu de memory
                MostrarMenu(jerarquiaMenuTareaMemory);
                //FindObjectOfType<MenuTareaMemory>().Actualizar();
            break;

            case OpcionesSeleccionablesMenu.MenuPerfiles:
                // perfiles d epacientes
                MostrarMenu(jerarquiaMenuPerfiles);
            break;

            case OpcionesSeleccionablesMenu.SalirAplicacion:
                Salir();
            break;

            case OpcionesSeleccionablesMenu.ComenzarTareaTopos:
                // establecer el nivel actual de topos
                // antes de lanzar la tarea
                Aplicacion.instancia.CargarNivelTareaTopos(
                    Configuracion.pacienteActual.ultimoNivelDesbloqueadoTareaTopos
                );
                // lanzamos la tarea topos
                SceneManager.LoadScene("TareaTopos");    
            break;

            case OpcionesSeleccionablesMenu.ComenzarTareaMemory:
                Aplicacion.instancia.CargarNivelTareaMemory(
                    Configuracion.pacienteActual.nivelActualTareaMemory
                );
                // lanzamos la tarea de memoria
                SceneManager.LoadScene("TareaMemory");
            break;

            /*
            case OpcionesSeleccionablesMenu.SiguienteNivel:
                // para la tarea de memory de momento
                //FindObjectOfType<MenuTareaMemory>().NivelSiguiente();
            break;

            case OpcionesSeleccionablesMenu.AnteriorNivel:
                // para la tarea de memory de momento
                //FindObjectOfType<MenuTareaMemory>().NivelAnterior();
            break;*/
            
            case OpcionesSeleccionablesMenu.SeleccionarPaciente1:
                SeleccionarPaciente(0);
            break;

            case OpcionesSeleccionablesMenu.SeleccionarPaciente2:
                SeleccionarPaciente(1);
            break;

            case OpcionesSeleccionablesMenu.SeleccionarNivelMemory:
                Debug.Log("Seleccionado nivel " + nivelACargar);
                // establecer un nivel como seleccionado
                FindObjectOfType<MenuTareaMemory>().Seleccionar(nivelACargar);
                
            break;

        }   
    }    

    private void SeleccionarPaciente(int indice)
    {
        Debug.Log("Paciente seleccionado " + indice);
        /*
        // obtener el codigo del paciente desde el prefs
        string clave = "codigoPaciente" + indice; 
        string codigoPaciente = PlayerPrefs.GetString(clave);
        // cargar el paciente actual         
        Aplicacion.instancia.CargarDatosPaciente(codigoPaciente);
        // indicar que ya hay un perfil seleccionado
        Configuracion.hayPacienteActivo = true; */

        // establecer el paciente actual 
        Configuracion.pacienteActual = configuracion.pacientes[indice];

        // test para guardar el paciente actual 
        // Aplicacion.instancia.GuardarDatosPaciente(configuracion.pacienteActual);

        // mostrar el menu 
        MostrarMenu(jerarquiaMenuPrincipal);   
        TrazadoProgreso();    
    }

    
    // sale del programa completamente 
    private void Salir()
    {
        Debug.Log("Saliendo del programa");
        Application.Quit();
    }

  
    
}
