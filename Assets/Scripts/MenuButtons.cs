using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    [SerializeField]
    GameObject menu;
    [SerializeField]
    GameObject boardPrefabEasy;
    [SerializeField]
    GameObject boardPrefabMedium;
    [SerializeField]
    GameObject boardPrefabHard;
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    GameObject timer;

    private void Awake()
    {
        timer.SetActive(false);
    }

    public void StartGameEasy()
    {
        Instantiate(boardPrefabEasy, canvas.transform);
        timer.SetActive(true);
        menu.SetActive(false);
    }

    public void StartGameMedium()
    {
        Instantiate(boardPrefabMedium, canvas.transform);
        timer.SetActive(true);
        menu.SetActive(false);
    }

    public void StartGameHard()
    {
        Instantiate(boardPrefabHard, canvas.transform);
        timer.SetActive(true);
        menu.SetActive(false);
    }
}
