﻿public enum EstimulosTareaTopos
{    
    Topo, 
    Pato, 
    Oveja, 
    Pinguino,
    Gato,
    Ninguno
};

public enum EstimulosTareaMemory
{
    Gato, 
    Perro,
    Zorro,
    Rana, 
    Hipo, 
    Koala, 
    Lemur, 
    Mono,
    Panda, 
    Pinguino, 
    Cerdo, 
    Conejo, 
    Oveja, 
    Zorrillo, 
    Tigre, 
    Lobo
};

public enum SimilitudEstimulos
{
    SoloEstimuloObjetivo, 
    DiferentesEstimulos,
    /*
    DiferentesEstimulosConElColorDelObjetivo, 
    EstimuloObjetivoConColorCambiante, 
    EstimuloObjetivoConDetallesCambiantes,
    */
    EstimuloObjetivoCambiante
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