using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
public class UINivelMemory : SeleccionarAlMirarUI
{
    [SerializeField] private int nivelACargar;
    [Header("Referencias")]
    //[SerializeField] private TextMeshPro tiempoRecord; 
    [SerializeField] private GameObject iconoTrofeo;
    [SerializeField] private TextMeshPro texto; 
    

    public void Configurar(int nivelACargar)
    {
        this.nivelACargar = nivelACargar; 
        this.texto.text = (nivelACargar + 1).ToString();
		iconoTrofeo.gameObject.SetActive(false);
    }

    protected override void Seleccionar()
    {
		FindObjectOfType<Menu>().EjecutarOpcionMenu(opcion, nivelACargar);
	}


    protected override void Inicio()
    {
        base.Inicio();
	
    }    
   

	public void Desactivar()
	{
		texto.color = Color.black; 
	}

	public void Activar()
	{
		texto.color = Color.white;
	}

	public void MarcarComoRecord()
	{       
		/*
		if(record == int.MaxValue)
		{
			//tiempoRecord.text = "--";
		} else {
			//tiempoRecord.text = record.ToString() + "seg.";				
		}*/

		//tiempoRecord.gameObject.SetActive(true); 
		iconoTrofeo.gameObject.SetActive(true);
	}
    
}
