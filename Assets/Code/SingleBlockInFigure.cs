using UnityEngine;
using System;
[Serializable]
public class SingleBlockInFigure
{
	public Vector3 BlockPosition;
	public Material BlockMaterial;
	[HideInInspector]public Color BlockColor;

	public SingleBlockInFigure(Vector3 pos, Material mat)
	{
		BlockPosition = pos;
		BlockMaterial = mat;
	}
	public SingleBlockInFigure(Vector3 pos, Color col)
	{
		BlockPosition = pos;
		BlockColor = col;
	}
}
