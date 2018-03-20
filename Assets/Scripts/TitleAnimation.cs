using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnimation : MonoBehaviour
{

    [SerializeField] private bool canMorph;

    private const int MIN_NUM_VERTICIES = 3;
    private const int MAX_NUM_VERTICIES = 10;
    private const float SECONDS_TO_WAIT = 1.0f;

    private Vector3 origin = new Vector3(0, 0, 0);
    private Vector3 fullTurn = new Vector3(0, 0, -360);

    public bool _incrementVertex;
    public int vertexCount;

    private DynamicShapeRenderer _dynamicShape;
    private PolygonCollider2D _collider;
    private TaskManager _tm = new TaskManager();
	// Use this for initialization
	void Start ()
    {
        canMorph = true;
        //_incrementVertex = true;
        _dynamicShape = GetComponent<DynamicShapeRenderer>();
        _collider = GetComponent<PolygonCollider2D>();
        _dynamicShape.vertexAnimationSpeed = 5.0f;
        _dynamicShape.Radius = 5;

        _tm.Do
        (
                    new Scale(gameObject, Vector3.zero, Vector3.one, 1.0f))
            .Then(  new ActionTask(InitSpeeds)
        );

        //ChangeShape();
       
	}

    private void ChangeShape()
    {
        Debug.Log("Rotate?");
        _tm.Do
        (
                    new Rotate(gameObject, origin, fullTurn, SECONDS_TO_WAIT))  
           //.Then(   new WaitTask(SECONDS_TO_WAIT))
           .Then(   new ActionTask(ChangeShape)
        );

        if (_incrementVertex)
        {
            //_dynamicShape.NumOfVerticies++;
        }
        else
        {
            //_dynamicShape.NumOfVerticies--;
        }

        vertexCount = _dynamicShape.NumOfVerticies;

        if (_dynamicShape.NumOfVerticies <= MIN_NUM_VERTICIES)
            _incrementVertex = true;

        if (_dynamicShape.NumOfVerticies >= MAX_NUM_VERTICIES)
            _incrementVertex = false;

    }

    private void InitSpeeds()
    {
        _dynamicShape.vertexAnimationSpeed = 9.0f; 
    }

    /*--------------------------------------------------------------------------------------*/
    /*																						*/
    /*	OnCollisionEnter2D: Called once on collision										*/
    /*			param: Collision2D other - the collider of the other object					*/
    /*																						*/
    /*--------------------------------------------------------------------------------------*/
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {

            _dynamicShape.NumOfVerticies++;
            if (_dynamicShape.NumOfVerticies <= MIN_NUM_VERTICIES)
                _dynamicShape.NumOfVerticies = MIN_NUM_VERTICIES;

            if (_dynamicShape.NumOfVerticies >= MAX_NUM_VERTICIES)
                _dynamicShape.NumOfVerticies = MAX_NUM_VERTICIES;
        }

        if (other.gameObject.CompareTag("Shockwave"))
        {
            _dynamicShape.NumOfVerticies = MAX_NUM_VERTICIES;

        }

    }

    private void SetNumOfVerticies(int verticies)
    {
        _dynamicShape.NumOfVerticies = verticies;
    }

    private void MakeOctogon()
    {
        if (canMorph)
        {
            Destroy(GetComponent<PolygonCollider2D>());
            gameObject.AddComponent<PolygonCollider2D>();
            _dynamicShape.NumOfVerticies = 6;
            canMorph = false;
        }
    }

    private void MakeSquare()
    {
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
        canMorph = true;
        _dynamicShape.NumOfVerticies = 4;
    }

    private void Solidify()
    {
        _tm.Do
        (
                    new ActionTask(MakeOctogon)
            
        );

        _tm.Do
        (
                    new WaitTask(1.0f))
              .Then(new ActionTask(MakeSquare)
        );
    }


    private void Update()
    {
        _tm.Update();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Solidify();
        }
    }
}
