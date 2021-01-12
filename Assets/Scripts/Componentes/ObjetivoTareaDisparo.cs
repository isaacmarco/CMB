using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetivoTareaDisparo : MonoBehaviour
{
       
    //public float vida = 100;
    public bool esObjetivo;
    public bool esGema; 
    private bool enUso = false;
    public bool EnUso {
        get { return this.enUso;}
    }
  
    void Start()
    {
        Iniciar();
    }

    public virtual void Mostrar()
    {        
        AnimacionMostrar();
        StartCoroutine(CorrtuinaMostrarObjetivo());
        enUso = true;        
    }

    protected virtual void Iniciar()
    {             
    }

    public virtual void RecibirDisparo()
    {       
        // instanciar disparos
        FindObjectOfType<JugadorTareaGaleriaTiro>().Disparar(gameObject.transform.position);
    }
    
    public virtual void Destruir()
    {

        // comprobar el tipo de diana
        if(esObjetivo)
        {
            if(esGema)
            {
                FindObjectOfType<TareaGaleriaTiro>().Bonus();
            } else {
                FindObjectOfType<TareaGaleriaTiro>().Acierto();
            }
            
        } else {
            FindObjectOfType<Tarea>().Error();
        }
        
        Ocultar();
        
    }


    protected virtual void Ocultar()
    {
        StopAllCoroutines();                     
        AnimacionOcultar();
        enUso = false; 
    }

    protected virtual void AnimacionMostrar()
    {}
    protected virtual void AnimacionOcultar()
    {}

    protected virtual IEnumerator CorrtuinaMostrarObjetivo()
    {
        // obtener la duracion del objetivo en pantalla
        float duracion = FindObjectOfType<TareaGaleriaTiro>().Nivel.duracionDiana;
        
        // esperamos
        yield return new WaitForSecondsRealtime(duracion);   

        // contabilizamos la omision si llegamos a este punto
        // y si el estimulo era objetivo. Las gemas no cuentan como omision
        if(esObjetivo && !esGema)
            FindObjectOfType<Tarea>().Omision();
            

        // ocultamos
        Ocultar();       
        
    }

   
}
