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
    }

    void Start()
    {
        configuracion.pacienteActual = null; 
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
        Debug.Log("Cargando datos de paciente " + codigo);
        // recuperamos el json desde el prefs y deserializamos
        string json = PlayerPrefs.GetString(codigo); 
        // cargamos los datos en el paciente actual 
        JsonUtility.FromJsonOverwrite(json, configuracion.pacienteActual); 
    }

    public void CargarPerfilesExistentes()
    {
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
