using UnityEngine;
using UnityEditor;
using NaughtyAttributes;
using System.Collections.Generic;

public class ModelScaner : MonoBehaviour
{
	public SingleBlock _blockModel;
	public float _cubeSize;
	[OnValueChanged(nameof(SetVizualDots))]
	public float _modelSize;
	public SingleBlock[] _blocks;

	public Transform XPos;
	public Transform YPos;
	public Transform ZPos;
	public Transform BlocksParent;

	public BuildFigureScriptableObject Figure;
	public MaterialVariantScriptableObject MaterialVariants;

	public int CurrentFillHieght;

	private List<SingleBlockInFigure> _blockPositions = new List<SingleBlockInFigure>();
	private SingleCalculatedPosition[,,] _calculatedPositions;

	private void SetVizualDots()
	{
		XPos.localPosition = new Vector3(_modelSize, 0, 0);
		YPos.localPosition = new Vector3(0, _modelSize, 0);
		ZPos.localPosition = new Vector3(0, 0, _modelSize);
	}

	[Button]
	public void ScanModel()
	{
		_blockPositions = new List<SingleBlockInFigure>();
		_blocks = GetComponentsInChildren<SingleBlock>();
		for (int i = 0; i < _blocks.Length; i++)
		{
			DestroyImmediate(_blocks[i].gameObject);
		}
		_calculatedPositions = new SingleCalculatedPosition[(int)(_modelSize / _cubeSize), (int)(_modelSize / _cubeSize), (int)(_modelSize / _cubeSize)];
		for (int i = 0; i < _calculatedPositions.GetLength(0); i++)
		{
			for (int j = 0; j < _calculatedPositions.GetLength(1); j++)
			{
				for (int k = 0; k < _calculatedPositions.GetLength(2); k++)
				{
					_calculatedPositions[i, j, k] = new SingleCalculatedPosition();
				}
			}
		}
		RaycastHit hit;
		// Does the ray intersect any objects excluding the player layer
		for (int j = 0; j < _modelSize / _cubeSize; j++)
		{
			for (int i = 0; i < _modelSize / _cubeSize; i++)
			{
				if (Physics.Raycast(transform.position + new Vector3(_cubeSize * i, _cubeSize * j, 0), Vector3.forward, out hit, Mathf.Infinity))
				{
					_blockPositions.Add(new SingleBlockInFigure(hit.point, MaterialVariants.GetClosestColorVariant(GetColor(hit)).VariantMaterial));
				}
			}
		}
		for (int j = 0; j < _modelSize / _cubeSize; j++)
		{
			for (int i = 0; i < _modelSize / _cubeSize; i++)
			{
				if (Physics.Raycast(transform.position + Vector3.forward * _modelSize + new Vector3(_cubeSize * i, _cubeSize * j, 0), Vector3.back, out hit, Mathf.Infinity))
				{
					_blockPositions.Add(new SingleBlockInFigure(hit.point, MaterialVariants.GetClosestColorVariant(GetColor(hit)).VariantMaterial));
				}
			}
		}
		for (int j = 0; j < _modelSize / _cubeSize; j++)
		{
			for (int i = 0; i < _modelSize / _cubeSize; i++)
			{
				if (Physics.Raycast(transform.position + Vector3.right * _modelSize + new Vector3(0, _cubeSize * j, _cubeSize * i), Vector3.left, out hit, Mathf.Infinity))
				{
					_blockPositions.Add(new SingleBlockInFigure(hit.point, MaterialVariants.GetClosestColorVariant(GetColor(hit)).VariantMaterial));
				}
			}
		}
		for (int j = 0; j < _modelSize / _cubeSize; j++)
		{
			for (int i = 0; i < _modelSize / _cubeSize; i++)
			{
				if (Physics.Raycast(transform.position + new Vector3(0, _cubeSize * j, _cubeSize * i), Vector3.right, out hit, Mathf.Infinity))
				{
					_blockPositions.Add(new SingleBlockInFigure(hit.point, MaterialVariants.GetClosestColorVariant(GetColor(hit)).VariantMaterial));
				}
			}
		}
		for (int j = 0; j < _modelSize / _cubeSize; j++)
		{
			for (int i = 0; i < _modelSize / _cubeSize; i++)
			{
				if (Physics.Raycast(transform.position + Vector3.up * _modelSize + new Vector3(_cubeSize * j, 0, _cubeSize * i), Vector3.down, out hit, Mathf.Infinity))
				{
					_blockPositions.Add(new SingleBlockInFigure(hit.point, MaterialVariants.GetClosestColorVariant(GetColor(hit)).VariantMaterial));
				}
			}
		}
		for (int j = 0; j < _modelSize / _cubeSize; j++)
		{
			for (int i = 0; i < _modelSize / _cubeSize; i++)
			{
				if (Physics.Raycast(transform.position + new Vector3(_cubeSize * j, 0, _cubeSize * i), Vector3.up, out hit, Mathf.Infinity))
				{
					_blockPositions.Add(new SingleBlockInFigure(hit.point, MaterialVariants.GetClosestColorVariant(GetColor(hit)).VariantMaterial));
				}
			}
		}
		for (int i = 0; i < _blockPositions.Count; i++)
		{
			AddPoint(_blockPositions[i]);
		}
		MonoBehaviour tempTransform;
		for (int i = 0; i < _calculatedPositions.GetLength(0); i++)
		{
			for (int j = 0; j < _calculatedPositions.GetLength(1); j++)
			{
				for (int k = 0; k < _calculatedPositions.GetLength(2); k++)
				{
					if (_calculatedPositions[i, j, k].IsHasBlock == true)
					{
						Instantiate(_blockModel, GetPointPosition(i, j, k) + transform.position, Quaternion.identity, BlocksParent).SetBlockMaterial(_calculatedPositions[i, j, k].BlockMaterial);
						//tempTransform = PrefabUtility.InstantiatePrefab(_blockModel) as MonoBehaviour;
						//tempTransform.transform.parent = transform;
						//tempTransform.transform.localPosition = GetPointPosition(i, j, k) /*+ transform.position*/;
					}
				}
			}
		}
	}

	[Button]
	private void SetFigurePositions()
	{
		_blocks = GetComponentsInChildren<SingleBlock>();
		SingleBlockInFigure[] positions = new SingleBlockInFigure[_blocks.Length];
		Vector3 Diffrence = new Vector3(_modelSize / 2f * -1f, 0, _modelSize / 2f * -1f);
		for (int i = 0; i < _blocks.Length; i++)
		{
			positions[i] = new SingleBlockInFigure(_blocks[i].transform.localPosition + Diffrence, _blocks[i].BlockMaterial);
		}
		Figure.FigureBlocks = SortArray(positions);
#if UNITY_EDITOR

		EditorUtility.SetDirty(Figure);
#endif
	}

	[Button]
	private void FixCurrentBlocksPositions()
	{
		_blocks = GetComponentsInChildren<SingleBlock>();
		Vector3 fixedPos = new Vector3();
		for (int i = 0; i < _blocks.Length; i++)
		{
			fixedPos = new Vector3();
			if (Mathf.Abs(_blocks[i].transform.localPosition.x % _cubeSize) > _cubeSize / 2f)
			{
				if (_blocks[i].transform.localPosition.x > 0)
					fixedPos.x = _blocks[i].transform.localPosition.x - (_blocks[i].transform.localPosition.x % _cubeSize) + _cubeSize;
				else
					fixedPos.x = _blocks[i].transform.localPosition.x - (_blocks[i].transform.localPosition.x % _cubeSize) - _cubeSize;
			}
			else
			{
				fixedPos.x = _blocks[i].transform.localPosition.x - (_blocks[i].transform.localPosition.x % _cubeSize);
			}
			if (Mathf.Abs(_blocks[i].transform.localPosition.y % _cubeSize) > _cubeSize / 2f)
			{
				if (_blocks[i].transform.localPosition.y > 0)
					fixedPos.y = _blocks[i].transform.localPosition.y - (_blocks[i].transform.localPosition.y % _cubeSize) + _cubeSize;
				else
					fixedPos.y = _blocks[i].transform.localPosition.y - (_blocks[i].transform.localPosition.y % _cubeSize) - _cubeSize;
			}
			else
			{
				fixedPos.y = _blocks[i].transform.localPosition.y - (_blocks[i].transform.localPosition.y % _cubeSize);
			}
			if (Mathf.Abs(_blocks[i].transform.localPosition.z % _cubeSize) > _cubeSize / 2f)
			{
				if (_blocks[i].transform.localPosition.z > 0)
					fixedPos.z = _blocks[i].transform.localPosition.z - (_blocks[i].transform.localPosition.z % _cubeSize) + _cubeSize;
				else
					fixedPos.z = _blocks[i].transform.localPosition.z - (_blocks[i].transform.localPosition.z % _cubeSize) - _cubeSize;
			}
			else
			{
				fixedPos.z = _blocks[i].transform.localPosition.z - (_blocks[i].transform.localPosition.z % _cubeSize);
			}
			_blocks[i].transform.localPosition = fixedPos;
		}
	}

	[Button]
	private void MakeComplexBlockPrefab()
	{
		_blocks = GetComponentsInChildren<SingleBlock>();
		SingleBlockInFigure[] positions = new SingleBlockInFigure[_blocks.Length];
		Vector3 Diffrence = new Vector3(_modelSize / 2f * -1f, 0, _modelSize / 2f * -1f);
		for (int i = 0; i < _blocks.Length; i++)
		{
			positions[i] = new SingleBlockInFigure(_blocks[i].transform.localPosition + Diffrence, _blocks[i].BlockMaterial);
		}
		GameObject Prefab = new GameObject("ComplexBlockPrefab");
		SingleBlock tempBlock;
		for (int i = 0; i < positions.Length; i++)
		{
			tempBlock = Instantiate(_blockModel, Prefab.transform);
			tempBlock.transform.localPosition = positions[i].BlockPosition;
			tempBlock.SetBlockMaterial(positions[i].BlockMaterial);
		}
		Prefab.tag = TagManager.GetTag(TagType.ComplexBlock);
		Prefab.AddComponent<Rigidbody>();
		Prefab.AddComponent<SingleComplexBlock>().CombineBlocks(positions.Length / 3);
	}
	private void AddPoint(SingleBlockInFigure pointPosition)
	{
		pointPosition.BlockPosition -= transform.position;
		int iNum = 0;
		int jNum = 0;
		int kNum = 0;
		float distance = float.MaxValue;
		for (int i = 0; i < _calculatedPositions.GetLength(0); i++)
		{
			for (int j = 0; j < _calculatedPositions.GetLength(1); j++)
			{
				for (int k = 0; k < _calculatedPositions.GetLength(2); k++)
				{
					if (Vector3.Distance(GetPointPosition(i, j, k), pointPosition.BlockPosition) <= distance)
					{
						iNum = i;
						jNum = j;
						kNum = k;
						distance = Vector3.Distance(GetPointPosition(i, j, k), pointPosition.BlockPosition);
						if (distance < _cubeSize)
							break;
					}
				}
				if (distance < _cubeSize)
					break;
			}
			if (distance < _cubeSize)
				break;
		}
		_calculatedPositions[iNum, jNum, kNum].IsHasBlock = true;
		_calculatedPositions[iNum, jNum, kNum].BlockMaterial = pointPosition.BlockMaterial;
	}
	private void AddPoint(int i, int j, int k, Material mat)
	{
		if (!_calculatedPositions[i, j, k].IsHasBlock)
		{
			_calculatedPositions[i, j, k].IsHasBlock = true;
			_calculatedPositions[i, j, k].BlockMaterial = mat;
		}
	}
	private Vector3 GetPointPosition(int i, int j, int k)
	{
		return new Vector3(i * _cubeSize, j * _cubeSize, k * _cubeSize);
	}
	[Button]
	private void FillModel()
	{
		_blocks = GetComponentsInChildren<SingleBlock>();
		for (int i = 0; i < _blocks.Length; i++)
		{
			DestroyImmediate(_blocks[i].gameObject);
		}
		SingleCalculatedPosition tempCalculatedPos = new SingleCalculatedPosition();
		bool isFilling = false;
		bool needToStopFilling = false;
		bool isFillStreakGoing = false;
		bool isEmptyStreakGoing = false;
		for (int k = 0; k < _calculatedPositions.GetLength(2); k++)
		{
			isFilling = false;
			needToStopFilling = false;
			isEmptyStreakGoing = false;
			for (int i = 0; i < _calculatedPositions.GetLength(0); i++)
			{
				if (_calculatedPositions[i, CurrentFillHieght, k].IsHasBlock)
				{
					if (!isFillStreakGoing)
					{
						if (isFilling)
						{
							isFilling = false;
						}
						else
						{
							tempCalculatedPos = _calculatedPositions[i, CurrentFillHieght, k];
							isFillStreakGoing = true;
							isFilling = true;
						}
					}
				}
				else
				{
					if (isFillStreakGoing)
						isFillStreakGoing = false;
					if (isFilling)
					{
						needToStopFilling = true;
						for (int h = i + 1; h < _calculatedPositions.GetLength(0); h++)
						{
							if (_calculatedPositions[h, CurrentFillHieght, k].IsHasBlock)
							{
								needToStopFilling = false;
								break;
							}
						}
						if (needToStopFilling)
							isFilling = false;

						if (isFilling)
							AddPoint(i, CurrentFillHieght, k, tempCalculatedPos.BlockMaterial);
					}

				}
			}
		}
		for (int i = 0; i < _calculatedPositions.GetLength(0); i++)
		{
			for (int j = 0; j < _calculatedPositions.GetLength(1); j++)
			{
				for (int k = 0; k < _calculatedPositions.GetLength(2); k++)
				{
					if (_calculatedPositions[i, j, k].IsHasBlock == true)
					{
						Instantiate(_blockModel, GetPointPosition(i, j, k) + transform.position, Quaternion.identity, BlocksParent).SetBlockMaterial(_calculatedPositions[i, j, k].BlockMaterial);
						//tempTransform = PrefabUtility.InstantiatePrefab(_blockModel) as MonoBehaviour;
						//tempTransform.transform.parent = transform;
						//tempTransform.transform.localPosition = GetPointPosition(i, j, k) /*+ transform.position*/;
					}
				}
			}
		}
	}
	private SingleBlockInFigure[] SortArray(SingleBlockInFigure[] blocksArray)
	{
		int indx; //переменная для хранения индекса минимального элемента массива
		for (int i = 0; i < blocksArray.Length; i++)
		{
			indx = i;
			for (int j = i; j < blocksArray.Length; j++) //ищем минимальный элемент в неотсортированной части
			{
				if (blocksArray[j].BlockPosition.y < blocksArray[indx].BlockPosition.y)
				{
					indx = j; //нашли в массиве число меньше, чем intArray[indx] - запоминаем его индекс в массиве
				}
			}
			if (blocksArray[indx] == blocksArray[i]) //если минимальный элемент равен текущему значению - ничего не меняем
				continue;
			//меняем местами минимальный элемент и первый в неотсортированной части
			SingleBlockInFigure temp = blocksArray[i]; //временная переменная, чтобы не потерять значение intArray[i]
			blocksArray[i] = blocksArray[indx];
			blocksArray[indx] = temp;
		}
		return blocksArray;
	}
	private Color GetColor(RaycastHit hit)
	{
		Renderer rend = hit.transform.GetComponent<Renderer>();
		MeshCollider meshCollider = hit.collider as MeshCollider;

		if (rend == null || rend.sharedMaterial == null || meshCollider == null)
			return Color.white;

		if (rend.sharedMaterial.mainTexture == null)
		{
			int hitedTriangle = hit.triangleIndex;
			for (int i = 0; i < meshCollider.sharedMesh.subMeshCount; i++)
			{
				int[] tempTriangles = meshCollider.sharedMesh.GetTriangles(i);
				for (int j = 0; j < tempTriangles.Length; j++)
				{
					if (tempTriangles[j] == hitedTriangle)
					{
						return rend.sharedMaterials[i].GetColor("_BaseColor");
					}
				}
			}
			return rend.sharedMaterial.GetColor("_BaseColor");
		}
		else
		{
			Texture2D tex = rend.sharedMaterial.mainTexture as Texture2D;
			Vector2 pixelUV = hit.textureCoord;
			pixelUV.x *= tex.width;
			pixelUV.y *= tex.height;

			return tex.GetPixel((int)pixelUV.x, (int)pixelUV.y);
		}
	}

}
