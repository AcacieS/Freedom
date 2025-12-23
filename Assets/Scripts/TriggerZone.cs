using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    [Header("Trigger Settings")]
    [SerializeField] private string collisionTag = "Player";
    [Header("Events")]
    public UnityEvent OnTriggerEnterEvent;
    public UnityEvent OnTriggerExitEvent;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(collisionTag))
        {
            OnTriggerEnterEvent?.Invoke();
        }
    }
}
