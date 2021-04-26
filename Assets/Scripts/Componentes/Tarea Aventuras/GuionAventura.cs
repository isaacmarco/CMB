using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuionAventura : MonoBehaviour
{
    protected TareaAventuras tarea; 
    
    void Start()
    {
        tarea = FindObjectOfType<TareaAventuras>();       
        Inicio();
        StartCoroutine(Guion());
    }

    // para establacer propiedades y referencias
    public virtual void Inicio()
    {        
    }
    
    protected virtual IEnumerator Mensajes()
    {
        yield return null; 
    }

    // ejecucion
    protected IEnumerator Guion()
    {
        // mostramos los mensajes del nivel 
        yield return StartCoroutine(Mensajes());        
        // despues desbloqueamos la tarea
        tarea.DesbloquearTarea();
        // comenzamos a comprobar que se cumplen las condiciones del guion 
        while(true)
        {
            ComprobacionesGuion();
            yield return null; 
        }
    }

    // comprobacion 
    public virtual void ComprobacionesGuion()
    {}
    
    public virtual void Fin()
    {
        // conseguido
        if(!tarea.TareaBloqueada)        
            tarea.GanarPartida();
    }
    
    public virtual void SalidaAlcanzada()
    {}

}
