using DuyTran;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DuyTran.GameEnum;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public StateGame stateGame;
    public System.Action<StateGame> OnchangeState;
    [Header("Player Control")]
    [SerializeField] PlayerBehaviour player;

    public float speedScaler = 1;
    [Header("Flatform")]
    [SerializeField] int maxFlatform = 4;
    private int countFlatform;
    [SerializeField] GameObject[] flatformPrefabs;

    public EffectsImpactPool impactPool;

    private Vector3 spawnPos = new Vector3(0, -6, 0);
    private Vector3 startFlatformPostision = new Vector3(0, -3.5f, 0);

    int timeWaitToStart = 3;
    bool isPlaying;

    Coroutine C_SpawnFlatform;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            DestroyImmediate(this);
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        OnchangeState += OnChangerStateGame;
        GameAction.OnGameOver += GameOver;
        GameAction.OnResetLevel += StartNewGame;
        GameAction.OnStartGame += StartNewGame;
        GameAction.a_SpawnFlatform += OnSpawnFlatform;
        GameAction.a_DeSpawnFlatform += OnDeSpawnFlatform;
    }
    private void OnDisable()
    {
        OnchangeState -= OnChangerStateGame;
        GameAction.OnGameOver -= GameOver;
        GameAction.OnResetLevel -= StartNewGame;
        GameAction.OnStartGame -= StartNewGame;
        GameAction.a_SpawnFlatform -= OnSpawnFlatform;
        GameAction.a_DeSpawnFlatform -= OnDeSpawnFlatform;
    }
    public void StartNewGame()
    {
        AudioManager.instance.PlayMusic(StringStatic.BGGamePlay);
        ChangerStateGame(StateGame.lightUp);
    }
    public bool CheckPlaying() { return isPlaying; }
    IEnumerator SpawnFlatform()
    {
        while (true)
        {
            if(countFlatform < maxFlatform)
            {
                GameObject flatform = flatformPrefabs[Random.Range(0, flatformPrefabs.Length)];
                if (flatform.TryGetComponent<FlatformBehaviour>(out var flatformBehaviour))
                {
                    float randX = UnityEngine.Random.Range(-1.25f, 1.25f);
                    Vector3 posSpawn = new Vector3(randX, spawnPos.y, 1);
                    flatformBehaviour.SpawnFlatform(posSpawn, Quaternion.identity);
                }
                float duration = UnityEngine.Random.Range(0.5f / speedScaler, 1.25f / speedScaler);
                yield return new WaitForSeconds(duration);
            }
            else
            {
                yield return null;
            }
        }
    }
    void OnSpawnFlatform()
    {
        countFlatform++;
        Debug.Log("Spawn: " + countFlatform);
    }
    void OnDeSpawnFlatform()
    {
        countFlatform--;
        Debug.Log("DeSpawn: " + countFlatform);
    }
    void SpawnFlatformStart()
    {
        if (flatformPrefabs[0].TryGetComponent<FlatformBehaviour>(out var flatformBehaviour))
        {
            flatformBehaviour.SpawnFlatform(startFlatformPostision, Quaternion.identity);
            //flatformBehaviour.SetGravityFlatform(0);
            //Invoke(nameof(ActiveFlatform), timeWaitToStart);
        }
        //void ActiveFlatform()
        //{
        //    flatformBehaviour.SetGravityFlatform(-1);
        //}
    }
    void SpawChanlegerEnemy()
    {

    }
    void SetupNewLevelLightUp()
    {
        player.EnablePlayerLightUp();
        SpawnFlatformStart();
        C_SpawnFlatform = StartCoroutine(SpawnFlatform());
        InvokeRepeating(nameof(UpdateSpeedScaler), 30, 30);
        speedScaler = 1;
        isPlaying = true;
    }
    void SetupLevelChanleger()
    {
        speedScaler = 1;
        player.EnablePlayerChanleger();
        SpawChanlegerEnemy();
    }
    void UpdateSpeedScaler()
    {
        speedScaler++;
        player.UpdateSpeed(speedScaler);
    }
    void OnChangerStateGame(StateGame state)
    {
        // Changer Player State
        switch (state)
        {
            case StateGame.lightUp:
                player.ResetPlayer();
                Invoke(nameof(SetupNewLevelLightUp), timeWaitToStart);
                break;
            case StateGame.chanlenger:
                player.ResetPlayer();
                SetupLevelChanleger();
                break;
        }
    }
    void ChangerStateGame(StateGame state)
    {
        stateGame = state;
        OnchangeState?.Invoke(state);
    }
    void GameOver()
    {
        player.DisablePlayer();
        if (C_SpawnFlatform != null)
            StopCoroutine(C_SpawnFlatform);
        CancelInvoke();
        AudioManager.instance.StopMusic();
    }
}
//[System.Serializable]
//public enum StateGame
//{
//    none = 0,
//    lightUp = 1,
//    chanlenger = 2
//}
