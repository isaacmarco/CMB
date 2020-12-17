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

    private IEnumerator Desplazar()
    {
        /*
        float x = posicionInicial.x; 
        float z = posicionInicial.z; 
        float y = posicionInicial.y; 

        int destinoZ = 10; 
        if(Random.value > 0.5)
            destinoZ = -10;
        float velocidad = 8; 

        iTween.MoveTo(gameObject, 
            iTween.Hash(
                "y", y + 2, 
                "z", z + destinoZ,                
            "looptype", iTween.LoopType.pingPong,
            "easetype", iTween.EaseType.linear, "speed", velocidad)
        );*/
        

        yield return null;
    }

    private Vector3 posicionInicial; 

    public void Iniciar(float momento)
    {
       
        if(Random.value > 0.5f)
            haciaIzquierda = true; 

        gameObject.GetComponent<Collider>().enabled = false; 

        this.momento = momento;         
        Vector3 posicionEnCurva = FindObjectOfType<TareaNaves>().EvaluarCurvas(momento);
        // aleatorizamos
        posicionEnCurva += new Vector3(
            Random.Range(-12, 13),  0, Random.Range(-20, 21) // profundiad es x, lateral es z
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
                float desplazamiento = 1.3f;                   
                float alturaRandom = Random.Range(0, 9) ;
                posicionEnCurva.y = altura + desplazamiento + alturaRandom; 
                gameObject.transform.position = posicionEnCurva;                 
            }
        }

        // recordamos la posicion inicial
        posicionInicial = gameObject.transform.position; 

        gameObject.GetComponent<Collider>().enabled = true; 

        // la diana mira a la camara
        gameObject.transform.LookAt(Camera.main.transform.position);

        rotacion = new Vector3(
            Random.Range(-90, 90),
            Random.Range(-90, 90),
            Random.Range(-90, 90)
        );

        //StartCoroutine(Desplazar());
    }

    private Vector3 rotacion; 
    private bool haciaIzquierda; 

    void Update()
    {
        // si el jugador esta en un momento posterior
        // a la mina, se destruye
        if(FindObjectOfType<NaveJugador>().Tiempo > momento)
            DestruirPorOmision();
        
         // la diana mira a la camara
        //gameObject.transform.LookAt(Camera.main.transform.position);
        /*
        float velocidad = 10f; 
        if(haciaIzquierda)
            velocidad = velocidad * -1;
        gameObject.transform.Translate
        (
            Vector3.right * Time.deltaTime * velocidad
        );*/
       

        /*
        Vector3 pos = gameObject.transform.position; 

              



        Vector3 direccionRayo = transform.TransformDirection(Vector3.down);
        // el punto de origen del rayo no es la mina, es un punto situado encima
        Vector3 origenRayo = new Vector3(
            pos.x, 100, pos.z
        );

        RaycastHit hit; 
        if (Physics.Raycast(origenRayo, direccionRayo, out hit, 1000))
        {               
            if(hit.collider.gameObject.name == "Terreno")
            {
                // obtenemos la altura, le añadimos un desplazamiento
                // y la asignamos a la nave
                float altura = hit.point.y; 
                float desplazamiento = 1.5f;                   
               
                pos.y = altura + desplazamiento; 
                gameObject.transform.position = pos;
            }
        } */
       
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
