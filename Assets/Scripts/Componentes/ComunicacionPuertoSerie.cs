﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.IO;
using System;

public class ComunicacionPuertoSerie : MonoBehaviour
{
    public enum MensajesPuertoSerie
    {
        Comienzo=65, 
        Fijacion, 
        Seguimiento, 
        Final
    };
    
    
	private SerialPort puerteSerie = new SerialPort("COM4", 115200, 0, 8, (StopBits) 1); 
    
    // devuelve si el puerto se encuentra abierto
    public bool PuertoAbierto {
        get { 
            if(puerteSerie == null)
                return false; 
            return puerteSerie.IsOpen;
        }
    }

	void Awake () {
        
        try
        {
            // intentamos abrir el puerto 
            puerteSerie.Open(); 
            // tiempo de timeout
		    puerteSerie.ReadTimeout = 10; 
            Debug.Log("Puerto de serie abierto");

        } catch(Exception)
        {
            Debug.LogError("No se puede abrir el puerto");
        }

       
	}
    
    //public void EnviarPorPuertoSerie(MensajesPuertoSerie mensaje)
    public void EnviarPorPuertoSerie(String mensaje)
    {
        if(puerteSerie!=null)
        {
            if(puerteSerie.IsOpen)
            {
                //puerteSerie.Write(mensaje.ToString());
                puerteSerie.Write(mensaje);
                Debug.Log("Enviado por puerto de serie: " + mensaje);
            } else {
                Debug.LogError("El puerto serie no esta abierto");
            }
        } else {
            Debug.LogError("Puerto no iniciado, no se puede enviar");
        }
    }

    public void CerrarPuertoSerie()
    {
        Debug.Log("Cerrando puerto de serie");
        if(puerteSerie!=null)        
            puerteSerie.Close();
    }


 
}
