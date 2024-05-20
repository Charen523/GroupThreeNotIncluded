using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Managers managers;

    private bool isPaused;
    private int currentScore;
    private float currentTime;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //씬 로드 시 데이터 초기화.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeGameData();
    }
    
    void Start()
    {
        InitializeGameData(); //게임 초기화
        
        managers = Managers.Instance;
        managers.OnPause += GetPauseStatus;
        managers.OnEnemyDie += AddScore;
        managers.OnGameOver += EndGame;
    }

    void Update()
    {
        //인게임 시간의 흐름 계산.
        currentTime += Time.deltaTime;
    }

    private void InitializeGameData()
    {
        Time.timeScale = 1f;
        currentTime = 0f;
        currentScore = 0;
        isPaused = false;
        //CreateHpUI();
    }

    private void GetPauseStatus(bool pause)
    {
        isPaused = pause; //추후 필요없으면 삭제.

        if (isPaused)
        {
            Time.timeScale = 0f;
        }else
        {
            Time.timeScale = 1f;
        }
    }

    //currentTime을 timeText에 전달하는 메서드.
    public float GetTime()
    {
        return currentTime;
    }

    // 점수 증가
    public void AddScore(int score)
    {
        currentScore += score;   
    }

    public int GetScore()
    {
        return currentScore;
    }

    //게임종료 이벤트.
    public void EndGame()
    {
        Time.timeScale = 0f;
    }

}
