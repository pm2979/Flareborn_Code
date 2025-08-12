using UnityEngine;
using Cinemachine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private CinemachineConfiner confiner;

    protected override void Awake()
    {
        base.Awake();
    }

    public void OnSceneLoaded(Collider collider)
    {
        // 카메라 콘파이너를 찾아 설정
        confiner = gameObject.GetComponentInChildren<CinemachineConfiner>();

        if (confiner != null)
        {
            confiner.m_BoundingVolume = collider;
            Logger.Log("Camera confiner set to: " + confiner.m_BoundingVolume);
        }
    }

    public void SetFollowTarget(Transform target)
    {
        if (vcam == null)
        {
            Logger.LogError("VCam is not assigned");
            return;
        }

        vcam.Follow = target;
       // vcam.LookAt = target;
    }
    
    public void SetCameraActive(bool active)
    {
        if (vcam != null)
            vcam.gameObject.SetActive(active);
    }
}
