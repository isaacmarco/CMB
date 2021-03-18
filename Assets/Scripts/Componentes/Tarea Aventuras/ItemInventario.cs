﻿using System.Collections;
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
        // skip de los items que no son usable
        // TODO
        //if(tipo == ObjetosAventuras.Diamante)
        //    return;


        FindObjectOfType<TareaAventuras>().UsarItem(tipo);
        this.tipo = ObjetosAventuras.Ninguno; 
        Actualizar();
        
    }
}
