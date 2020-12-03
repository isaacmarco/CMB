using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveJugador : MonoBehaviour
{    
    [SerializeField] private GameObject laserPrefab; 
    private TareaNaves tarea; 
    // energia de la nave 
    private int vida = 100; 
    // el momento actual para evaluar las curvas
    private float tiempo; 
    private float velocidad = 1; 
    private bool disparando = false; 
    private Vector3 posicionObjetivo; 

    public float Tiempo
    {
        get { return this.tiempo;}
    }
    private void InstanciarLaser()
    {
        // instanciamos un nuevo disparo  y hacemos
        // que se diriga al objetivo actual
        GameObject laser = (GameObject) Instantiate(laserPrefab);
        laser.name = "laser";
        laser.transform.position = gameObject.transform.position; 
        laser.GetComponent<Laser>().Disparar(posicionObjetivo);
    }

    private IEnumerator GeneracionDisparos()
    {
        while(true)
        {
            if(disparando && Random.value > 0.8f)
            {
                InstanciarLaser();
            }
            disparando = false; 
            yield return null;             
        }
    }
    public void Disparar(Vector3 posicion)
    {        
        // estamos mirando al objetivo, instanciamos
        // lasers como feedback 
        this.posicionObjetivo = posicion; 
        disparando = true; 
    }
    
    void Start()
    {
        tarea = FindObjectOfType<TareaNaves>(); 
        gameObject.transform.position = tarea.PosicionInicial; 
        StartCoroutine(GeneracionDisparos());

    }

    void FixedUpdate()
    {
                // configraucion de la nave
        velocidad = tarea.Configuracion.velocidadDeLaNave;
        
        // calcular el momento actual
        tiempo += Time.deltaTime * velocidad; 
        // calcular momento siguiente
        float incrementoTiempo = 0.1f; 
        float tiempoSiguiente = tiempo + incrementoTiempo; 
        // la nave se posiciona evaluando la curva en t, 
        // y orienandose a t+1
        if(tarea!=null)
        {
            gameObject.transform.position = tarea.EvaluarCurvas(tiempo);
            gameObject.transform.LookAt(tarea.EvaluarCurvas(tiempoSiguiente));

            // ajustar la altura mediante raycasting
            Vector3 direccionRayo = transform.TransformDirection(Vector3.down);
            // el punto de origen del rayo no es la nave, es un punto
            // situado encima de la nave
            Vector3 origenRayo = new Vector3(
                transform.position.x, 100, transform.position.z
            );
            // ralizamos el test 
            RaycastHit hit; 
            if (Physics.Raycast(origenRayo, direccionRayo, out hit, 1000))
            {
                if(hit.collider.gameObject.name == "Terreno")
                {
                    // obtenemos la altura, le añadimos un desplazamiento
                    // y la asignamos a la nave
                    float altura = hit.point.y; 
                    float desplazamiento = 1f; 
                    Vector3 posicion =  gameObject.transform.position;
                    posicion.y = altura + desplazamiento; 
                    gameObject.transform.position = posicion; 
                }
            }
                
        }
    }

  
    

    public void RecibirImpacto()
    {
    }


}
