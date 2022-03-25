using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    BoardScript board;
    public float timeValue = 120;
    public bool gameDone = false;

    private void Update()
    {
        board = GameObject.FindGameObjectWithTag("Board").GetComponent<BoardScript>();

        

        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            timeValue = 0;
            if (!gameDone)
            {
                gameDone = true;
                board.isGameDone = true;
                board.textBox.SetText("Times Up!");
            }
        }

        float minutes = Mathf.FloorToInt(timeValue / 60);
        float seconds = Mathf.FloorToInt(timeValue % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }
}
