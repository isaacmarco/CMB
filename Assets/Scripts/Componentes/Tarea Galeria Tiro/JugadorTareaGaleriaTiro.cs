using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;
public class JugadorTareaGaleriaTiro : MonoBehaviour
{
    
    [SerializeField] private GameObject laserPrefab; 
    [SerializeField] private GameObject arma; 
    [SerializeField] private GameObject mira; 

    
    private bool alterno = false; 
    private bool disparando = false; 
    private Vector3 posicionObjetivo; 
    
    public GameObject Arma {
        get { return arma; }
    }
    private float municion = 100f; 
    
    public void Orientar(GameObject o)
    {
        //objetivo = o; 
        
    }

    private GameObject objetivo; 
    
    private GameObject MouseTest()
    {
        RaycastHit hit; 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            if ( Physics.Raycast (ray,out hit,100.0f)) {
                return hit.collider.gameObject; 
            }   
            return null; 
  
    }
    void Update()
    {
         
        // actualizar alpha del punto dependiendo de si estamos
        // mirando a un objeto del juego 
        GameObject objetoFijado = TobiiAPI.GetFocusedObject();
        
        // SOLO PARA EL EDITOR!
        //objetoFijado = MouseTest();

       float velocidad = 10f; 

        if(objetoFijado!=null)
        {
            Debug.Log(objetoFijado.name);
          
                arma.transform.rotation = Quaternion.Slerp(
                arma.transform.rotation, (
                Quaternion.LookRotation(
                    (objetoFijado.transform.position) - arma.transform.position)), Time.deltaTime * velocidad);  
            
        } else {
            
         
                
                arma.transform.rotation = Quaternion.Slerp(
                arma.transform.rotation, (
                Quaternion.LookRotation(
                    (mira.transform.position) - arma.transform.position)), Time.deltaTime * velocidad); 
           
            
        }

       

        // obtenemos la posicion en pantalla a la que se mira
        /*
        Vector2 mirando = FindObjectOfType<PuntoVision>().PosicionEnPantalla; 

        int mitadPantallaHorizontal = Screen.width / 2; 
        int mitadPantallaVertical = Screen.height / 2; 

        if(mirando.x > mitadPantallaHorizontal)
        {

        } else {

        }

        if(mirando.y > mitadPantallaVertical)
        {

        } else {

        }*/

      /*
        RaycastHit hit;        
        if (Physics.Raycast(
            transform.position, 
            transform.TransformDirection(Vector3.forward), 
            out hit, Mathf.Infinity))
        {
            arma.transform.LookAt(hit.point);
        }*/
    }

    public bool HayMunicion()
    {
        // si no es necesario recargar en el nivel entonces
        // siempre devolvemos q hay municion 
        if ( !FindObjectOfType<TareaGaleriaTiro>().Nivel.esNecesarioRecargar)
            return true; 
            
        return municion > 0; 
    }
    public int Municion {
        get { return (int) municion;}
    }

    public void Recargar()
    {       
        municion = FindObjectOfType<TareaGaleriaTiro>().Nivel.municionCargador; 
    }

    void Start()
    {                   
        municion = FindObjectOfType<TareaGaleriaTiro>().Nivel.municionCargador; 
        StartCoroutine(GeneracionDisparos());
    }

    private void InstanciarLaser()
    {
        // instanciamos un nuevo disparo  y hacemos
        // que se diriga al objetivo actual
        GameObject laser = (GameObject) Instantiate(laserPrefab);
        laser.name = "laser";

        laser.transform.parent = arma.transform; //  Camera.main.gameObject.transform; 
      
        float separacionLaser = 0.02f; 
        separacionLaser = 0.005f;

        float altura = -0.03f; 
        float d = 0.05f; 

        laser.transform.localPosition = new Vector3 (separacionLaser, altura, d);

        if(alterno)
        {
            laser.transform.localPosition = new Vector3
            (-separacionLaser, altura, d);
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
                ControlMunicion();
                

            }
            disparando = false; 
            yield return null;             
        }
    }
    private void ControlMunicion()
    {
        // actualizar municion  
        float gasto = 10f;        
        municion -= gasto; 
    }
    
    public void Disparar(Vector3 posicion)
    {        
        // estamos mirando al objetivo, instanciamos
        // lasers como feedback 
        this.posicionObjetivo = posicion; 
        disparando = true; 
    }
    
    

}
