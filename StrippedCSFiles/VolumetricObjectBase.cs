using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class VolumetricObjectBase : MonoBehaviour
{
	public string volumeShader;

	protected Material volumetricMaterial;

	public float visibility;

	public Color volumeColor;

	public Texture3D texture;

	public float textureScale;

	public Vector3 textureMovement;

	protected Mesh meshInstance;

	protected Material materialInstance;

	protected Transform thisTransform;

	protected float previousVisibility;

	protected Color previousVolumeColor;

	protected Vector3 forcedLocalScale;

	protected Texture3D previousTexture;

	protected float previousTextureScale;

	protected Vector3 previousTextureMovement;

	protected Vector3[] unitVerts;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VolumetricObjectBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void CleanUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void PopulateShaderName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool HasChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetChangedValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateVolume()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupUnitVerts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Mesh CreateCube()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ScaleMesh(Mesh mesh, Vector3 scaleFactor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ScaleMesh(Mesh mesh, Vector3 scaleFactor, Vector3 addVector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 ScaleVector(Vector3 vector, Vector3 scale)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Mesh CopyMesh(Mesh original)
	{
		throw null;
	}
}
