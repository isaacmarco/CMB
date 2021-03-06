﻿using System.Collections;
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
    [SerializeField] private Transform jerarquiaMenuTareaGaleriaTiro;
    [SerializeField] private Transform jerarquiaMenuTareaEvaluacion;
    [SerializeField] private Transform jerarquiaMenuTareaAventuras;
    
    [SerializeField] private Transform jerarquiaMenuPerfiles;     
    [Header("Debug")]
    [SerializeField] private Text debug; 
    [Header("Multiplicador de velocidad")]
    [SerializeField] private Slider sliderMultiplicadorVelocidad; 
    [SerializeField] private Text valorMultiplicadorVelocidad;
    private bool mostrarDebug = false; 

    private void TrazadoProgreso()
    {        
        if(Configuracion.pacienteActual==null)
            return; 

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
     
        // debug
        if(Input.GetKeyDown(KeyCode.F1))
            mostrarDebug = !mostrarDebug;
        if(mostrarDebug)
            TrazadoProgreso();        
        debug.gameObject.SetActive(mostrarDebug);

    }


    private void RecuperarMultiplicadorVelocidad()
    {
        if( Configuracion.pacienteActual.multiplicadorVelocidad == 0)
            Configuracion.pacienteActual.multiplicadorVelocidad = 1; 

        Configuracion.multiplicadorVelocidad = Configuracion.pacienteActual.multiplicadorVelocidad; 
        // actualizamos la UI
        sliderMultiplicadorVelocidad.value =  Configuracion.multiplicadorVelocidad ;
        Debug.Log("Multiplicador de velocidad es " +  Configuracion.multiplicadorVelocidad );        
    }
    
    private void CambioEnSliderMultiplicadorVelocidad()
    {
        // salvamos la opcion
        Configuracion.multiplicadorVelocidad = sliderMultiplicadorVelocidad.value;       
        Configuracion.pacienteActual.multiplicadorVelocidad = Configuracion.multiplicadorVelocidad; 
        valorMultiplicadorVelocidad.text = Configuracion.multiplicadorVelocidad + "";
    }

    public void GuardarCambiosMultiplicadorVelocidad()
    {        
        Aplicacion.instancia.GuardarDatosPaciente(Configuracion.pacienteActual);
    }

    void Start()
    {   

        // nuevo codigo para ajustar la velocidad de algunas
        // tareas mediente la UI 

        // incluir el listener al slider de recuperacion 
        sliderMultiplicadorVelocidad.onValueChanged.AddListener(delegate {
            CambioEnSliderMultiplicadorVelocidad(); 
        });
      

        // arranque normal del menu
       
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
                case Tareas.GaleriaTiro:
                    // menu de la galeria de tiro
                    MostrarMenu(jerarquiaMenuTareaGaleriaTiro);
                break;
                case Tareas.Aventuras:
                    // menu de aventuras
                    MostrarMenu(jerarquiaMenuTareaAventuras);
                break;
                
            }
           
        }

       
    }


    public void MostrarMenu(Transform jerarquia)
    {                
      
        Debug.Log("Mostrando menu " + jerarquia.name);

        if(jerarquia == jerarquiaMenuTareaMemory || jerarquia == jerarquiaMenuTareaTopos || 
        jerarquia == jerarquiaMenuTareaGaleriaTiro || jerarquia == jerarquiaMenuTareaAventuras )
        {
            // actualizar ui dependiendo el progreso
            FindObjectOfType<MenuTareaTopos>().Actualizar();
            FindObjectOfType<MenuTareaMemory>().Actualizar();
            FindObjectOfType<MenuTareaGaleriaTiro>().Actualizar();
            FindObjectOfType<MenuTareaAventuras>().Actualizar();
            // feedback
            FindObjectOfType<Audio>().FeedbackElegirTarea();
        }

        if(jerarquia == jerarquiaMenuPerfiles)
        {
            // actualizar la ui de perfiles
            FindObjectOfType<MenuPerfilesPacientes>().ActualizarPerfiles();
        }
        
        Transform[] jerarquias = {
            jerarquiaMenuPrincipal, jerarquiaMenuTareaTopos, jerarquiaMenuPerfiles,
            jerarquiaMenuTareaMemory, jerarquiaMenuTareaEvaluacion, 
            jerarquiaMenuTareaGaleriaTiro, jerarquiaMenuTareaAventuras
        };
        // desactivamos menus
        foreach(Transform j in jerarquias)
            j.gameObject.SetActive(false); 
        // activamos el correspondiente
        jerarquia.gameObject.SetActive(true); 

    }

    public void ComenzarTareaEvaluacion()
    {
        // comprobar contraseña
        if( GetComponent<MenuTareaEvaluacion>().PasswordActual == Configuracion.passwordTareaEvaluacion)
        {
            SceneManager.LoadScene("TareaEvaluacion");
        } else {
            Debug.LogError("Password incorrecta");
        }
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
            break;

            case OpcionesSeleccionablesMenu.MenuTareaMemory:
                // menu de memory
                MostrarMenu(jerarquiaMenuTareaMemory);
            break;

            case OpcionesSeleccionablesMenu.MenuTareaGaleriaTiro:
                MostrarMenu(jerarquiaMenuTareaGaleriaTiro);
            break;

            case OpcionesSeleccionablesMenu.MenuTareaAventuras:
                MostrarMenu(jerarquiaMenuTareaAventuras);
            break;

            case OpcionesSeleccionablesMenu.MenuTareaEvaluacion:
                // limpiar la password de la tarea de evaluacion
                GetComponent<MenuTareaEvaluacion>().LimpiarPassword();
                // menu de evaluacion
                MostrarMenu(jerarquiaMenuTareaEvaluacion);
            break;

            case OpcionesSeleccionablesMenu.MenuPerfiles:
                // perfiles d epacientes
                MostrarMenu(jerarquiaMenuPerfiles);
            break;

            case OpcionesSeleccionablesMenu.SalirAplicacion:
                Salir();
            break;

            case OpcionesSeleccionablesMenu.ComenzarTareaEvaluacion:
                SceneManager.LoadScene("TareaEvaluacion");
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

            case OpcionesSeleccionablesMenu.ComenzarTareaGaleriaTiro:
                Aplicacion.instancia.CargarNivelTareaGaleriaTiro(
                    Configuracion.pacienteActual.ultimoNivelDesbloqueadoTareaGaleriaTiro                    
                );
                SceneManager.LoadScene("TareaGaleriaTiro");
                break;

            case OpcionesSeleccionablesMenu.ComenzarTareaMemory:
                Aplicacion.instancia.CargarNivelTareaMemory(
                    Configuracion.pacienteActual.nivelActualTareaMemory
                );
                // lanzamos la tarea de memoria
                SceneManager.LoadScene("TareaMemory");
            break;

            case OpcionesSeleccionablesMenu.ComenzarTareaAventuras:
                Aplicacion.instancia.CargarNivelTareaAventuras(
                    Configuracion.pacienteActual.ultimoNivelDesbloqueadoTareaAventuras
                );
                // lanzamos la tarea de memoria
                SceneManager.LoadScene("TareaAventuras");
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
      
        // establecer el paciente actual 
        Configuracion.pacienteActual = configuracion.pacientes[indice];

          
        // recuperar la configuracion de velocidad
        RecuperarMultiplicadorVelocidad();

        // feedback
        FindObjectOfType<Audio>().FeedbackElegirPerfil();

        // mostrar el menu 
        MostrarMenu(jerarquiaMenuPrincipal);   
      
    }

    
    // sale del programa completamente 
    private void Salir()
    {
        Debug.Log("Saliendo del programa");
        Application.Quit();
    }

  
    
}
