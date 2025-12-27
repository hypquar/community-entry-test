using UnityEngine;

public class JumpStopper : MonoBehaviour
{
    [SerializeField] private Player _player;
    public void OnLand()
    {
        _player.StopJumpPhase();
    }
}
