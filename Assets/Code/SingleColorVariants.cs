using System;
using UnityEngine;
using NaughtyAttributes;

[Serializable]
public class SingleColorVariants
{
	[OnValueChanged(nameof(SetCorrectColor))]
	public Material VariantMaterial;
	public MainColor VariantMainColor;
	[OnValueChanged(nameof(SetCorrectColor))]
	public Color ThisColor;

	private float _currentColorH;
	private float _currentColorS;
	private float _currentColorV;
	private float _checkColorH;
	private float _checkColorS;
	private float _checkColorV;

	private float sum;
	
	public void SetCorrectColor()
	{
		ThisColor = VariantMaterial.GetColor("_BaseColor");
	}
	public float GetColorDistance(Color color)
	{
		sum = 0;
		Color.RGBToHSV(VariantMaterial.GetColor("_BaseColor"), out _currentColorH, out _currentColorS, out _currentColorV);
		Color.RGBToHSV(color, out _checkColorH, out _checkColorS, out _checkColorV);
		if (_currentColorH > _checkColorH)
		{
			sum += (_currentColorH - _checkColorH);
		}
		else
		{
			sum += (_checkColorH - _currentColorH);
		}
		if (_currentColorS > _checkColorS)
		{
			sum += _currentColorS - _checkColorS;
		}
		else
		{
			sum += _checkColorS - _currentColorS;
		}
		if (_currentColorV > _checkColorV)
		{
			sum += _currentColorV - _checkColorV;
		}
		else
		{
			sum += _checkColorV - _currentColorV;
		}
		return sum;
	}
}