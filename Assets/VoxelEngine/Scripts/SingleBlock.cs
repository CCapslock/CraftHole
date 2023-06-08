using NaughtyAttributes;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SingleBlock : MonoBehaviour
{
	public Material BlockMaterial;

	// чисто для старого сканера
	[HideInInspector] public Color BlockColor;

	[Layer] [SerializeField] private int _blockLayer;
	[Layer] [SerializeField] private int _defaultLayer;

	[SerializeField] private Rigidbody _rigidbody;
	[SerializeField] private Renderer _renderer;
	[SerializeField] private List<SingleBlock> _neighbours;
	private BoxCollider _blockCollider;
	private Material _cloneMaterial;
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
	public void SetBlockMaterial(Material mat)
	{
		BlockMaterial = mat;
		SetCorrectMaterial();
	}
	[Button]
	public void SetCorrectMaterial()
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
#if UNITY_EDITOR
	[Button]
	private void CreateMaterialAsset()
	{
		Material material = new Material(_cloneMaterial);
		string[] temp = { "Assets/Art/SelectedMaterials" };
		AssetDatabase.CreateAsset(material, "Assets/Art/SelectedMaterials/NewSelectedMaterial" + (AssetDatabase.FindAssets("", temp).Length + 1).ToString() + ".mat");
	}
#endif
}