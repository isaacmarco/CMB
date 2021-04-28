using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemInventario : MonoBehaviour
{
    private GestorSpritesAventuras gestorSprites; 
    [SerializeField]
    private Image imagen;
    private bool libre; 
    [SerializeField]
    private ObjetosAventuras tipo;     
   

    public bool Libre {
        get { return libre;}
    }
    
    public ObjetosAventuras Tipo {
        get { return tipo; }
    }
    
    void Start()
    {
        // inciar
        gestorSprites = FindObjectOfType<GestorSpritesAventuras>();        
        Actualizar();
    }

    private void Actualizar()
    {
        // marcar como libre y cambiar la imagen
        libre = tipo == ObjetosAventuras.Ninguno;
        imagen.enabled = !libre;         
        imagen.sprite = gestorSprites.ObtenerSprite(tipo);
    }

    public void Agregar(ObjetosAventuras tipo)
    {
        this.tipo = tipo; 
        Actualizar();
    }

    

    public void Usar()
    {
        // saltamos los items que no se pueden usar 
        if(tipo == ObjetosAventuras.Diamante || tipo == ObjetosAventuras.Esmeralda ||
        tipo == ObjetosAventuras.Rubi || tipo == ObjetosAventuras.Tronco || 
        tipo == ObjetosAventuras.Espada || tipo == ObjetosAventuras.Llave ||
        tipo == ObjetosAventuras.Lingote)
        {
            Debug.Log("Este item no se puede usar");
            return; 
        }

        FindObjectOfType<TareaAventuras>().UsarItem(tipo);
        this.tipo = ObjetosAventuras.Ninguno; 
        Actualizar();        
    }

}
