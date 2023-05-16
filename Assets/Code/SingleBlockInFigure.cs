using UnityEngine;
using System;
[Serializable]
public class SingleBlockInFigure
{
	public Vector3 BlockPosition;
	public Color BlockColor;

	public SingleBlockInFigure(Vector3 pos, Color col)
	{
		BlockPosition = pos;
		BlockColor = col;
	}
}
