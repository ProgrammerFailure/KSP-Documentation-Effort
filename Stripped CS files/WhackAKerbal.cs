using System;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]
public class WhackAKerbal : MonoBehaviour
{
	public PrimitiveType projectileType;

	public Rect dialogRect = new Rect(10f, 220f, 80f, 120f);

	public float mass = 50f;

	public float speed = 100f;

	public float size = 2f;

	public bool armed = true;

	public int maxObjects = 15;

	public GameObject[] projectiles;

	public int pCursor;

	public bool over;

	public void Start()
	{
		projectiles = new GameObject[50];
		pCursor = 0;
	}

	public void Update()
	{
		if (armed && Input.GetMouseButtonDown(2))
		{
			GameObject gameObject = GameObject.CreatePrimitive(projectileType);
			gameObject.transform.localScale = Vector3.one * size;
			gameObject.layer = 15;
			gameObject.AddComponent<Rigidbody>();
			gameObject.GetComponent<Rigidbody>().mass = mass;
			gameObject.transform.position = base.transform.position;
			gameObject.GetComponent<Rigidbody>().AddForce(GetComponent<Camera>().ScreenPointToRay(Input.mousePosition).direction * speed, ForceMode.VelocityChange);
			gameObject.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.On;
			gameObject.GetComponent<Renderer>().receiveShadows = false;
			if (projectiles[pCursor] != null)
			{
				UnityEngine.Object.Destroy(projectiles[pCursor]);
			}
			projectiles[pCursor] = gameObject;
			pCursor = (pCursor + 1) % maxObjects;
		}
	}

	public void OnGUI()
	{
		dialogRect = GUILayout.Window(89, dialogRect, drawProjectileOptions, "Whack-A-Kerbal");
		if (dialogRect.Contains(new Vector3(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y, Input.mousePosition.z)))
		{
			if (!over)
			{
				over = true;
				InputLockManager.SetControlLock(ControlTypes.ALLBUTCAMERAS, "DebugToolbar-WhackAKerbal");
			}
		}
		else if (over)
		{
			over = false;
			InputLockManager.RemoveControlLock("DebugToolbar-WhackAKerbal");
		}
	}

	public void drawProjectileOptions(int id)
	{
		GUILayout.Label("Object Type");
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("<"))
		{
			projectileType = (PrimitiveType)Mathf.Clamp((int)(projectileType - 1), 0, Enum.GetNames(typeof(PrimitiveType)).Length - 2);
		}
		GUILayout.Label(projectileType.ToString(), GUILayout.Width(80f));
		if (GUILayout.Button(">"))
		{
			projectileType = (PrimitiveType)Mathf.Clamp((int)(projectileType + 1), 0, Enum.GetNames(typeof(PrimitiveType)).Length - 2);
		}
		GUILayout.EndHorizontal();
		GUILayout.Label("Object Mass: " + KSPUtil.LocalizeNumber(mass, "0.0"));
		mass = GUILayout.HorizontalSlider(mass, 1f, 100f);
		GUILayout.Label("Object Size: " + KSPUtil.LocalizeNumber(size, "0.0"));
		size = GUILayout.HorizontalSlider(size, 0.1f, 10f);
		GUILayout.Label("Speed: " + KSPUtil.LocalizeNumber(speed, "0.0"));
		speed = GUILayout.HorizontalSlider(speed, 1f, 100f);
		GUILayout.Space(5f);
		armed = GUILayout.Toggle(armed, armed ? "ARMED" : "Safe");
		GUILayout.Label("MMB to fire.");
		GUI.DragWindow();
	}

	public void OnDestroy()
	{
		for (int i = 0; i < projectiles.Length; i++)
		{
			UnityEngine.Object.Destroy(projectiles[i]);
		}
		InputLockManager.RemoveControlLock("DebugToolbar-WhackAKerbal");
	}
}
