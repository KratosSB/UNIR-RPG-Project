using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private GameObject _player;

        private void Awake()
        {
            _player = GameObject.FindWithTag("Player");
        }

        private void OnEnable()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void OnDisable()
        {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }

        private void DisableControl(PlayableDirector playableDirector)
        {
            Debug.Log("DisableControl");
            _player.GetComponent<ActionScheduler>().CancelCurrentAction();
            _player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnableControl(PlayableDirector playableDirector)
        {
            Debug.Log("EnableControl");
            _player.GetComponent<PlayerController>().enabled = true;
        }
    }
}

/*
//Ejemplo de evento, imaginando que estuviera en PlayableDirector
public event Action<flot> onFinish;
private vois Start(){
    OnFinish();
}
void OnFinish(){
    onFinish(1.0f);
}
//Suscripción al evento anterior
GetComponent<PlayableDirector>().onFinish += MetodoALlamar
*/