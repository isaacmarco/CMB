using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PuntosGaleriaTiroVFX : MonoBehaviour
{
    
    public Color32 colorAciterto; 
    public Color32 colorError; 

    void Start()
    {
        

        // uso escala negativa en x para invertir el texto 
        gameObject.transform.localScale = Vector3.zero;
        Vector3 destino = new Vector3(-0.1f, 0.1f, 0.1f);
        iTween.ScaleTo(gameObject, destino, 0.7f);
    }

    public void Mostrar(bool acierto)
    {
        GetComponent<TextMeshPro>().color = colorAciterto;
        if(!acierto)
            GetComponent<TextMeshPro>().color = colorError; 
    }

    void Update()
    {
        float velocidad = 1f; 
        gameObject.transform.Translate(Vector3.up * Time.deltaTime * velocidad); 
        transform.LookAt(Camera.main.gameObject.transform.position);
    }
}
