﻿using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealthSystem : HealthSystem
{
    public GameObject[] PlayerHealthUI;
    private Animator PlayerAnim;

    private PlayerStatHandler statHandler;
    private Coroutine invincibleCoroutine;

    private readonly int invicibleItemTime = 3;

    protected void Awake()
    {
        statHandler = GetComponent<PlayerStatHandler>();
        PlayerAnim = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();

        StartHealth = 3;
        CurrentHealth = StartHealth;
        MaxHealth = statHandler.currentStat.maxHealth;
    }

    protected override void Update()
    {
        base.Update();

        if (hitDuration < healthChangeDelay)
            PlayerAnim.SetBool("Hit", true);
        else
        {
            PlayerAnim.SetBool("Hit", false);
            PlayerAnim.SetBool("IsInvincible", isInvincible);
        }
    }

    protected override void DestroyEntity()
    {
        base.DestroyEntity();

        GameObject tempP1 = GameObject.Find("Player1");
        GameObject tempP2 = GameObject.Find("Player2");

        if ((tempP1 == null || tempP1.GetComponent<PlayerHealthSystem>().CurrentHealth == 0) 
            && (tempP2 == null || tempP2.GetComponent<PlayerHealthSystem>().CurrentHealth == 0))
        {
            
            Managers.Instance.OnGameOverEvent();
        }
        AudioManager.Instance.PlaySFX(3);
    }

    protected void DisableHP()
    {
        PlayerHealthUI[CurrentHealth].SetActive(false);
        AudioManager.Instance.PlaySFX(2);
    }
    
    public void EnableHP()
    {
        if (CurrentHealth < MaxHealth)
        {
            PlayerHealthUI[CurrentHealth].SetActive(true);
            CurrentHealth += 1;
        }
    }

    public override bool ChangeHealth(float change)
    {
        if (hitDuration < healthChangeDelay || isInvincible)
        {
            return false;
        }

        hitDuration = 0f;
        CurrentHealth += (int)change;

        // 최솟값과 최댓값을 설정
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        if (CurrentHealth <= 0)
        {
            DisableHP();
            DestroyEntity();
        }
        else
        {
            DisableHP();
            isInvincible = true;
            StartCoroutine(HitInvincible());
        }

        return true;
    }

    private IEnumerator HitInvincible()
    {
        PlayerAnim.SetTrigger("Hit");  // 피격 애니메이션 트리거
        yield return new WaitForSeconds(hitDuration);
    }

    private IEnumerator ItemInvicible()
    {
        isInvincible = true;

        yield return new WaitForSeconds(invicibleItemTime);

        isInvincible = false;
        invincibleCoroutine = null;
    }

    public void OnInvincibleEvent()
    {
        if (invincibleCoroutine != null)
        {
            StopCoroutine(invincibleCoroutine);
        }

        //새로 3초 무적시간 적용.
        invincibleCoroutine = StartCoroutine(ItemInvicible());
    }
}
