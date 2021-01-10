using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

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
        // reiniciar estado 
        ReiniciarEstadoPrograma();
        // establecer resolucion
        Screen.SetResolution(1920, 1080, true);
        // no permitir el uso de raton fuera del editor
        if(!Application.isEditor)
        {
            configuracion.utilizarRatonAdicionalmente = false; 
        }
    }


    public void ReiniciarEstadoPrograma()
    {
        // reiniciamos el estado del programa
        configuracion.pacienteActual = null; 
        configuracion.nivelActual = null; 
        configuracion.tareaActual = Tareas.Ninguna;
    }

    public void CargarNivelTareaEvaluacion()
    {
        // TODO 
        // CARGAR AQUI EL NIVEL CONFIGURADO
    }

    public void CargarNivelTareaTopos(int nivel)
    {
        string ruta = "Niveles Topos/NivelTopos " + nivel; 
        NivelToposScriptable nivelTopos = (NivelToposScriptable) Resources.Load(ruta); 
        configuracion.nivelActual = nivelTopos; 		
        configuracion.tareaActual = Tareas.Topos;
        Debug.Log("Cargado nivel de topos " + nivelTopos.numeroDelNivel);
    }


    public void CargarNivelTareaMemory(int nivel)
    {
        string ruta = "Niveles Memory/NivelMemory " + nivel;
        NivelMemoryScriptable nivelmemory = (NivelMemoryScriptable) Resources.Load(ruta); 
        configuracion.nivelActual = nivelmemory; 
        configuracion.tareaActual = Tareas.Memory;	
        Debug.Log("Cargado nivel de memory " + nivelmemory.numeroDelNivel);
    }

    public void GuardarDatosPaciente(PacienteScriptable paciente)
    {
        Debug.Log("Guardando datos del paciente " + paciente.codigo);
        // serializamos y guardamos en el prefs 
        Serializador.GuardarFicheroDatos(paciente);
    }
    
    public void BorrarDatos()
    {
        Debug.Log("Borrando todos los perfiles");
        
        for(int i=0; i<configuracion.pacientes.Length; i++)
        {
            configuracion.pacientes[i].Reiniciar();
            GuardarDatosPaciente(configuracion.pacientes[i]);
        }


        CargarPerfilesExistentes();
    }

    public void CargarPerfilesExistentes()
    {

        // cargar las configuraciones de cada uno de los
        // scriptables posibles para pacientes 
        Debug.Log("Leyendo ficheros JSON de pacientes");

      

        for(int i=0; i<configuracion.pacientes.Length; i++)
        {
            // construir clave
            string clave = "codigoPaciente" + i;

            // devolver la ruta para el fichero
           

            // obtener JSON desde el fichero de datos
            bool exito = Serializador.CargarFicheroDatos(
                configuracion.pacientes[i]
            );

            if(exito)
            {
                // los pacientes se han deserizaliado correctamente
                // en los scriptables
                Debug.Log("Paciente cargado correctamente");

            } else {
                
                Debug.LogError("Error cargando fichero de datos, creamos un paciente vacio");
                
                // no hay datos para deserializar
                // creamos perfiles vacios de debug
                PacienteScriptable paciente = configuracion.pacientes[i];                
                // reiniciamos el perfil
                paciente.Reiniciar();
                // creamos el fichero de datos
                Serializador.GuardarFicheroDatos(
                    configuracion.pacientes[i]
                );
            }


        }
    }

}
