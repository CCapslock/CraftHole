using System;
using UnityEngine;

public class TimerController : MonoBehaviour
{
	public float LevelTime;

	private UIController _uiController;
	private float _timer;
	private bool _isTimerGoing;

	public event Action onTimeEnd;

	private void Awake()
	{
		_uiController = GetComponent<UIController>();
	}
	public void StartTimer()
	{
		_timer = LevelTime;
		_isTimerGoing = true;
	}
	private void Update()
	{
		if (_isTimerGoing)
		{
			_timer -= Time.deltaTime;
			_uiController.SetTimerAmount((int)_timer, LevelTime, _timer);
			if (_timer <= 0f)
			{
				onTimeEnd?.Invoke();
			}
		}
	}
}
