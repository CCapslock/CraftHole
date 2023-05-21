using UnityEngine;
using NaughtyAttributes;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelEditor : MonoBehaviour
{
	public Color ColorMain1;
	public Color ColorMain2;
	public Color ColorForScan;

	public SingleBlock[] SingleBlocks;
	public Material[] RandomMaterials;
	public MaterialVariantScriptableObject MaterialVariants;

	public GameObject[] OldObjects;
	public MonoBehaviour NewObject;

	public Transform[] RotateObjects;
	public Quaternion[] RandomRotations;


	[Button]
	public void ReplaceObjects()
	{
		for (int i = 0; i < OldObjects.Length; i++)
		{
			MonoBehaviour temp =  PrefabUtility.InstantiatePrefab(NewObject) as MonoBehaviour;
			temp.transform.position = OldObjects[i].transform.position;
			temp.transform.parent = OldObjects[i].transform.parent;
			DestroyImmediate(OldObjects[i]);
		}
	}
	[Button]
	private void MakeRandomRotations()
	{
		for (int i = 0; i < RotateObjects.Length; i++)
		{
			RotateObjects[i].transform.rotation = RandomRotations[Random.Range(0, RandomRotations.Length)];
		}
	}

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
	private void SetRandomColorsFromMaterialList()
	{
		for (int i = 0; i < SingleBlocks.Length; i++)
		{
			SingleBlocks[i].SetBlockMaterial(RandomMaterials[Random.Range(0, RandomMaterials.Length)]);
		}
	}
	[Button]
	private void SetRandomColorsFromVariantList()
	{
		for (int i = 0; i < SingleBlocks.Length; i++)
		{
			SingleBlocks[i].SetBlockMaterial(MaterialVariants.ColorVariants[Random.Range(0, MaterialVariants.ColorVariants.Count)].VariantMaterial);
		}
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