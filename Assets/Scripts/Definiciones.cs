public enum ObjetosAventuras
{
    Ninguno = 0, 
    Cofre = 1, 
    Llave = 2, 
    Manzana = 3,
    Platano = 4, 
    Agua = 5, 
    PocimaSalud = 6,
    Tronco = 7, 
    Oro = 8, 
    Diamante = 9, 
    Esmeralda = 10, 
    Rubi = 11,
    Corazon = 12
};

public enum Dificultad 
{
    Baja, 
    Media, 
    Dificil
};

public enum ManejoTareaEvaluacion
{
    Joystick, 
    Vista
};
public enum CondicionTareaEvaluacion
{
    Entrenamiento, 
    Experimental
};

public enum MovimientoDiana
{
    Ninguno, 
    HorizontalDerecha, 
    HorizontalIzquierda,
    VerticalArriba,
    VerticalAbajo
};

public enum BloqueDeDianas
{
    PrimerBloqueDianas, 
    SegundoBloqueDianas, 
    TercerBloqueDianas
};

public enum DificultadTareaGaleriaTiro
{
    SoloDianaObjetivo,
    VariosTiposDiana
};

public enum EstimuloTareaGaleriaTiro
{
    Ninguno, 
    DianaObjetivo, 
    DianaErroena, 
    Gema
};

public enum TipoNivel 
{
    Normal, 
    Demostracion,
    Tutorial
};

public enum EstimuloTareaNaves
{
    Diana, 
    Silueta
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
    VolverMenuPrincipal = 0,    
    MenuPerfiles = 1, 
    SalirAplicacion = 2,
    MenuTareaTopos = 3, 
    MenuTareaMemory = 4, 
    MenuTareaEvaluacion = 12,
    MenuTareaGaleriaTiro = 14,
    MenuTareaAventuras = 16,
    ComenzarTareaTopos = 5,
    ComenzarTareaMemory = 6,        
    ComenzarTareaEvaluacion = 13,
    ComenzarTareaGaleriaTiro = 15,
    ComenzarTareaAventuras = 17,
    SeleccionarPaciente1 = 7, 
    SeleccionarPaciente2 = 8,
    SeleccionarPaciente3 = 9,
    SeleccionarPaciente4 = 10,   
    SeleccionarNivelMemory = 11    

}

public enum Tareas
{
    Ninguna, 
    Topos,
    Memory,
    GaleriaTiro,
    Evaluacion,
    Aventuras
}