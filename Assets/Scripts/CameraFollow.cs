using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    
    private void LateUpdate()
    {
        if (player) transform.position = player.position + offset;
    }
}
