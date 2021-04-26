using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mensaje : MonoBehaviour
{
    public enum TipoMensaje
    {
        Exito, Fallo, Aviso, Bonus, Record, Tiempo, Topos, Memory, Comienzo, Ojo,
        GaleriaTiro, DianaAzul, DianaRoja, Municion, 
        Refugio, Frutas, Troncos, Fuego, Tesoros, 
        Serpientes, Puerta, Llave, Agua, Esqueletos, 
        Murcielago, Corazon, Espada
    };
   
    [SerializeField] private Text mensaje; 
    [SerializeField] private Image imagen; 
    [SerializeField] Image imagenTipoMensaje;    
    [SerializeField] private Sprite spriteExito, spriteFallo, 
        spriteAviso, spriteBonus, spriteRecord, spriteReloj, spriteTopos, spriteMemory,
        spriteComienzo, spriteOjo, spriteDianaAzul, spriteDianaRoja, spriteMunicion,
        spriteRefugio, spriteFrutas, spriteTroncos, spriteFuego, spriteTesoros, 
        spriteSerpiente, spritePuerta, spriteLlave, spriteAgua, spriteEsqueletos, 
        spriteMurcielagos, spriteCorazon, spriteEspada;  
    
    void Awake()
    {

    }

    public void Ocultar()
    {
        gameObject.SetActive(false); 
        GetComponent<CanvasGroup>().alpha = 0f; 
    }

    private IEnumerator Animar()
    {
        Vector2 posicionOriginal = new Vector2(0, 400);
        gameObject.GetComponent<RectTransform>().anchoredPosition = posicionOriginal; 
        
        
        while(GetComponent<RectTransform>().anchoredPosition.y > 0)
        {
            // bajar la ui
            Vector2 posicion = GetComponent<RectTransform>().anchoredPosition; 
            posicion.y -= 100 * Time.deltaTime; 
            GetComponent<RectTransform>().anchoredPosition = posicion; 
            yield return null; 
        }       
    }

    public void Mostrar(string mensaje, Sprite imagen = null, TipoMensaje tipoMensaje = TipoMensaje.Aviso)
    {
        
        gameObject.SetActive(true);
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
            case TipoMensaje.Tiempo:
                imagenTipoMensaje.sprite = spriteReloj; 
            break;
            case TipoMensaje.Topos:
                imagenTipoMensaje.sprite = spriteTopos;
            break;
            case TipoMensaje.Memory:
                imagenTipoMensaje.sprite = spriteMemory;
            break;
            case TipoMensaje.Comienzo:
                imagenTipoMensaje.sprite = spriteComienzo;
            break;
            case TipoMensaje.Ojo:
                imagenTipoMensaje.sprite = spriteOjo; 
            break;
            case TipoMensaje.DianaAzul:
                imagenTipoMensaje.sprite = spriteDianaAzul; 
            break;
            case TipoMensaje.DianaRoja:
                imagenTipoMensaje.sprite = spriteDianaRoja; 
            break;
            case TipoMensaje.Municion:
                imagenTipoMensaje.sprite = spriteMunicion;
            break;
            case TipoMensaje.Refugio:
                imagenTipoMensaje.sprite = spriteRefugio; 
            break;
            case TipoMensaje.Frutas:
                imagenTipoMensaje.sprite = spriteFrutas; 
            break;
            case TipoMensaje.Troncos:
                imagenTipoMensaje.sprite = spriteTroncos; 
            break;
            case TipoMensaje.Fuego:
                imagenTipoMensaje.sprite = spriteFuego; 
            break;
            case TipoMensaje.Tesoros:
                imagenTipoMensaje.sprite = spriteTesoros; 
            break;
            case TipoMensaje.Serpientes:
                imagenTipoMensaje.sprite = spriteSerpiente; 
            break;
            case TipoMensaje.Puerta:
                imagenTipoMensaje.sprite = spritePuerta; 
            break;
            case TipoMensaje.Llave:
                imagenTipoMensaje.sprite = spriteLlave; 
            break;
            case TipoMensaje.Agua:
                imagenTipoMensaje.sprite = spriteAgua; 
            break;
            case TipoMensaje.Esqueletos:
                imagenTipoMensaje.sprite = spriteEsqueletos; 
            break;
            case TipoMensaje.Murcielago:
                imagenTipoMensaje.sprite = spriteMurcielagos; 
            break;
            case TipoMensaje.Corazon:
                imagenTipoMensaje.sprite = spriteCorazon; 
            break;
            case TipoMensaje.Espada:
                imagenTipoMensaje.sprite = spriteEspada; 
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
        //StartCoroutine(Alpha());
        GetComponent<CanvasGroup>().alpha = 1f; 
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
