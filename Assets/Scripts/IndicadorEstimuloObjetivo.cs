using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicadorEstimuloObjetivo : MonoBehaviour
{
   
    [SerializeField] Sprite[] estimulos; 
    [SerializeField] Image indicadorEstimulo; 

    private TareaTopos tarea;

    void Start()
    {
        tarea = FindObjectOfType<TareaTopos>();
    }

    void Update()
    {
        if(tarea!=null)
            indicadorEstimulo.sprite = estimulos[ (int) tarea.Nivel.estimuloObjetivo ];
    }
    
}
