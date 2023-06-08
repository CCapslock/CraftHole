using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

public class SingleComplexBlock : MonoBehaviour
{
	[SerializeField] private List<SingleBlock> _blocksList = new List<SingleBlock>();
	[SerializeField] private int _requiredContactBlocks;
	[SerializeField] private int _blocksInHoleContact;
	private int _currentLayer;
	private string _partName = "ComplexPart";

	private void Start()
	{
		_currentLayer = gameObject.layer;
	}
	[Button]
	public void CombineBlocks()
	{
		SingleBlock[] blocksArray = GetComponentsInChildren<SingleBlock>();
		_blocksList = new List<SingleBlock>();
		for (int i = 0; i < blocksArray.Length; i++)
		{
			_blocksList.Add(blocksArray[i]);
			_blocksList[i].CombineBlock(transform);
		}
	}
	public void CombineBlocks(int contactBlocksAmount)
	{
		SingleBlock[] blocksArray = GetComponentsInChildren<SingleBlock>();
		_blocksList = new List<SingleBlock>();
		for (int i = 0; i < blocksArray.Length; i++)
		{
			_blocksList.Add(blocksArray[i]);
			_blocksList[i].CombineBlock(transform);
		}
		_requiredContactBlocks = contactBlocksAmount;
	}
	[Button]
	public void SeparateBlocks()
	{
		for (int i = 0; i < _blocksList.Count; i++)
		{
			_blocksList[i].SeparateBlock();
		}
	}
	public void HoleEnter(int layer)
	{
		_blocksInHoleContact++;
		if (_currentLayer != layer && _blocksInHoleContact >= _requiredContactBlocks)
		{
			ChangeLayer(layer);
		}
	}
	public void HoleExit(int layer)
	{
		_blocksInHoleContact--;
		if (_currentLayer != layer && _blocksInHoleContact < _requiredContactBlocks)
		{
			ChangeLayer(layer);
		}
	}
	private void ChangeLayer(int layer)
	{
		_currentLayer = layer;
		gameObject.layer = layer;
		for (int i = 0; i < _blocksList.Count; i++)
		{
			_blocksList[i].gameObject.layer = layer;
		}
	}
	public void DisassembleBlock(SingleBlock block)
	{
		_blocksList.Remove(block);
		_requiredContactBlocks--;
		block.SeparateBlock();
	}

	public void MakeParts()
	{
		for (int i = 0; i < _blocksList.Count; i++)
		{
			_blocksList[i].transform.parent = transform;
		}

	}
}