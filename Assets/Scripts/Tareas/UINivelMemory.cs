using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
public class UINivelMemory : SeleccionarAlMirarUI
{
    [SerializeField] private int nivelACargar;
    [Header("Referencias")]
    [SerializeField] private TextMeshPro tiempoRecord; 
    [SerializeField] private GameObject iconoCrono;
    [SerializeField] private TextMeshPro texto; 
    

    public void Configurar(int nivelACargar)
    {
        this.nivelACargar = nivelACargar; 
        this.texto.text = (nivelACargar + 1).ToString();
    }

    protected override void Seleccionar()
    {
		FindObjectOfType<Menu>().EjecutarOpcionMenu(opcion, nivelACargar);
	}


    protected override void Inicio()
    {
        base.Inicio();
        // ocultar el tiempo medio 
		//tiempoRecord.gameObject.SetActive(false); 
		//iconoCrono.gameObject.SetActive(false);
    }    
    
    /*
	public void FijarTexto(string texto)
	{
		this.texto.fontSize = 5;
		this.texto.text = texto; 
	}*/


	public void Desactivar()
	{
		texto.color = Color.black; 
	}

	public void Activar()
	{
		texto.color = Color.white;
	}

	public void MostrarTiempoRecord(int record)
	{       
		if(record == int.MaxValue)
		{
			tiempoRecord.text = "--";
		} else {
			tiempoRecord.text = record.ToString() + "seg.";				
		}

		tiempoRecord.gameObject.SetActive(true); 
		iconoCrono.gameObject.SetActive(true);
	}
    
}
