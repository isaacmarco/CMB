using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serpiente : MonoBehaviour
{
    
    public enum DireccionMovimiento
    {
        Vertical, 
        Horizontal
    };

    public DireccionMovimiento direccion = DireccionMovimiento.Vertical; 
    public float velocidad = 1f; 
    public float distanciaCubierta = 10f; 
    Rigidbody2D rbody;
    IsometricCharacterRenderer isoRenderer;

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        FindObjectOfType<TareaAventuras>().RecibirImpacto();
    }

    private float distanciaRecorridaEnUnSentido = 0; // distancia recorrida en cada sentido
    private float sentido = 1f;  // sentido en la direccion del movimiento

    void FixedUpdate()
    {
        /*           
        isoRenderer.SetDirection(direccion);
        rbody.MovePosition(nuevaPosicion);           
        */
        switch(direccion)
        {
            case DireccionMovimiento.Horizontal:
            break;

            case DireccionMovimiento.Vertical:
            break;
        }
    }

}
