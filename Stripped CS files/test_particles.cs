using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test_particles : MonoBehaviour
{
	public float power = 0.5f;

	public float powerVariation;

	public float throttleSpeed = 2f;

	public List<float> flameInitSizeValuesNewSystem;

	public List<float> flameInitLifeValuesNewSystem;

	public List<float> smokeInitSizeValuesNewSystem;

	public List<float> smokeInitLifeValuesNewSystem;

	public List<ParticleSystem> flameoutList;

	public List<ParticleSystem> smokeList;

	public List<ParticleSystem> flameList;

	public bool flameEnabled = true;

	public bool smokeEnabled = true;

	public Text throttleText;

	public Text smokeText;

	public Text flameText;

	public Camera testcamera;

	public float camMoveSpeed = 15f;

	public float camRotateSpeed = 2f;

	public Vector3 dragOrigin;

	public Vector3 camMove;

	public void Start()
	{
		flameInitSizeValuesNewSystem = new List<float>();
		flameInitLifeValuesNewSystem = new List<float>();
		smokeInitSizeValuesNewSystem = new List<float>();
		smokeInitLifeValuesNewSystem = new List<float>();
		for (int i = 0; i < flameList.Count; i++)
		{
			ParticleSystem.MainModule main = flameList[i].main;
			flameInitSizeValuesNewSystem.Add(main.startSize.constantMax);
			flameInitLifeValuesNewSystem.Add(main.startLifetime.constantMax);
		}
		for (int j = 0; j < smokeList.Count; j++)
		{
			ParticleSystem.MainModule main2 = smokeList[j].main;
			smokeInitSizeValuesNewSystem.Add(main2.startSize.constantMax);
			smokeInitLifeValuesNewSystem.Add(main2.startLifetime.constantMax);
		}
		for (int k = 0; k < flameoutList.Count; k++)
		{
			flameoutList[k].Stop();
		}
		SetPower(power);
	}

	public void Update()
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			SetPower(power + Time.deltaTime / throttleSpeed);
		}
		else if (Input.GetKey(KeyCode.LeftControl))
		{
			SetPower(power - Time.deltaTime / throttleSpeed);
		}
		if (Input.GetKeyDown(KeyCode.X))
		{
			SetPower(0f);
		}
		if (Input.GetKeyDown(KeyCode.Z))
		{
			SetPower(1f);
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			for (int i = 0; i < flameoutList.Count; i++)
			{
				ParticleSystem particleSystem = flameoutList[i];
				particleSystem.Stop();
				particleSystem.Play();
			}
		}
		if (Input.GetKeyDown(KeyCode.F))
		{
			flameEnabled = !flameEnabled;
			SetPower(power);
		}
		if (Input.GetKeyDown(KeyCode.G))
		{
			smokeEnabled = !smokeEnabled;
			SetPower(power);
		}
		camMove = default(Vector3);
		if (Input.GetKey(KeyCode.W))
		{
			camMove.z += camMoveSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.S))
		{
			camMove.z -= camMoveSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.A))
		{
			camMove.x -= camMoveSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.D))
		{
			camMove.x += camMoveSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.Q))
		{
			camMove.y += camMoveSpeed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.E))
		{
			camMove.y -= camMoveSpeed * Time.deltaTime;
		}
		MouseDrag();
		if (throttleText != null)
		{
			throttleText.text = $"Throttle: {power:0.00}";
		}
		if (smokeText != null)
		{
			smokeText.text = $"Smoke: {smokeEnabled}";
		}
		if (flameText != null)
		{
			flameText.text = $"Flame: {flameEnabled}";
		}
	}

	public void MouseDrag()
	{
		if (Input.GetMouseButton(1))
		{
			Camera.main.transform.Rotate(new Vector3((0f - Input.GetAxis("Mouse Y")) * camRotateSpeed, Input.GetAxis("Mouse X") * camRotateSpeed, 0f), Space.Self);
		}
	}

	public void LateUpdate()
	{
		if (testcamera != null)
		{
			testcamera.transform.Translate(camMove);
		}
	}

	public void SetPower(float pwr)
	{
		power = Mathf.Clamp01(pwr);
		int i = 0;
		for (int count = flameList.Count; i < count; i++)
		{
			ParticleSystem particleSystem = flameList[i];
			if (flameEnabled)
			{
				ParticleSystem.MainModule main = particleSystem.main;
				ParticleSystem.MinMaxCurve startLifetime = main.startLifetime;
				startLifetime.constantMin = flameInitLifeValuesNewSystem[i] * power;
				startLifetime.constantMax = flameInitLifeValuesNewSystem[i] * power;
				startLifetime.mode = ParticleSystemCurveMode.TwoConstants;
				main.startLifetime = startLifetime;
				ParticleSystem.MinMaxCurve startSize = main.startSize;
				startSize.constantMin = flameInitSizeValuesNewSystem[i] * power;
				startSize.constantMax = flameInitSizeValuesNewSystem[i] * power;
				startSize.mode = ParticleSystemCurveMode.TwoConstants;
				main.startSize = startSize;
				if (!particleSystem.isPlaying)
				{
					particleSystem.Play();
				}
			}
			else if (particleSystem.isPlaying)
			{
				particleSystem.Stop();
			}
		}
		int j = 0;
		for (int count2 = smokeList.Count; j < count2; j++)
		{
			ParticleSystem particleSystem = smokeList[j];
			if (smokeEnabled)
			{
				ParticleSystem.MainModule main2 = particleSystem.main;
				ParticleSystem.MinMaxCurve startLifetime2 = main2.startLifetime;
				startLifetime2.constantMin = smokeInitLifeValuesNewSystem[j] * power - powerVariation * 0.5f * power;
				startLifetime2.constantMax = smokeInitLifeValuesNewSystem[j] * power + powerVariation * 0.5f * power;
				startLifetime2.mode = ParticleSystemCurveMode.TwoConstants;
				main2.startLifetime = startLifetime2;
				ParticleSystem.MinMaxCurve startSize2 = main2.startSize;
				startSize2.constantMin = smokeInitSizeValuesNewSystem[j] * power - powerVariation * 0.5f * power;
				startSize2.constantMax = smokeInitSizeValuesNewSystem[j] * power + powerVariation * 0.5f * power;
				startSize2.mode = ParticleSystemCurveMode.TwoConstants;
				main2.startSize = startSize2;
				if (!particleSystem.isPlaying)
				{
					particleSystem.Play();
				}
			}
			else if (particleSystem.isPlaying)
			{
				particleSystem.Stop();
			}
		}
	}
}
