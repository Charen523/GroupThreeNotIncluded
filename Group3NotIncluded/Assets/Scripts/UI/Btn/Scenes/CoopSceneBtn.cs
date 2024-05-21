using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoopSceneBtn : MonoBehaviour
{
    public void LoadCoopScene()
    {
        SceneManager.LoadScene(2);

        if (Managers.Instance.enemyManager == null) return;
        Managers.Instance.enemyManager.ClearEnemyManager();
    }

    void Start()
    {
        // ������� ���
        AudioManager.Instance.PlayBackgroundMusic(2);
    }

    public void PlayEffectSound()
    {
        //ȿ���� ���
        AudioManager.Instance.PlaySFX(0);
    }
}