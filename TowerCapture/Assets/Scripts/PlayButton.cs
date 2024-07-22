using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayButton : MonoBehaviour
{
    [SerializeField] UnityEvent hideUI;
    [SerializeField] UnityEvent startPlay;
    [SerializeField] PlayerTower playerTowerScript;
    [SerializeField] EnemyTower enemyTowerScript;
    [SerializeField] TouchInputManager touchInputManagerScript;
    [Header("Audio Sources")]
    [SerializeField] AudioSource buttonClick;
    [SerializeField] AudioSource menuMusic;
    [SerializeField] AudioSource playMusic;

    public void StartGame()
    {
        buttonClick.Play();
        menuMusic.Stop();
        playMusic.Play();
        hideUI?.Invoke();
        startPlay?.Invoke();
    }
}
