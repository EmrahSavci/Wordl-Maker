using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Threading.Tasks;
public class CameraManager : Singleton<CameraManager>
{
    private CinemachineVirtualCamera[] cinemachineVirtualCameras;
    [SerializeField]
    private List<VirtualCamera> virtualCameras = new List<VirtualCamera>();
    protected override void Awake()
    {
        base.Awake();

        cinemachineVirtualCameras = FindObjectsOfType<CinemachineVirtualCamera>();

        for (int i = 0; i < cinemachineVirtualCameras.Length; i++)
        {
            var vCamera = new VirtualCamera(cinemachineVirtualCameras[i], (Cameras)i);
            virtualCameras.Add(vCamera);
        }

    }

    public void SetFocusCam(Cameras camera)
    {
        foreach (var item in virtualCameras)
        {
            if (item.camera == camera)
                item.virtualCamera.Priority = 1;
            else
                item.virtualCamera.Priority = 0;
        }
    }

    public CinemachineVirtualCamera GetCinemachineVirtualCamera(Cameras camera)
    {
        return virtualCameras[(int)camera].virtualCamera;
    }

    public VirtualCamera GetVirtualCamera(Cameras camera)
    {
        return virtualCameras[(int)camera];
    }
}

public static class CameraEvents
{
    public static VirtualCamera GetVirtualCamera(this Cameras camera)
    {
        return CameraManager.Instance.GetVirtualCamera(camera);
    }
    public static void SetFocusCam(this Cameras camera)
    {
        CameraManager.Instance.SetFocusCam(camera);
    }

    public static CinemachineVirtualCamera GetCinemachineVirtualCamera(this Cameras camera)
    {
        return CameraManager.Instance.GetCinemachineVirtualCamera(camera);
    }

}

[System.Serializable]
public class VirtualCamera
{
    public CinemachineVirtualCamera virtualCamera;
    public Cameras camera;

    public VirtualCamera(CinemachineVirtualCamera virtualCamera, Cameras camera)
    {
        this.virtualCamera = virtualCamera;
        this.camera = camera;
    }

    public void SetCamera(Transform target)
    {
        virtualCamera.Follow = target;
        virtualCamera.LookAt = target;
    }

    public void SetAim(Transform target)
    {
        virtualCamera.LookAt = target;
    }

    public void SetFollow(Transform target)
    {
        virtualCamera.Follow = target;
    }

    public void SetOffset(Vector3 offset)
    {
        var body = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        body.m_FollowOffset = offset;
    }

    public void SetAimOffset(Vector3 offset)
    {
        var body = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
        body.m_TrackedObjectOffset = offset;
    }

    public async void Shake(float intensity, float duration, float time)
    {
        var body = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        body.m_AmplitudeGain = intensity;
        body.m_FrequencyGain = duration;

        await Task.Delay((int)time * 1000);

        body.m_AmplitudeGain = 0;
        body.m_FrequencyGain = 0;
    }
}
