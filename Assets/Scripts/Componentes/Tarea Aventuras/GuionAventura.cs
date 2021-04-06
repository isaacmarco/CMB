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
    {}
    // ejecucion
    protected IEnumerator Guion()
    {
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
    {}
    
    public virtual void SalidaAlcanzada()
    {
        Debug.Log("Salida alcanzada");
    }

}
