using TMPro;
using UnityEngine;

public class DamageFeedback : MonoBehaviour
{
    public int damage;
    public TMP_Text damageText;
    
    void Start()
    {
        damageText.text = damage.ToString();
        Invoke("RemoveAfter", 1.0f);
    }

    private void RemoveAfter()
    {
        Destroy(this.gameObject);
    }
}
