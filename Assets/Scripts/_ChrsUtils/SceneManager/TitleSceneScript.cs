using UnityEngine;
using UnityEngine.UI;

public class TitleSceneScript : Scene<TransitionData>
{
    public KeyCode startGame = KeyCode.Space;

    [SerializeField]private float SECONDS_TO_WAIT = 0.5f;

    private TaskManager _tm = new TaskManager();

    private Text title;
    private Text clickToStart;

    private Color bandColor;

    internal override void OnEnter(TransitionData data)
    {
        title = GameObject.Find("Title").GetComponent<Text>();
        clickToStart = GameObject.Find("CLICK TO PLAY").GetComponent<Text>();

    }

    internal override void OnExit()
    {

    }

    private void StartGame()
    {
        _tm.Do
        (
               
                        new WaitTask(SECONDS_TO_WAIT))
               .Then(   new LERPColor(clickToStart, clickToStart.color, Color.white, 0.2f))
               .Then(   new LERPColor(title, title.color, Color.white, 1.0f))
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

    private void Update()
    {
        _tm.Update();
        if (Input.GetKeyDown(startGame) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Services.AudioManager.PlayClip(Clips.CLICK);
            StartGame();
        }
    }
}
