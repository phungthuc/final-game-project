using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private List<LevelData> ListLevelData;

    [SerializeField] 
    private TextMeshProUGUI TextLevel;

    [SerializeField] 
    private TextMeshProUGUI BestScoreText;
    
    [SerializeField] 
    private TextMeshProUGUI TargetScore;
    
    public bool isGameStarted = false;

    public bool isGameEnded = false;

    public TextMeshProUGUI textTimer;

    private int level = 1;

    public GameObject[] rooms;

    public GameObject[] tutorial;

    public GameObject ScreenEnd;

    public int timerFull = 0;
    public int scoreWin = 500;

    public int scoreCurrent = 0;

    public TextMeshProUGUI textScoreFinal;

    public TextMeshProUGUI textScoreBest;

    public TextMeshProUGUI textScore;

    private int currentLevel = 1;

    private bool isGamePlaying = false;

    public UnityEvent EventLose;


    private void Awake()
    {
        BestScoreText.text = PlayerPrefs.GetInt("scoreMax").ToString();
    }

    private void Update()
    {
        if (!isGamePlaying)
        {
            return;
        }
        UpdateBestScore();
        scoreCurrent = int.Parse(textScore.text);
        BestScoreText.text = PlayerPrefs.GetInt("scoreMax").ToString();
        if (scoreCurrent >= scoreWin)
        {
            OnWin();
        }
    }

    public void startGame()
    {
        isGamePlaying = true;
        LoadLevel(currentLevel - 1);
        isGameStarted = true;
        tutorial[0].SetActive(false);
        tutorial[1].SetActive(true);
        InvokeRepeating("SetTimer", 1, 1);
    }

    public void LoadLevel(int level)
    {
        timerFull = ListLevelData[level].time;
        scoreWin = ListLevelData[level].score;
        TargetScore.text = "Target Score: " + scoreWin;
        TextLevel.text = "Level " + currentLevel.ToString();
    }

    public void SetTimer()
    {
        timerFull--;
        textTimer.text = timerFull.ToString();
        if (timerFull == 0)
        {
            EventLose.Invoke();
            isGameEnded = true;
            CancelInvoke();
            ScreenEnd.SetActive(true);
            textScoreFinal.text = "Score: " + textScore.text;

        }
    }

    public void RestartGame()
    {
        timerFull = ListLevelData[currentLevel - 1].time; 
        Application.LoadLevel(Application.loadedLevelName); 
    }

    private void OnWin()
    {
        currentLevel++;
       LoadLevel(currentLevel - 1);
    }

    private void UpdateBestScore()
    {
        int scoreMax = PlayerPrefs.GetInt("scoreMax", 0);

        if ( scoreCurrent > scoreMax)
        {
            PlayerPrefs.SetInt("scoreMax", scoreCurrent);
        }
    }
}
