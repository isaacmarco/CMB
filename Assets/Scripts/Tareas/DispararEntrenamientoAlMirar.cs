using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
public class DispararEntrenamientoAlMirar : SeleccionaAlMirar
{
    
    private ObjetivoBaseEntrenamiento objetivo; 

  
    protected override void Inicio()
    {
        base.Inicio();
        objetivo = gameObject.GetComponent<ObjetivoBaseEntrenamiento>();         
        DetenerFijacion();	
    }
    
    protected override void SeleccionarEstimulo()
    {
        objetivo.Impactar();
    }


  
   

}
