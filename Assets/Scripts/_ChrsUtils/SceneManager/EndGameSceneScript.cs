using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameSceneScript : Scene<TransitionData>
{

    [SerializeField] private float SECONDS_TO_WAIT = 0.5f;
    TaskManager _tm = new TaskManager();

    private Text niceJob;
    private Text click;
    private Timer timer;

    internal override void OnEnter(TransitionData data)
    {
        niceJob = GameObject.Find("NiceJob").GetComponent<Text>();
        click = GameObject.Find("Click").GetComponent<Text>();
        timer = FindObjectOfType<Timer>();
        timer.AddDurationInSeconds(Services.GameManager.duration);
        timer.DisplayTime();
        
    }

    internal override void OnExit()
    {

    }

    private void RefreshGame()
    {
        _tm.Do
        (
                    new WaitTask(2.0f))
               .Then(new LERPColor(click, click.color, Color.white, 0.5f))
               .Then(new LERPColor(niceJob, niceJob.color, Color.white, 0.5f))
               .Then(new LERPColor(timer.currentTime, timer.currentTime.color, Color.white, 1.0f))
               .Then(new ActionTask(ChangeScene)
        );
    }

    private void TitleTransition()
    {

    }

    private void ChangeScene()
    {
        Services.Scenes.Swap<InstructionSceneScript>();
    }

    // Update is called once per frame
    void Update()
    {
        _tm.Update();
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Services.AudioManager.PlayClip(Clips.CLICK);
            RefreshGame();
        }
    }
}
