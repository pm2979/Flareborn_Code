using UnityEngine;

public class CameraConfiner : MonoBehaviour
{
    private Collider _collider;
    
    void Start()
    {
        _collider = GetComponent<Collider>();

        CameraManager.Instance.OnSceneLoaded(_collider);
    }
}
