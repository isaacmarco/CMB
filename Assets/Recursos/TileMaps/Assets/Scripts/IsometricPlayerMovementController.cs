using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tobii.Gaming;

public class IsometricPlayerMovementController : MonoBehaviour
{
    public AnimationCurve curvaVelocidad; 
    public float movementSpeed = 5f;
    public RectTransform canvasRect; 
    IsometricCharacterRenderer isoRenderer;
    private Vector2 puntoFiltrado = Vector2.zero;   
    Rigidbody2D rbody;
    private Coroutine corrutinaImpacto; 
    
    private Tarea tarea; 
   
    public void RecibirImpacto()
    {
        if(corrutinaImpacto != null)
            StopCoroutine(corrutinaImpacto);

        corrutinaImpacto = StartCoroutine(CorrutinaImpacto());
    }

    private IEnumerator CorrutinaImpacto()
    {
        Color normal = new Color(1f, 1f, 1f, 1f);
        Color impacto = new Color(1f, 0f, 0f, 1f);

        // hacemos parpadear el sprite
        for(int i=0; i<5; i++)
        {
            isoRenderer.SpriteRenderer.color = impacto; 
            yield return new WaitForSeconds(0.3f);
            isoRenderer.SpriteRenderer.color = normal; 
            yield return new WaitForSeconds(0.3f);
        }
        
        isoRenderer.SpriteRenderer.color = normal;                
    }

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        isoRenderer = GetComponentInChildren<IsometricCharacterRenderer>();
        Camera.main.transparencySortMode = TransparencySortMode.CustomAxis;
        Camera.main.transparencySortAxis = new Vector3(0,1,0);
        canvasRect = FindObjectOfType<Tarea>().CanvasRect;
        tarea = FindObjectOfType<Tarea>(); 
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        
        // evitar el movimiento cuando la tarea esta 
        // bloqueada
        if(tarea.TareaBloqueada)
            return; 
            
        // obtenemos la posicion mirada
        GazePoint gazePoint = TobiiAPI.GetGazePoint();

        if (gazePoint.IsValid)
		{
			Vector2 posicionGaze = gazePoint.Screen;	
            puntoFiltrado = Vector2.Lerp(puntoFiltrado, posicionGaze, 0.5f);
			Vector2 posicionEntera = new Vector2(
                Mathf.RoundToInt(puntoFiltrado.x), 
                Mathf.RoundToInt(puntoFiltrado.y)
            );

            Mover(posicionGaze);
            

		} else {        

            // mover usando el raton 
            Mover(Input.mousePosition);
        }


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
        Vector3 _p = new Vector3(p.x, p.y, 0) ;

        // obtenemos la direccion normalizada entre el jugador
        // y el desitno
        Vector3 direccion = destino - _p;        
        direccion.Normalize();      
      
        float distanciaJugadorDestino = Vector2.Distance(_p, destino);
 
        // evaluar curva de velocidad, partimos la distancia entre 100
        // para ajustarnos a la escala de la grafica en el eje X
        float velocidadInterpolada = curvaVelocidad.Evaluate( distanciaJugadorDestino / 100f );
            
        // nueva posicion
        Vector2 nuevaPosicion = rbody.position + (Vector2) direccion * velocidadInterpolada * Time.fixedDeltaTime;
        
        // solo desplazamos el jugador si tenemos un minimo de velocidad, 
        // ademas, tenemos que estar mirando por debajo de la interfaz de inventario
        float alturaComienzoInterfazInventario = 920; 
        if( velocidadInterpolada > 0.1f && destino.y < alturaComienzoInterfazInventario)
        {   
            // movemos el jugador 
            isoRenderer.SetDirection(direccion);
            rbody.MovePosition(nuevaPosicion);           
            
        } else {

            // detenemos el jugador por completo             
            isoRenderer.SetDirection(Vector2.zero);
            rbody.MovePosition(rbody.position);
        }
   

        
       
    }
}
