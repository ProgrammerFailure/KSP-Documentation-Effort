using UnityEngine;

public class SunCoronas : MonoBehaviour
{
	public Camera m_Camera;

	public float Rotation;

	public int Speed;

	public float updateInterval = 10f;

	public float scaleLimitX;

	public float scaleLimitY;

	public Vector3 startingScale;

	public Vector3 scaleChange;

	public float scaleSpeed = 1f;

	public Vector3 targetScale;

	public float elapsedTime;

	public void Start()
	{
		startingScale = base.transform.localScale;
	}

	public void FixedUpdate()
	{
		elapsedTime += Time.deltaTime;
		base.transform.rotation = Quaternion.LookRotation(m_Camera.transform.position - base.transform.position, Vector3.up);
		base.transform.Rotate(new Vector3(0f, 0f, Rotation * (float)Speed));
		if (elapsedTime >= updateInterval)
		{
			targetScale = new Vector3(Random.Range(0f, scaleLimitX), Random.Range(0f, scaleLimitY), 0f);
			elapsedTime = 0f;
		}
		scaleChange = new Vector3(Mathf.Lerp(scaleChange.x, targetScale.x, scaleSpeed), Mathf.Lerp(scaleChange.y, targetScale.y, scaleSpeed), 0f);
		base.transform.localScale = startingScale + scaleChange;
	}
}
