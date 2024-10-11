using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class PSpaceObject : MonoBehaviour
{
	[SerializeField]
	protected GameObject visualObject;

	[SerializeField]
	internal Mesh visualMesh;

	[SerializeField]
	protected bool procedural;

	protected Renderer visualRenderer;

	[SerializeField]
	internal GameObject colliderObject;

	[SerializeField]
	protected Mesh colliderMesh;

	protected SpaceObjectCollider soc;

	protected Part partComponent;

	[SerializeField]
	protected GameObject convexObject;

	[SerializeField]
	protected Mesh convexColliderMesh;

	[SerializeField]
	protected int genTime;

	public float volume;

	public float highestPoint;

	[SerializeField]
	protected float maxRange;

	[SerializeField]
	protected float minRange;

	protected Callback onGenComplete;

	public Renderer VisualRenderer
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Mesh ConvexColliderMesh
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected PSpaceObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(Mesh visualMesh, Material visualMaterial, string visualLayer, string visualTag, Mesh colliderMesh, PhysicMaterial colliderMaterial, string colliderLayer, string colliderTag, Mesh convexMesh, PhysicMaterial convexMaterial, string convexLayer, string convexTag, Func<Transform, float> rangefinder, Callback onGenComplete)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateVisual(Mesh visualMesh, Material visualMaterial, string visualLayer, string visualTag)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateCollider(Mesh colliderMesh, PhysicMaterial colliderMaterial, string colliderLayer, string colliderTag, Func<Transform, float> rangefinder)
	{
		throw null;
	}

	public abstract void SetupPartParameters();

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateConvexCollider(Mesh convexMesh, PhysicMaterial convexMaterial, string convexLayer, string convexTag)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onSOCCSetupComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetMaterialColor(string name, Color value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal Renderer GetVisualObjectRenderer()
	{
		throw null;
	}
}
