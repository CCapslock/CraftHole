using UnityEngine;

public class CollectTrigger : MonoBehaviour
{
	private HoleController _holeController;

	private void Awake()
	{
		_holeController = FindObjectOfType<HoleController>();
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(TagManager.GetTag(TagType.Block)))
		{
			_holeController.CollectBlock(other.GetComponent<SingleBlock>());
		}
	}
}
