using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esqueleto : MonoBehaviour
{
    
   
    public enum DireccionMovimiento
    {
        Vertical, 
        Horizontal
    };

    public DireccionMovimiento direccion = DireccionMovimiento.Vertical; 
    public float velocidad = 1f; 
    public float umbralBusqueda = 2f; 
    public float distanciaPorCubrir = 10f; 
    private Rigidbody2D rbody;
    private IsometricCharacterRenderer isoRenderer;
    private SpriteRenderer spriteRenderer; 
    private float distanciaRecorridaEnUnSentido = 0; // distancia recorrida en cada sentido
    private float sentido = 1f;  // sentido en la direccion del movimiento    
    private bool jugadorEncontrado = false; 
    private GameObject jugador; 
    private Vector3 posicionInicial; 

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
        jugador = GameObject.FindGameObjectWithTag("Player");
        posicionInicial = gameObject.transform.position; 
    }

    void Start()
    {        
        StartCoroutine(CorrutinaIA());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
            FindObjectOfType<TareaAventuras>().RecibirImpacto();
    }

    private IEnumerator CorrutinaIA()
    {
        // tomar decisiones cada 2s
        while(true)
        {      
            // medir distancia al jugador         
            Vector3 posicionJugador = jugador.transform.position; 

            // si esta dentro del alcance lo persigue
            if(Vector2.Distance(gameObject.transform.position, posicionJugador) < umbralBusqueda)
            {               
                jugadorEncontrado = true; 
            } else {               
                jugadorEncontrado = false; 
            }

            yield return new WaitForSeconds(1f);
        }
    }


    void FixedUpdate()
    {   
    
        // direccion y posicion del murcielago
        Vector2 vectorDireccion = Vector3.zero; 
        // posicoin del jugador
        Vector3 posicionJugador = jugador.transform.position; 

        // si el jugador esta cerca vamos hacia el         
        if(jugadorEncontrado)
        {
            vectorDireccion = posicionJugador - gameObject.transform.position;
           
        } else {

            // si no encontramos el jugador vamos a la 
            // posicion inicial
            // vectorDireccion = posicionInicial - gameObject.transform.position;
            
            vectorDireccion = new Vector2(0, 1) * sentido; 
            spriteRenderer.flipX = false; 

            if (distanciaRecorridaEnUnSentido > distanciaPorCubrir * 100) 
            {
                // cambio de sentido
                distanciaRecorridaEnUnSentido = 0; 
                sentido = sentido * -1;
            }
        }

        vectorDireccion.Normalize();

        Vector2 nuevaPosicion = 
            rbody.position + (Vector2) vectorDireccion * velocidad * Time.fixedDeltaTime;

        
        // acumulador
        distanciaRecorridaEnUnSentido += nuevaPosicion.magnitude; 

        // orientar sprite
        spriteRenderer.flipX = vectorDireccion.x > 0 ? true : false;

        // actualizar el enemigo 
        isoRenderer.SetDirection(vectorDireccion);
        rbody.MovePosition(nuevaPosicion);        

    }

}

