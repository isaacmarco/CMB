using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ImportadorNiveles 
{

    [MenuItem("Niveles/Importar")]
    public static void ImportarNiveles()
    {

        // cargar scriptable que contiene referencias
        // a los ficheros excel 
        ExcelNivelesScriptable refExcel = Resources.Load<ExcelNivelesScriptable>("NivelesExcel");

        // obtener los ficheros
        string ficheroNivelesTopos = refExcel.nivelesTopos.text;
        string ficheroNivelesMemory = refExcel.nivelesMemory.text; 

        // leer cada fichero creando los scriptables de los niveles
        CrearNivelesMemory(ficheroNivelesMemory);
        
        
        /*
        MyScriptableObjectClass asset = ScriptableObject.CreateInstance<MyScriptableObjectClass>();

        AssetDatabase.CreateAsset(asset, "Assets/NewScripableObject.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        */
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
            // Nivel	Ancho	Alto	Size	Elementos	Errores	Dificultad
                                
            // parseamos los campos 
            int numeroNivel = -1; 
            int altoMatriz = -1; 
            int anchoMatriz = -1; 
            
            int.TryParse(campos[0], out numeroNivel);
            int.TryParse(campos[1], out anchoMatriz);            
            int.TryParse(campos[2], out altoMatriz);

            
            // creamos el scriptable
            string nombreScriptable = "NivelMemory " + numeroNivel + ".asset";
            string ruta ="Assets/Configuracion/Importador de niveles/" + nombreScriptable;
            NivelMemoryScriptable nivel = ScriptableObject.CreateInstance<NivelMemoryScriptable>();
            AssetDatabase.CreateAsset(nivel, ruta);
            
            // configuramos el scriptable
            nivel.numeroDelNivel = numeroNivel; 
            nivel.anchoMatriz = anchoMatriz;
            nivel.altoMatriz = altoMatriz;

            Debug.Log("Creado " + nombreScriptable);

        }
    }


    private void CrearNivelesTopos(string fichero)
    {
    }
   
}
