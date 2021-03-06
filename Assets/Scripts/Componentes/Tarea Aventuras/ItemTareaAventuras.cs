﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class ItemTareaAventuras : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer; 
    private GestorSpritesAventuras gestorSprites;

    public ObjetosAventuras tipo; 
    
   
     

    void OnTriggerEnter2D(Collider2D col)
    {
        // solo el jguador puede recoger objetos
        if(col.gameObject.tag != "Player")
            return; 

        // se puede consumir directamente o coger
        // dependiendo del tipo
        if(tipo == ObjetosAventuras.Corazon || tipo == ObjetosAventuras.Oro || 
        tipo == ObjetosAventuras.Lingote || tipo == ObjetosAventuras.Monedas)
        {
            ProcesarItem();
        } else {
            CogerItem();
        }
    }

 

    void Start()
    {
        // creamos las referencias
        gestorSprites = FindObjectOfType<GestorSpritesAventuras>();       
        // spriteRenderer = GetComponent<SpriteRenderer>();
        // actualizamos el sprite
        spriteRenderer.sprite = gestorSprites.ObtenerSprite(tipo);    
      
        iTween.MoveBy(spriteRenderer.gameObject, 
            iTween.Hash("y", 0.2f, "loopType", "pingPong" 
        ));

    }

    private void ProcesarItem()
    {
        FindObjectOfType<TareaAventuras>().ConsumirItem(tipo);
        Destroy(this.gameObject);
    }

    private void CogerItem()
    {
        // recoger el item y destruirlo, se devuelve true
        // si habia espacio en el inventario
        if (FindObjectOfType<TareaAventuras>().RecogerItem(tipo) )
        {
            Destroy(this.gameObject);
        } else {
            // no habia espacio en el inventario para
            // recoger el objeto, no hacemos nada
        }            
        
    }

}
