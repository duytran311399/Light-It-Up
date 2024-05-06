using System;
using UnityEngine;

public static class GameAction
{
    public static Action actionA;
    public static Action actionB;
    public static Action actionC;
    public static Action OnStartGame;
    public static Action OnStartChanlenger;
    public static Action OnUpdateScore;
    public static Action OnGameOver;
    public static Action OnResetLevel;
    public static Action<Vector2> OnClick;
}
