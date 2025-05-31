using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public int maxHp = 100;
    [Header("UI")]
    public Image imageBar;

    private Animator animator;
    
    private int _hp = 0;
    private bool _isDead = false;

    private void Awake()
    {
        _hp = maxHp;
        animator = GetComponent<Animator>();
    }

    public void Damage(int damage)
    {
        if (_isDead) return;
        
        _hp -= damage;
        if (_hp <= 0)
        {
            _isDead = true;
            DestroyParent();
            return;
        }
        
        float fill = (float)_hp / (float)maxHp;
        imageBar.fillAmount = fill;
        
        animator.SetTrigger("Damage");
    }

    private void DestroyParent()
    {
        GameObject parent = transform.parent.gameObject;
        if (parent)
            Destroy(parent);
    }
}
