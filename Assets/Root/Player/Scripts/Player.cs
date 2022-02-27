using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{

	private BoxCollider2D m_collider;

	[Header("Gravity Parameters")]
	public float Gravity = -4.2f;
	[SerializeField] private float _timeToFall = 1.5f;
	[SerializeField] private float _timeToJump = 0.5f;

	private Vector3 _velocity;

	[Header("Rotation")]
	[SerializeField] private float _speedRotation = 5f;
	[SerializeField] private float _maxRotation = 30f;
	[SerializeField] private float _minRotation = -90f;

	private float _zRotation;

	[SerializeField, Header("Jump Parameters")]
	private float _jumpSpeed = 2.5f;
	

	void Start()
	{
		m_collider = GetComponent<BoxCollider2D>();		
	}

	void Update()
	{
		CheckJump();
		_velocity.y += DeltaGravity();
		_velocity.y = Mathf.Clamp(_velocity.y, Gravity * _timeToFall, -Gravity * _timeToJump);
		transform.Translate(_velocity * Time.deltaTime, Space.World);

		_zRotation = (_velocity.y > -0.2f) ? _zRotation - DeltaGravity() * _speedRotation : _zRotation + DeltaGravity() * _speedRotation;
		_zRotation = Mathf.Clamp(_zRotation, _minRotation, _maxRotation);
		transform.rotation = Quaternion.Euler(0, 0, _zRotation);	
	}

	private void CheckJump()
    {
		if (!Input.GetKeyDown(KeyCode.Space)) return;

		//AGREGAR VELOCIDAD AL PAJARO
		_velocity.y = _jumpSpeed;
	}


	private float DeltaGravity() => Gravity * Time.deltaTime;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}
