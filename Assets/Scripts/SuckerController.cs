using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckerController : MonoBehaviour
{
    public bool haveGrip { get; private set; }

    private const int LEFT_CLICK = 0;
    private const int RIGHT_CLICK = 1;

    private bool carryMode;
    private bool prepareToWalk;
    private float growthRate;
    private float[] growthRates;
    private Vector3 prepareSize;
    private Vector3 startSize;
    private Vector3 carrySize;
    private Vector3 keepGrip;
    private Vector3 loseGrip;
    private Vector3 endSize;
    private Color noGripColor;
    private GameObject sucker;

    private SpriteRenderer sprite;

    private TaskManager tm = new TaskManager();
	// Use this for initialization
	void Start ()
    {
        haveGrip = true;
        carryMode = false;
        prepareToWalk = false;
        sucker = transform.GetChild(0).gameObject;
        growthRates = new float[] { 0.5f, 1.0f, 1.5f, 2.0f};
        growthRate = growthRates[Random.Range(0, growthRates.Length - 1)];
        prepareSize = new Vector3(0.1f, 0.1f);
        startSize = new Vector3(0.2f, 0.2f);
        keepGrip = new Vector3(0.1f, 0.1f);
        loseGrip = new Vector3(-0.015f, -0.015f);
        endSize = new Vector3(0.9f, 0.9f);
        noGripColor = new Color(191f / 256f, 151f / 256f, 151f / 256f);
        sprite = GetComponent<SpriteRenderer>();

        SuckerTask();

	}

    public void StartCarryMode()
    {
        carrySize = sucker.transform.localScale;
        carryMode = true;
    }

    private void SuckerTask()
    {
        if(!carryMode)
        {
            tm.Do
            (
                        new Scale(sucker, startSize, endSize, growthRate))
                .Then(  new WaitTask(0.5f))
                .Then(  new Scale(sucker, endSize, startSize, growthRate))
                .Then(  new ActionTask(SuckerTask)
            );
        }
        else
        {
            
            tm.Do
            (
                        new Scale(sucker, prepareSize, carrySize, 0.2f))
                .Then(  new ActionTask(PrepWalk)
            );
            Services.AudioManager.PlayClipVaryPitch(Clips.SUCK);
        }
    }

    private void PrepWalk() { prepareToWalk = true; }
	
	// Update is called once per frame
	void Update ()
    {

        if( carryMode && prepareToWalk && haveGrip &&
            (Input.GetMouseButtonDown(LEFT_CLICK) || Input.GetMouseButtonDown(RIGHT_CLICK)))
        {
            sucker.transform.localScale += keepGrip;

            if (sucker.transform.localScale.x > carrySize.x)
            {
                sucker.transform.localScale = carrySize;
            }
        }

        if(carryMode && prepareToWalk && haveGrip)
        {
            sucker.transform.localScale += loseGrip;
        }

        if (sucker.transform.localScale.x < 0 && haveGrip)
        {
            
            sucker.transform.localScale = Vector3.zero;
            Services.AudioManager.PlayClipVaryPitch(Clips.POP);
            haveGrip = false;
        }

        if(!haveGrip) sprite.color = Color.Lerp(sprite.color, noGripColor, Time.deltaTime * 2.0f);
        tm.Update();	
	}
}
