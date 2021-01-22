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
    [SerializeField] private GameObject bronce; 
	[SerializeField] private GameObject plata; 
	[SerializeField] private GameObject oro;



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

	public void ConfigurarMedallas(int medalla)
	{
		// 0
		// 1 bronce
		// 2 plata
		// 3 oro
		bronce.SetActive(false);
		plata.SetActive(false);
		oro.SetActive(false);
		if(medalla > 0)
			bronce.SetActive(true); 
		if(medalla > 1)
			plata.SetActive(true);
		if(medalla > 2)
			oro.SetActive(true);
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
