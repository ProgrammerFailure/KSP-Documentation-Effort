using System;
using UnityEngine;

public class DishController : MonoBehaviour
{
	[Serializable]
	public class Dish
	{
		public Transform elevationTransform;

		public Transform rotationTransform;

		[HideInInspector]
		public Quaternion elevationInit;

		[HideInInspector]
		public Quaternion rotationInit;
	}

	[Serializable]
	public class TimeStep
	{
		public int time;

		public float setupTime;

		public float elevationStart;

		public float elevationEnd;

		public float rotationStart;

		public float rotationEnd;

		public Quaternion previousRotQ;

		public Quaternion startRotQ;

		public Quaternion endRotQ;

		public Quaternion previousElevQ;

		public Quaternion startElevQ;

		public Quaternion endElevQ;
	}

	public Dish[] dishes;

	public float minElevation = 15f;

	public float maxElevation = 90f;

	public float maxSpeed = 10f;

	public float targetChangetime = 100f;

	public float fakeTimeWarp = 1f;

	public int currentTimeStep;

	public TimeStep tsPrev;

	public TimeStep tsCur;

	public Quaternion currentRot;

	public Quaternion currentElev;

	public float delta;

	public float timeStepF;

	public float tsInterval;

	public int timeStep;

	public float time;

	public void Start()
	{
		int i = 0;
		for (int num = dishes.Length; i < num; i++)
		{
			dishes[i].elevationInit = dishes[i].elevationTransform.localRotation;
			dishes[i].rotationInit = dishes[i].rotationTransform.localRotation;
		}
		if (Planetarium.fetch == null)
		{
			time = Time.time;
		}
		else
		{
			time = (float)Planetarium.fetch.time;
		}
		SetTimeStep(Mathf.FloorToInt(time / targetChangetime));
	}

	public void Update()
	{
		if (Planetarium.fetch == null)
		{
			time += Time.deltaTime * fakeTimeWarp;
		}
		else
		{
			time = (float)Planetarium.fetch.time;
		}
		timeStepF = time / targetChangetime;
		timeStep = Mathf.FloorToInt(timeStepF);
		if (timeStep != currentTimeStep)
		{
			SetTimeStep(timeStep);
		}
		tsInterval = (timeStepF - (float)timeStep) * targetChangetime;
		if (tsInterval < tsCur.setupTime)
		{
			delta = tsInterval / tsCur.setupTime;
			currentElev = Quaternion.Lerp(tsCur.previousElevQ, tsCur.startElevQ, delta);
			currentRot = Quaternion.Lerp(tsCur.previousRotQ, tsCur.startRotQ, delta);
		}
		else
		{
			delta = (tsInterval - tsCur.setupTime) / (targetChangetime - tsCur.setupTime);
			currentElev = Quaternion.Lerp(tsCur.startElevQ, tsCur.endElevQ, delta);
			currentRot = Quaternion.Lerp(tsCur.startRotQ, tsCur.endRotQ, delta);
		}
		int i = 0;
		for (int num = dishes.Length; i < num; i++)
		{
			dishes[i].elevationTransform.localRotation = dishes[i].elevationInit * currentElev;
			dishes[i].rotationTransform.localRotation = dishes[i].rotationInit * currentRot;
		}
	}

	public void SetTimeStep(int timeStep)
	{
		currentTimeStep = timeStep;
		if (tsCur != null)
		{
			tsPrev = tsCur;
		}
		else
		{
			tsPrev = CreateTimeStep(timeStep - 1);
		}
		tsCur = CreateTimeStep(timeStep);
		tsCur.previousElevQ = tsPrev.endElevQ;
		tsCur.previousRotQ = tsPrev.endRotQ;
		float a = Mathf.Abs(tsCur.elevationStart - tsPrev.elevationEnd) / maxSpeed;
		float b = Mathf.Abs(tsCur.rotationStart - tsPrev.rotationEnd) / maxSpeed;
		tsCur.setupTime = Mathf.Max(a, b);
	}

	public TimeStep CreateTimeStep(int time)
	{
		TimeStep obj = new TimeStep
		{
			time = time
		};
		UnityEngine.Random.InitState(time);
		obj.elevationStart = UnityEngine.Random.Range(minElevation, maxElevation);
		obj.elevationEnd = UnityEngine.Random.Range(minElevation, maxElevation);
		obj.rotationStart = UnityEngine.Random.Range(0f, 360f);
		obj.rotationEnd = UnityEngine.Random.Range(0f, 360f);
		obj.startElevQ = Quaternion.AngleAxis(obj.elevationStart, Vector3.right);
		obj.endElevQ = Quaternion.AngleAxis(obj.elevationEnd, Vector3.right);
		obj.startRotQ = Quaternion.AngleAxis(obj.rotationStart, Vector3.up);
		obj.endRotQ = Quaternion.AngleAxis(obj.rotationEnd, Vector3.up);
		return obj;
	}
}
