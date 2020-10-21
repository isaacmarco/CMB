public enum Dificultad 
{
    Baja, 
    Media, 
    Dificil
};

public enum TipoNivel 
{
    Normal, 
    Demostracion,
    Tutorial
};

public enum EstimulosTareaTopos
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
    Puerta, 
    Botella, 
    Papel,
    Globos, 
    Lampara, 
    Guitarra, 
    Pato, 
    Secador, 
    Exprimidor, 
    Taza,
    Cubiertos, 
    Cepillo, 
    Vaso, 
    Olla, 
    Rodillo, 
    Percha, 
    Traba, 
    Helado, 
    Cerveza, 
    Peine

    /*
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
    */
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
    MenuPerfiles, 
    SalirAplicacion,
    MenuTareaTopos, 
    MenuTareaMemory, 
    ComenzarTareaTopos,
    ComenzarTareaMemory,        
    SeleccionarPaciente1, 
    SeleccionarPaciente2,
    SeleccionarPaciente3,
    SeleccionarPaciente4,
    SiguienteNivel, 
    AnteriorNivel,
    SeleccionarNivel
}

public enum Tareas
{
    Ninguna, 
    Topos
}