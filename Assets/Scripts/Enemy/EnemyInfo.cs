using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInfo", menuName = "Scriptable Objects/EnemyInfo")]
public class EnemyInfo : ScriptableObject
{
    public string type;
    public float point = 1f;
    public float speed = 3f;
    public float minRestRate = 2f;
    public float maxRestRate = 5f;
    public float minRest = 1f;
    public float maxRest = 3f;
}
