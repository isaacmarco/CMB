using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mina : MonoBehaviour
{    
    private float momento; 
    private float vida = 100; 
  

    public float Vida {
        get { return this.vida; }
    }

    public void Iniciar(float momento)
    {
       
        
        gameObject.GetComponent<Collider>().enabled = false; 

        this.momento = momento;         
        Vector3 posicionEnCurva = FindObjectOfType<TareaNaves>().EvaluarCurvas(momento);
        // aleatorizamos
        posicionEnCurva += new Vector3(
            Random.Range(-8, 9),  0, Random.Range(-8, 9)
        );
        //gameObject.transform.position = posicionEnCurva; 
        
        Vector3 direccionRayo = transform.TransformDirection(Vector3.down);
        // el punto de origen del rayo no es la mina, es un punto situado encima
        Vector3 origenRayo = new Vector3(
            posicionEnCurva.x, 100, posicionEnCurva.z
        );

        RaycastHit hit; 
        if (Physics.Raycast(origenRayo, direccionRayo, out hit, 1000))
        {               
            if(hit.collider.gameObject.name == "Terreno")
            {
                // obtenemos la altura, le añadimos un desplazamiento
                // y la asignamos a la nave
                float altura = hit.point.y; 
                float desplazamiento = 1f;                   
                float alturaRandom = Random.Range(0, 9) ;
                posicionEnCurva.y = altura + desplazamiento + alturaRandom; 
                gameObject.transform.position = posicionEnCurva;                 
            }
        }

        gameObject.GetComponent<Collider>().enabled = true; 

        // la diana mira a la camara
        gameObject.transform.LookAt(Camera.main.transform.position);

        rotacion = new Vector3(
            Random.Range(-90, 90),
            Random.Range(-90, 90),
            Random.Range(-90, 90)
        );
    }

    private Vector3 rotacion; 
    void Update()
    {
        // si el jugador esta en un momento posterior
        // a la mina, se destruye
        if(FindObjectOfType<NaveJugador>().Tiempo > momento)
            DestruirPorOmision();

        float velocidadRotacion = 1f; 
        /*
        gameObject.transform.Rotate(rotacion * velocidadRotacion * Time.deltaTime);*/
    }

    private void DestruirPorOmision()
    {
        FindObjectOfType<TareaNaves>().Error();
        Destroy(this.gameObject);
    }

    private void DestruirPorImpacto()
    {
        FindObjectOfType<TareaNaves>().Acierto();
        Destroy(this.gameObject);
    }
    
    public void RecibirImpacto()
    {
        FindObjectOfType<NaveJugador>().Disparar(gameObject.transform.position);
        float vidaPorImpacto = 150;         
        vida -= vidaPorImpacto * Time.deltaTime;        
        if(vida <= 0)
            DestruirPorImpacto();
        
    }
}
