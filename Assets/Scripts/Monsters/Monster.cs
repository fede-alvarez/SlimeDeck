using UnityEngine;

public class Monster : MonoBehaviour
{
    public enum MonsterType
    {
        Ghost,
        Slime
    }
    
    public MonsterType type = MonsterType.Ghost;
    
}
