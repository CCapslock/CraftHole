using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class HoleController : MonoBehaviour
{
	[SerializeField] private Transform _playerTransform;
	[SerializeField] private SizeRequirments[] _requirments;
	[SerializeField] private float _changeSpeed;
	[SerializeField] private float _sliderSpeed;
	[SerializeField] private Slider _progressSlider;
	[SerializeField] private Vector3 _startSize;

	private List<SingleBlock> _collectedBlocks = new List<SingleBlock>();
	private Vector3 _newSize;
	private int _currentLevel;
	[SerializeField] private int _blocksAmount;
	private int _maxAmountOfBlocks;
	private bool _needToChangeSize;
	public event Action onCollectingComplete;

	private void Awake()
	{
		_playerTransform.localScale = _startSize;
		_progressSlider.maxValue = _requirments[_currentLevel].BlocksRequired;
	}
	public List<SingleBlock> GetCollectedBlocks()
	{
		return _collectedBlocks;
	}
	public void SetMaxAmountOfBlocks(int amount)
	{
		_maxAmountOfBlocks = amount;
	}
	private void CheckForCollectingComplete()
	{
		if (_blocksAmount >= _maxAmountOfBlocks)
		{
			onCollectingComplete?.Invoke();
		}
	}
	public void CollectBlock(SingleBlock block)
	{
		_collectedBlocks.Add(block);
		block.gameObject.SetActive(false);
		_blocksAmount++;
		CheckForUpgrade();
		CheckForCollectingComplete();
	}
	public void CheckForUpgrade()
	{
		if (_blocksAmount >= _requirments[_currentLevel].BlocksRequired)
		{
			_newSize = _requirments[_currentLevel].Size;
			_needToChangeSize = true;
			_currentLevel++;
			_progressSlider.maxValue = _requirments[_currentLevel].BlocksRequired;
		}
	}
	private void FixedUpdate()
	{
		if (_needToChangeSize)
		{
			_playerTransform.localScale = Vector3.MoveTowards(_playerTransform.localScale, _newSize, _changeSpeed);
			if (_playerTransform.localScale == _newSize)
			{
				_needToChangeSize = false;
			}
		}
		if (_progressSlider.value != _blocksAmount)
		{
			_progressSlider.value += _sliderSpeed;
			if (_progressSlider.value >= _blocksAmount)
			{
				_progressSlider.value = _blocksAmount;
			}
		}
	}
}
[Serializable]
public class SizeRequirments
{
	public int BlocksRequired;
	public Vector3 Size;
}