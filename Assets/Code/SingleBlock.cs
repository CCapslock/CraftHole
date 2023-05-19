using NaughtyAttributes;
using UnityEngine;
using System.Collections.Generic;

public class SingleBlock : MonoBehaviour
{
	public Material BlockMaterial;

	[Layer] [SerializeField] private int _blockLayer;
	[Layer] [SerializeField] private int _defaultLayer;

	[SerializeField] private Rigidbody _rigidbody;
	[SerializeField] private Renderer _renderer;
	[SerializeField] private List<SingleBlock> _neighbours;
	private BoxCollider _blockCollider;
	private Vector3 _goalPosition;
	[SerializeField] private float _blockSize;

	private void Start()
	{
		_blockCollider = GetComponent<BoxCollider>();
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
	public void SetBlockColor(Material mat)
	{
		BlockMaterial = mat;
		SetCorrectColor();
	}
	[Button]
	public void SetCorrectColor()
	{
		_renderer.sharedMaterial = BlockMaterial;
	}
	[Button]
	public void GetNeighbours()
	{
		_neighbours = new List<SingleBlock>();
		_blockCollider = GetComponent<BoxCollider>();
		_blockCollider.enabled = false;

		Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

		RaycastHit hit;
		SingleBlock temp;
		// Does the ray intersect any objects excluding the player layer
		for (int i = 0; i < directions.Length; i++)
		{
			if (Physics.Raycast(transform.position, directions[i], out hit, _blockSize))
			{
				if (hit.collider.TryGetComponent(out temp))
					_neighbours.Add(temp);
			}
		}
		_blockCollider.enabled = true;
	}
}