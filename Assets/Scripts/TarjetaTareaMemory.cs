using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarjetaTareaMemory : MonoBehaviour
{
    [SerializeField] private GameObject objeto;
    public Texture[] texturasParaEstimulos;
    private EstimulosTareaMemory estimulo; 
    private bool volteada; 
    public bool Volteda {
        get { return volteada; }
    }
    public EstimulosTareaMemory Estimulo {
        get { return estimulo;}
    }

    void Awake()
    {
        Ocultar();
    }

    public void Voltear()
    {
        if(volteada)
            return; 

        objeto.SetActive(true); 
        volteada = true; 
        // FindObjectOfType<TareaMemory>().VoltearTarjeta(this); 
        //StartCoroutine(CorrutinaVoltear());
    }
/*
    private IEnumerator CorrutinaVoltear()
    {
        yield return new WaitForSeconds(1f);
        Reiniciar();
        
    }
    */
    public void Ocultar()
    {
        objeto.SetActive(false);
        volteada = false; 
    }

    public void AsignarEstimulo(EstimulosTareaMemory estimulo)
    {
        
        this.estimulo = estimulo; 
        
        // actualizar la textura 
        Texture texturaEstimulo = texturasParaEstimulos[(int) estimulo];
        objeto.GetComponent<Renderer>().material.mainTexture = texturaEstimulo; 

    }

}
