using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ShowDamage : MonoBehaviour
{
    public Transform attacksContainer;
    public GameObject simpleAttack;
    public GameObject damagePrefab;

    public UnityEvent onDamage;
    public int damage;

    public HPBar hpBar;
    
    public void ShowAt(Vector3 pos)
    {
        GameObject obj = Instantiate(damagePrefab, pos, Quaternion.identity);
        obj.GetComponent<DamageFeedback>().damage = damage;
        
        hpBar.Damage(damage);
        
        onDamage?.Invoke();
        
        ShowAttack();
    }

    public void ShowAttack()
    {
        if (attacksContainer)
            attacksContainer.transform.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));
        simpleAttack.SetActive(true);
        Invoke("AttackEnds", 1.0f);
    }

    private void AttackEnds()
    {
        simpleAttack.SetActive(false);
    }
}
