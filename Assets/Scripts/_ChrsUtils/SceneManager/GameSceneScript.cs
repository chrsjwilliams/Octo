using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneScript : Scene<TransitionData>
{
    public bool startedGame;

    public static bool hasWon { get; private set; }

    public const int LEFT_CLICK = 0;
    public const int RIGHT_CLICK = 1;

    public SuckerController[] suckers;
    public List<SuckerController> lostSuckers;
    private Timer timer;

    TaskManager tm = new TaskManager();

    private void Start()
    {
        startedGame = false;
        hasWon = false;
    }

    internal override void OnEnter(TransitionData data)
    {
        startedGame = false;
        suckers = FindObjectsOfType<SuckerController>();
        timer = FindObjectOfType<Timer>();
    }

    internal override void OnExit()
    {
        hasWon = false;
        
    }

    private void SwapScene()
    {
        Services.Scenes.Swap<EndGameSceneScript>();
    }

    private void EndGame()
    {
        tm.Do
        (
                        new WaitTask(2.0f))
                .Then(  new ActionTask(SwapScene)
        );
    }
    
	// Update is called once per frame
	void Update ()
    {
        if((Input.GetMouseButtonDown(LEFT_CLICK) || Input.GetMouseButtonDown(RIGHT_CLICK)) && !startedGame)
        {
            startedGame = true;
            Services.EventManager.Fire(new StartTimerEvent(false));
            
            for (int i = 0; i < suckers.Length; i++)
            {
                suckers[i].StartCarryMode();
            }
        }

        for(int i = 0; i < suckers.Length; i++)
        {
            if (!suckers[i].haveGrip && !lostSuckers.Contains(suckers[i]))
                lostSuckers.Add(suckers[i]);
        }

        if (lostSuckers.Count == suckers.Length)
        {
            Services.EventManager.Fire(new StopTimerEvent());
            Services.GameManager.SetDuration(timer.duration);
            EndGame();
        }

        tm.Update();
	}
}
