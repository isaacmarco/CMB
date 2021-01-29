using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuTareaEvaluacion : MonoBehaviour
{
   
    public Toggle fondoGrisToggle, experimentoToggle, entrenamientoToggle, joystickToggle, vistaToggle; 
   

    void Start()
    {
        fondoGrisToggle.onValueChanged.AddListener ( (value) => {              
            ToggleFondo(value);                      
            }  
        );   

        experimentoToggle.onValueChanged.AddListener ( (value) => {              
            ToggleExperimental(value);                      
            }  
        );   
        entrenamientoToggle.onValueChanged.AddListener ( (value) => {              
            ToggleEntrenamiento(value);                      
            }  
        );   
        joystickToggle.onValueChanged.AddListener ( (value) => {              
            ToggleJoystick(value);                      
            }  
        );  
        vistaToggle.onValueChanged.AddListener ( (value) => {              
            ToggleVista(value);                      
            }  
        );  
        
        Aplicacion.instancia.configuracion.condicionTareaEvaluaion = CondicionTareaEvaluacion.Experimental;
        Aplicacion.instancia.configuracion.manejoTareaEvalucion = ManejoTareaEvaluacion.Vista;
        Aplicacion.instancia.configuracion.usarFondoGrisTareaEvaluacion = false; 
        
        experimentoToggle.isOn = true; 
        entrenamientoToggle.isOn = false;         
        vistaToggle.isOn = true; 
        joystickToggle.isOn = false; 
        fondoGrisToggle.isOn = false;
    }
    private void ToggleJoystick(bool activo)
    {
        if(activo)
            Aplicacion.instancia.configuracion.manejoTareaEvalucion = ManejoTareaEvaluacion.Joystick;
    }
    private void ToggleVista(bool activo)
    {
        if(activo)
            Aplicacion.instancia.configuracion.manejoTareaEvalucion = ManejoTareaEvaluacion.Vista; 
    }
    private void ToggleFondo(bool activo)
    {
        Aplicacion.instancia.configuracion.usarFondoGrisTareaEvaluacion = activo;
    }
    private void ToggleExperimental(bool activo)
    {        
        if(activo)
            Aplicacion.instancia.configuracion.condicionTareaEvaluaion = CondicionTareaEvaluacion.Experimental;
    }
    private void ToggleEntrenamiento(bool activo)
    {
        if(activo)
            Aplicacion.instancia.configuracion.condicionTareaEvaluaion = CondicionTareaEvaluacion.Entrenamiento;
    }
}
