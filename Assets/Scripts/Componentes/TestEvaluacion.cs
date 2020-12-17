using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEvaluacion : MonoBehaviour
{

    public Transform pelota;

    float intervaloActualizacion = 0.016f; // 60HZ
    private int nTimerTicks = 0;     

    private double b0 = 2;
    private double a1 = -4;
    private double b1 = 3;
    private double a2 = -4.9;
    private double b2 = -3.6;
    private double a3 = 3.9;
    private double b3 = 4.5;
    private double a4 = 0;
    private double b4 = 1;
    private double a5 = -3.8;
    private double b5 = -0.5;
    private double a6 = 1; 
    private double b6 = 2.5;

    public int amplitud = 9; 

    void Start()
    {       
      
        StartCoroutine(Tick());        
    }

    private IEnumerator Tick()
    {

        while(true)
        {
            
            nTimerTicks++;
            float VI = Time.time; 

            // obtener el centro
            int centroX = Screen.width / 2;
            int centroY = Screen.height / 2;

            int xTarget =  (int) (amplitud *  // amplitud es 9 por defecto
                (
                b0 + a1 * Mathf.Sin(VI) + 
                b1 * Mathf.Cos(VI) + 
                a2 * Mathf.Sin(2 * VI) + 
                b2 * Mathf.Cos(2 * VI) + 
                a3 * Mathf.Sin(3 * VI) + 
                b3 * Mathf.Cos(3 * VI) + 
                a4 * Mathf.Sin(4 * VI) + 
                b4 * Mathf.Cos(4 * VI) + 
                a5 * Mathf.Sin(5 * VI) + 
                b5 * Mathf.Cos(5 * VI) + 
                a6 * Mathf.Sin(6 * VI) + 
                b6 * Mathf.Cos(6 * VI)
                )
            );
         
            pelota.GetComponent<RectTransform>().anchoredPosition = new Vector2
            (
                xTarget + centroX, centroY
            );

            yield return new WaitForSeconds(intervaloActualizacion);
        }
    }




}
