using System;
using UnityEngine;

public class TimingManager : MonoBehaviour
{
	public enum TimingStage
	{
		ObscenelyEarly,
		Early,
		Precalc,
		Earlyish,
		Normal,
		FashionablyLate,
		FlightIntegrator,
		Late,
		BetterLateThanNever
	}

	public delegate void UpdateAction();

	public Timing0 timing0;

	public Timing1 timing1;

	public Timing2 timing2;

	public Timing3 timing3;

	public Timing4 timing4;

	public Timing5 timing5;

	public TimingPre timingPre;

	public TimingFI timingFI;

	public static TimingManager Instance { get; set; }

	public UpdateAction onUpdate { get; set; }

	public UpdateAction onFixedUpdate { get; set; }

	public UpdateAction onLateUpdate { get; set; }

	public virtual void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("TimingManager: Instance already exists, destroying.", base.gameObject);
			UnityEngine.Object.Destroy(this);
			return;
		}
		Instance = this;
		timing0 = base.gameObject.AddComponent<Timing0>();
		timing1 = base.gameObject.AddComponent<Timing1>();
		timing2 = base.gameObject.AddComponent<Timing2>();
		timing3 = base.gameObject.AddComponent<Timing3>();
		timing4 = base.gameObject.AddComponent<Timing4>();
		timing5 = base.gameObject.AddComponent<Timing5>();
		timingPre = base.gameObject.AddComponent<TimingPre>();
		timingFI = base.gameObject.AddComponent<TimingFI>();
	}

	public virtual void OnDestroy()
	{
		if (timing0 != null)
		{
			UnityEngine.Object.Destroy(timing0);
		}
		if (timing1 != null)
		{
			UnityEngine.Object.Destroy(timing1);
		}
		if (timing2 != null)
		{
			UnityEngine.Object.Destroy(timing2);
		}
		if (timing3 != null)
		{
			UnityEngine.Object.Destroy(timing3);
		}
		if (timing4 != null)
		{
			UnityEngine.Object.Destroy(timing4);
		}
		if (timing5 != null)
		{
			UnityEngine.Object.Destroy(timing5);
		}
		if (timingPre != null)
		{
			UnityEngine.Object.Destroy(timingPre);
		}
		if (timingFI != null)
		{
			UnityEngine.Object.Destroy(timingFI);
		}
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public virtual void Update()
	{
		if (onUpdate != null)
		{
			onUpdate();
		}
	}

	public virtual void FixedUpdate()
	{
		if (onFixedUpdate != null)
		{
			onFixedUpdate();
		}
	}

	public virtual void LateUpdate()
	{
		if (onLateUpdate != null)
		{
			onLateUpdate();
		}
	}

	public static void UpdateAdd(TimingStage stage, UpdateAction action)
	{
		if (action != null && !(Instance == null))
		{
			switch (stage)
			{
			case TimingStage.ObscenelyEarly:
			{
				Timing0 obj8 = Instance.timing0;
				obj8.onUpdate = (UpdateAction)Delegate.Combine(obj8.onUpdate, action);
				break;
			}
			case TimingStage.Early:
			{
				Timing1 obj7 = Instance.timing1;
				obj7.onUpdate = (UpdateAction)Delegate.Combine(obj7.onUpdate, action);
				break;
			}
			case TimingStage.Precalc:
			{
				TimingPre obj6 = Instance.timingPre;
				obj6.onUpdate = (UpdateAction)Delegate.Combine(obj6.onUpdate, action);
				break;
			}
			case TimingStage.Earlyish:
			{
				Timing2 obj5 = Instance.timing2;
				obj5.onUpdate = (UpdateAction)Delegate.Combine(obj5.onUpdate, action);
				break;
			}
			case TimingStage.Normal:
			{
				TimingManager instance = Instance;
				instance.onUpdate = (UpdateAction)Delegate.Combine(instance.onUpdate, action);
				break;
			}
			case TimingStage.FashionablyLate:
			{
				Timing3 obj4 = Instance.timing3;
				obj4.onUpdate = (UpdateAction)Delegate.Combine(obj4.onUpdate, action);
				break;
			}
			case TimingStage.FlightIntegrator:
			{
				TimingFI obj3 = Instance.timingFI;
				obj3.onUpdate = (UpdateAction)Delegate.Combine(obj3.onUpdate, action);
				break;
			}
			case TimingStage.Late:
			{
				Timing4 obj2 = Instance.timing4;
				obj2.onUpdate = (UpdateAction)Delegate.Combine(obj2.onUpdate, action);
				break;
			}
			case TimingStage.BetterLateThanNever:
			{
				Timing5 obj = Instance.timing5;
				obj.onUpdate = (UpdateAction)Delegate.Combine(obj.onUpdate, action);
				break;
			}
			}
		}
	}

	public static void UpdateRemove(TimingStage stage, UpdateAction action)
	{
		if (action == null || Instance == null)
		{
			return;
		}
		switch (stage)
		{
		case TimingStage.ObscenelyEarly:
			if (Instance.timing0.onUpdate != null)
			{
				Timing0 obj2 = Instance.timing0;
				obj2.onUpdate = (UpdateAction)Delegate.Remove(obj2.onUpdate, action);
			}
			break;
		case TimingStage.Early:
			if (Instance.timing1.onUpdate != null)
			{
				Timing1 obj6 = Instance.timing1;
				obj6.onUpdate = (UpdateAction)Delegate.Remove(obj6.onUpdate, action);
			}
			break;
		case TimingStage.Precalc:
			if (Instance.timingPre.onUpdate != null)
			{
				TimingPre obj8 = Instance.timingPre;
				obj8.onUpdate = (UpdateAction)Delegate.Remove(obj8.onUpdate, action);
			}
			break;
		case TimingStage.Earlyish:
			if (Instance.timing2.onUpdate != null)
			{
				Timing2 obj4 = Instance.timing2;
				obj4.onUpdate = (UpdateAction)Delegate.Remove(obj4.onUpdate, action);
			}
			break;
		case TimingStage.Normal:
			if (Instance.onUpdate != null)
			{
				TimingManager instance = Instance;
				instance.onUpdate = (UpdateAction)Delegate.Remove(instance.onUpdate, action);
			}
			break;
		case TimingStage.FashionablyLate:
			if (Instance.timing3.onUpdate != null)
			{
				Timing3 obj7 = Instance.timing3;
				obj7.onUpdate = (UpdateAction)Delegate.Remove(obj7.onUpdate, action);
			}
			break;
		case TimingStage.FlightIntegrator:
			if (Instance.timingFI.onUpdate != null)
			{
				TimingFI obj5 = Instance.timingFI;
				obj5.onUpdate = (UpdateAction)Delegate.Remove(obj5.onUpdate, action);
			}
			break;
		case TimingStage.Late:
			if (Instance.timing4.onUpdate != null)
			{
				Timing4 obj3 = Instance.timing4;
				obj3.onUpdate = (UpdateAction)Delegate.Remove(obj3.onUpdate, action);
			}
			break;
		case TimingStage.BetterLateThanNever:
			if (Instance.timing5.onUpdate != null)
			{
				Timing5 obj = Instance.timing5;
				obj.onUpdate = (UpdateAction)Delegate.Remove(obj.onUpdate, action);
			}
			break;
		}
	}

	public static void FixedUpdateAdd(TimingStage stage, UpdateAction action)
	{
		if (action != null && !(Instance == null))
		{
			switch (stage)
			{
			case TimingStage.ObscenelyEarly:
			{
				Timing0 obj8 = Instance.timing0;
				obj8.onFixedUpdate = (UpdateAction)Delegate.Combine(obj8.onFixedUpdate, action);
				break;
			}
			case TimingStage.Early:
			{
				Timing1 obj7 = Instance.timing1;
				obj7.onFixedUpdate = (UpdateAction)Delegate.Combine(obj7.onFixedUpdate, action);
				break;
			}
			case TimingStage.Precalc:
			{
				TimingPre obj6 = Instance.timingPre;
				obj6.onFixedUpdate = (UpdateAction)Delegate.Combine(obj6.onFixedUpdate, action);
				break;
			}
			case TimingStage.Earlyish:
			{
				Timing2 obj5 = Instance.timing2;
				obj5.onFixedUpdate = (UpdateAction)Delegate.Combine(obj5.onFixedUpdate, action);
				break;
			}
			case TimingStage.Normal:
			{
				TimingManager instance = Instance;
				instance.onFixedUpdate = (UpdateAction)Delegate.Combine(instance.onFixedUpdate, action);
				break;
			}
			case TimingStage.FashionablyLate:
			{
				Timing3 obj4 = Instance.timing3;
				obj4.onFixedUpdate = (UpdateAction)Delegate.Combine(obj4.onFixedUpdate, action);
				break;
			}
			case TimingStage.FlightIntegrator:
			{
				TimingFI obj3 = Instance.timingFI;
				obj3.onFixedUpdate = (UpdateAction)Delegate.Combine(obj3.onFixedUpdate, action);
				break;
			}
			case TimingStage.Late:
			{
				Timing4 obj2 = Instance.timing4;
				obj2.onFixedUpdate = (UpdateAction)Delegate.Combine(obj2.onFixedUpdate, action);
				break;
			}
			case TimingStage.BetterLateThanNever:
			{
				Timing5 obj = Instance.timing5;
				obj.onFixedUpdate = (UpdateAction)Delegate.Combine(obj.onFixedUpdate, action);
				break;
			}
			}
		}
	}

	public static void FixedUpdateRemove(TimingStage stage, UpdateAction action)
	{
		if (action == null || Instance == null)
		{
			return;
		}
		switch (stage)
		{
		case TimingStage.ObscenelyEarly:
			if (Instance.timing0.onFixedUpdate != null)
			{
				Timing0 obj2 = Instance.timing0;
				obj2.onFixedUpdate = (UpdateAction)Delegate.Remove(obj2.onFixedUpdate, action);
			}
			break;
		case TimingStage.Early:
			if (Instance.timing1.onFixedUpdate != null)
			{
				Timing1 obj6 = Instance.timing1;
				obj6.onFixedUpdate = (UpdateAction)Delegate.Remove(obj6.onFixedUpdate, action);
			}
			break;
		case TimingStage.Precalc:
			if (Instance.timingPre.onFixedUpdate != null)
			{
				TimingPre obj8 = Instance.timingPre;
				obj8.onFixedUpdate = (UpdateAction)Delegate.Remove(obj8.onFixedUpdate, action);
			}
			break;
		case TimingStage.Earlyish:
			if (Instance.timing2.onFixedUpdate != null)
			{
				Timing2 obj4 = Instance.timing2;
				obj4.onFixedUpdate = (UpdateAction)Delegate.Remove(obj4.onFixedUpdate, action);
			}
			break;
		case TimingStage.Normal:
			if (Instance.onFixedUpdate != null)
			{
				TimingManager instance = Instance;
				instance.onFixedUpdate = (UpdateAction)Delegate.Remove(instance.onFixedUpdate, action);
			}
			break;
		case TimingStage.FashionablyLate:
			if (Instance.timing3.onFixedUpdate != null)
			{
				Timing3 obj7 = Instance.timing3;
				obj7.onFixedUpdate = (UpdateAction)Delegate.Remove(obj7.onFixedUpdate, action);
			}
			break;
		case TimingStage.FlightIntegrator:
			if (Instance.timingFI.onFixedUpdate != null)
			{
				TimingFI obj5 = Instance.timingFI;
				obj5.onFixedUpdate = (UpdateAction)Delegate.Remove(obj5.onFixedUpdate, action);
			}
			break;
		case TimingStage.Late:
			if (Instance.timing4.onFixedUpdate != null)
			{
				Timing4 obj3 = Instance.timing4;
				obj3.onFixedUpdate = (UpdateAction)Delegate.Remove(obj3.onFixedUpdate, action);
			}
			break;
		case TimingStage.BetterLateThanNever:
			if (Instance.timing5.onFixedUpdate != null)
			{
				Timing5 obj = Instance.timing5;
				obj.onFixedUpdate = (UpdateAction)Delegate.Remove(obj.onFixedUpdate, action);
			}
			break;
		}
	}

	public static void LateUpdateAdd(TimingStage stage, UpdateAction action)
	{
		if (action != null && !(Instance == null))
		{
			switch (stage)
			{
			case TimingStage.ObscenelyEarly:
			{
				Timing0 obj8 = Instance.timing0;
				obj8.onLateUpdate = (UpdateAction)Delegate.Combine(obj8.onLateUpdate, action);
				break;
			}
			case TimingStage.Early:
			{
				Timing1 obj7 = Instance.timing1;
				obj7.onLateUpdate = (UpdateAction)Delegate.Combine(obj7.onLateUpdate, action);
				break;
			}
			case TimingStage.Precalc:
			{
				TimingPre obj6 = Instance.timingPre;
				obj6.onLateUpdate = (UpdateAction)Delegate.Combine(obj6.onLateUpdate, action);
				break;
			}
			case TimingStage.Earlyish:
			{
				Timing2 obj5 = Instance.timing2;
				obj5.onLateUpdate = (UpdateAction)Delegate.Combine(obj5.onLateUpdate, action);
				break;
			}
			case TimingStage.Normal:
			{
				TimingManager instance = Instance;
				instance.onLateUpdate = (UpdateAction)Delegate.Combine(instance.onLateUpdate, action);
				break;
			}
			case TimingStage.FashionablyLate:
			{
				Timing3 obj4 = Instance.timing3;
				obj4.onLateUpdate = (UpdateAction)Delegate.Combine(obj4.onLateUpdate, action);
				break;
			}
			case TimingStage.FlightIntegrator:
			{
				TimingFI obj3 = Instance.timingFI;
				obj3.onLateUpdate = (UpdateAction)Delegate.Combine(obj3.onLateUpdate, action);
				break;
			}
			case TimingStage.Late:
			{
				Timing4 obj2 = Instance.timing4;
				obj2.onLateUpdate = (UpdateAction)Delegate.Combine(obj2.onLateUpdate, action);
				break;
			}
			case TimingStage.BetterLateThanNever:
			{
				Timing5 obj = Instance.timing5;
				obj.onLateUpdate = (UpdateAction)Delegate.Combine(obj.onLateUpdate, action);
				break;
			}
			}
		}
	}

	public static void LateUpdateRemove(TimingStage stage, UpdateAction action)
	{
		if (action == null || Instance == null)
		{
			return;
		}
		switch (stage)
		{
		case TimingStage.ObscenelyEarly:
			if (Instance.timing0.onLateUpdate != null)
			{
				Timing0 obj2 = Instance.timing0;
				obj2.onLateUpdate = (UpdateAction)Delegate.Remove(obj2.onLateUpdate, action);
			}
			break;
		case TimingStage.Early:
			if (Instance.timing1.onLateUpdate != null)
			{
				Timing1 obj6 = Instance.timing1;
				obj6.onLateUpdate = (UpdateAction)Delegate.Remove(obj6.onLateUpdate, action);
			}
			break;
		case TimingStage.Precalc:
			if (Instance.timingPre.onLateUpdate != null)
			{
				TimingPre obj8 = Instance.timingPre;
				obj8.onLateUpdate = (UpdateAction)Delegate.Remove(obj8.onLateUpdate, action);
			}
			break;
		case TimingStage.Earlyish:
			if (Instance.timing2.onLateUpdate != null)
			{
				Timing2 obj4 = Instance.timing2;
				obj4.onLateUpdate = (UpdateAction)Delegate.Remove(obj4.onLateUpdate, action);
			}
			break;
		case TimingStage.Normal:
			if (Instance.onLateUpdate != null)
			{
				TimingManager instance = Instance;
				instance.onLateUpdate = (UpdateAction)Delegate.Remove(instance.onLateUpdate, action);
			}
			break;
		case TimingStage.FashionablyLate:
			if (Instance.timing3.onLateUpdate != null)
			{
				Timing3 obj7 = Instance.timing3;
				obj7.onLateUpdate = (UpdateAction)Delegate.Remove(obj7.onLateUpdate, action);
			}
			break;
		case TimingStage.FlightIntegrator:
			if (Instance.timingFI.onLateUpdate != null)
			{
				TimingFI obj5 = Instance.timingFI;
				obj5.onLateUpdate = (UpdateAction)Delegate.Remove(obj5.onLateUpdate, action);
			}
			break;
		case TimingStage.Late:
			if (Instance.timing4.onLateUpdate != null)
			{
				Timing4 obj3 = Instance.timing4;
				obj3.onLateUpdate = (UpdateAction)Delegate.Remove(obj3.onLateUpdate, action);
			}
			break;
		case TimingStage.BetterLateThanNever:
			if (Instance.timing5.onLateUpdate != null)
			{
				Timing5 obj = Instance.timing5;
				obj.onLateUpdate = (UpdateAction)Delegate.Remove(obj.onLateUpdate, action);
			}
			break;
		}
	}
}
