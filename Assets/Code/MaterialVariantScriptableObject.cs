using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "MaterialVariants", menuName = "ScriptableObjects/MaterialVariants", order = 1)]
public class MaterialVariantScriptableObject : ScriptableObject
{
	public SingleColorVariants[] ColorVariants;
	public Color _color;
	public Color _recivedColor;

	public SingleColorVariants GetClosestColorVariant(Color color)
	{
		float minDistance = float.MaxValue;
		int colorIndex = 0;
		for (int i = 0; i < ColorVariants.Length; i++)
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
		for (int i = 0; i < ColorVariants.Length; i++)
		{
			ColorVariants[i].SetCorrectColor();
		}
	}
}