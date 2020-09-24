using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    // referencias a los audiosources
    [SerializeField] private AudioSource ASfeedbackAcierto, ASfeedbackError, ASfeedbackOmision;     
    private TareaTopos tarea;
    
    void Start()
    {
        tarea = FindObjectOfType<TareaTopos>();
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
