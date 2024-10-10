using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class VolumetricObjectBase : MonoBehaviour
{
	public string volumeShader = "";

	public Material volumetricMaterial;

	public float visibility = 3f;

	public Color volumeColor = new Color(1f, 1f, 1f, 1f);

	public Texture3D texture;

	public float textureScale = 1f;

	public Vector3 textureMovement = new Vector3(0f, -0.1f, 0f);

	public Mesh meshInstance;

	public Material materialInstance;

	public Transform thisTransform;

	public float previousVisibility = 1f;

	public Color previousVolumeColor = new Color(1f, 1f, 1f, 1f);

	public Vector3 forcedLocalScale = Vector3.one;

	public Texture3D previousTexture;

	public float previousTextureScale = 10f;

	public Vector3 previousTextureMovement = new Vector3(0f, 0.1f, 0f);

	public Vector3[] unitVerts = new Vector3[8];

	public virtual void OnEnable()
	{
		SetupUnitVerts();
		thisTransform = base.transform;
		if (meshInstance != null)
		{
			Object.Destroy(meshInstance);
		}
		meshInstance = CreateCube();
		GetComponent<MeshFilter>().sharedMesh = meshInstance;
		if (materialInstance != null)
		{
			Object.Destroy(materialInstance);
		}
		if (volumeShader == "")
		{
			PopulateShaderName();
		}
		volumetricMaterial = new Material(Shader.Find(volumeShader));
		MeshRenderer component = GetComponent<MeshRenderer>();
		component.sharedMaterial = volumetricMaterial;
		materialInstance = component.sharedMaterial;
		component.castShadows = false;
		component.receiveShadows = false;
		if ((bool)Camera.current)
		{
			Camera.current.depthTextureMode |= DepthTextureMode.Depth;
		}
		if ((bool)Camera.main)
		{
			Camera.main.depthTextureMode |= DepthTextureMode.Depth;
		}
		UpdateVolume();
	}

	public virtual void OnDestroy()
	{
		CleanUp();
	}

	public virtual void OnDisable()
	{
		CleanUp();
	}

	public virtual void CleanUp()
	{
		if ((bool)materialInstance)
		{
			Object.DestroyImmediate(materialInstance);
		}
		if ((bool)meshInstance)
		{
			Object.DestroyImmediate(meshInstance);
		}
	}

	public virtual void PopulateShaderName()
	{
		volumeShader = "Advanced SS/Volumetric/Box Volume";
	}

	public void LateUpdate()
	{
		if (HasChanged())
		{
			SetChangedValues();
			UpdateVolume();
		}
	}

	public virtual bool HasChanged()
	{
		if (visibility == previousVisibility && !(volumeColor != previousVolumeColor) && !(thisTransform.localScale != forcedLocalScale) && !(texture != previousTexture) && textureScale == previousTextureScale && !(textureMovement != previousTextureMovement))
		{
			return false;
		}
		return true;
	}

	public virtual void SetChangedValues()
	{
		previousVisibility = visibility;
		previousVolumeColor = volumeColor;
		thisTransform.localScale = forcedLocalScale;
		previousTexture = texture;
		previousTextureScale = textureScale;
		previousTextureMovement = textureMovement;
	}

	public virtual void UpdateVolume()
	{
	}

	public void SetupUnitVerts()
	{
		float num = 0.5f;
		unitVerts[0].x = -0.5f;
		unitVerts[0].y = -0.5f;
		unitVerts[0].z = -0.5f;
		unitVerts[1].x = num;
		unitVerts[1].y = -0.5f;
		unitVerts[1].z = -0.5f;
		unitVerts[2].x = num;
		unitVerts[2].y = num;
		unitVerts[2].z = -0.5f;
		unitVerts[3].x = num;
		unitVerts[3].y = -0.5f;
		unitVerts[3].z = num;
		unitVerts[4].x = num;
		unitVerts[4].y = num;
		unitVerts[4].z = num;
		unitVerts[5].x = -0.5f;
		unitVerts[5].y = num;
		unitVerts[5].z = -0.5f;
		unitVerts[6].x = -0.5f;
		unitVerts[6].y = num;
		unitVerts[6].z = num;
		unitVerts[7].x = -0.5f;
		unitVerts[7].y = -0.5f;
		unitVerts[7].z = num;
	}

	public Mesh CreateCube()
	{
		Mesh mesh = new Mesh();
		Vector3[] array = new Vector3[unitVerts.Length];
		unitVerts.CopyTo(array, 0);
		mesh.vertices = array;
		mesh.triangles = new int[36]
		{
			0, 2, 1, 0, 5, 2, 3, 6, 7, 3,
			4, 6, 1, 4, 3, 1, 2, 4, 7, 5,
			0, 7, 6, 5, 7, 1, 3, 7, 0, 1,
			5, 4, 2, 5, 6, 4
		};
		mesh.RecalculateBounds();
		return mesh;
	}

	public void ScaleMesh(Mesh mesh, Vector3 scaleFactor)
	{
		ScaleMesh(mesh, scaleFactor, Vector3.zero);
	}

	public void ScaleMesh(Mesh mesh, Vector3 scaleFactor, Vector3 addVector)
	{
		Vector3[] array = new Vector3[mesh.vertexCount];
		for (int i = 0; i < mesh.vertexCount; i++)
		{
			array[i] = ScaleVector(unitVerts[i], scaleFactor) + addVector;
		}
		mesh.vertices = array;
	}

	public Vector3 ScaleVector(Vector3 vector, Vector3 scale)
	{
		return new Vector3(vector.x * scale.x, vector.y * scale.y, vector.z * scale.z);
	}

	public Mesh CopyMesh(Mesh original)
	{
		Mesh mesh = new Mesh();
		Vector3[] array = new Vector3[original.vertices.Length];
		original.vertices.CopyTo(array, 0);
		mesh.vertices = array;
		Vector2[] array2 = new Vector2[original.uv.Length];
		original.uv.CopyTo(array2, 0);
		mesh.uv = array2;
		Vector2[] array3 = new Vector2[original.uv2.Length];
		original.uv2.CopyTo(array3, 0);
		mesh.uv2 = array3;
		Vector2[] array4 = new Vector2[original.uv2.Length];
		original.uv2.CopyTo(array4, 0);
		mesh.uv2 = array4;
		Vector3[] array5 = new Vector3[original.normals.Length];
		original.normals.CopyTo(array5, 0);
		mesh.normals = array5;
		Vector4[] array6 = new Vector4[original.tangents.Length];
		original.tangents.CopyTo(array6, 0);
		mesh.tangents = array6;
		Color[] array7 = new Color[original.colors.Length];
		original.colors.CopyTo(array7, 0);
		mesh.colors = array7;
		mesh.subMeshCount = original.subMeshCount;
		for (int i = 0; i < original.subMeshCount; i++)
		{
			int[] triangles = original.GetTriangles(i);
			int[] array8 = new int[triangles.Length];
			triangles.CopyTo(array8, 0);
			mesh.SetTriangles(triangles, i);
		}
		mesh.RecalculateBounds();
		return mesh;
	}
}
