﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    // referencias a los audiosources
    [SerializeField] private AudioSource ASfeedbackAcierto, ASfeedbackError, ASfeedbackOmision,
    ASfeedbackPartidaGanada, ASfeedbackPartidaPerdida;     
    [Header("Audio para el menu")]
    [SerializeField] private AudioSource ASelegirPerfil;
    [SerializeField] private AudioSource ASelegirTarea;  

    private Tarea tarea;
    
    void Start()
    {
        tarea = FindObjectOfType<Tarea>();
    }

    public void FeedbackElegirPerfil()
    {
        ASelegirPerfil.Play();
    }
    
    public void FeedbackElegirTarea()
    {
        ASelegirTarea.Play();
    }

    public void FeedbackPartidaGanada()
    {
        ASfeedbackPartidaGanada.volume = tarea.Configuracion.volumenDelFeedback; 
        ASfeedbackPartidaGanada.Play();
    }

    public void FeedbackPartidaPerdida()
    {
        ASfeedbackPartidaPerdida.volume = tarea.Configuracion.volumenDelFeedback; 
        ASfeedbackPartidaPerdida.Play();
    }
    
    public void FeedbackOmision()
    {
        ASfeedbackOmision.volume = tarea.Configuracion.volumenDelFeedback; 
        ASfeedbackOmision.Play();
    }

    public void FeedbackAcierto()
    {
        ASfeedbackAcierto.volume = tarea.Configuracion.volumenDelFeedback; 
        ASfeedbackAcierto.Play();
    }

    public void FeedbackError()
    {        
        ASfeedbackError.volume = tarea.Configuracion.volumenDelFeedback; 
        ASfeedbackError.Play();
    }

}
