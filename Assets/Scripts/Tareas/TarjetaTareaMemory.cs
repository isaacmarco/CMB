using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarjetaTareaMemory : MonoBehaviour
{
    [SerializeField] private GameObject objetoDibujo;
    [SerializeField] private GameObject objetoPieza; 
    public Texture[] texturasParaEstimulos;
    private EstimulosTareaMemory estimulo; 
    private bool volteada; 
    private bool vistaPorJugador; 
    private Vector3 posicionOriginal; 
    public bool Volteda {
        get { return volteada; }
    }
    public EstimulosTareaMemory Estimulo {
        get { return estimulo;}
    }
    public bool VistaPorJugador {
        get { return vistaPorJugador;}
        set { vistaPorJugador = value; }
    }


    public void Seleccionar()
    {
        if(volteada)
            return; 
        
        // elegimos esta tarjeta 
        FindObjectOfType<TareaMemory>().VoltearTarjeta(this); 
        
    }

    public void Voltear()
    {
        StartCoroutine(CorrutinaVoltear());
        // la descubrimos 
        volteada = true; 
    }  

    public void Ocultar()
    {    
        StartCoroutine(CorrutinaOcultar());
        volteada = false; 
    }

    private void Iniciar()
    {        
        volteada = false; 
    }

    public void Resuelta()
    {
        // mantenemos su pocion fijada en el punto correcto 
        Vector3 p = posicionOriginal; 
        float tiempoAnimacion = 0.5f; 
        Vector3 posicionDestino = p;
        iTween.MoveTo(gameObject, 
            iTween.Hash("position", posicionDestino, "islocal", true, "time", tiempoAnimacion)
        );
    }

    private IEnumerator CorrutinaVoltear()
    {
        Vector3 p = gameObject.transform.localPosition;
        float tiempoAnimacion = 0.5f; 
        Vector3 posicionDestino = p + new Vector3(0, 0.2f, 0);        
        iTween.MoveTo(gameObject, 
            iTween.Hash("position", posicionDestino, "islocal", true, "time", tiempoAnimacion)
        );

        iTween.RotateAdd(objetoPieza, new Vector3(0, 0, 180), 1);

        yield return null;
    }

    private IEnumerator CorrutinaOcultar()
    {
        Vector3 p = posicionOriginal;
        float tiempoAnimacion = 0.5f; 
        Vector3 posicionDestino = p; 
        iTween.MoveTo(gameObject, 
            iTween.Hash("position", posicionDestino, "islocal", true, "time", tiempoAnimacion)
        );

        iTween.RotateAdd(objetoPieza, new Vector3(0, 0, -180), 1);
        yield return null;
    }


    public void AsignarEstimulo(EstimulosTareaMemory estimulo)
    {
        
        this.estimulo = estimulo; 
        
        // actualizar la textura 
        Texture texturaEstimulo = texturasParaEstimulos[(int) estimulo];
        objetoDibujo.GetComponent<Renderer>().material.mainTexture = texturaEstimulo; 

        posicionOriginal = gameObject.transform.localPosition;

        Iniciar();

    }

}
