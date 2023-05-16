using UnityEngine;

public class DestructionHoleTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.tag);
		if (other.CompareTag(TagManager.GetTag(TagType.ComplexBlock)))
		{
			other.GetComponentInParent<SingleComplexBlock>().SeparateBlocks();
		}
	}
}