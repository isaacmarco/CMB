using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NivelMemoryScriptable : NivelScriptable
{
    [Header("Configuracion")]
    [Range(2, 10)]
    public int anchoMatriz = 4; 
    [Range(2, 10)]
    public int altoMatriz = 4; 
    public EstimulosTareaMemory[] listaEstimulosParaFormarParejas;
    [Header("Dificultad")]    
    public float tiempoParaSeleccionarPareja = 2f; 
    [Header("Requisitos para la superacion")]    
    public int erroresParaPerder;      
    
    private void OnValidate()
    {
        // comprobaciones de validez del nivel de dificultad
        
        int numeroTarjetas = anchoMatriz * altoMatriz; 

        if(listaEstimulosParaFormarParejas == null)
        {
            Debug.LogError("La lista de estimulos esta vacia");
            return; 
        }

        int numeroEstimulos = listaEstimulosParaFormarParejas.Length; 

        // como el juego es de parejas, el numero de tarjetas
        // en la matriz debe ser par 
        if ( numeroTarjetas % 2 != 0 )
        {            
            Debug.LogError("El numero de elementos de la matriz debe ser par");
            return; 
        }

        // comprobar que hay estimulos
        if (numeroEstimulos == 0)
        {
            //configuracionCorrecta = false; 
            Debug.LogError("La lista de estimulos esta vacia");
            return; 
        }
        
        // el numero de estimulos correcto es la mitad del numero
        // de tarjetas que hay en la matriz porque van por parejas
        int numeroCorrectoEstimulos = numeroTarjetas / 2;

        // tiene que haber suficientes estimulos para cubrir
        // la matriz de tarjetas 
        if( numeroEstimulos < numeroCorrectoEstimulos)
        {           
            Debug.LogError("No hay suficientes estimulos para esta matriz, deben ser " + numeroCorrectoEstimulos);
            return; 
        }
        
        if( numeroEstimulos > numeroCorrectoEstimulos)
        {
            Debug.LogError("Hay demasiados estimulos para esta matriz, deben ser " + numeroCorrectoEstimulos);
            return; 
        }

        // comprobar que los estimulos no esten duplicados en la lista
        
        foreach(EstimulosTareaMemory estimulo in listaEstimulosParaFormarParejas)
        {
            if(EstimuloDuplicado(estimulo))
            {
                Debug.LogError("Hay estimulos duplicados en la lista");
                return;
            }
        }

        // si el metodo llega a este punto la configuracion es correcta
        Debug.Log("La configuracion del nivel es correcta"); 


    }

    private bool EstimuloDuplicado(EstimulosTareaMemory estimuloBuscado)
    {
        int contador = 0; 
        foreach(EstimulosTareaMemory estimulo in listaEstimulosParaFormarParejas)
        {
            if(estimulo == estimuloBuscado)
            {
                contador++;
            }
        }

        return contador > 1; 
    }

    
}
