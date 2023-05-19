using UnityEngine;
using System;
[Serializable]
public class SingleBlockInFigure
{
	public Vector3 BlockPosition;
	public Material BlockMaterial;

	public SingleBlockInFigure(Vector3 pos, Material mat)
	{
		BlockPosition = pos;
		BlockMaterial = mat;
	}
}
