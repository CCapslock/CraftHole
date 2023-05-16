using UnityEngine;
using NaughtyAttributes;

public class SingleLevel : MonoBehaviour
{
	public BuildFigureScriptableObject LevelFigure;

	public int BlocksAmount;

	[Button]
	public void GetBlocksAmount()
	{
		BlocksAmount = GetComponentsInChildren<SingleBlock>().Length;
	}
}
