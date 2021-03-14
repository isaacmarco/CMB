using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventario : MonoBehaviour
{
    
    private bool libre; 
    private ObjetosAventuras tipo;     

    public bool Libre {
        get { return libre;}
    }
    
    public ObjetosAventuras Tipo {
        get { return tipo; }
    }
    
    void Start()
    {
        // liberar
        libre = true; 
        // TODO: CAMBIAR MARCO
    }

    public void Agregar(ObjetosAventuras tipo)
    {
        this.tipo = tipo; 
        libre = false; 
    }

    public void Usar()
    {
        libre = true; 
    }
}
