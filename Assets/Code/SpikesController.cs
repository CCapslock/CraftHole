using UnityEngine;
using System.Collections.Generic;

public class SpikesController : MonoBehaviour
{
	[SerializeField] private Transform _spikesTransform;
	[SerializeField] private Transform[] _allSpikes;
	[SerializeField] private float _rotationSpeed;
	[SerializeField] private int _spikesAmount;
	[SerializeField] private Vector3 _baseLocalRotation;
	[SerializeField] private Transform _center;
	[SerializeField] private float _addForcePower;
	[SerializeField] private ForceMode _forceMode;


	[SerializeField] private List<SingleBlock> _collidedBlocks = new List<SingleBlock>();
	[SerializeField] private List<SingleBlock> _removeBlocks = new List<SingleBlock>();

	private void Start()
	{
		ActivateSpikes();
	}
	private void FixedUpdate()
	{
		_spikesTransform.Rotate(Vector3.forward * _rotationSpeed);
		for (int i = 0; i < _collidedBlocks.Count; i++)
		{
			_collidedBlocks[i].AddForceToBlock(_center.transform.position, _addForcePower, _forceMode);
		}
		for (int i = _removeBlocks.Count - 1; i >= 0; i--)
		{
			_collidedBlocks.Remove(_removeBlocks[i]);
			_removeBlocks.RemoveAt(i);
		}
	}
	private void ActivateSpikes()
	{
		float singleStep = 360f / _spikesAmount;
		for (int i = 0; i < _allSpikes.Length; i++)
		{
			_allSpikes[i].gameObject.SetActive(false);
		}
		for (int i = 0; i < _spikesAmount; i++)
		{
			_allSpikes[i].localRotation = Quaternion.Euler(_baseLocalRotation + Vector3.forward * (singleStep * i));
			_allSpikes[i].gameObject.SetActive(true);
		}
	}
	public void SeparateBlock(SingleBlock block)
	{
		block.SeparateBlock();
		if (!_collidedBlocks.Contains(block))
			_collidedBlocks.Add(block);
	}
	public void RemoveBlockFromList(SingleBlock block)
	{
		_removeBlocks.Add(block);
	}
}