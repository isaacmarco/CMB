using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugadorTareaDisparo : MonoBehaviour
{
    
    [SerializeField] private GameObject laserPrefab; 
    private bool alterno = false; 
    private bool disparando = false; 
    private Vector3 posicionObjetivo; 

    void Start()
    {        
        StartCoroutine(GeneracionDisparos());
    }

    private void InstanciarLaser()
    {
        // instanciamos un nuevo disparo  y hacemos
        // que se diriga al objetivo actual
        GameObject laser = (GameObject) Instantiate(laserPrefab);
        laser.name = "laser";

        laser.transform.parent = gameObject.transform; 
        laser.transform.localPosition = Vector3.zero; 
        laser.transform.position = gameObject.transform.position; 
        
        float separacionLaser = 0.2f; 

        separacionLaser = 0.05f;

        laser.transform.localPosition = new Vector3
        (separacionLaser, 0, 0);

        if(alterno)
        {
            laser.transform.localPosition = new Vector3
            (-separacionLaser, 0, 0);
            alterno = false; 
        } else {
            alterno = true; 
        }
        
        laser.transform.parent = null; 

        laser.GetComponent<Laser>().Disparar(posicionObjetivo);

        // audio
        Audio audio = FindObjectOfType<Audio>();
        if(!audio.ASinteraccion.isPlaying)
        {
            //if(Random.value < 0.5f)
            //{
                float pitch = Random.Range(0.5f, 0.6f );
                audio.ASinteraccion.pitch = pitch;
                audio.FeedbackInteraccion();

            //}
        }
    }

      private IEnumerator GeneracionDisparos()
    {
        while(true)
        {
            
            if(disparando && Random.value > 0.8f)
            {
                yield return new WaitForSeconds(0.1f);
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
    
    

}
