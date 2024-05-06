using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DuyTran;

public class ScreenMainMenu : ScreenLayer
{
    [SerializeField] Button btnStartGame;
    [SerializeField] Button btnChanleger;
    [SerializeField] Button btnHightScore;
    [SerializeField] Button btnSetting;
    [SerializeField] Button btnQuit;

    private void Start()
    {
        AddListener();
    }
    void AddListener()
    {
        btnStartGame.onClick.AddListener(() => { GameAction.OnStartGame?.Invoke(); CloseLayer(); ScreenManager._instance.ShowScreen(ScreenName.GamePlay); });
        btnStartGame.onClick.AddListener(() => GameAction.OnStartChanlenger?.Invoke());
        btnHightScore.onClick.AddListener(() => Debug.Log("Show Hight Score"));
        btnSetting.onClick.AddListener(() => Debug.Log("Show Setting"));
        btnQuit.onClick.AddListener(() => Application.Quit());
    }

    public override void CloseLayer()
    {
        base.CloseLayer();
    }

    public override void ShowLayer()
    {
        base.ShowLayer();
    }
}
