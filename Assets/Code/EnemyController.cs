using UnityEngine;
using System;
using NaughtyAttributes;

public class EnemyController : MonoBehaviour
{
	private float _enemyHealth;
	private bool _isEnemyDefeated = false;

	public event Action<bool> OnEnemyComplete;

	[Button]
	private void EnemyDefeated()
	{
		_isEnemyDefeated = false;
		OnEnemyComplete?.Invoke(_isEnemyDefeated);
	}
	[Button]
	private void PlayerDefeated()
	{
		_isEnemyDefeated = false;
		OnEnemyComplete?.Invoke(_isEnemyDefeated);
	}
	public void SetHealth(float amount)
	{
		_enemyHealth = amount;
	}
	public void TakeDamage(float amount)
	{
		_enemyHealth -= amount;
		if(_enemyHealth <= 0f)
		{
			EnemyDefeated();
		}
	}
}
