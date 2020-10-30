using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class Serializador 
{
    // rutas 
    private static string rutaEscritorio = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
    private static string directorioBaseDatos = "Registros";        
    private static string rutaDirectorioRegistros = rutaEscritorio + "\\" + directorioBaseDatos;
    private static string rutaFichero = rutaDirectorioRegistros + "\\";

    public static bool GuardarFicheroDatos(this ScriptableObject scriptable)
	{        
        // crear el directorio para los datos si no existen 
        Directory.CreateDirectory(rutaDirectorioRegistros);        

		try
		{
			System.IO.File.WriteAllText(
                rutaFichero + scriptable.name + ".txt",
				JsonUtility.ToJson(scriptable, true)
			);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static bool CargarFicheroDatos(this ScriptableObject scriptable)
	{		
        string path = rutaFichero + scriptable.name + ".txt";
		if (System.IO.File.Exists(path))
		{
			string str = System.IO.File.ReadAllText(path);
			JsonUtility.FromJsonOverwrite(
				str,
				scriptable
			);
			return true;
		}
		return false;
	}

	public static string SerializarScriptable(this ScriptableObject scriptable)
	{
		return JsonUtility.ToJson(scriptable, true);
	}

	public static void DeserializarScriptable(this ScriptableObject scriptable, string json)
	{
		JsonUtility.FromJsonOverwrite(
			json,
			scriptable
		);
	}
}
