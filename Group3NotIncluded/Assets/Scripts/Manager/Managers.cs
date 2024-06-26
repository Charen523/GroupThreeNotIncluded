using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    public static Managers Instance;

    public GameManager gameManager;
    public AudioManager audioManager;
    public RankingManager rankingManager;
    public EnemyManager enemyManager;

    /*이벤트 모음*/
    public event Action<bool> OnPause;
    public event Action<GameObject> OnEnemyDie;
    public event Action OnGameOver;

    private bool isInitialized = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            gameManager = gameObject.GetComponent<GameManager>();
            audioManager = gameObject.GetComponent<AudioManager>();
            rankingManager = GetComponent<RankingManager>();

            isInitialized = true; // 초기화 완료
        }
        else if (Instance != this) 
        {
            Destroy(this.gameObject); 
        }
    }

    public bool IsInitialized()
    {
        return isInitialized;
    }

    public void OnPauseEvent(bool pause)
    {
        OnPause?.Invoke(pause);
    }

    public void OnEnemyDead(GameObject obj)
    {
        OnEnemyDie?.Invoke(obj);
    }

    public void OnGameOverEvent()
    {
        OnGameOver?.Invoke();
    }
}