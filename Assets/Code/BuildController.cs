using UnityEngine;
using System.Collections.Generic;
using System;

public class BuildController : MonoBehaviour
{
	[SerializeField] private SingleBlock _blockPrefab;
	[SerializeField] private Transform _playerTransform;
	[SerializeField] private Transform _figureTransform;
	[SerializeField] private Transform _cameraGoalTransform;
	[SerializeField] private float _blocksSpeed;
	[SerializeField] private float _buildTime = 5f;

	private BuildFigureScriptableObject _figure;
	[SerializeField] private List<SingleBlock> _blocksForMovement = new List<SingleBlock>();
	[SerializeField] private List<SingleBlock> _collectedBlocks;
	[SerializeField] private int _blocksPlaced = 0;
	[SerializeField] private int _blocksPerTic = 1;
	private bool _isBuilding;

	public event Action<bool> onBuildingComplete;
	public void StartBuilding(List<SingleBlock> collectedBlocks, BuildFigureScriptableObject figure)
	{
		_figure = figure;
		_collectedBlocks = collectedBlocks;
		_blocksPerTic = (int)(_figure.FigureBlocks.Length / (_buildTime * 50f));
		_isBuilding = true;
	}
	private void FixedUpdate()
	{
		for (int i = _blocksForMovement.Count - 1; i >= 0; i--)
		{
			_blocksForMovement[i].MoveBlockNew(_blocksSpeed);
			if (_blocksForMovement[i].IsBlockAchivedGoalNew())
			{
				_blocksForMovement.RemoveAt(i);
			}
		}
		if (_isBuilding)
		{
			if (Input.GetMouseButton(0))
			{
				PlaceBlockNew();
			}
		}
	}
	public Transform GetCameraGoalTransform()
	{
		return _cameraGoalTransform;
	}
	private void PlaceBlock()
	{
		for (int i = 0; i < _blocksPerTic; i++)
		{
			if (_blocksPlaced < _figure.FigureBlocks.Length && _collectedBlocks.Count > 0)
			{
				SingleBlock temp = _collectedBlocks[_collectedBlocks.Count - 1];
				_collectedBlocks.RemoveAt(_collectedBlocks.Count - 1);
				temp.gameObject.SetActive(true);
				temp.transform.position = _playerTransform.position;
				temp.transform.rotation = Quaternion.identity;
				temp.StartMovingBlock(_figureTransform.position + _figure.FigureBlocks[_blocksPlaced].BlockPosition);
				temp.SetBlockMaterial(_figure.FigureBlocks[_blocksPlaced].BlockMaterial);
				_blocksPlaced++;
				_blocksForMovement.Add(temp);
			}
			else
			{
				_isBuilding = false;
				onBuildingComplete?.Invoke(true);
				break;
			}
		}
	}
	private void PlaceBlockNew()
	{
		for (int i = 0; i < 1; i++)
		{
			if (_blocksPlaced < _figure.FigureBlocks.Length && _collectedBlocks.Count > 0)
			{
				SingleBlock temp = _collectedBlocks[_collectedBlocks.Count - 1];
				_collectedBlocks.RemoveAt(_collectedBlocks.Count - 1);
				temp.gameObject.SetActive(true);
				temp.transform.position = _figureTransform.position + _figure.FigureBlocks[_blocksPlaced].BlockPosition + Vector3.up * 10f;
				temp.transform.rotation = Quaternion.identity;
				temp.StartMovingBlock(_figureTransform.position + _figure.FigureBlocks[_blocksPlaced].BlockPosition);
				temp.SetBlockMaterial(_figure.FigureBlocks[_blocksPlaced].BlockMaterial);
				_blocksPlaced++;
				_blocksForMovement.Add(temp);
			}
			else
			{
				_isBuilding = false;
				onBuildingComplete?.Invoke(true);
				break;
			}
		}
	}
}
