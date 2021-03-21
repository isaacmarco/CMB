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
    public float distanciaPorCubrir = 10f; 
    private Rigidbody2D rbody;
    private IsometricCharacterRenderer isoRenderer;
    private SpriteRenderer spriteRenderer; 

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
            FindObjectOfType<TareaAventuras>().RecibirImpacto();
    }

    private float distanciaRecorridaEnUnSentido = 0; // distancia recorrida en cada sentido
    private float sentido = 1f;  // sentido en la direccion del movimiento

    void FixedUpdate()
    {   
       
        // direccion y posicion de la serpiente
        Vector2 vectorDireccion = new Vector2(1, 0) * sentido;

        if(direccion == DireccionMovimiento.Vertical)
        {
            vectorDireccion = new Vector2(0, 1) * sentido; 
            spriteRenderer.flipX = false; 

        } else {

            // se esta moviendo en horizontal, actualizamos el 
            // sentido del sprite segun el sentido 
            spriteRenderer.flipX = sentido > 0 ? false : true; 
        }

        Vector2 nuevaPosicion = 
            rbody.position + (Vector2) vectorDireccion * velocidad * Time.fixedDeltaTime;

        // acumulador
        distanciaRecorridaEnUnSentido += nuevaPosicion.magnitude; 

        // comprobamos si ha llegado al final
       
        if (distanciaRecorridaEnUnSentido > distanciaPorCubrir * 100) 
        {
            // cambio de sentido
            distanciaRecorridaEnUnSentido = 0; 
            sentido = sentido * -1;
        }

        // actualizar el enemigo 
        isoRenderer.SetDirection(vectorDireccion);
        rbody.MovePosition(nuevaPosicion);           
      
    }

}
