using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DuyTran
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager _instance { get; private set; }
        public UserData userData;
        public GameData gameData;

        public void Awake()
        {
            #region Singleton
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else
                DestroyImmediate(gameObject);
            #endregion
            LoadUserData();
        }
        //private void OnEnable()
        //{
        //    GameAction.OnGameOver += GameOver;
        //}
        //private void OnDisable()
        //{
        //    GameAction.OnGameOver -= GameOver;
        //}
        void LoadUserData()
        {
            userData = ES3.Load<UserData>("userData", "userData.data", new UserData());
            gameData = ES3.Load<GameData>("gameData", "gameData.data", new GameData());
        }

        public void SaveUserData()
        {
            if (userData != null)
                ES3.Save("userData", userData, "userData.data");
            if (gameData != null)
                ES3.Save("gameData", userData, "gameData.data");
        }
        public void GameOver()
        {
            GameAction.OnGameOver?.Invoke();
        }
        public void ResetLevel()
        {
            GameAction.OnResetLevel?.Invoke();
        }
    }
}
