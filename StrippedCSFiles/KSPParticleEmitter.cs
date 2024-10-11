using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("KSP/Particle Emitter")]
[ExecuteInEditMode]
public class KSPParticleEmitter : MonoBehaviour
{
	public enum EmissionShape
	{
		Ellipsoid,
		Ellipse,
		Sphere,
		Ring,
		Cuboid,
		Plane,
		Line,
		Point
	}

	[SerializeField]
	public ParticleSystem ps;

	public bool emit;

	public EmissionShape shape;

	public Vector3 shape3D;

	public Vector2 shape2D;

	public float shape1D;

	public Color color;

	public bool useWorldSpace;

	public float minSize;

	public float maxSize;

	public float minEnergy;

	public float maxEnergy;

	public int minEmission;

	public int maxEmission;

	public Vector3 worldVelocity;

	public Vector3 localVelocity;

	public Vector3 rndVelocity;

	public float emitterVelocityScale;

	public float angularVelocity;

	public float rndAngularVelocity;

	public bool rndRotation;

	public bool doesAnimateColor;

	public Color[] colorAnimation;

	public Vector3 worldRotationAxis;

	public Vector3 localRotationAxis;

	public float sizeGrow;

	public Vector3 rndForce;

	public Vector3 force;

	public float damping;

	public bool castShadows;

	public bool recieveShadows;

	public float lengthScale;

	public float velocityScale;

	public float maxParticleSize;

	public ParticleSystemRenderMode particleRenderMode;

	public int uvAnimationXTile;

	public int uvAnimationYTile;

	public int uvAnimationCycles;

	public Material material;

	private bool dirty;

	private int framesAlive;

	private float particlesThisFrame;

	private float amountPerSec;

	private float overflow;

	private int itr;

	private Vector3 position;

	private float posFloat;

	private Vector3 velocity;

	private float rotation;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPParticleEmitter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnsureParticleSystemCreated()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDirty()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupProperties()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EmitParticle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Emit()
	{
		throw null;
	}
}
