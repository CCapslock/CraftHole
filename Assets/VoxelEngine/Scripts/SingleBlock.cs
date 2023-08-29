using NaughtyAttributes;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SingleBlock : MonoBehaviour
{
	public Material BlockMaterial;
	public GameObject StaticObject;

	[HideInInspector] public Color BlockColor;

	[Layer] [SerializeField] private int _blockLayer;
	[Layer] [SerializeField] private int _defaultLayer;

	[SerializeField] private Rigidbody _rigidbody;
	[SerializeField] private Renderer _renderer;
	[SerializeField] private List<SingleBlock> _neighbours;
	private BoxCollider _blockCollider;
	private Material _cloneMaterial;
	private SingleComplexBlock _parentComplexBlock;
	private Vector3 _goalPosition;
	private Vector3[] _goalPositions = new Vector3[5];
	[SerializeField] private float _blockSize;
	private int _goalPositionNum;
	private bool _isGoingDown;
	private bool _isSeparated;

	private void Start()
	{
		_blockCollider = GetComponent<BoxCollider>();
		if (!transform.parent.TryGetComponent(out _parentComplexBlock))
		{
			_isSeparated = true;
			UnStatic();
		}
	}
	[Button]
	public void MakeCorrectColor()
	{
		BlockMaterial = _renderer.sharedMaterial;
		StaticObject.GetComponent<Renderer>().sharedMaterial = BlockMaterial;
	}
	public void UnStatic()
	{
		_renderer.enabled = true;
		StaticObject.SetActive(false);
	}
	public Material GetMaterialFromRenderer()
	{
		return _renderer.sharedMaterial;
	}
	public void SeparateBlock()
	{
		if (_isSeparated)
			return;
		_isSeparated = true;
		transform.parent = null;
		tag = TagManager.GetTag(TagType.Block);
		_rigidbody = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
		//_rigidbody = GetComponent<Rigidbody>();
		_rigidbody.isKinematic = false;
		_rigidbody.useGravity = true;
		_rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

	}
	public void AddForceToBlock(Vector3 goal, float force, ForceMode forceMode)
	{
		_rigidbody.AddForce((goal - transform.position).normalized * force, ForceMode.Acceleration);
	}
	public void SeparateBlock(PhysicMaterial mat)
	{
		if (_isSeparated)
			return;
		_isSeparated = true;
		transform.parent = null;
		tag = TagManager.GetTag(TagType.Block);
		_blockCollider.material = mat;
		_rigidbody = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
		//_rigidbody = GetComponent<Rigidbody>();
		_rigidbody.isKinematic = false;
		_rigidbody.useGravity = true;
		_rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

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
		_goalPositions[0] = goalPosition;
		_goalPositions[1] = goalPosition + Vector3.up * 0.5f;
		_goalPositions[2] = goalPosition;
		_goalPositions[3] = goalPosition + Vector3.up * 0.2f;
		_goalPositions[4] = goalPosition;
	}
	public void MoveBlock(float speed)
	{
		transform.position = Vector3.MoveTowards(transform.position, _goalPosition, speed);

	}
	public void MoveBlockNew(float speed)
	{
		transform.position = Vector3.MoveTowards(transform.position, _goalPositions[_goalPositionNum], speed);
		if (transform.position == _goalPositions[_goalPositionNum])
		{
			if (_goalPositionNum < 4)
			{
				_goalPositionNum++;
			}
		}
	}
	public bool IsBlockAchivedGoal()
	{
		return transform.position == _goalPosition;
	}
	public bool IsBlockAchivedGoalNew()
	{
		return transform.position == _goalPositions[4] && _goalPositionNum == 2;
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