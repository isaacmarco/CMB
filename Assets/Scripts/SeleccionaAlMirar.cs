using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class SeleccionaAlMirar : MonoBehaviour
{
	public float distanciaMaxima = 40;
	public float tiempoDesactivacion = 1f;
	private GazeAware gazeAware;
	private float tiempoActivado;
    private Topo topo;
	//private AudioSource _audio;
    //public Transform MarkIcon;

	void Awake()
	{
		gazeAware = GetComponent<GazeAware>();	
        topo = GetComponent<Topo>();	
		tiempoActivado = -1000f;
        //_audio = GetComponent<AudioSource>();
		//MarkIcon.localScale = Vector3.one * 0.1f;
		//MarkIcon.gameObject.SetActive(false);
	}

    public void Seleccionar()
    {
		//Debug.Log("GOLPEADO");
        topo.Golpedo();
        /*
        // Show icon
			if (!MarkIcon.gameObject.activeInHierarchy)
			{
				_audio.Play();
			}

			MarkIcon.gameObject.SetActive(true);
			MarkIcon.localScale = Vector3.Lerp(MarkIcon.localScale, Vector3.one, Time.unscaledDeltaTime * 3);
			if (MarkIcon.localScale.x > 1)
			{
				MarkIcon.localScale = Vector3.one;
			}
        */
    }

    public void Deseleccionar()
    {
		
        /*
        // Remove icon
			if (MarkIcon.localScale.x > 0.1f)
			{
				MarkIcon.gameObject.SetActive(true);
				MarkIcon.localScale = Vector3.Lerp(MarkIcon.localScale, Vector3.zero, Time.unscaledDeltaTime * 3);
			}
			else
			{
				MarkIcon.localScale = Vector3.one * 0.1f;
				MarkIcon.gameObject.SetActive(false);
			}
        */
    }

	void Update()
	{
		if (gazeAware.HasGazeFocus) // && Vector3.Distance(transform.position, Camera.main.transform.position) < distanciaMaxima)
		{
			Seleccionar();			// tiempoActivado = Time.unscaledTime;
		}
		/*
		if (Time.unscaledTime - tiempoActivado > tiempoDesactivacion)
		{
            Deseleccionar();			
		} else 	{
            Seleccionar();			
		}
		*/
	}    
    
}
