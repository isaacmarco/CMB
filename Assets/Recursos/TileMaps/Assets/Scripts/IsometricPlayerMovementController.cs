using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IsometricPlayerMovementController : MonoBehaviour
{
    
    public float movementSpeed = 5f;
    public RectTransform canvasRect; 
    IsometricCharacterRenderer isoRenderer;

    Rigidbody2D rbody;

    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();

        Camera.main.transparencySortMode = TransparencySortMode.CustomAxis;
        Camera.main.transparencySortAxis = new Vector3(0,1,0);
        

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        
        /*
        Vector2 currentPos = rbody.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector2 movement = inputVector * movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;
        isoRenderer.SetDirection(movement);
        rbody.MovePosition(newPos);*/


        //if(Input.GetMouseButton(0))
            Mover(Input.mousePosition);


    }

    private Vector2 ObtenerPosicionPantalla(Vector3 posicion)
    {   
         
        // transformar las coordeandas
        Vector2 posicionViewport = Camera.main.WorldToViewportPoint(posicion);
        Vector2 posicionEnPantalla = new Vector2(
            ((posicionViewport.x * canvasRect.sizeDelta.x)-(canvasRect.sizeDelta.x * 0.5f))+960,
            ((posicionViewport.y * canvasRect.sizeDelta.y)-(canvasRect.sizeDelta.y * 0.5f))+540
        );
        return posicionEnPantalla;
    }


    private void Mover(Vector3 destino)
    {
        // obtener la posicion en pantalla del jugador
        Vector2 p = ObtenerPosicionPantalla(gameObject.transform.position);
        //Debug.Log("Mouse " + destino + "; Jugador " + p);

        
        Vector3 _p = new Vector3(p.x, p.y, 0) ;

        //Vector3 p = gameObject.transform.position; 

        //destino.z = 1;         
        //p.z = 1; 
        //gameObject.transform.position = p; 

        Vector3 direccion = destino - _p;

     
        // normalizar la direccion
        direccion.Normalize();     
        isoRenderer.SetDirection(direccion);
        float velocidad = 1f; 
        Vector2 newPos = _p + direccion * velocidad * Time.fixedDeltaTime;
        rbody.velocity = direccion * velocidad; 
        //rbody.MovePosition(newPos);
    }
}
