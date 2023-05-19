using UnityEngine;
using NaughtyAttributes;

public class LevelEditor : MonoBehaviour
{
	public Color ColorMain1;
	public Color ColorMain2;
	public Color ColorForScan;


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
	[Button]
	private void CompareColors()
	{

	}
	private float GetDistance(float color1, float color2)
	{
		if (color1 > color2)
		{
			return color1 - color2;
		}
		else
		{
			return color2 - color1;
		}
	}
}