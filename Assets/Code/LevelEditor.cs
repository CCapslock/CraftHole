using UnityEngine;
using NaughtyAttributes;

public class LevelEditor : MonoBehaviour
{
	[Button]
	private void CombineComplexBlocks()
	{
		SingleComplexBlock[] complexBlocks = FindObjectsOfType<SingleComplexBlock>();
		for (int i = 0; i < complexBlocks.Length; i++)
		{
			complexBlocks[i].CombineBlocks();
		}
	}
	[Button]
	private void SetBlocksColors()
	{
		SingleBlock[] complexBlocks = FindObjectsOfType<SingleBlock>();
		for (int i = 0; i < complexBlocks.Length; i++)
		{
			complexBlocks[i].SetCorrectColor();
		}
	}
}