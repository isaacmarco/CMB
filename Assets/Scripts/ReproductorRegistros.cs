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
    [Header("Tarea de evaluacion")]
    public GameObject estimuloTareaEvaluacion; 
    public GameObject estimuloFijacion; 
    [Header("Tarea de Aventuras")]
    public GameObject[] items; 
    public GameObject[] peligros; 
    public GameObject jugadorAventuras; 

    [Header("Configuracion reproductor")]
    public bool reproducirTopos; 
    public bool reproducirGaleriaTiro; 
    public bool reproducirEvaluacion;
    public bool reproducirMemory;      
    public bool reproducirAventuras; 

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
        } else if (reproducirEvaluacion)
        {
            registros = CargarFicheroEvaluacion();
            StartCoroutine(ReproducirRegistroEvaluacion());
        } else if (reproducirMemory)
        {
            // memory
        } else if (reproducirAventuras)
        {
            // aventuras
            registros = CargarFicheroAventuras();
            StartCoroutine(ReproducirRegistroAventuras());
        }
        
       
    }
    

    private IEnumerator ReproducirRegistroAventuras()
    {
        Debug.Log("Comenzado la reproduccion de " + registros.Count);
       
      
        int contador = 0; 
        float esperaEntreRegistros = 0.01f; 

        while(contador < registros.Count)
        {   
            Debug.Log(contador);

            // extraemos un registro
            RegirstroPosicionOcultarTareaAventuras registro = 
                (RegirstroPosicionOcultarTareaAventuras) registros[contador];
            
            // posicionamos el punto de vision 
            puntoVision.GetComponent<RectTransform>().anchoredPosition = new Vector2
            (
                registro.X, registro.Y
            );

            // jugador 
            // TODO (siempre en el centro)

            // items 
            for(int i=0; i<items.Length; i++)
            {
                items[i].GetComponent<RectTransform>().anchoredPosition = new Vector2
                (
                    registro.items[i].x, registro.items[i].y
                );
            }

            // peligros
            for(int i=0; i<peligros.Length; i++)
            {
                peligros[i].GetComponent<RectTransform>().anchoredPosition = new Vector2
                (
                    registro.peligros[i].x, registro.peligros[i].y
                );
            }

            contador++;
            yield return new WaitForSeconds(esperaEntreRegistros);
        }
        Debug.Log("Reproduccion terminada");
    }

    private IEnumerator ReproducirRegistroEvaluacion()
    {
         
        Debug.Log("Comenzado la reproduccion");
       
      
        int contador = 0; 
        float esperaEntreRegistros = 0.06f; 

        while(contador < registros.Count)
        {   
            // extraemos un registro
            RegistroPosicionOcularTareaEvaluacion registro = 
                (RegistroPosicionOcularTareaEvaluacion) registros[contador];
            
            // posicionamos el punto de vision 
            puntoVision.GetComponent<RectTransform>().anchoredPosition = new Vector2
            (
                registro.X, registro.Y
            );

            // estimulo circular
            estimuloTareaEvaluacion.GetComponent<RectTransform>().anchoredPosition = new Vector2
            ( 
                registro.objetivoX, registro.objetivoY
            );

            // visibilidad
            estimuloFijacion.SetActive( registro.mostrandoEstimuloFijacion );
            estimuloTareaEvaluacion.SetActive( !registro.mostrandoEstimuloFijacion );


            contador++;
            yield return new WaitForSeconds(esperaEntreRegistros);
        }
        Debug.Log("Reproduccion terminada");
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

    private ArrayList CargarFicheroAventuras()
    {
       
        /*      
            tiempo;             
            mirando x; 
            mirando y; 
            [x, y items]
            [x, y peligros]
        */

        registros = new ArrayList();
                
        // obtenemos todas las lineas
        string[] lineas = fichero.text.Split('\n');
        // convertimos cada linea en un nuevo registro
        for(int i=0; i<lineas.Length; i++)
        {            

            string[] campos = lineas[i].Split(';');
            string ctiempo = campos[0];         
            string x = campos[1];
            string y = campos[2];
            
            // de 3 -> 22 (x,y de los items)
            Vector2[] items = new Vector2[10]; 

           
            items[0].x = int.Parse(campos[3]);
            items[0].y = int.Parse(campos[4]);

            items[1].x = int.Parse(campos[5]);
            items[1].y = int.Parse(campos[6]);

            items[2].x = int.Parse(campos[7]);
            items[2].y = int.Parse(campos[8]);

            items[3].x = int.Parse(campos[9]);
            items[3].y = int.Parse(campos[10]);

            items[4].x = int.Parse(campos[11]);
            items[4].y = int.Parse(campos[12]);

            items[5].x = int.Parse(campos[13]);
            items[5].y = int.Parse(campos[14]);

            items[6].x = int.Parse(campos[15]);
            items[6].y = int.Parse(campos[16]);

            items[7].x = int.Parse(campos[17]);
            items[7].y = int.Parse(campos[18]);

            items[8].x = int.Parse(campos[19]);
            items[8].y = int.Parse(campos[20]);

            items[9].x = int.Parse(campos[21]);
            items[9].y = int.Parse(campos[22]);
            
            
            // de 23 -> 42 (x,y de los peligros)
            Vector2[] peligros = new Vector2[10];
            
            peligros[0].x = int.Parse(campos[23]);
            peligros[0].y = int.Parse(campos[24]);

            peligros[1].x = int.Parse(campos[25]);
            peligros[1].y = int.Parse(campos[26]);

            peligros[2].x = int.Parse(campos[27]);
            peligros[2].y = int.Parse(campos[28]);

            peligros[3].x = int.Parse(campos[29]);
            peligros[3].y = int.Parse(campos[30]);

            peligros[4].x = int.Parse(campos[31]);
            peligros[4].y = int.Parse(campos[32]);

            peligros[5].x = int.Parse(campos[33]);
            peligros[5].y = int.Parse(campos[34]);

            peligros[6].x = int.Parse(campos[35]);
            peligros[6].y = int.Parse(campos[36]);

            peligros[7].x = int.Parse(campos[37]);
            peligros[7].y = int.Parse(campos[38]);

            peligros[8].x = int.Parse(campos[39]);
            peligros[8].y = int.Parse(campos[40]);
            
            peligros[9].x = int.Parse(campos[41]);
            peligros[9].y = int.Parse(campos[42]);
            

            RegirstroPosicionOcultarTareaAventuras registro = new RegirstroPosicionOcultarTareaAventuras
            (
                0, int.Parse(x), int.Parse(y), null, null 
            );

            registro.items = items; 
            registro.peligros = peligros;          
            registros.Add(registro);
  
        }

        Debug.Log("Registros creados: " + registros.Count);

        return registros; 
    }

    private ArrayList CargarFicheroEvaluacion()
    {
       
        /*           
            0,0000;True;1;1747;97;960;540
            tiempo; 
            estimulo fijacion visible; 
            numero bloque actual; 
            mirando x; 
            mirando y; 
            estimulo objetivo x; 
            estimulo objetivo y";
        */

        registros = new ArrayList();

        
        // obtenemos todas las lineas
        string[] lineas = fichero.text.Split('\n');
        // convertimos cada linea en un nuevo registro
        for(int i=0; i<lineas.Length; i++)
        {
            string[] campos = lineas[i].Split(';');
            string ctiempo = campos[0];
            string fijacion = campos[1];
            string bloque = campos[2];
            string x = campos[3];
            string y = campos[4];
            string ex = campos[5];
            string ey = campos[6];
            
            bool mostrandoFijacion = fijacion == "True" ? true : false; 

            /*
              public RegistroPosicionOcularTareaEvaluacion(
                float tiempo, 
                int x, 
                int y,
                int objetivoX, 
                int objetivoY, 
                int numeroBloqueEvaluacion, 
                bool mostrandoEstimuloFijacion) 
            {
            */

            RegistroPosicionOcularTareaEvaluacion registro = new RegistroPosicionOcularTareaEvaluacion
            (
                0, int.Parse(x), int.Parse(y), int.Parse(ex), int.Parse(ey), 0, mostrandoFijacion
            );
         
            registros.Add(registro);
  
        }

        Debug.Log("Registros creados: " + registros.Count);
        return registros; 

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
