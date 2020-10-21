using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ImportadorNiveles 
{

#if UNITY_EDITOR

    [MenuItem("Niveles/Importar niveles memory")]
    public static void ImportarNivelesMemory()
    {
        ExcelNivelesScriptable refExcel = Resources.Load<ExcelNivelesScriptable>("NivelesExcel");
        string ficheroNivelesMemory = refExcel.nivelesMemory.text; 
        CrearNivelesMemory(ficheroNivelesMemory);
    }

    [MenuItem("Niveles/Importar niveles topos")]
    public static void ImportarNivelesTopos()
    {
        ExcelNivelesScriptable refExcel = Resources.Load<ExcelNivelesScriptable>("NivelesExcel");
        string ficheroNivelesTopos = refExcel.nivelesTopos.text;
        CrearNivelesTopos(ficheroNivelesTopos);
    }


    private static void CrearNivelesMemory(string fichero)
    {
        Debug.Log("Comenzando importacion de niveles de memory");

        // partir el fichero en lineas
        string[] lineas = fichero.Split('\n');
        
        Debug.Log(lineas.Length + " niveles encontrados");

        for(int i=0; i<lineas.Length; i++)
        {
            // para cada linea, partir la linea en campos
            string[] campos = lineas[i].Split('\t');

            // orden de los campos 
            // 0 Nivel
            // 1 Ancho
            // 2 Alto
            // 3 Estimulos[]
            // 4 Errores	
            // 5 Dificultad
                                
            // parseamos los campos
            int numeroNivel = -1; 
            int altoMatriz = -1; 
            int anchoMatriz = -1; 
            int errores = -1; 
            Dificultad dificultad = Dificultad.Baja; 
            
            // campos enteros     
            int.TryParse(campos[0], out numeroNivel);
            int.TryParse(campos[1], out anchoMatriz);            
            int.TryParse(campos[2], out altoMatriz);
            int.TryParse(campos[4], out errores);

            // campos enumerados     

            // obtener la lista de estimulos separados por comas
            
            EstimulosTareaMemory[] estimulosDisponibles = {

                EstimulosTareaMemory.Puerta, EstimulosTareaMemory.Botella, 
                EstimulosTareaMemory.Papel, EstimulosTareaMemory.Globos, 
                EstimulosTareaMemory.Lampara, EstimulosTareaMemory.Guitarra, 
                EstimulosTareaMemory.Pato, EstimulosTareaMemory.Secador, 
                EstimulosTareaMemory.Exprimidor, EstimulosTareaMemory.Taza,
                EstimulosTareaMemory.Cubiertos, EstimulosTareaMemory.Cepillo, 
                EstimulosTareaMemory.Vaso, EstimulosTareaMemory.Olla, 
                EstimulosTareaMemory.Rodillo, EstimulosTareaMemory.Percha, 
                EstimulosTareaMemory.Traba, EstimulosTareaMemory.Helado, 
                EstimulosTareaMemory.Cerveza, EstimulosTareaMemory.Peine
                /*
                EstimulosTareaMemory.Gato, EstimulosTareaMemory.Perro, EstimulosTareaMemory.Zorro, 
                EstimulosTareaMemory.Rana, EstimulosTareaMemory.Hipo, EstimulosTareaMemory.Koala, 
                EstimulosTareaMemory.Lemur, EstimulosTareaMemory.Mono, EstimulosTareaMemory.Panda, 
                EstimulosTareaMemory.Pinguino, EstimulosTareaMemory.Cerdo, EstimulosTareaMemory.Conejo, 
                EstimulosTareaMemory.Oveja, EstimulosTareaMemory.Zorrillo, EstimulosTareaMemory.Tigre,
                EstimulosTareaMemory.Lobo*/
            };

            int numeroEstimulos = anchoMatriz * altoMatriz / 2; 
            EstimulosTareaMemory[] estimulos = new EstimulosTareaMemory[numeroEstimulos];
            string[] listaEstimulosCadena = campos[3].Split(','); 
            for( int j=0; j<estimulos.Length; j++)           
            {           
                // obtener el entero parseado                
                int numeroEstimulo = 0; 
                int.TryParse(listaEstimulosCadena[j], out numeroEstimulo);
                // el indice de estimulo en excel empieza en 1 pero en el codigo en 0
                EstimulosTareaMemory estimulo = estimulosDisponibles[numeroEstimulo-1];                
                estimulos[j] = estimulo; 
            }


            string dificultadCadena = campos[5];

            switch(dificultadCadena)
            {
                case "Baja":
                    dificultad = Dificultad.Baja; 
                break;
                case "Media":
                    dificultad = Dificultad.Media;
                break;
                case "Dificil":
                    dificultad = Dificultad.Dificil;
                break;
            }



            
            // creamos el scriptable
            string nombreScriptable = "NivelMemory " + numeroNivel + ".asset";
            string ruta ="Assets/Configuracion/Importador de niveles/" + nombreScriptable;
            // Assets/Resources/Niveles Memory/
            NivelMemoryScriptable nivel = ScriptableObject.CreateInstance<NivelMemoryScriptable>();
            AssetDatabase.CreateAsset(nivel, ruta);
            
            // configuramos el scriptable
            nivel.numeroDelNivel = numeroNivel; 
            nivel.anchoMatriz = anchoMatriz;
            nivel.altoMatriz = altoMatriz;
            nivel.listaEstimulosParaFormarParejas = estimulos;
            nivel.dificultad = dificultad;
            nivel.erroresParaPerder = errores; 
            Debug.Log("Creado " + nombreScriptable);

        }
    }


    private static void CrearNivelesTopos(string fichero)
    {
        Debug.Log("Comenzando importacion de niveles de Topos");

        
        // partir el fichero en lineas
        string[] lineas = fichero.Split('\n');
        
        Debug.Log(lineas.Length + " niveles encontrados");

        for(int i=0; i<lineas.Length; i++)
        {
            // para cada linea, partir la linea en campos
            string[] campos = lineas[i].Split('\t');

            // orden de los campos 
            // 0nivel
            // 1estimulo objetivo
            // 2aciertos    
            // 3errores
            // 4similutd(dificultad)
            // 5tiempo para nuevo estimulo       
            // 6tiempo permanencia
            // 7dificultad 
                                
            // parseamos los campos 
            int numeroNivel = -1; 
            int aciertos = -1; 
            int errores = -1; 
            float tiempoParaNuevoEstimulo = -1; 
            float tiempoPermanenciaEstimulo = -1; 
            SimilitudEstimulos similitud = SimilitudEstimulos.SoloEstimuloObjetivo;
            Dificultad dificultad = Dificultad.Baja; 
            EstimulosTareaTopos estimuloObjetivo = EstimulosTareaTopos.Topo; 


            // campos enumerados 
            string similitudCadena = campos[4];
            string dificultadCadena = campos[7];
            string estimuloObjetivoCadena = campos[1];
            
            switch(similitudCadena)
            {
                case "solo_estimulo_objetivo":
                    similitud = SimilitudEstimulos.SoloEstimuloObjetivo;
                break;
                case "diferentes_estimulos":
                    similitud = SimilitudEstimulos.DiferentesEstimulos;
                break;
                case "estimulo_objetivo_cambiante":
                    similitud = SimilitudEstimulos.EstimuloObjetivoCambiante;
                break;
            }

            switch(dificultadCadena)
            {
                case "Bajo":
                    dificultad = Dificultad.Baja; 
                break;
                case "Medio":
                    dificultad = Dificultad.Media;
                break;
                case "Alto":
                    dificultad = Dificultad.Dificil;
                break;
            }

            switch(estimuloObjetivoCadena)
            {
                case "Topo":
                    estimuloObjetivo = EstimulosTareaTopos.Topo; 
                break;
                case "Pato":
                    estimuloObjetivo = EstimulosTareaTopos.Pato; 
                break;
                case "Oveja":
                    estimuloObjetivo = EstimulosTareaTopos.Oveja; 
                break;
                case "Pinguino":
                    estimuloObjetivo = EstimulosTareaTopos.Pinguino; 
                break;
                case "Gato":
                    estimuloObjetivo = EstimulosTareaTopos.Gato; 
                break; 
            }
             

            // campos enteros 
            int.TryParse(campos[0], out numeroNivel);
            int.TryParse(campos[2], out aciertos);            
            int.TryParse(campos[3], out errores);

            // campos float, cambiamos antes el punto decimal
            // de . a , 
            float.TryParse(campos[5].Replace('.', ','), out tiempoParaNuevoEstimulo);
            float.TryParse(campos[6].Replace('.', ','), out tiempoPermanenciaEstimulo); 
          
            
            
            // creamos el scriptable
            string nombreScriptable = "NivelTopos " + numeroNivel + ".asset";
            string ruta ="Assets/Configuracion/Importador de niveles/" + nombreScriptable;
            // Assets/Resources/Niveles Topos/
            NivelToposScriptable nivel = ScriptableObject.CreateInstance<NivelToposScriptable>();
            AssetDatabase.CreateAsset(nivel, ruta);
            
            // configuramos el scriptable
            nivel.numeroDelNivel = numeroNivel; 
            nivel.estimuloObjetivo = estimuloObjetivo;
            nivel.aciertosParaSuperarElNivel = aciertos; 
            nivel.omisionesOErroresParaPerder = errores; 
            nivel.similitudEntreEstimulos = similitud;
            nivel.tiempoParaNuevoEstimulo = tiempoParaNuevoEstimulo; 
            nivel.tiempoPermanenciaDelEstimulo = tiempoPermanenciaEstimulo; 
            nivel.dificultad = dificultad; 

            Debug.Log("Creado " + nombreScriptable);

        }

    }

#endif
   
}
