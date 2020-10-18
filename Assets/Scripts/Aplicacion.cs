using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Aplicacion : MonoBehaviour
{

	private static Aplicacion _instancia;
		
	public static Aplicacion instancia
	{
		get
		{
			if (_instancia == null)
				_instancia = FindObjectOfType<Aplicacion>();
			if (_instancia == null)
			{
				var res = Resources.Load("Aplicacion", typeof(GameObject)) as GameObject;
				GameObject go = GameObject.Instantiate(res);
				_instancia = go.GetComponent<Aplicacion>();
				_instancia.Iniciar();
			}
			return _instancia;
		}
	}
	
	bool instanciaIniciada = false;
	
	
	void Iniciar()
	{
		if (instanciaIniciada) return;
		
		// ...
		
		instanciaIniciada = true;
	}   
	
	
    public ConfiguracionScriptable configuracion; 
    
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        ReiniciarEstadoPrograma();
    }


    public void ReiniciarEstadoPrograma()
    {
        // reiniciamos el estado del programa
        configuracion.pacienteActual = null; 
        configuracion.nivelActual = null; 
    }

    public void CargarNivelTareaTopos(int nivel)
    {
        string ruta = "Niveles Topos/NivelTopos " + nivel; 
        NivelToposScriptable nivelTopos = (NivelToposScriptable) Resources.Load(ruta); 
        configuracion.nivelActual = nivelTopos; 		
        Debug.Log("Cargado nivel de topos " + nivelTopos.numeroDelNivel);
    }


    public void CargarNivelTareaMemory(int nivel)
    {
        string ruta = "Niveles Memory/NivelMemory " + nivel; 
        NivelMemoryScriptable nivelmemory = (NivelMemoryScriptable) Resources.Load(ruta); 
        configuracion.nivelActual = nivelmemory; 	
        Debug.Log("Cargado nivel de memory " + nivelmemory.numeroDelNivel);
    }

    public void GuardarDatosPaciente(PacienteScriptable paciente)
    {
        Debug.Log("Guardando datos del paciente " + paciente.codigo);
        // serializamos y guardamos en el prefs 
        string json = JsonUtility.ToJson(paciente, true);
        PlayerPrefs.SetString(paciente.codigo, json);
    }

    public void CargarDatosPaciente(string codigo)
    {
        //Debug.Log("Cargando datos de paciente " + codigo);
        // recuperamos el json desde el prefs y deserializamos
        //string json = PlayerPrefs.GetString(codigo); 
        // cargamos los datos en el paciente actual 
        //JsonUtility.FromJsonOverwrite(json, configuracion.pacienteActual); 
    }

    public void CargarPerfilesExistentes()
    {

        // crear codigos de pacietes falsos, estos datos deben 
        // configurarse desde algun fichero o menu
        Debug.LogError("Creando codigos de pacientes para debug");
        PlayerPrefs.SetString("codigoPaciente0", "000");
        PlayerPrefs.SetString("codigoPaciente1", "001");
                


        // cargar las configuraciones de cada uno de los
        // scriptables posibles para pacientes 
       
        for(int i=0; i<configuracion.pacientes.Length; i++)
        {
            // conseguir el json desde el prefs
            string clave = "codigoPaciente" + i;
            string codigoPaciente = PlayerPrefs.GetString(clave);
            string json = PlayerPrefs.GetString(codigoPaciente);            
            // deserializar el json en un paciente scriptable de la
            // lista de perfiles disponibles 
            JsonUtility.FromJsonOverwrite(json, configuracion.pacientes[i]);
        }
    }

}
