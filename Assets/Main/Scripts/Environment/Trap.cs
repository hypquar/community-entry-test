using UnityEngine;

public class Trap : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField]
    private int _damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player player))
        {
            player.TakeDamage(_damage);
        }
    }
}
