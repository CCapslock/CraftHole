using UnityEngine;

[CreateAssetMenu(fileName = "BuildFigure", menuName = "ScriptableObjects/BuildFigure", order = 1)]
public class BuildFigureScriptableObject : ScriptableObject
{
	public SingleBlockInFigure[] FigureBlocks;	
}