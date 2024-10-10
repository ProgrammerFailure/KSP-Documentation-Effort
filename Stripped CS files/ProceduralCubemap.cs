using UnityEngine;

public class ProceduralCubemap : MonoBehaviour
{
	public int cubeSize = 4096;

	public float cameraNear = 1f;

	public float cameraFar = 1000f;

	public string outputName = "";

	public bool individualFaces;

	public Texture2D[] CreateIndividualFaces()
	{
		return RenderIndividualFaces(base.transform.position, cubeSize, cameraNear, cameraFar, TextureFormat.RGB24, createMipmaps: false);
	}

	public static Texture2D[] RenderIndividualFaces(Vector3 position, int cubeSize, float cameraNear, float cameraFar, TextureFormat textureFormat, bool createMipmaps)
	{
		Camera[] array = new Camera[6]
		{
			CreateCamera("CamXP", position, Vector3.right, 90f, cameraNear, cameraFar),
			CreateCamera("CamXN", position, Vector3.left, 90f, cameraNear, cameraFar),
			CreateCamera("CamYP", position, Vector3.down, 90f, cameraNear, cameraFar),
			CreateCamera("CamYN", position, Vector3.up, 90f, cameraNear, cameraFar),
			CreateCamera("CamZP", position, Vector3.forward, 90f, cameraNear, cameraFar),
			CreateCamera("CamZN", position, Vector3.back, 90f, cameraNear, cameraFar)
		};
		Texture2D[] array2 = new Texture2D[6];
		for (int i = 0; i < 6; i++)
		{
			array2[i] = RenderCamera(array[i], cubeSize, cubeSize, textureFormat);
			Texture2D obj = array2[i];
			CubemapFace cubemapFace = (CubemapFace)i;
			obj.name = cubemapFace.ToString();
			Object.DestroyImmediate(array[i].gameObject);
		}
		return array2;
	}

	public Cubemap CreateCubeMap()
	{
		return RenderCubemap(base.transform.position, cubeSize, cameraNear, cameraFar, TextureFormat.RGB24, createMipmaps: false);
	}

	public static Cubemap RenderCubemap(Vector3 position, int cubeSize, float cameraNear, float cameraFar, TextureFormat textureFormat, bool createMipmaps)
	{
		Camera[] array = new Camera[6]
		{
			CreateCamera("CamXP", position, Vector3.right, 90f, cameraNear, cameraFar),
			CreateCamera("CamXN", position, Vector3.left, 90f, cameraNear, cameraFar),
			CreateCamera("CamYP", position, Vector3.down, 90f, cameraNear, cameraFar),
			CreateCamera("CamYN", position, Vector3.up, 90f, cameraNear, cameraFar),
			CreateCamera("CamZP", position, Vector3.forward, 90f, cameraNear, cameraFar),
			CreateCamera("CamZN", position, Vector3.back, 90f, cameraNear, cameraFar)
		};
		Cubemap cubemap = new Cubemap(cubeSize, textureFormat, createMipmaps);
		Texture2D texture2D = null;
		for (int i = 0; i < 6; i++)
		{
			texture2D = RenderCamera(array[i], cubeSize, cubeSize, textureFormat);
			cubemap.SetPixels(texture2D.GetPixels(), (CubemapFace)i);
			Object.DestroyImmediate(texture2D);
			Object.DestroyImmediate(array[i].gameObject);
		}
		cubemap.Apply(createMipmaps);
		return cubemap;
	}

	public static Camera CreateCamera(string name, Vector3 pos, Vector3 dir, float size, float near, float far)
	{
		GameObject obj = new GameObject(name);
		obj.transform.position = pos;
		obj.transform.forward = dir;
		Camera camera = obj.AddComponent<Camera>();
		camera.orthographic = false;
		camera.fieldOfView = size;
		camera.nearClipPlane = near;
		camera.farClipPlane = far;
		camera.clearFlags = CameraClearFlags.Depth;
		camera.backgroundColor = Color.black;
		return camera;
	}

	public static Camera CreateCameraOrtho(string name, Vector3 pos, Vector3 dir, float size, float near, float far)
	{
		GameObject obj = new GameObject(name);
		obj.transform.position = pos;
		obj.transform.forward = dir;
		Camera camera = obj.AddComponent<Camera>();
		camera.orthographic = true;
		camera.orthographicSize = size;
		camera.nearClipPlane = near;
		camera.farClipPlane = far;
		camera.clearFlags = CameraClearFlags.Depth;
		camera.backgroundColor = Color.black;
		return camera;
	}

	public static Texture2D RenderCamera(Camera cam, int width, int height, TextureFormat textureFormat)
	{
		RenderTexture renderTexture = new RenderTexture(width, height, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
		renderTexture.Create();
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = renderTexture;
		cam.targetTexture = renderTexture;
		cam.Render();
		Texture2D texture2D = new Texture2D(width, height, textureFormat, mipChain: false);
		texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
		texture2D.Apply();
		RenderTexture.active = active;
		cam.targetTexture = null;
		renderTexture.Release();
		Object.DestroyImmediate(renderTexture);
		renderTexture = null;
		return texture2D;
	}
}
