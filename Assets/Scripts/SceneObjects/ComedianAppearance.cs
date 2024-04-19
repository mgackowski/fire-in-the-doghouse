using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ComedianAppearance : MonoBehaviour
{
    public ComedianType comedianType;

    Animator anim;
    //TODO: Adapt to new event system
    //NightManager manager;

    void Start()
    {
        anim = GetComponent<Animator>();
        //manager = GameObject.FindGameObjectWithTag("NightManager").GetComponent<NightManager>();
    }

    void OnJokeTold(CardPlay play)
    {
        if(play.player.Type == comedianType)
        {
            anim.SetBool("tellingJoke", true);
        }
    }

    void OnJokeFinished()
    {
        anim.SetBool("tellingJoke", false);
    }

/*    public void UpdateActBindings()
    {
        manager.act.CardPlayEvent += OnJokeTold;
        manager.act.EffectResolutionEvent += OnJokeFinished;
    }*/

}
