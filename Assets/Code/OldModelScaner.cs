using UnityEngine;
using UnityEditor;
using NaughtyAttributes;
using System.Collections.Generic;

public class OldModelScaner : MonoBehaviour
{
	public SingleBlock _blockModel;
	public float _cubeSize;
	[OnValueChanged(nameof(SetVizualDots))]
	public float _modelSize;
	public SingleBlock[] temp;

	public Transform XPos;
	public Transform YPos;
	public Transform ZPos;
	public Transform BlocksParent;

	public BuildFigureScriptableObject Figure;

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
		temp = GetComponentsInChildren<SingleBlock>();
		for (int i = 0; i < temp.Length; i++)
		{
			DestroyImmediate(temp[i].gameObject);
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
				if (Physics.Raycast(transform.position + new Vector3(_cubeSize * i, _cubeSize * j, 0), Vector3.forward, out hit, _modelSize))
				{
					_blockPositions.Add(new SingleBlockInFigure(hit.point, GetColor(hit)));
				}
			}
		}
		for (int j = 0; j < _modelSize / _cubeSize; j++)
		{
			for (int i = 0; i < _modelSize / _cubeSize; i++)
			{
				if (Physics.Raycast(transform.position + Vector3.forward * _modelSize + new Vector3(_cubeSize * i, _cubeSize * j, 0), Vector3.back, out hit, _modelSize))
				{
					_blockPositions.Add(new SingleBlockInFigure(hit.point, GetColor(hit)));
				}
			}
		}
		for (int j = 0; j < _modelSize / _cubeSize; j++)
		{
			for (int i = 0; i < _modelSize / _cubeSize; i++)
			{
				if (Physics.Raycast(transform.position + Vector3.right * _modelSize + new Vector3(0, _cubeSize * j, _cubeSize * i), Vector3.left, out hit, _modelSize))
				{
					_blockPositions.Add(new SingleBlockInFigure(hit.point, GetColor(hit)));
				}
			}
		}
		for (int j = 0; j < _modelSize / _cubeSize; j++)
		{
			for (int i = 0; i < _modelSize / _cubeSize; i++)
			{
				if (Physics.Raycast(transform.position + new Vector3(0, _cubeSize * j, _cubeSize * i), Vector3.right, out hit, _modelSize))
				{
					_blockPositions.Add(new SingleBlockInFigure(hit.point, GetColor(hit)));
				}
			}
		}
		for (int j = 0; j < _modelSize / _cubeSize; j++)
		{
			for (int i = 0; i < _modelSize / _cubeSize; i++)
			{
				if (Physics.Raycast(transform.position + Vector3.up * _modelSize + new Vector3(_cubeSize * j, 0, _cubeSize * i), Vector3.down, out hit, _modelSize))
				{
					_blockPositions.Add(new SingleBlockInFigure(hit.point, GetColor(hit)));
				}
			}
		}
		for (int j = 0; j < _modelSize / _cubeSize; j++)
		{
			for (int i = 0; i < _modelSize / _cubeSize; i++)
			{
				if (Physics.Raycast(transform.position + new Vector3(_cubeSize * j, 0, _cubeSize * i), Vector3.up, out hit, _modelSize))
				{
					_blockPositions.Add(new SingleBlockInFigure(hit.point, GetColor(hit)));
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
						Instantiate(_blockModel, GetPointPosition(i, j, k) + transform.position, Quaternion.identity, BlocksParent).SetBlockColor(_calculatedPositions[i, j, k].BlockColor);
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
		temp = GetComponentsInChildren<SingleBlock>();
		SingleBlockInFigure[] positions = new SingleBlockInFigure[temp.Length];
		Vector3 Diffrence = new Vector3(_modelSize / 2f * -1f, 0, _modelSize / 2f * -1f);
		for (int i = 0; i < temp.Length; i++)
		{
			positions[i] = new SingleBlockInFigure(temp[i].transform.localPosition + Diffrence, temp[i].BlockColor);
		}
		Figure.FigureBlocks = SortArray(positions);
#if UNITY_EDITOR

		EditorUtility.SetDirty(Figure);
#endif
	}
	[Button]
	private void MakeComplexBlockPrefab()
	{
		temp = GetComponentsInChildren<SingleBlock>();
		SingleBlockInFigure[] positions = new SingleBlockInFigure[temp.Length];
		Vector3 Diffrence = new Vector3(_modelSize / 2f * -1f, 0, _modelSize / 2f * -1f);
		for (int i = 0; i < temp.Length; i++)
		{
			positions[i] = new SingleBlockInFigure(temp[i].transform.localPosition + Diffrence, temp[i].BlockColor);
		}
		GameObject Prefab = new GameObject("ComplexBlockPrefab");
		SingleBlock tempBlock;
		for (int i = 0; i < positions.Length; i++)
		{
			tempBlock = Instantiate(_blockModel, Prefab.transform);
			tempBlock.transform.localPosition = positions[i].BlockPosition;
			tempBlock.SetBlockColor(positions[i].BlockColor);
		}
		Prefab.AddComponent<SingleComplexBlock>().CombineBlocks();
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
		Debug.Log("distance = " + distance);
		_calculatedPositions[iNum, jNum, kNum].IsHasBlock = true;
		_calculatedPositions[iNum, jNum, kNum].BlockColor = pointPosition.BlockColor;
	}
	private Vector3 GetPointPosition(int i, int j, int k)
	{
		return new Vector3(i * _cubeSize, j * _cubeSize, k * _cubeSize);
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
			Texture2D tex = rend.material.mainTexture as Texture2D;
			Vector2 pixelUV = hit.textureCoord;
			pixelUV.x *= tex.width;
			pixelUV.y *= tex.height;

			return tex.GetPixel((int)pixelUV.x, (int)pixelUV.y);
		}
	}

}
