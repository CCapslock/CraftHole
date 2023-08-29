using UnityEngine;

public class UnStaticTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(TagManager.GetTag(TagType.ComplexBlock)))
		{
			other.GetComponentInParent<SingleComplexBlock>().UnStatic();
		}
	}
}
