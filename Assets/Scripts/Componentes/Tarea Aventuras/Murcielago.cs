using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Murcielago : MonoBehaviour
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

    void Start()
    {
        StartCoroutine(CorrutinaIA());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
            FindObjectOfType<TareaAventuras>().RecibirImpacto();
    }

    private float distanciaRecorridaEnUnSentido = 0; // distancia recorrida en cada sentido
    private float sentido = 1f;  // sentido en la direccion del movimiento

    private bool detenido = false; 

    private IEnumerator CorrutinaDetener()
    {   
        float duracion = Random.Range(1, 3); 
        detenido = true; 
        yield return new WaitForSeconds(duracion);
        detenido = false;         
    }

    private IEnumerator CorrutinaIA()
    {
        // tomar decisiones cada 2s
        while(true)
        {            
            // comprobar si se detiene
            if(Random.value > 0.8f && !detenido)
            {
                StartCoroutine(CorrutinaDetener());
            }

            // comprobar si hacemos un cambio de sentido
            if(Random.value > 0.7f && !detenido)
                CambioSentido();

            yield return new WaitForSeconds(2f);
        }
    }

    private void CambioSentido()
    {
        // cambio de sentido
        distanciaRecorridaEnUnSentido = 0; 
        sentido = sentido * -1;
    }

    void FixedUpdate()
    {   
        
     
        if(detenido)
            return; 

        // direccion y posicion del murcielago
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
            CambioSentido();
        


        // actualizar el enemigo 
        isoRenderer.SetDirection(vectorDireccion);
        rbody.MovePosition(nuevaPosicion);           
       
    }
}
