using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerMovementController : MonoBehaviour
{
	[SerializeField] private Transform _playerTransform;
	[SerializeField] private Transform _buildTransform;
	[SerializeField] private float _buildRadius;
	[SerializeField] private float _moveAroundSpeed;
	[SerializeField] private Joystick _input;
	[SerializeField] private float _speed = 5f;

	private Vector3 _movement;
	private float _acceleration = 0.06f;
	private float _currentAcceleration;
	private float _angle;
	private bool _isFirstInputTaken;
	private bool _needToMoveAroundBuilding;

	public event Action onFirstInputTaken;

	void FixedUpdate()
	{
		if (!_isFirstInputTaken && _input.Direction != Vector2.zero)
		{
			_isFirstInputTaken = true;
			onFirstInputTaken?.Invoke();
		}
		if (_needToMoveAroundBuilding)
		{
			MoveAroundBuilding();
		}
		else
		{
			Move(_input.Direction);
		}
	}
	public void StartBuildingMovement()
	{
		_needToMoveAroundBuilding = true;
	}
	public void Move(Vector3 movementDirection)
	{
		if (movementDirection == Vector3.zero)
			return;
		movementDirection.z = movementDirection.y;
		movementDirection.y = 0f;
		_playerTransform.position = (_playerTransform.position + movementDirection * (_speed * Time.deltaTime));
	}
	public void MoveToBuildPosition()
	{
		_playerTransform.position = Vector3.MoveTowards(_playerTransform.position, _buildTransform.position, _speed);
	}
	private void MoveAroundBuilding()
	{
		_angle += _moveAroundSpeed;
		_playerTransform.position = _buildTransform.position + new Vector3(_buildRadius * Mathf.Sin(_angle), 0f, _buildRadius * Mathf.Cos(_angle));
	}
}
