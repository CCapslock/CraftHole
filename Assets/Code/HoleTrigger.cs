using UnityEngine;
using NaughtyAttributes;

public class HoleTrigger : MonoBehaviour
{
	public bool DestroyObjectsParts;
	[Layer] [SerializeField] private int _blockLayer;
	[Layer] [SerializeField] private int _defaultLayer;


	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(TagManager.GetTag(TagType.Block)))
		{
			EnableBlockLayer(true, other.gameObject);
		}
		else if (other.CompareTag(TagManager.GetTag(TagType.ComplexBlock)))
		{
			if (DestroyObjectsParts)
			{
				other.GetComponentInParent<SingleComplexBlock>().DisassembleBlock(other.GetComponent<SingleBlock>());
			}
			else
			{
				other.GetComponentInParent<SingleComplexBlock>().HoleEnter(_blockLayer);
			}
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag(TagManager.GetTag(TagType.Block)))
		{
			EnableBlockLayer(false, other.gameObject);
		}
		else if (other.CompareTag(TagManager.GetTag(TagType.ComplexBlock)))
		{
			if (!DestroyObjectsParts)
			{
				other.GetComponentInParent<SingleComplexBlock>().HoleExit(_defaultLayer);
			}
		}
	}
	private void EnableBlockLayer(bool state, GameObject block)
	{
		if (state)
		{
			block.layer = _blockLayer;
		}
		else
		{
			block.layer = _defaultLayer;
		}
	}
}