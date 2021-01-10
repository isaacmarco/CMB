using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DianaEntrenamiento : ObjetivoTareaDisparo
{
    
    [Header("Modelos 3D")]
    public GameObject modeloDianaCorrecta;
    public GameObject modeloDianaIncorrecta;

    protected override void Iniciar()
    {
        base.Iniciar();
        // ocultar y mostrar solo el modelo 3D correspondiente
        OcultarModelos();               
    }
    
    public override void Mostrar()
    {
        OcultarModelos();
        base.Mostrar();
        // mostrar la diana correspondiente
        if(esObjetivo)
        {
            modeloDianaCorrecta.SetActive(true);
        } else {
            modeloDianaIncorrecta.SetActive(true);
        }
    }    
    protected override void AnimacionMostrar()
    {
        // animacion de la diana al aparecer
        gameObject.transform.localScale = Vector3.zero;
        iTween.ScaleTo(gameObject, Vector3.one, 0.5f);
    }

    protected override void AnimacionOcultar()
    {
        // animacion de la diana al aparecer       
        iTween.ScaleTo(gameObject, Vector3.zero, 0.5f);        
    }
  
    private void OcultarModelos()
    {
        modeloDianaCorrecta.SetActive(false);
        modeloDianaIncorrecta.SetActive(false);        
    }




}
