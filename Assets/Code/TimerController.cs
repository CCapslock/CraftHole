using System;
using UnityEngine;

public class TimerController : MonoBehaviour
{
	public float LevelTime;

	[SerializeField] private float _timer;
	private bool _isTimerGoing;

	public event Action onTimeEnd;

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
			if (_timer <= 0f)
			{
				onTimeEnd?.Invoke();
			}
		}
	}
}
