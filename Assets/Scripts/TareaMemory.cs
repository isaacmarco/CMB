using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

public class TareaMemory : Tarea
{
    private enum EstadoTareaMemory
    {
        Ninguno, 
        EligiendoPrimeraPieza, 
        EligiendoSegundaPieza
    };
    
    [SerializeField] private GameObject avatar; 
    [SerializeField] private TarjetaTareaMemory[] tarjetas; 
    private ControladorIK controladorIK; 
    private Vector2 puntoFiltrado; 
    private EstadoTareaMemory estado;
    

     public NivelMemoryScriptable Nivel { 
        get { return (NivelMemoryScriptable) Configuracion.nivelActual;} 
    }

    protected override void Inicio()
    {    
        // referenciar el controladorIK del avatar
        controladorIK = avatar.GetComponent<ControladorIK>();
    }

    // genera las tarjetas para el tablero
    private void GenerarTarjetas()
    {
        // obtenemos la lista de estimulos de la tarea
        EstimulosTareaMemory[] estimulos = Nivel.listaEstimulosParaFormarParejas;
        // aleatorizamos la lista de estimulos
        AleatorizarListaEstimulos(estimulos);
        // asignamos a cada tarjeta del escenario su estimulo
        for(int i=0; i<tarjetas.Length; i++)
            tarjetas[i].AsignarEstimulo(estimulos[i]);
    }

    private void AleatorizarListaEstimulos(EstimulosTareaMemory[] lista)
    {
        int n = lista.Length; 
        for(int i=0; i<n; i++)
        {
            int r = i + (int)(Random.value * ( n-i));
            EstimulosTareaMemory t = lista[r];
            lista[r] = lista[i];
            lista[i] = t; 
        }
    }

    // todo algoritmo pescador aqui!


    void Update()
    {
        if(controladorIK!=null)
        {            
            ActualizarBrazoVirtual();
        }
    }

    
    private void ActualizarBrazoVirtual()
    {
        // obtnemos el punto 
        GazePoint gazePoint = TobiiAPI.GetGazePoint();

        // si es valido actualizamos el brazo 
		if (gazePoint.IsValid)
		{
            // coordenadas en espacio de pantalla
			Vector2 posicionGaze = gazePoint.Screen;	
            // interpolamos y redondeamos las coordenadas
            puntoFiltrado = Vector2.Lerp(puntoFiltrado, posicionGaze, 0.5f);
			Vector2 posicionEntera = new Vector2(
                Mathf.RoundToInt(puntoFiltrado.x), 
                Mathf.RoundToInt(puntoFiltrado.y)
            );         
            MoverBrazoVirtual(posicionEntera); 			
		} 
      

    }
    private void MoverBrazoVirtual(Vector2 posicionEnPantalla)
    {        
        Ray ray;
     	RaycastHit hit;
		ray = Camera.main.ScreenPointToRay(posicionEnPantalla); // Input.mousePosition);	
        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.gameObject.name == "Mesa")
            {
                Vector3 posicionColisionPlano = hit.point;
                controladorIK.MoverObjetivo(posicionColisionPlano);
            }
		    
        }


    }
}
