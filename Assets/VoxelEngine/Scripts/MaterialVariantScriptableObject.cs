using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "MaterialVariants", menuName = "ScriptableObjects/MaterialVariants", order = 1)]
public class MaterialVariantScriptableObject : ScriptableObject
{
	public Material[] AllMaterials;
	public List<SingleColorVariant> ColorVariants;
	public Color _color;
	public Color _recivedColor;
	public string[] _allMaterialsNames;

	public SingleColorVariant GetClosestColorVariant(Color color)
	{
		float minDistance = float.MaxValue;
		int colorIndex = 0;
		for (int i = 0; i < ColorVariants.Count; i++)
		{
			if (ColorVariants[i].GetColorDistance(color) < minDistance)
			{
				minDistance = ColorVariants[i].GetColorDistance(color);
				colorIndex = i;
			}
		}
		return ColorVariants[colorIndex];
	}
	[Button]
	public void CheckChecker()
	{
		_recivedColor = GetClosestColorVariant(_color).VariantMaterial.GetColor("_BaseColor");
	}
	[Button]
	public void SetCorrectColors()
	{
		for (int i = 0; i < ColorVariants.Count; i++)
		{
			ColorVariants[i].SetCorrectColor();
		}
	}
#if UNITY_EDITOR
	[Button]
	public void SetAllMaterial()
	{
		string[] path = { "Assets/Art/SelectedMaterials" };
		_allMaterialsNames = AssetDatabase.FindAssets("", path);
		
		AllMaterials = new Material[_allMaterialsNames.Length];
		for (int i = 0; i < _allMaterialsNames.Length; i++)
		{
			AllMaterials[i] = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(_allMaterialsNames[i]), typeof(Material)) as Material;
		}
		bool hasMaterialAlredy;
		for (int i = 0; i < AllMaterials.Length; i++)
		{
			hasMaterialAlredy = false;
			for (int j = 0; j < ColorVariants.Count; j++)
			{
				if (ColorVariants[j].VariantMaterial == AllMaterials[i])
				{
					hasMaterialAlredy = true;
					break;
				}
			}
			if (!hasMaterialAlredy)
			{
				ColorVariants.Add(new SingleColorVariant(AllMaterials[i]));
			}
		}
	
	}
#endif
}