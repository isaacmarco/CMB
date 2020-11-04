using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class MenuPerfilesPacientes : MonoBehaviour
{
    
    [SerializeField] private TextMeshPro paciente1; 
    [SerializeField] private TextMeshPro paciente2; 

    // continuar ...

    public void ActualizarPerfiles()
    {
        // obtenemos los perfiles y actualizamos la configuracion 
        ConfiguracionScriptable configuracion = Aplicacion.instancia.configuracion; 

        TextMeshPro[] nombres = {paciente1, paciente2};
        for(int i=0; i<nombres.Length; i++)
        {
            nombres[i].text = configuracion.pacientes[i].nombre;    
        }

        // continuar ...

    }

}
