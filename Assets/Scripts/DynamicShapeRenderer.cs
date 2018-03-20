using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicShapeRenderer : MonoBehaviour
{
    private const int MIN_VERTICIES = 1;
    private const int MAX_VERTICIES = 32;
    [SerializeField]
    [Range(1, 32)]
    private int _points;
    public int NumOfVerticies
    {
        get { return _points; }
        set
        {
            _points = value;
            if (_points < MIN_VERTICIES)
                _points = MIN_VERTICIES;
            else if (_points > MAX_VERTICIES)
                _points = MAX_VERTICIES;
        }
    }

    private const float MIN_RADIUS = 0.2f;
    private const float MAX_RADIUS = 10.0f;
    [SerializeField]
    private float _radius = 1;
    public float Radius
    {
        get { return _radius; }
        set
        {
            _radius = value;
            if (_radius < MIN_RADIUS)
                _radius = MIN_RADIUS;
            else if (_radius > MAX_RADIUS)
                _radius = MAX_RADIUS;
        }
    }

    private const float MIN_MOVESPEED = 1.0f;
    private const float MAX_MOVESPEED = 10.0f;
    [SerializeField]
    private float _vertexAnimationSpeed;
    public float vertexAnimationSpeed
    {
        get { return _vertexAnimationSpeed; }
        set
        {
            _vertexAnimationSpeed = value;
            if (_vertexAnimationSpeed < MIN_MOVESPEED)
                _vertexAnimationSpeed = MIN_MOVESPEED;
            else if (_vertexAnimationSpeed > MAX_MOVESPEED)
                _vertexAnimationSpeed = MAX_MOVESPEED;
        }
    }

    List<Vector2> colliderPoints = new List<Vector2>();

    [SerializeField]
    private LineRenderer _lineRenderer;
    private LinkedList<DynamicVerticies>_verticies;
    

    public class DynamicVerticies
    {
        public Vector3 position;
        public Vector3 target;

        private const float MIN_SPEED = 1.0f;
        private const float MAX_SPEED = 10.0f;
        private float _speed;
        public float Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                if (_speed < MIN_SPEED)
                    _speed = MIN_SPEED;
                else if (_speed > MAX_SPEED)
                    _speed = MAX_SPEED;
            }
        }


        public DynamicVerticies()
        {
            position = Vector3.zero;
            target = Vector3.zero;
            _speed = 1;
        }

        public DynamicVerticies(float speed)
        {
            position = Vector3.zero;
            target = Vector3.zero;
            _speed = speed;
        }

        public DynamicVerticies(Vector3 vertex, Vector3 target, float speed)
        {
            position = vertex;
            this.target = target;
            _speed = speed;
        }
    }


	// Use this for initialization
	void Start ()
    {
        _lineRenderer = GetComponentInChildren<LineRenderer>();

        NumOfVerticies = 3;
        _verticies = new LinkedList<DynamicVerticies>();
        _verticies.AddLast(new DynamicVerticies());
        _verticies.AddLast(new DynamicVerticies());
        _verticies.AddLast(new DynamicVerticies());
    }

	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            _points++;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _points--;
        }

        UpdateVertexList();

        foreach (var vertex in _verticies)
        {
            vertex.position = Vector3.Lerp(vertex.position, vertex.target, Time.deltaTime * vertexAnimationSpeed);
        }

        var renderList = new Vector3[_verticies.Count + 2];
        int index = 0;
        foreach (var vert in _verticies)
        {
            renderList[index++] = vert.position;
        }
        //  Forces the last vertex and the first vertex to be identical
        renderList[renderList.Length - 2] = renderList[0];
        renderList[renderList.Length - 1] = renderList[1];

        _lineRenderer.positionCount = renderList.Length;
        _lineRenderer.SetPositions(renderList);
    }

    void addCollider()
    {
        //  Collect all points
        //  make colliders with each point
    }

    private void UpdateVertexList()
    {
        int vertexPoints = _verticies.Count;
        int i = 0;
        foreach (var point in _verticies)
        {

            //  Cos represents x
            //  Sin represents y

            //  Taking the fraction of how far around the circle we've gone and multiplying it 
            //  by the total size of the circle. Creates points number of steps around the circle
            //  so we could just add a new vertex

            if (false)
            {
                float x = Mathf.Cos((i / (float)_points) * 2 * Mathf.PI);
                float y = Mathf.Sin((i / (float)_points) * 2 * Mathf.PI);
                point.target = new Vector3(x, y, 0) * _radius;
            }
            else
            {
                float x = Mathf.Sin((i / (float)_points) * 2 * Mathf.PI);
                float y = Mathf.Cos((i / (float)_points) * 2 * Mathf.PI);
                point.target = new Vector3(x, y, 0) * _radius;
            }



            i++;
        }

        for (i = vertexPoints; i < _points; ++i)
        {
            float x = Mathf.Cos((i / (float)_points) * 2 * Mathf.PI);
            float y = Mathf.Sin((i / (float)_points) * 2 * Mathf.PI);
            _verticies.AddLast(new DynamicVerticies(_verticies.First.Value.position,
                                                    new Vector3(x, y, 0) * _radius,
                                                    _verticies.First.Value.Speed));
        }

        while (_verticies.Count > _points)
        {
            _verticies.RemoveLast();
        }
        addCollider();
    }
 }