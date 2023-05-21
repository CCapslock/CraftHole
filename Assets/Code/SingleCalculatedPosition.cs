using System;
using UnityEngine;

[Serializable]
public class SingleCalculatedPosition 
{
    public bool IsHasBlock = false;
    public Material BlockMaterial ;
    [HideInInspector]public Color BlockColor;

    public SingleCalculatedPosition()
	{

	}
}