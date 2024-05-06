using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DuyTran;

public class ScreenGamePlay : ScreenLayer
{
    [SerializeField] Button btnRestart;
    [SerializeField] Button btnBackToMainMenu;
    [SerializeField] TextMeshProUGUI textScore;
    [SerializeField] TextMeshProUGUI textNofication;
    [SerializeField] GameObject panelBlack;
    int score;
    public void OnEnable()
    {
        GameAction.OnUpdateScore += UpdateScore;
        GameAction.OnGameOver += OnGameOver;
        GameAction.OnResetLevel += OnResetLevel;
    }
    public void OnDisable()
    {
        GameAction.OnUpdateScore -= UpdateScore;
        GameAction.OnGameOver -= OnGameOver;
        GameAction.OnResetLevel -= OnResetLevel;
    }
    private void Start()
    {
        AddListener();
    }
    void AddListener()
    {
        btnRestart.onClick.AddListener(() => { GameManager._instance.ResetLevel(); });
        btnBackToMainMenu.onClick.AddListener(() => { ScreenManager._instance.ShowScreenMain(); });
    }
    void UpdateScore()
    {
        score++;
        textScore.text = score.ToString();
    }
    void OnGameOver()
    {
        textNofication.gameObject.SetActive(true);
        textNofication.text = "Game Over";
        btnRestart.gameObject.SetActive(true);
        btnBackToMainMenu.gameObject.SetActive(true);
        panelBlack.SetActive(true);
    }
    void OnResetLevel()
    {
        InitScreen();
    }
    #region Override
    public override void CloseLayer()
    {
        base.CloseLayer();
    }

    public override void ShowLayer()
    {
        base.ShowLayer();
        InitScreen();
    }
    #endregion
    void InitScreen()
    {
        StartCoroutine(SetTimeCountDownDuration(3));
        textScore.text = "0";
        score = 0;
        panelBlack.SetActive(false);
        textNofication.gameObject.SetActive(true);
        btnRestart.gameObject.SetActive(false);
        btnBackToMainMenu.gameObject.SetActive(false);
    }
    IEnumerator SetTimeCountDownDuration(int duration)
    {
        while(duration >= 0)
        {
            if (duration == 0) { textNofication.text = "GO!"; }
            else { textNofication.text = duration.ToString(); }
            duration--;
            yield return new WaitForSeconds(1);
        }
        textNofication.gameObject.SetActive(false);
    }
}
