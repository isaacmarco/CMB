using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reloj : MonoBehaviour
{
    [SerializeField] private Text reloj; 
    
    private int tiempo = 0; 

    public int Tiempo {
        get { return this.tiempo;}
    }
    
    void Start()
    {
        reloj.text = string.Empty; 
    }

    private IEnumerator CorrutinaReloj()
    {
        // contamos el timepo que pasa
        while(true)
        {
            yield return new WaitForSeconds(1f);
            tiempo ++;
            reloj.text = tiempo.ToString();
        }
    }

    private IEnumerator CorrutinaContraReloj(int tiempoInicio)
    {       
        
        // cuenta atras
        while(tiempoInicio > 0)
        {
            // esperamos y restamos un segundo
            yield return new WaitForSeconds(1f); 
            tiempoInicio --; 
            // actualizamos el reloj 
            reloj.text = tiempoInicio.ToString();

        }

        Debug.Log("Cronometro a cero");
        // notificar a la tarea que se ha acabado el timpo
        FindObjectOfType<Tarea>().TiempoExcedido();
    }

    public void IniciarCuentaAtras(int tiempoInicio)
    {        
        StartCoroutine(CorrutinaContraReloj(tiempoInicio));
    }
    public void IniciarReloj()
    {
        StartCoroutine(CorrutinaReloj());
    }

}
