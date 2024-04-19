using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    //TODO: Adapt to new event system
    //public NightManager manager;


    public CinemachineVirtualCamera humanPerformer;
    public CinemachineVirtualCamera cpuPerformer;
    public CinemachineVirtualCamera stage;
    public CinemachineVirtualCamera audience;

    List<CinemachineVirtualCamera> cameras;

    private void Awake()
    {
         cameras = new List<CinemachineVirtualCamera>
            {
                humanPerformer, cpuPerformer, stage, audience
            };
    }

    private void Start()
    {
        //UpdateActBinding();
    }

    public void UpdateActBinding()
    {
        //SubscribeToActEvents();
    }

    public enum CameraType
    {
        HUMAN,
        CPU,
        STAGE,
        AUDIENCE
    }

    public void SwitchTo(CameraType camera)
    {
        CinemachineVirtualCamera targetCamera = null;
        switch (camera)
        {
            case CameraType.HUMAN:
                targetCamera = humanPerformer;
                break;
            case CameraType.CPU:
                targetCamera = cpuPerformer;
                break;
            case CameraType.STAGE:
                targetCamera = stage;
                break;
            case CameraType.AUDIENCE:
                targetCamera = audience;
                break;
        }
        foreach (CinemachineVirtualCamera cam in cameras)
        {
            cam.Priority = 0;
        }
        targetCamera.Priority = 100;
    }

    public void OnTurnStarted()
    {
        SwitchTo(CameraType.STAGE);
    }

    public void OnScoreResolved(int score)
    {
        SwitchTo(CameraType.AUDIENCE);
    }

    public void OnCardPlay(CardPlay play) {
        if (play.player.Type == ComedianType.PLAYER)
        {
            SwitchTo(CameraType.HUMAN);
        }
        else if (play.player.Type == ComedianType.CPU)
        {
            SwitchTo(CameraType.CPU);
        }
    }

/*    void SubscribeToActEvents()
    {
        if (manager.act == null)
        {
            return;
        }
        manager.act.CardPlayEvent += OnCardPlay;
        manager.act.TurnStartedEvent += OnTurnStarted;
        manager.act.ScoreResolutionEvent += OnScoreResolved;
    }*/


}