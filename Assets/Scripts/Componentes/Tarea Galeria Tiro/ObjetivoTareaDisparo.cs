using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetivoTareaDisparo : MonoBehaviour
{
    public EstimuloTareaGaleriaTiro estimulo; 
    public bool esObjetivo;
    public bool esGema; 
    [SerializeField]
    private bool enUso = false;
    
    protected Vector3 puntoAparicion; 
    protected PuntoAparicionDiana puntoAparicionDiana;

    public bool EnUso {
        get { return this.enUso;}
    }
  
    public bool EsObjetivo {
        get { return this.esObjetivo;}
    }
    
    void Start()
    {
        Iniciar();
    }

    public virtual void Mostrar(PuntoAparicionDiana puntoAparicionDiana, MovimientoDiana movimientoDiana, bool enMovimiento = false)
    {   
        this.puntoAparicionDiana = puntoAparicionDiana;
        this.puntoAparicionDiana.Usar();
        
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
        
        FindObjectOfType<TareaGaleriaTiro>().InstanciarVFXDestruccion(
            gameObject.transform.position,esGema, esObjetivo);
      
        Ocultar();
        
    }


    protected virtual void Ocultar()
    {
        StopAllCoroutines();                     
        AnimacionOcultar();
        enUso = false; 
        puntoAparicionDiana.Liberar();
    }

    protected virtual void AnimacionMostrar()
    {}
    protected virtual void AnimacionOcultar()
    {}

    protected virtual IEnumerator CorrtuinaMostrarObjetivo()
    {
        // obtener la duracion del objetivo en pantalla
        TareaGaleriaTiro tarea = FindObjectOfType<TareaGaleriaTiro>();
        float duracion = tarea.Nivel.duracionDiana;
        
        // esperamos, multiplicando la espera por el factor
        // de velocidad 
        yield return new WaitForSecondsRealtime(duracion * tarea.Configuracion.multiplicadorVelocidad);   

        // contabilizamos la omision si llegamos a este punto
        // y si el estimulo era objetivo. Las gemas no cuentan como omision
        if(esObjetivo && !esGema)
            FindObjectOfType<Tarea>().Omision();
            

        // ocultamos
        Ocultar();       
        
    }

   
}
