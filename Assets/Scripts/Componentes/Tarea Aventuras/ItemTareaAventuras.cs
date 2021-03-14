using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTareaAventuras : MonoBehaviour
{
    
    public ObjetosAventuras tipo; 
    
    void OnTriggerEnter2D(Collider2D col)
    {
        CogerItem();
    }

    private void CogerItem()
    {
        // recoger el item y destruirlo
        if (FindObjectOfType<TareaAventuras>().RecogerItem(tipo) )
        {
            Destroy(this.gameObject);
        } else {
            // no habia espacio
        }
            
        
    }

}
