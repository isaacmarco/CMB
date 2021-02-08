using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 


public class Interfaz : MonoBehaviour
{
    public Text debug; 
    public Text puntuacion; 
    private TareaTopos tarea; 

    void Start()
    {
        tarea = gameObject.GetComponent<TareaTopos>();
    }
    
    void Update()
    {
        
        if(tarea!=null)    
        {          
  
            string informacion = "aciertos " + tarea.Aciertos + "\n" + "errores " + tarea.Errores + "\n" + "omisiones " + tarea.Omisiones; 
            debug.text = informacion; 

            puntuacion.text = tarea.Puntuacion.ToString(); 

        }
   

    }
}
