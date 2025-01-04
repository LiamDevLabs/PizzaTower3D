using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;


public class BossFightManager : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private Animator phaseTransitionAnimator;
    private PeppinoController player;

    [SerializeField] private Transform playerEnablePosition, bossEnablePosition;
    [SerializeField] private UnityEvent FightStarted;
    [SerializeField] private UnityEvent FightFinished;
    bool finished;

    [SerializeField] float initialTimeIntro;
    int phase;
    [SerializeField] float[] initialTimeByPhase;

    private void Awake()
    {
        phase = 0;
        finished = false;
        director.initialTime = initialTimeIntro;
        phaseTransitionAnimator.WriteDefaultValues();
        phaseTransitionAnimator.writeDefaultValuesOnDisable = true;
    }


    private IEnumerator Start()
    {
        yield return null;
        player = FindObjectOfType<PeppinoController>();
        EnableGameplay(false);
        director.Play();
    }

    public void NextPhase(bool finish=false)
    {
        director.initialTime = initialTimeByPhase[phase];
        EnableGameplay(false);
        director.Play();
        phase++;
        finished = finish;
    }

    public void FinishedPhaseTransition()
    {
        if(!finished)
        {
            director.Stop();
            EnableGameplay(true);
            if (phase == 0) FightStarted.Invoke();
        }
        else
        {
            FightFinished.Invoke();
        }

    }

    public void EnableGameplay(bool enable)
    { 
        if (enable)
        {
            player.Rigidbody.transform.position = playerEnablePosition.position;
            if (boss != null) boss.transform.position = bossEnablePosition.position;
        }

        if(boss != null) boss.SetActive(enable);
        player.gameObject.SetActive(enable);
        player.player.HUD.enabled = enable;
        //player.player.gameObject.SetActive(enable);
    }
}
