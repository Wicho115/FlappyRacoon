using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Internal;

public class Obstacle : MonoBehaviour
{
	public delegate void OnObstacleDestroyDel(Obstacle obstacle);
	public event OnObstacleDestroyDel OnObstacleDestroy;


	private ObstacleManager _manager;
	private Transform _transform;
	private Vector3 _moveDir;
	private float _speed;

	public void Init(ObstacleManager manager, float speed, [DefaultValue("Vector3.left")] Vector3 dir)
    {
		_transform = transform;
		_speed = speed;
		_manager = manager;
		_moveDir = dir;
    }

	void Update()
	{
		_transform.position = Vector3.MoveTowards(_transform.position , _moveDir + _transform.position, _speed * Time.deltaTime);
	}

    private void OnTriggerEnter2D(Collider2D other)
    {		
		if (!other.tag.Equals("DestroyBounds")) return;
		OnObstacleDestroy?.Invoke(this);
	}
}
