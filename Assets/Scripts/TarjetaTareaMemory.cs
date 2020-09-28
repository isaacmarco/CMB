using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarjetaTareaMemory : MonoBehaviour
{
    [SerializeField] private GameObject objeto;
    public Texture[] texturasParaEstimulos;
    private EstimulosTareaMemory estimulo; 
    public EstimulosTareaMemory Estimulo {
        get { return estimulo;}
    }
       

    public void AsignarEstimulo(EstimulosTareaMemory estimulo)
    {
        
        this.estimulo = estimulo; 
        
        // actualizar la textura 
        Texture texturaEstimulo = texturasParaEstimulos[(int) estimulo];
        objeto.GetComponent<Renderer>().material.mainTexture = texturaEstimulo; 

    }

}
