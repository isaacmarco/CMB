using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ReproductorRegistros : MonoBehaviour
{
    
    public TextAsset fichero; 
    private ArrayList registros; 
    public GameObject puntoVision; 
    [Header("Tarea de topos")]
    public GameObject[] estimulosTareaTopos; 
    public GameObject estimuloObjetivoTareaTopos; 
    [Header("Tarea de galeria de tiro")]
    public GameObject dianaA; 
    public GameObject dianaB; 
    public GameObject dianaC; 

    public bool reproducirTopos; 
    public bool reproducirGaleriaTiro; 

    void Start()
    {
        if(fichero==null)
            return; 
        
        if(reproducirTopos)
        {
            registros = CargarFicheroTopos();
            StartCoroutine(ReproducirRegistroTopos());
        } else if (reproducirGaleriaTiro)
        {
            registros = CargarFicheroGaleriaTiro();
            StartCoroutine(ReproducirRegistroGaleriaTiro());
        } else
        {
            // memory
        }
        
       
    }
    
    private IEnumerator ReproducirRegistroGaleriaTiro()
    {
            
        Debug.Log("Comenzado la reproduccion");
       
      
        int contador = 0; 
        float esperaEntreRegistros = 0.01f; 

        while(contador < registros.Count)
        {   
            // extraemos un registro
            RegistroPosicionOcularTareaGaleriaTiro registro = 
                (RegistroPosicionOcularTareaGaleriaTiro) registros[contador];
            
            // posicionamos el punto de vision 
            puntoVision.GetComponent<RectTransform>().anchoredPosition = new Vector2
            (
                registro.X, registro.Y
            );

            // visibilidad y posicion de los estimulos
            EstimuloTareaGaleriaTiro[] estimulos = registro.Estimulos;
            GameObject[] dianas = {dianaA, dianaB, dianaC};

            for(int i=0; i<dianas.Length; i++)
            {
                dianas[i].SetActive(false); 
                if(estimulos[i]!=EstimuloTareaGaleriaTiro.Ninguno)
                {
                    dianas[i].SetActive(true);                     
                }
            }

            dianaA.GetComponent<RectTransform>().anchoredPosition = new Vector2
                (registro.AX, registro.AY);
            
            dianaB.GetComponent<RectTransform>().anchoredPosition = new Vector2
                (registro.BX, registro.BY);

            dianaC.GetComponent<RectTransform>().anchoredPosition = new Vector2
                (registro.CX, registro.CY);
            
                

            contador++;
            yield return new WaitForSeconds(esperaEntreRegistros);
        }
        Debug.Log("Reproduccion terminada");
    }
    private IEnumerator ReproducirRegistroTopos()
    {

        Debug.Log("Comenzado la reproduccion");

        // iniciar cosas de la tarea

        // posicionar estimulos
        Vector2[] posiciones = {
            new Vector2(680,669),
            new Vector2(950,669),
            new Vector2(1221,669),
            new Vector2(638,459),
            new Vector2(944,456),
            new Vector2(1250,460),
            new Vector2(589,170),
            new Vector2(947,166),
            new Vector2(1314,163) 
        };
        Vector2 offset = new Vector2(0, 150);
        for(int i=0; i<estimulosTareaTopos.Length; i++)
        {
            estimulosTareaTopos[i].GetComponent<RectTransform>().anchoredPosition = posiciones[i] + offset;
        }

        int contador = 0; 
        float esperaEntreRegistros = 0.01f; 

        while(contador < registros.Count)
        {   
            // extraemos un registro
            RegistroPosicionOcultarTareaTopos registro = (RegistroPosicionOcultarTareaTopos) registros[contador];
            
            // posicionamos el punto de vision 
            puntoVision.GetComponent<RectTransform>().anchoredPosition = new Vector2
            (
                registro.X, registro.Y
            );

            Vector2 dimensiones = new Vector2(25, 25);
            Vector2 dimensionesObjetivo = new Vector2(70, 70); 
             
            // ocultamos todos los estimulos de la matriz para
            // mostrar solo los visibles 
            EstimulosTareaTopos[] matriz = registro.Matriz;
            for(int i=0; i<matriz.Length; i++)
            {
                estimulosTareaTopos[i].SetActive(false);
                if(matriz[i] != EstimulosTareaTopos.Ninguno)
                    estimulosTareaTopos[i].SetActive(true);
                
                // si el estimulo es objetivo le damos mayor tamaño
                estimulosTareaTopos[i].GetComponent<RectTransform>().sizeDelta = dimensiones; 
                if(registro.EstimuloObjetivo == matriz[i])
                    estimulosTareaTopos[i].GetComponent<RectTransform>().sizeDelta = dimensionesObjetivo;
                    
            }
            
                

            contador++;
            yield return new WaitForSeconds(esperaEntreRegistros);
        }
        Debug.Log("Reproduccion terminada");
    }

    private ArrayList CargarFicheroGaleriaTiro()
    {
        registros = new ArrayList();
        
        /*
         AX; AY; BX; BY; CX; CY; recargando?; municion
        0,0000;-1;-1;Ninguno;Ninguno;Ninguno;-1;-1;-1;-1;-1;-1;True;500
        */
        
        // obtenemos todas las lineas
        string[] lineas = fichero.text.Split('\n');
        // convertimos cada linea en un nuevo registro
        for(int i=0; i<lineas.Length; i++)
        {
            string[] campos = lineas[i].Split(';');
            string ctiempo = campos[0];
            string cx = campos[1];
            string cy = campos[2];
            string cA = campos[3];
            string cB = campos[4];
            string cC = campos[5];
            string cAx = campos[6];
            string cAy = campos[7];
            string cBx = campos[8];
            string cBy = campos[9];
            string cCx = campos[10];
            string cCy = campos[11];

            // nuevo registro 
            RegistroPosicionOcularTareaGaleriaTiro registro = new RegistroPosicionOcularTareaGaleriaTiro
            (
                0, int.Parse(cx), int.Parse(cy),
                (EstimuloTareaGaleriaTiro) Enum.Parse(typeof(EstimuloTareaGaleriaTiro), cA),
                (EstimuloTareaGaleriaTiro) Enum.Parse(typeof(EstimuloTareaGaleriaTiro), cB),
                (EstimuloTareaGaleriaTiro) Enum.Parse(typeof(EstimuloTareaGaleriaTiro), cC),
                int.Parse(cAx), int.Parse(cAy),
                int.Parse(cBx), int.Parse(cBy),
                int.Parse(cCx), int.Parse(cCy), 
                false, 0
            );
         
            registros.Add(registro);
  
        }

        Debug.Log("Registros creados: " + registros.Count);
        return registros; 
    }

    private ArrayList CargarFicheroTopos()
    {
        registros = new ArrayList();

        // obtenemos todas las lineas
        string[] lineas = fichero.text.Split('\n');
        // convertimos cada linea en un nuevo registro
        for(int i=0; i<lineas.Length; i++)
        {
            string[] campos = lineas[i].Split(';');
            string ctiempo = campos[0];
            string cObjetivo = campos[1];
            string cx = campos[2];
            string cy = campos[3];
            // matriz         
         
            EstimulosTareaTopos[] matrizTarea = {
                (EstimulosTareaTopos) Enum.Parse(typeof(EstimulosTareaTopos), campos[4]),
                (EstimulosTareaTopos) Enum.Parse(typeof(EstimulosTareaTopos), campos[5]),
                (EstimulosTareaTopos) Enum.Parse(typeof(EstimulosTareaTopos), campos[6]),
                (EstimulosTareaTopos) Enum.Parse(typeof(EstimulosTareaTopos), campos[7]),
                (EstimulosTareaTopos) Enum.Parse(typeof(EstimulosTareaTopos), campos[8]),
                (EstimulosTareaTopos) Enum.Parse(typeof(EstimulosTareaTopos), campos[9]),
                (EstimulosTareaTopos) Enum.Parse(typeof(EstimulosTareaTopos), campos[10]),
                (EstimulosTareaTopos) Enum.Parse(typeof(EstimulosTareaTopos), campos[11]),
                (EstimulosTareaTopos) Enum.Parse(typeof(EstimulosTareaTopos), campos[12])
            };

            // nuevo registro 
            RegistroPosicionOcultarTareaTopos registro = new RegistroPosicionOcultarTareaTopos
            (
                0, int.Parse(cx), int.Parse(cy), 
                (EstimulosTareaTopos) Enum.Parse(typeof(EstimulosTareaTopos), cObjetivo), 
                matrizTarea
            );
            // meter en la lista a reproducir
            registros.Add(registro);

           
  
        }

        Debug.Log("Registros creados: " + registros.Count);
        return registros; 
    }


}
