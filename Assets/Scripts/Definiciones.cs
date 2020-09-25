public enum EstimulosTareaTopos
{
    Topo, 
    Pato, 
    Oveja, 
    Pinguino,
    Gato
};

public enum EstimulosTareaMemory
{
    Triangulo, 
    Cuadrado, 
    Circulo
};

public enum SimilitudEstimulos
{
    SoloEstimuloObjetivo, 
    DiferentesEstimulos,
    DiferentesEstimulosConElColorDelObjetivo, 
    EstimuloObjetivoConColorCambiante, 
    EstimuloObjetivoConDetallesCambiantes
};

public enum OpcionesSeleccionablesMenu
{
    VolverMenuPrincipal,
    MenuTareaTopos, 
    ComenzarTareaTopos,
    SalirAplicacion
}

public enum Tareas
{
    Ninguna, 
    Topos
}