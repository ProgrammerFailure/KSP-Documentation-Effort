using System.Runtime.CompilerServices;
using UnityEngine;

public class ProceduralCubemap : MonoBehaviour
{
	public int cubeSize;

	public float cameraNear;

	public float cameraFar;

	public string outputName;

	public bool individualFaces;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProceduralCubemap()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Texture2D[] CreateIndividualFaces()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Texture2D[] RenderIndividualFaces(Vector3 position, int cubeSize, float cameraNear, float cameraFar, TextureFormat textureFormat, bool createMipmaps)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Cubemap CreateCubeMap()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Cubemap RenderCubemap(Vector3 position, int cubeSize, float cameraNear, float cameraFar, TextureFormat textureFormat, bool createMipmaps)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Camera CreateCamera(string name, Vector3 pos, Vector3 dir, float size, float near, float far)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Camera CreateCameraOrtho(string name, Vector3 pos, Vector3 dir, float size, float near, float far)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Texture2D RenderCamera(Camera cam, int width, int height, TextureFormat textureFormat)
	{
		throw null;
	}
}
