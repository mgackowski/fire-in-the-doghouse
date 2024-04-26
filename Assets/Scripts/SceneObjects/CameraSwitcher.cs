using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera humanPerformer;
    [SerializeField] CinemachineVirtualCamera cpuPerformer;
    [SerializeField] CinemachineVirtualCamera stage;
    [SerializeField] CinemachineVirtualCamera audience;

    List<CinemachineVirtualCamera> cameras;

    public void SwitchTo(CinemachineVirtualCamera camera)
    {
        foreach (CinemachineVirtualCamera cam in cameras)
        {
            cam.Priority = 0;
        }
        camera.Priority = 100;
    }

    private void OnActIntroStarted(GameplayStateArgs args)
    {
        //TODO: Have a nicer little stage overview
        SwitchTo(stage);
    }

    private void OnTurnStartAnimationsStarted(GameplayStateArgs args)
    {
        SwitchTo(stage);
    }

    private void OnCardPlayAnimationsStarted(CardPlayArgs args)
    {
        if (args.CardPlay.player.Type == ComedianType.CPU)
        {
            SwitchTo(cpuPerformer);
        }
        else
        {
            SwitchTo(humanPerformer);
        }
    }

    private void OnEffectResolutionStarted(CardPlayArgs args)
    {
        SwitchTo(stage);
    }

    private void OnScoreResolutionStarted(ScoreArgs args)
    {
        SwitchTo(audience);
    }

    private void OnTurnEndingStarted(DefaultEventArgs args)
    {
        SwitchTo(stage);
    }

    private void OnActEndingStarted(DefaultEventArgs args)
    {
        SwitchTo(stage);
    }
    private void Awake()
    {
        cameras = new List<CinemachineVirtualCamera>
            {
                humanPerformer, cpuPerformer, stage, audience
            };

        GameplayEventBus.Instance().Subscribe<ActIntroStartedEvent, GameplayStateArgs>(OnActIntroStarted);
        GameplayEventBus.Instance().Subscribe<TurnStartAnimationsStartedEvent, GameplayStateArgs>(OnTurnStartAnimationsStarted);
        GameplayEventBus.Instance().Subscribe<CardPlayAnimationsStartedEvent, CardPlayArgs>(OnCardPlayAnimationsStarted);
        GameplayEventBus.Instance().Subscribe<EffectResolutionStartedEvent, CardPlayArgs>(OnEffectResolutionStarted);
        GameplayEventBus.Instance().Subscribe<ScoreResolutionStartedEvent, ScoreArgs>(OnScoreResolutionStarted);
        GameplayEventBus.Instance().Subscribe<TurnEndingStartedEvent, DefaultEventArgs>(OnTurnEndingStarted);
        GameplayEventBus.Instance().Subscribe<ActEndingStartedEvent, DefaultEventArgs>(OnActEndingStarted);

    }

    private void OnDestroy()
    {
        GameplayEventBus.Instance().Unsubscribe<ActIntroStartedEvent, GameplayStateArgs>(OnActIntroStarted);
        GameplayEventBus.Instance().Unsubscribe<TurnStartAnimationsStartedEvent, GameplayStateArgs>(OnTurnStartAnimationsStarted);
        GameplayEventBus.Instance().Unsubscribe<CardPlayAnimationsStartedEvent, CardPlayArgs>(OnCardPlayAnimationsStarted);
        GameplayEventBus.Instance().Unsubscribe<EffectResolutionStartedEvent, CardPlayArgs>(OnEffectResolutionStarted);
        GameplayEventBus.Instance().Unsubscribe<ScoreResolutionStartedEvent, ScoreArgs>(OnScoreResolutionStarted);
        GameplayEventBus.Instance().Unsubscribe<TurnEndingStartedEvent, DefaultEventArgs>(OnTurnEndingStarted);
        GameplayEventBus.Instance().Unsubscribe<ActEndingStartedEvent, DefaultEventArgs>(OnActEndingStarted);

    }

}