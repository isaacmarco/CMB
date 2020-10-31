using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mensaje : MonoBehaviour
{
    public enum TipoMensaje
    {
        Exito, Fallo, Aviso, Bonus, Record
    };
   
    [SerializeField] private Text mensaje; 
    [SerializeField] private Image imagen; 
    [SerializeField] Image imagenTipoMensaje;    
    [SerializeField] private Sprite spriteExito, spriteFallo, spriteAviso, spriteBonus, spriteRecord; 
    
    public void Ocultar()
    {
        GetComponent<CanvasGroup>().alpha = 0f; 
    }

    public void Mostrar(string mensaje, Sprite imagen = null, TipoMensaje tipoMensaje = TipoMensaje.Aviso)
    {
        Debug.Log("Mensaje: " + mensaje);        
        
        // icono segun el tipo de mensaje, por defecto es de tipo aviso 
        switch(tipoMensaje)
        {
            case TipoMensaje.Aviso:
                imagenTipoMensaje.sprite = spriteAviso; 
            break;
            case TipoMensaje.Exito:
                imagenTipoMensaje.sprite = spriteExito; 
            break;
            case TipoMensaje.Fallo:
                imagenTipoMensaje.sprite = spriteFallo;
            break;
            case TipoMensaje.Bonus:
                imagenTipoMensaje.sprite = spriteBonus;
            break;
            case TipoMensaje.Record:
                imagenTipoMensaje.sprite = spriteRecord;
            break;
        }

        // asignar parametros 
        this.mensaje.text = mensaje;
        this.imagen.enabled = false; 

        if(imagen!=null) 
        {
            this.imagen.sprite = imagen;
            this.imagen.enabled = true;
        }
        
        // hacer el mensaje opaco 
        StartCoroutine(Alpha());
        //GetComponent<CanvasGroup>().alpha = 1f; 
    }

    private IEnumerator Alpha()
    {
        while(GetComponent<CanvasGroup>().alpha < 1)
        {
            float velocidad = 10f; 
            GetComponent<CanvasGroup>().alpha += velocidad * Time.deltaTime;
            yield return null;
        }        
    }



}
