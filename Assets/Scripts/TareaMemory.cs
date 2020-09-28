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
    [SerializeField] private GameObject prefabTarjeta;
    [SerializeField] private Transform jerarquiaTarjetas;
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

        GenerarTarjetas();
    }

    // instancia una tarjeta en la matriz y devuelve el copmonente
    private TarjetaTareaMemory InstanciarTarjeta(int x, int y)
    {
        // instanciamos la tarjeta y la colocamos en la jerarquina
        GameObject tarjeta = (GameObject) Instantiate(prefabTarjeta); 
        tarjeta.name = "Tarjeta (" + x + ", " + y + ")";
        tarjeta.transform.parent = jerarquiaTarjetas;
        // posicionamos la tarjeta sobre la mesa
        float alturaTarjeta = 0.15f;         
        tarjeta.transform.localPosition = Vector3.zero;         
        tarjeta.transform.localPosition = new Vector3(x, alturaTarjeta, y);
        // ajustamos la escala y la rotacion 
        tarjeta.transform.localScale = new Vector3(0.9f, 0.02f, 0.9f);
        tarjeta.transform.localEulerAngles = Vector3.zero;
        // devolvemos el componente
        return tarjeta.GetComponent<TarjetaTareaMemory>();
    }

    // genera las tarjetas para el tablero
    private void GenerarTarjetas()
    {

        // instanciamos la matriz de tarjetas
        int contadorTarjetas = 0; 
        tarjetas = new TarjetaTareaMemory[Nivel.anchoMatriz * Nivel.altoMatriz];
        for(int i=0; i<Nivel.altoMatriz; i++)
        {
            for(int j=0; j<Nivel.anchoMatriz; j++)
            {
                // instanciamos la tarjeta e insertamos su componente
                // devuelto en el vector de componentes de tarjetas
                tarjetas[contadorTarjetas] = InstanciarTarjeta(j, i);
                contadorTarjetas++;
            }
        }
        

        // generamos los estimulos para las tarjetas
        EstimulosTareaMemory[] estimulos;

        // duplicamos cada uno de los estimulos de la lista del nivel para generar las parejas        
        estimulos = new EstimulosTareaMemory[Nivel.listaEstimulosParaFormarParejas.Length * 2]; 
        // copiamos la lista
        Nivel.listaEstimulosParaFormarParejas.CopyTo(estimulos, 0 );
        // la volvemos a copiar para duplicarla
        Nivel.listaEstimulosParaFormarParejas.CopyTo(estimulos, Nivel.listaEstimulosParaFormarParejas.Length);


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
