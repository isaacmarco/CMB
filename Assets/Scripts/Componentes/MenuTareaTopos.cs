using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class MenuTareaTopos : MonoBehaviour
{
    
    [SerializeField] private TextMeshPro nivelActual; 

    public void Actualizar()
    {
        // mostrar el nivel actual 
        int nivelActual = FindObjectOfType<Menu>().Configuracion.pacienteActual.ultimoNivelDesbloqueadoTareaTopos;
        this.nivelActual.text = nivelActual.ToString();
    }

}
