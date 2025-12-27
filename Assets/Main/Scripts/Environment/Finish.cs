using UnityEngine;
using UnityEngine.Events;

public class Finish : MonoBehaviour
{
    public UnityEvent OnFinished;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Player player))
        {
            OnFinished.Invoke();
        }
    }
}
