using NaughtyAttributes;
using UnityEngine;

public class SingleBlock : MonoBehaviour
{
	public Color BlockColor;

	[Layer] [SerializeField] private int _blockLayer;
	[Layer] [SerializeField] private int _defaultLayer;

	[SerializeField] private Rigidbody _rigidbody;
	[SerializeField] private Renderer _renderer;
	[SerializeField] private Material _cloneMaterial;
	private Vector3 _goalPosition;

	private void Start()
	{
	}
	public void SeparateBlock()
	{
		transform.parent = null;
		tag = TagManager.GetTag(TagType.Block);
		_rigidbody = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
		//_rigidbody = GetComponent<Rigidbody>();
		_rigidbody.isKinematic = false;
		_rigidbody.useGravity = true;
		//_rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

	}
	public void CombineBlock(Transform parentTransform)
	{
		_rigidbody = GetComponent<Rigidbody>();
		DestroyImmediate(_rigidbody);
		transform.parent = parentTransform;
		tag = parentTransform.tag;
	}
	public void StartMovingBlock(Vector3 goalPosition)
	{
		_rigidbody.isKinematic = true;
		_rigidbody.useGravity = false;
		_goalPosition = goalPosition;
	}
	public void MoveBlock(float speed)
	{
		transform.position = Vector3.MoveTowards(transform.position, _goalPosition, speed);
	}
	public bool IsBlockAchivedGoal()
	{
		return transform.position == _goalPosition;
	}
	public void SetBlockColor(Color color)
	{
		BlockColor = color;
		SetCorrectColor();
	}
	[Button]
	public void SetCorrectColor()
	{
		_cloneMaterial = new Material(_renderer.sharedMaterials[0]);
		_cloneMaterial.SetColor("_BaseColor", BlockColor);
		_renderer.sharedMaterial = _cloneMaterial;
	}
}