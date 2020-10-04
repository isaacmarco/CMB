using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private Tareas tareaActual; 
    public ConfiguracionScriptable configuracion; 
    public ConfiguracionScriptable Configuracion { get { return configuracion;} }
    [Header("Canvas del menu")]
    [SerializeField] private RectTransform canvasRect; 
    [Header("Jerarquia de menus")]
    [SerializeField] private Transform jerarquiaMenuPrincipal;
    [SerializeField] private Transform jerarquiaMenuTarea;   
    [SerializeField] private Transform jerarquiaMenuPerfiles; 
    [Header("Progreso de tareas")]
    [SerializeField] private Transform jerarquiaProgreso;     
    

    public RectTransform CanvasRect 
    {
        get {return canvasRect;}
    }

    void Start()
    {
        MostrarMenu(jerarquiaMenuPrincipal);
    }

    public void MostrarMenu(Transform jerarquia)
    {
        Debug.Log("Mostrando menu " + jerarquia.name);
        Transform[] jerarquias = {
            jerarquiaMenuPrincipal, jerarquiaMenuTarea, jerarquiaMenuPerfiles
        };
        // desactivamos menus
        foreach(Transform j in jerarquias)
            j.gameObject.SetActive(false); 
        // activamos el correspondiente
        jerarquia.gameObject.SetActive(true); 

        // actualizar el progreso de la tarea si corresponde
        if(jerarquia == jerarquiaMenuTarea)
        {            
            jerarquiaProgreso.gameObject.SetActive(true);
            //progresoTarea.fillAmount = Random.value; 
         
            
        } else {
            jerarquiaProgreso.gameObject.SetActive(false);
        }
    }

    public void EjecutarOpcionMenu(OpcionesSeleccionablesMenu opcion, Tareas tarea)
    {
        tareaActual = tarea; 

        // reproducir el metodo apropiado
        switch(opcion)
        { 
            case OpcionesSeleccionablesMenu.VolverMenuPrincipal:
                // menu principal 
                MostrarMenu(jerarquiaMenuPrincipal);
            break;
            
            case OpcionesSeleccionablesMenu.MenuTareaTopos:
                // menu de la tarea seleccionada
				MostrarMenu(jerarquiaMenuTarea);
            break;

            case OpcionesSeleccionablesMenu.MenuPerfiles:
                // perfiles d epacientes
                MostrarMenu(jerarquiaMenuPerfiles);
            break;

            case OpcionesSeleccionablesMenu.SalirAplicacion:
                Salir();
            break;

            case OpcionesSeleccionablesMenu.ComenzarTareaTopos:
                // lanzamos la tarea topos
                SceneManager.LoadScene("TareaTopos");
    
            break;

            case OpcionesSeleccionablesMenu.ComenzarTareaMemory:
                // lanzamos la tarea de memoria
                SceneManager.LoadScene("TareaMemory");
            break;

        }   
    }    

    // sale del programa completamente 
    private void Salir()
    {
        Debug.Log("Saliendo del programa");
        Application.Quit();
    }

  
    
}
