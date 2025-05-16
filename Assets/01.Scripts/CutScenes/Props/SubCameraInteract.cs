using System;
using Cinemachine;
using UnityEngine;

public class SubCameraInteract: MonoBehaviour
{
    private CinemachineBrain cameraBrain;
    
    public CinemachineVirtualCamera virCam1;
    public CinemachineVirtualCamera virCam2;
    
    private CinemachineVirtualCamera activeCam;
    
    private CinemachineConfiner2D confiner2D;
    private CinemachineConfiner2D virCam1Confiner2D;
    private CinemachineConfiner2D virCam2Confiner2D;


    private void Awake()
    {
        cameraBrain = FindObjectOfType<CinemachineBrain>();
        
        confiner2D = MapSpawnManager.Instance.virtualCamera.GetComponent<CinemachineConfiner2D>();
        virCam1Confiner2D = virCam1.GetComponent<CinemachineConfiner2D>();
        virCam2Confiner2D = virCam2.GetComponent<CinemachineConfiner2D>();
    }

    private void Start()
    {
        activeCam = virCam1;
    }

    public void Init()
    {
        virCam1Confiner2D.m_BoundingShape2D = confiner2D.m_BoundingShape2D;
        virCam2Confiner2D.m_BoundingShape2D = confiner2D.m_BoundingShape2D;
    }
    
    private void SetUp()
    {
        cameraBrain.m_DefaultBlend.m_Time = 0.8f;

        activeCam.Follow = null;
        activeCam.LookAt = null;
        activeCam.Priority = 11;
    }

    public void Reset()
    {
        cameraBrain.m_DefaultBlend.m_Time = 0.0f;
        
        virCam1.Priority = 0;
        virCam2.Priority = 0;

        virCam1.Follow = null;
        virCam1.LookAt = null;
        
        virCam2.Follow = null;
        virCam2.LookAt = null;
    }
    
    public void Focus(Vector3 targetPos)
    {
        SetUp();
        CinemachineVirtualCamera nextCam = (activeCam == virCam1) ? virCam2 : virCam1;
        
        nextCam.transform.position = targetPos;
        nextCam.Priority = 12;

        activeCam = nextCam;
    }
    
    public void Focus(Transform target)
    {
        SetUp();
        CinemachineVirtualCamera nextCam = (activeCam == virCam1) ? virCam2 : virCam1;
        
        nextCam.Follow = target;
        nextCam.LookAt = target;
        nextCam.Priority = 12;

        activeCam = nextCam;
    }
}