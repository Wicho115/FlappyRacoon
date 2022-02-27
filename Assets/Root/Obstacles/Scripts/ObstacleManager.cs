using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class ObstacleManager : MonoBehaviour
{
	public static ObstacleManager Instance;

    #region EVENTS
    public delegate void OnObstacleSpawnDel(Obstacle obstacle);
    public event OnObstacleSpawnDel onObstacleSpawn;


    #endregion

    [Header("Obstacle Manager Parameters")]
    public int MaxObstacles;
    [SerializeField] private int _firstNumObstacles = 6;
    [SerializeField] private Transform _parentObstacles;

    private List<Obstacle> _obstacles;
    private Obstacle _lastGenObstacle = null;

    [Header("Prefabs")]
    [SerializeField] private Obstacle _firstObstacle;
    [SerializeField] private Obstacle _obstaclePrefab;

    [Header("Obstacle Gen Parameters")]
    [SerializeField] private float _defaultSpeed;
    [SerializeField] private float _distanceBetweenObstacles = 5f;
    [SerializeField] private Transform _genPos;

    private Coroutine _genCoroutine = null;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        _obstacles = new List<Obstacle>();
        InitGen();
    }

    public void InitGen()
    {
        SpawnObstacle(new Vector3(_genPos.position.x, _firstObstacle.transform.position.y), _firstObstacle);
        GenFirstObstacles(_firstNumObstacles);
    }

    private void GenFirstObstacles(int numObstacles)
    {
        for (int i = 1; i < numObstacles + 1; i++)
        {
            SpawnObstacle(new Vector3(_genPos.position.x + _distanceBetweenObstacles * i, GetRandomObstacleHeight()), _obstaclePrefab);
        }
    }

    private float GetRandomObstacleHeight() => 0f;

    private IEnumerator GenerateObstacle()
    {
        if (_obstacles.Count >= MaxObstacles) {

            yield return new WaitUntil(() =>
            {
                return _genPos.position.x - _lastGenObstacle.transform.position.x >= _distanceBetweenObstacles;
            });
        }

        SpawnObstacle(
            new Vector3(_lastGenObstacle.transform.position.x + _distanceBetweenObstacles, GetRandomObstacleHeight()),
            _obstaclePrefab);

        _genCoroutine = null;
        yield return null;
    }

    private void OnObstacleDestroy(Obstacle destroyedObstacle)
    {
        destroyedObstacle.OnObstacleDestroy -= OnObstacleDestroy;
        Destroy(destroyedObstacle.gameObject);
        _genCoroutine = _genCoroutine ?? StartCoroutine(GenerateObstacle());
    }

    public void SpawnObstacle(Vector3 pos, Obstacle toSpawn)
    {
        Obstacle newObstacle = Instantiate(toSpawn, pos, Quaternion.identity, _parentObstacles);
        newObstacle.Init(this, _defaultSpeed, Vector3.left);
        _obstacles.Add(newObstacle);
        onObstacleSpawn?.Invoke(newObstacle);
        _lastGenObstacle = newObstacle;
        newObstacle.OnObstacleDestroy += OnObstacleDestroy;
    }

}
