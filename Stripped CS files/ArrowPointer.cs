using UnityEngine;
using UnityEngine.Rendering;

public class ArrowPointer : MonoBehaviour
{
	public Vector3 offset;

	public Vector3 direction;

	public float length;

	public Color color;

	public bool worldSpace;

	public Transform parent;

	public MeshFilter mf;

	public MeshRenderer mr;

	public Mesh mesh;

	public Vector3[] vertices;

	public Color32[] vertexColors;

	public bool vertexColorsRequireUpdate;

	public bool verticesRequireUpdate;

	public Vector3 Offset
	{
		get
		{
			return offset;
		}
		set
		{
			offset = value;
			verticesRequireUpdate = true;
		}
	}

	public Vector3 Direction
	{
		get
		{
			return direction;
		}
		set
		{
			direction = value;
			verticesRequireUpdate = true;
		}
	}

	public float Length
	{
		get
		{
			return length;
		}
		set
		{
			length = value;
			verticesRequireUpdate = true;
		}
	}

	public Color Color
	{
		get
		{
			return color;
		}
		set
		{
			color = value;
			vertexColorsRequireUpdate = true;
		}
	}

	public bool WorldSpace
	{
		get
		{
			return worldSpace;
		}
		set
		{
			worldSpace = value;
			verticesRequireUpdate = true;
		}
	}

	public static ArrowPointer Create(Transform parent, Vector3 offset, Vector3 direction, float length, Color color, bool worldSpace)
	{
		GameObject gameObject = new GameObject("ArrowPointer");
		if (parent != null)
		{
			gameObject.transform.SetParent(parent, worldPositionStays: false);
		}
		ArrowPointer arrowPointer = gameObject.AddComponent<ArrowPointer>();
		arrowPointer.parent = parent;
		arrowPointer.offset = offset;
		arrowPointer.direction = direction;
		arrowPointer.length = length;
		arrowPointer.color = color;
		arrowPointer.worldSpace = worldSpace;
		arrowPointer.CreateMesh();
		return arrowPointer;
	}

	public void CreateMesh()
	{
		mesh = new Mesh();
		mesh.MarkDynamic();
		vertices = new Vector3[4];
		UpdateVerts(offset, direction, length, worldSpace);
		vertexColors = new Color32[4];
		UpdateVertexColors(color);
		int[] indices = new int[9] { 0, 1, 2, 0, 2, 3, 0, 3, 1 };
		mesh.SetIndices(indices, MeshTopology.Triangles, 0);
		Vector3[] array = new Vector3[4];
		Vector2[] array2 = new Vector2[4];
		for (int num = 3; num >= 0; num--)
		{
			array[num] = Vector3.one;
			array2[num] = Vector2.zero;
		}
		mesh.normals = array;
		mesh.uv = array2;
		mf = base.gameObject.AddComponent<MeshFilter>();
		mf.sharedMesh = mesh;
		mr = base.gameObject.AddComponent<MeshRenderer>();
		mr.sharedMaterial = ArrowPointerSystem.Material;
		mr.shadowCastingMode = ShadowCastingMode.Off;
		mr.receiveShadows = false;
	}

	public void UpdateVerts(Vector3 offset, Vector3 direction, float length, bool worldSpace)
	{
		if (direction.IsSmallerThan(0.01f))
		{
			direction = Vector3.forward;
			length = 0f;
		}
		if (worldSpace && parent != null)
		{
			direction = parent.InverseTransformDirection(direction);
		}
		Quaternion quaternion = Quaternion.LookRotation(direction, Vector3.forward);
		vertices[0] = offset + quaternion * new Vector3(0f, 0f, length);
		vertices[1] = offset + quaternion * new Vector3(-0.5f, 0.866f, 0f) * ArrowPointerSystem.BaseSize;
		vertices[2] = offset + quaternion * new Vector3(-0.5f, -0.866f, 0f) * ArrowPointerSystem.BaseSize;
		vertices[3] = offset + quaternion * new Vector3(1f, 0f, 0f) * ArrowPointerSystem.BaseSize;
		mesh.vertices = vertices;
	}

	public void UpdateVertexColors(Color32 color)
	{
		for (int num = 3; num >= 0; num--)
		{
			vertexColors[num] = color;
		}
		mesh.colors32 = vertexColors;
	}

	public void Update()
	{
		if (verticesRequireUpdate || worldSpace)
		{
			verticesRequireUpdate = false;
			UpdateVerts(offset, direction, length, worldSpace);
		}
		if (vertexColorsRequireUpdate)
		{
			vertexColorsRequireUpdate = false;
			UpdateVertexColors(color);
		}
	}

	public void OnDestroy()
	{
		base.gameObject.SetActive(value: false);
		if (mr != null)
		{
			Object.Destroy(mr);
			mr = null;
		}
		if (mf != null)
		{
			Object.Destroy(mf);
			mf = null;
		}
		if (mesh != null)
		{
			Object.Destroy(mesh);
			mesh = null;
		}
		parent = null;
	}
}
