using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Transform _cameraTransform;
	[SerializeField] private Quaternion _buildRotation;
	[SerializeField] private float _changeSpeed = 0.01f;

	private Transform _cameraGoalTransform;
	private float _currentRoatationTime = 0;
	private bool _needToChangePosition;

	public void StartChangeCameraRotation(Transform goalTransform)
	{
		_cameraGoalTransform = goalTransform;
		_cameraTransform.parent = null;
		_needToChangePosition = true;
	}
	private void Update()
	{
		if (_needToChangePosition)
		{
			ChangeCameraPosition();
		}
	}
	private void ChangeCameraPosition()
	{
		_currentRoatationTime += Time.deltaTime * _changeSpeed;

		_cameraTransform.rotation = Quaternion.Lerp(_cameraTransform.rotation, _cameraGoalTransform.rotation, _currentRoatationTime);
		_cameraTransform.position = Vector3.Lerp(_cameraTransform.position, _cameraGoalTransform.position, _currentRoatationTime);

		if (_currentRoatationTime >= 1f)
		{
			_needToChangePosition = false;
			_cameraTransform.rotation = _cameraGoalTransform.rotation;
			_cameraTransform.position = _cameraGoalTransform.position;
			_cameraTransform.parent = _cameraGoalTransform;
		}
	}
}