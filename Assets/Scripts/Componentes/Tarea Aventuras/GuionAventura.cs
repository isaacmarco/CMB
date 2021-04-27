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
        // mensaje por defecto
         yield return StartCoroutine(tarea.MostrarMensaje(
            "En este juego las partidas siempre duran " + tarea.Nivel.tiempoLimiteEnMinutos + " minutos"
            ,0,null,Mensaje.TipoMensaje.Tiempo)
        );
                

        // mostramos los mensajes del nivel 
        yield return StartCoroutine(Mensajes());        
        // despues desbloqueamos la tarea
        tarea.DesbloquearTarea();
        
        // comenzqamos a contar el tiempo
        StartCoroutine(CorrutinaTiempoPartida());

        // comenzamos a comprobar que se cumplen las condiciones del guion 
        while(true)
        {
            ComprobacionesGuion();
            yield return null; 
        }
    }

    private IEnumerator CorrutinaTiempoPartida()
    {
        Debug.Log("Contabilizando tiempo");
        int duracionEnSegundos = tarea.Nivel.tiempoLimiteEnMinutos * 60;
        yield return new WaitForSeconds(duracionEnSegundos); 
        tarea.TiempoExcedido();
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
