using UnityEngine;
using System.Collections.Generic;

public class SingleSpike : MonoBehaviour
{
	[SerializeField] private Transform _center;
	[SerializeField] private float _addForcePower;
	[SerializeField] private SpikeTrigger _spikeTrigger;
	[SerializeField] private ForceMode _forceMode;

	private SingleBlock _collidedBlock;
	private SingleBlock _removingBlock;
	private MeshCollider _spikeCollider;
	private SpikesController _spikesController;

	[SerializeField] private List<SingleBlock> _collidedBlocks = new List<SingleBlock>();
	[SerializeField] private List<SingleBlock> _removeBlocks = new List<SingleBlock>();
	private void Start()
	{
		_spikeCollider = GetComponent<MeshCollider>();
		_spikeTrigger = GetComponentInChildren<SpikeTrigger>();
		_spikesController = FindObjectOfType<SpikesController>();

		_spikeTrigger.onTriggerEnter += SeparateBlock;
		_spikeTrigger.onTriggerStay += MoveBlockToCenter;
		_spikeTrigger.onTriggerExit += RemoveBlockFromList;
	}
	private void OnCollisionEnter(Collision collision)
	{
		//if (collision.collider.TryGetComponent(out _collidedBlock))
		//{
		//	_collidedBlock.SeparateBlock(_spikeCollider.material);
		//}
	}
	private void FixedUpdate()
	{
		//for (int i = 0; i < _collidedBlocks.Count; i++)
		//{
		//	_collidedBlocks[i].AddForceToBlock(_center.transform.position, _addForcePower, _forceMode);
		//}
		//for (int i = _removeBlocks.Count - 1; i >= 0; i--)
		//{
		//	_collidedBlocks.Remove(_removeBlocks[i]);
		//	_removeBlocks.RemoveAt(i);
		//}
	}
	private void SeparateBlock(Collider other)
	{
		if (other.TryGetComponent(out _collidedBlock))
		{
			_spikesController.SeparateBlock(_collidedBlock);
			//_collidedBlock.SeparateBlock();
			//_collidedBlocks.Add(_collidedBlock);
		}
	}
	private void MoveBlockToCenter(Collider other)
	{

	}
	private void RemoveBlockFromList(Collider other)
	{
		if (other.TryGetComponent(out _removingBlock))
		{
			_spikesController.RemoveBlockFromList(_removingBlock);
			//_removeBlocks.Add(_removingBlock);
		}
	}
}
