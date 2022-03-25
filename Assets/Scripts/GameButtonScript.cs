using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButtonScript : MonoBehaviour
{
    [SerializeField]
    GameObject game;
    [SerializeField]
    GameObject menu;
    [SerializeField]
    GameObject timer;

    private void Awake()
    {
        menu = GameObject.FindGameObjectWithTag("Menu");
    }

    private void Update()
    {
        timer = GameObject.FindGameObjectWithTag("Timer");
    }

    public void ExitGame()
    {
        menu.SetActive(true);
        timer.SetActive(false);
        Destroy(game);
    }
}
