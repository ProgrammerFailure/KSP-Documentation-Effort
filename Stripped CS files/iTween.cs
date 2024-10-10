using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class iTween : MonoBehaviour
{
	public delegate float EasingFunction(float start, float end, float value);

	public delegate void ApplyTween();

	public enum EaseType
	{
		easeInQuad,
		easeOutQuad,
		easeInOutQuad,
		easeInCubic,
		easeOutCubic,
		easeInOutCubic,
		easeInQuart,
		easeOutQuart,
		easeInOutQuart,
		easeInQuint,
		easeOutQuint,
		easeInOutQuint,
		easeInSine,
		easeOutSine,
		easeInOutSine,
		easeInExpo,
		easeOutExpo,
		easeInOutExpo,
		easeInCirc,
		easeOutCirc,
		easeInOutCirc,
		linear,
		spring,
		bounce,
		easeInBack,
		easeOutBack,
		easeInOutBack,
		elastic,
		punch
	}

	public enum LoopType
	{
		none,
		loop,
		pingPong
	}

	public enum NamedValueColor
	{
		_Color,
		_SpecColor,
		_Emission,
		_ReflectColor
	}

	public static class Defaults
	{
		public static float time = 1f;

		public static float delay = 0f;

		public static NamedValueColor namedColorValue = NamedValueColor._Color;

		public static LoopType loopType = LoopType.none;

		public static EaseType easeType = EaseType.easeOutExpo;

		public static float lookSpeed = 3f;

		public static bool isLocal = false;

		public static Space space = Space.Self;

		public static bool orientToPath = false;

		public static Color color = Color.white;

		public static float updateTimePercentage = 0.05f;

		public static float updateTime = 1f * updateTimePercentage;

		public static int cameraFadeDepth = 999999;

		public static float lookAhead = 0.05f;

		public static bool useRealTime = false;

		public static Vector3 up = Vector3.up;
	}

	public class CRSpline
	{
		public Vector3[] pts;

		public CRSpline(params Vector3[] pts)
		{
			this.pts = new Vector3[pts.Length];
			Array.Copy(pts, this.pts, pts.Length);
		}

		public Vector3 Interp(float t)
		{
			int num = pts.Length - 3;
			int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
			float num3 = t * (float)num - (float)num2;
			Vector3 vector = pts[num2];
			Vector3 vector2 = pts[num2 + 1];
			Vector3 vector3 = pts[num2 + 2];
			Vector3 vector4 = pts[num2 + 3];
			return 0.5f * ((-vector + 3f * vector2 - 3f * vector3 + vector4) * (num3 * num3 * num3) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * (num3 * num3) + (-vector + vector3) * num3 + 2f * vector2);
		}
	}

	public static ArrayList tweens = new ArrayList();

	public static GameObject cameraFade;

	public string id;

	public string type;

	public string method;

	public EaseType easeType;

	public float time;

	public float delay;

	public LoopType loopType;

	public bool isRunning;

	public bool isPaused;

	public float runningTime;

	public float percentage;

	public float delayStarted;

	public bool kinematic;

	public bool isLocal;

	public bool loop;

	public bool reverse;

	public bool wasPaused;

	public bool physics;

	public Hashtable tweenArguments;

	public Space space;

	public EasingFunction ease;

	public ApplyTween apply;

	public AudioSource audioSource;

	public Vector3[] vector3s;

	public Vector2[] vector2s;

	public Color[,] colors;

	public float[] floats;

	public Rect[] rects;

	public CRSpline path;

	public Vector3 preUpdate;

	public Vector3 postUpdate;

	public NamedValueColor namedcolorvalue;

	public float lastRealTime;

	public bool useRealTime;

	public static void CameraFadeFrom(float amount, float time)
	{
		if ((bool)cameraFade)
		{
			CameraFadeFrom(Hash("amount", amount, "time", time));
		}
		else
		{
			Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	public static void CameraFadeFrom(Hashtable args)
	{
		if ((bool)cameraFade)
		{
			ColorFrom(cameraFade, args);
		}
		else
		{
			Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	public static void CameraFadeTo(float amount, float time)
	{
		if ((bool)cameraFade)
		{
			CameraFadeTo(Hash("amount", amount, "time", time));
		}
		else
		{
			Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	public static void CameraFadeTo(Hashtable args)
	{
		if ((bool)cameraFade)
		{
			ColorTo(cameraFade, args);
		}
		else
		{
			Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	public static void ValueTo(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		if (args.Contains("onupdate") && args.Contains("from") && args.Contains("to"))
		{
			args["type"] = "value";
			if (args["from"].GetType() == typeof(Vector2))
			{
				args["method"] = "vector2";
			}
			else if (args["from"].GetType() == typeof(Vector3))
			{
				args["method"] = "vector3";
			}
			else if (args["from"].GetType() == typeof(Rect))
			{
				args["method"] = "rect";
			}
			else if (args["from"].GetType() == typeof(float))
			{
				args["method"] = "float";
			}
			else
			{
				if (!(args["from"].GetType() == typeof(Color)))
				{
					Debug.LogError("iTween Error: ValueTo() only works with interpolating Vector3s, Vector2s, floats, ints, Rects and Colors!");
					return;
				}
				args["method"] = "color";
			}
			if (!args.Contains("easetype"))
			{
				args.Add("easetype", EaseType.linear);
			}
			Launch(target, args);
		}
		else
		{
			Debug.LogError("iTween Error: ValueTo() requires an 'onupdate' callback function and a 'from' and 'to' property.  The supplied 'onupdate' callback must accept a single argument that is the same type as the supplied 'from' and 'to' properties!");
		}
	}

	public static void FadeFrom(GameObject target, float alpha, float time)
	{
		FadeFrom(target, Hash("alpha", alpha, "time", time));
	}

	public static void FadeFrom(GameObject target, Hashtable args)
	{
		ColorFrom(target, args);
	}

	public static void FadeTo(GameObject target, float alpha, float time)
	{
		FadeTo(target, Hash("alpha", alpha, "time", time));
	}

	public static void FadeTo(GameObject target, Hashtable args)
	{
		ColorTo(target, args);
	}

	public static void ColorFrom(GameObject target, Color color, float time)
	{
		ColorFrom(target, Hash("color", color, "time", time));
	}

	public static void ColorFrom(GameObject target, Hashtable args)
	{
		Color color = default(Color);
		Color color2 = default(Color);
		args = CleanArgs(args);
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			foreach (Transform item in target.transform)
			{
				Hashtable hashtable = (Hashtable)args.Clone();
				hashtable["ischild"] = true;
				ColorFrom(item.gameObject, hashtable);
			}
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", EaseType.linear);
		}
		if ((bool)target.GetComponent(typeof(Image)))
		{
			color2 = (color = target.GetComponent<Image>().color);
		}
		else if ((bool)target.GetComponent(typeof(Text)))
		{
			color2 = (color = target.GetComponent<Text>().material.color);
		}
		else if ((bool)target.GetComponent<Renderer>())
		{
			color2 = (color = target.GetComponent<Renderer>().material.color);
		}
		else if ((bool)target.GetComponent<Light>())
		{
			color2 = (color = target.GetComponent<Light>().color);
		}
		if (args.Contains("color"))
		{
			color = (Color)args["color"];
		}
		else
		{
			if (args.Contains("r"))
			{
				color.r = (float)args["r"];
			}
			if (args.Contains("g"))
			{
				color.g = (float)args["g"];
			}
			if (args.Contains("b"))
			{
				color.b = (float)args["b"];
			}
			if (args.Contains("a"))
			{
				color.a = (float)args["a"];
			}
		}
		if (args.Contains("amount"))
		{
			color.a = (float)args["amount"];
			args.Remove("amount");
		}
		else if (args.Contains("alpha"))
		{
			color.a = (float)args["alpha"];
			args.Remove("alpha");
		}
		if ((bool)target.GetComponent(typeof(Image)))
		{
			target.GetComponent<Image>().color = color;
		}
		else if ((bool)target.GetComponent(typeof(Text)))
		{
			target.GetComponent<Text>().material.color = color;
		}
		else if ((bool)target.GetComponent<Renderer>())
		{
			target.GetComponent<Renderer>().material.color = color;
		}
		else if ((bool)target.GetComponent<Light>())
		{
			target.GetComponent<Light>().color = color;
		}
		args["color"] = color2;
		args["type"] = "color";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void ColorTo(GameObject target, Color color, float time)
	{
		ColorTo(target, Hash("color", color, "time", time));
	}

	public static void ColorTo(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			foreach (Transform item in target.transform)
			{
				Hashtable hashtable = (Hashtable)args.Clone();
				hashtable["ischild"] = true;
				ColorTo(item.gameObject, hashtable);
			}
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", EaseType.linear);
		}
		args["type"] = "color";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void AudioFrom(GameObject target, float volume, float pitch, float time)
	{
		AudioFrom(target, Hash("volume", volume, "pitch", pitch, "time", time));
	}

	public static void AudioFrom(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		AudioSource audioSource;
		if (args.Contains("audiosource"))
		{
			audioSource = (AudioSource)args["audiosource"];
		}
		else
		{
			if (!target.GetComponent(typeof(AudioSource)))
			{
				Debug.LogError("iTween Error: AudioFrom requires an AudioSource.");
				return;
			}
			audioSource = target.GetComponent<AudioSource>();
		}
		Vector2 vector = default(Vector2);
		Vector2 vector2 = default(Vector2);
		vector.x = (vector2.x = audioSource.volume);
		vector.y = (vector2.y = audioSource.pitch);
		if (args.Contains("volume"))
		{
			vector2.x = (float)args["volume"];
		}
		if (args.Contains("pitch"))
		{
			vector2.y = (float)args["pitch"];
		}
		audioSource.volume = vector2.x;
		audioSource.pitch = vector2.y;
		args["volume"] = vector.x;
		args["pitch"] = vector.y;
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", EaseType.linear);
		}
		args["type"] = "audio";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void AudioTo(GameObject target, float volume, float pitch, float time)
	{
		AudioTo(target, Hash("volume", volume, "pitch", pitch, "time", time));
	}

	public static void AudioTo(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", EaseType.linear);
		}
		args["type"] = "audio";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void Stab(GameObject target, AudioClip audioclip, float delay)
	{
		Stab(target, Hash("audioclip", audioclip, "delay", delay));
	}

	public static void Stab(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "stab";
		Launch(target, args);
	}

	public static void LookFrom(GameObject target, Vector3 looktarget, float time)
	{
		LookFrom(target, Hash("looktarget", looktarget, "time", time));
	}

	public static void LookFrom(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		Vector3 eulerAngles = target.transform.eulerAngles;
		if (args["looktarget"].GetType() == typeof(Transform))
		{
			target.transform.LookAt((Transform)args["looktarget"], ((Vector3?)args["up"]) ?? Defaults.up);
		}
		else if (args["looktarget"].GetType() == typeof(Vector3))
		{
			target.transform.LookAt((Vector3)args["looktarget"], ((Vector3?)args["up"]) ?? Defaults.up);
		}
		if (args.Contains("axis"))
		{
			Vector3 eulerAngles2 = target.transform.eulerAngles;
			switch ((string)args["axis"])
			{
			case "z":
				eulerAngles2.x = eulerAngles.x;
				eulerAngles2.y = eulerAngles.y;
				break;
			case "y":
				eulerAngles2.x = eulerAngles.x;
				eulerAngles2.z = eulerAngles.z;
				break;
			case "x":
				eulerAngles2.y = eulerAngles.y;
				eulerAngles2.z = eulerAngles.z;
				break;
			}
			target.transform.eulerAngles = eulerAngles2;
		}
		args["rotation"] = eulerAngles;
		args["type"] = "rotate";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void LookTo(GameObject target, Vector3 looktarget, float time)
	{
		LookTo(target, Hash("looktarget", looktarget, "time", time));
	}

	public static void LookTo(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		if (args.Contains("looktarget") && args["looktarget"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["looktarget"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
		}
		args["type"] = "look";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void MoveTo(GameObject target, Vector3 position, float time)
	{
		MoveTo(target, Hash("position", position, "time", time));
	}

	public static void MoveTo(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		if (args.Contains("position") && args["position"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["position"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "move";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void MoveFrom(GameObject target, Vector3 position, float time)
	{
		MoveFrom(target, Hash("position", position, "time", time));
	}

	public static void MoveFrom(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		bool flag = ((!args.Contains("islocal")) ? Defaults.isLocal : ((bool)args["islocal"]));
		if (args.Contains("path"))
		{
			Vector3[] array2;
			if (args["path"].GetType() == typeof(Vector3[]))
			{
				Vector3[] array = (Vector3[])args["path"];
				array2 = new Vector3[array.Length];
				Array.Copy(array, array2, array.Length);
			}
			else
			{
				Transform[] array3 = (Transform[])args["path"];
				array2 = new Vector3[array3.Length];
				for (int i = 0; i < array3.Length; i++)
				{
					array2[i] = array3[i].position;
				}
			}
			if (array2[array2.Length - 1] != target.transform.position)
			{
				Vector3[] array4 = new Vector3[array2.Length + 1];
				Array.Copy(array2, array4, array2.Length);
				if (flag)
				{
					array4[array4.Length - 1] = target.transform.localPosition;
					target.transform.localPosition = array4[0];
				}
				else
				{
					array4[array4.Length - 1] = target.transform.position;
					target.transform.position = array4[0];
				}
				args["path"] = array4;
			}
			else
			{
				if (flag)
				{
					target.transform.localPosition = array2[0];
				}
				else
				{
					target.transform.position = array2[0];
				}
				args["path"] = array2;
			}
		}
		else
		{
			Vector3 vector;
			Vector3 vector2 = ((!flag) ? (vector = target.transform.position) : (vector = target.transform.localPosition));
			if (args.Contains("position"))
			{
				if (args["position"].GetType() == typeof(Transform))
				{
					vector = ((Transform)args["position"]).position;
				}
				else if (args["position"].GetType() == typeof(Vector3))
				{
					vector = (Vector3)args["position"];
				}
			}
			else
			{
				if (args.Contains("x"))
				{
					vector.x = (float)args["x"];
				}
				if (args.Contains("y"))
				{
					vector.y = (float)args["y"];
				}
				if (args.Contains("z"))
				{
					vector.z = (float)args["z"];
				}
			}
			if (flag)
			{
				target.transform.localPosition = vector;
			}
			else
			{
				target.transform.position = vector;
			}
			args["position"] = vector2;
		}
		args["type"] = "move";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void MoveAdd(GameObject target, Vector3 amount, float time)
	{
		MoveAdd(target, Hash("amount", amount, "time", time));
	}

	public static void MoveAdd(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "move";
		args["method"] = "add";
		Launch(target, args);
	}

	public static void MoveBy(GameObject target, Vector3 amount, float time)
	{
		MoveBy(target, Hash("amount", amount, "time", time));
	}

	public static void MoveBy(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "move";
		args["method"] = "by";
		Launch(target, args);
	}

	public static void ScaleTo(GameObject target, Vector3 scale, float time)
	{
		ScaleTo(target, Hash("scale", scale, "time", time));
	}

	public static void ScaleTo(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		if (args.Contains("scale") && args["scale"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["scale"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "scale";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void ScaleFrom(GameObject target, Vector3 scale, float time)
	{
		ScaleFrom(target, Hash("scale", scale, "time", time));
	}

	public static void ScaleFrom(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		Vector3 localScale;
		Vector3 vector = (localScale = target.transform.localScale);
		if (args.Contains("scale"))
		{
			if (args["scale"].GetType() == typeof(Transform))
			{
				localScale = ((Transform)args["scale"]).localScale;
			}
			else if (args["scale"].GetType() == typeof(Vector3))
			{
				localScale = (Vector3)args["scale"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				localScale.x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				localScale.y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				localScale.z = (float)args["z"];
			}
		}
		target.transform.localScale = localScale;
		args["scale"] = vector;
		args["type"] = "scale";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void ScaleAdd(GameObject target, Vector3 amount, float time)
	{
		ScaleAdd(target, Hash("amount", amount, "time", time));
	}

	public static void ScaleAdd(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "scale";
		args["method"] = "add";
		Launch(target, args);
	}

	public static void ScaleBy(GameObject target, Vector3 amount, float time)
	{
		ScaleBy(target, Hash("amount", amount, "time", time));
	}

	public static void ScaleBy(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "scale";
		args["method"] = "by";
		Launch(target, args);
	}

	public static void RotateTo(GameObject target, Vector3 rotation, float time)
	{
		RotateTo(target, Hash("rotation", rotation, "time", time));
	}

	public static void RotateTo(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		if (args.Contains("rotation") && args["rotation"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["rotation"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "rotate";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void RotateFrom(GameObject target, Vector3 rotation, float time)
	{
		RotateFrom(target, Hash("rotation", rotation, "time", time));
	}

	public static void RotateFrom(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		bool flag = ((!args.Contains("islocal")) ? Defaults.isLocal : ((bool)args["islocal"]));
		Vector3 vector;
		Vector3 vector2 = ((!flag) ? (vector = target.transform.eulerAngles) : (vector = target.transform.localEulerAngles));
		if (args.Contains("rotation"))
		{
			if (args["rotation"].GetType() == typeof(Transform))
			{
				vector = ((Transform)args["rotation"]).eulerAngles;
			}
			else if (args["rotation"].GetType() == typeof(Vector3))
			{
				vector = (Vector3)args["rotation"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				vector.x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				vector.y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				vector.z = (float)args["z"];
			}
		}
		if (flag)
		{
			target.transform.localEulerAngles = vector;
		}
		else
		{
			target.transform.eulerAngles = vector;
		}
		args["rotation"] = vector2;
		args["type"] = "rotate";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void RotateAdd(GameObject target, Vector3 amount, float time)
	{
		RotateAdd(target, Hash("amount", amount, "time", time));
	}

	public static void RotateAdd(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "rotate";
		args["method"] = "add";
		Launch(target, args);
	}

	public static void RotateBy(GameObject target, Vector3 amount, float time)
	{
		RotateBy(target, Hash("amount", amount, "time", time));
	}

	public static void RotateBy(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "rotate";
		args["method"] = "by";
		Launch(target, args);
	}

	public static void ShakePosition(GameObject target, Vector3 amount, float time)
	{
		ShakePosition(target, Hash("amount", amount, "time", time));
	}

	public static void ShakePosition(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "position";
		Launch(target, args);
	}

	public static void ShakeScale(GameObject target, Vector3 amount, float time)
	{
		ShakeScale(target, Hash("amount", amount, "time", time));
	}

	public static void ShakeScale(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "scale";
		Launch(target, args);
	}

	public static void ShakeRotation(GameObject target, Vector3 amount, float time)
	{
		ShakeRotation(target, Hash("amount", amount, "time", time));
	}

	public static void ShakeRotation(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "rotation";
		Launch(target, args);
	}

	public static void PunchPosition(GameObject target, Vector3 amount, float time)
	{
		PunchPosition(target, Hash("amount", amount, "time", time));
	}

	public static void PunchPosition(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "position";
		args["easetype"] = EaseType.punch;
		Launch(target, args);
	}

	public static void PunchRotation(GameObject target, Vector3 amount, float time)
	{
		PunchRotation(target, Hash("amount", amount, "time", time));
	}

	public static void PunchRotation(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "rotation";
		args["easetype"] = EaseType.punch;
		Launch(target, args);
	}

	public static void PunchScale(GameObject target, Vector3 amount, float time)
	{
		PunchScale(target, Hash("amount", amount, "time", time));
	}

	public static void PunchScale(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "scale";
		args["easetype"] = EaseType.punch;
		Launch(target, args);
	}

	public void GenerateTargets()
	{
		switch (type)
		{
		case "color":
		{
			string text = method;
			if (text == "to")
			{
				GenerateColorToTargets();
				apply = ApplyColorToTargets;
			}
			break;
		}
		case "move":
			switch (method)
			{
			case "by":
			case "add":
				GenerateMoveByTargets();
				apply = ApplyMoveByTargets;
				break;
			case "to":
				if (tweenArguments.Contains("path"))
				{
					GenerateMoveToPathTargets();
					apply = ApplyMoveToPathTargets;
				}
				else
				{
					GenerateMoveToTargets();
					apply = ApplyMoveToTargets;
				}
				break;
			}
			break;
		case "punch":
			switch (method)
			{
			case "scale":
				GeneratePunchScaleTargets();
				apply = ApplyPunchScaleTargets;
				break;
			case "rotation":
				GeneratePunchRotationTargets();
				apply = ApplyPunchRotationTargets;
				break;
			case "position":
				GeneratePunchPositionTargets();
				apply = ApplyPunchPositionTargets;
				break;
			}
			break;
		case "scale":
			switch (method)
			{
			case "add":
				GenerateScaleAddTargets();
				apply = ApplyScaleToTargets;
				break;
			case "by":
				GenerateScaleByTargets();
				apply = ApplyScaleToTargets;
				break;
			case "to":
				GenerateScaleToTargets();
				apply = ApplyScaleToTargets;
				break;
			}
			break;
		case "value":
			switch (method)
			{
			case "rect":
				GenerateRectTargets();
				apply = ApplyRectTargets;
				break;
			case "color":
				GenerateColorTargets();
				apply = ApplyColorTargets;
				break;
			case "vector3":
				GenerateVector3Targets();
				apply = ApplyVector3Targets;
				break;
			case "vector2":
				GenerateVector2Targets();
				apply = ApplyVector2Targets;
				break;
			case "float":
				GenerateFloatTargets();
				apply = ApplyFloatTargets;
				break;
			}
			break;
		case "shake":
			switch (method)
			{
			case "rotation":
				GenerateShakeRotationTargets();
				apply = ApplyShakeRotationTargets;
				break;
			case "scale":
				GenerateShakeScaleTargets();
				apply = ApplyShakeScaleTargets;
				break;
			case "position":
				GenerateShakePositionTargets();
				apply = ApplyShakePositionTargets;
				break;
			}
			break;
		case "rotate":
			switch (method)
			{
			case "by":
				GenerateRotateByTargets();
				apply = ApplyRotateAddTargets;
				break;
			case "add":
				GenerateRotateAddTargets();
				apply = ApplyRotateAddTargets;
				break;
			case "to":
				GenerateRotateToTargets();
				apply = ApplyRotateToTargets;
				break;
			}
			break;
		case "look":
		{
			string text = method;
			if (text == "to")
			{
				GenerateLookToTargets();
				apply = ApplyLookToTargets;
			}
			break;
		}
		case "stab":
			GenerateStabTargets();
			apply = ApplyStabTargets;
			break;
		case "audio":
		{
			string text = method;
			if (text == "to")
			{
				GenerateAudioToTargets();
				apply = ApplyAudioToTargets;
			}
			break;
		}
		}
	}

	public void GenerateRectTargets()
	{
		rects = new Rect[3];
		rects[0] = (Rect)tweenArguments["from"];
		rects[1] = (Rect)tweenArguments["to"];
	}

	public void GenerateColorTargets()
	{
		colors = new Color[1, 3];
		colors[0, 0] = (Color)tweenArguments["from"];
		colors[0, 1] = (Color)tweenArguments["to"];
	}

	public void GenerateVector3Targets()
	{
		vector3s = new Vector3[3];
		vector3s[0] = (Vector3)tweenArguments["from"];
		vector3s[1] = (Vector3)tweenArguments["to"];
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	public void GenerateVector2Targets()
	{
		vector2s = new Vector2[3];
		vector2s[0] = (Vector2)tweenArguments["from"];
		vector2s[1] = (Vector2)tweenArguments["to"];
		if (tweenArguments.Contains("speed"))
		{
			Vector3 a = new Vector3(vector2s[0].x, vector2s[0].y, 0f);
			Vector3 b = new Vector3(vector2s[1].x, vector2s[1].y, 0f);
			float num = Math.Abs(Vector3.Distance(a, b));
			time = num / (float)tweenArguments["speed"];
		}
	}

	public void GenerateFloatTargets()
	{
		floats = new float[3];
		floats[0] = (float)tweenArguments["from"];
		floats[1] = (float)tweenArguments["to"];
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(floats[0] - floats[1]);
			time = num / (float)tweenArguments["speed"];
		}
	}

	public void GenerateColorToTargets()
	{
		if ((bool)GetComponent(typeof(Image)))
		{
			colors = new Color[1, 3];
			colors[0, 0] = (colors[0, 1] = GetComponent<Image>().color);
		}
		else if ((bool)GetComponent(typeof(Text)))
		{
			colors = new Color[1, 3];
			colors[0, 0] = (colors[0, 1] = GetComponent<Text>().material.color);
		}
		else if ((bool)GetComponent<Renderer>())
		{
			colors = new Color[GetComponent<Renderer>().materials.Length, 3];
			for (int i = 0; i < GetComponent<Renderer>().materials.Length; i++)
			{
				colors[i, 0] = GetComponent<Renderer>().materials[i].GetColor(namedcolorvalue.ToString());
				colors[i, 1] = GetComponent<Renderer>().materials[i].GetColor(namedcolorvalue.ToString());
			}
		}
		else if ((bool)GetComponent<Light>())
		{
			colors = new Color[1, 3];
			colors[0, 0] = (colors[0, 1] = GetComponent<Light>().color);
		}
		else
		{
			colors = new Color[1, 3];
		}
		if (tweenArguments.Contains("color"))
		{
			for (int j = 0; j < colors.GetLength(0); j++)
			{
				colors[j, 1] = (Color)tweenArguments["color"];
			}
		}
		else
		{
			if (tweenArguments.Contains("r"))
			{
				for (int k = 0; k < colors.GetLength(0); k++)
				{
					colors[k, 1].r = (float)tweenArguments["r"];
				}
			}
			if (tweenArguments.Contains("g"))
			{
				for (int l = 0; l < colors.GetLength(0); l++)
				{
					colors[l, 1].g = (float)tweenArguments["g"];
				}
			}
			if (tweenArguments.Contains("b"))
			{
				for (int m = 0; m < colors.GetLength(0); m++)
				{
					colors[m, 1].b = (float)tweenArguments["b"];
				}
			}
			if (tweenArguments.Contains("a"))
			{
				for (int n = 0; n < colors.GetLength(0); n++)
				{
					colors[n, 1].a = (float)tweenArguments["a"];
				}
			}
		}
		if (tweenArguments.Contains("amount"))
		{
			for (int num = 0; num < colors.GetLength(0); num++)
			{
				colors[num, 1].a = (float)tweenArguments["amount"];
			}
		}
		else if (tweenArguments.Contains("alpha"))
		{
			for (int num2 = 0; num2 < colors.GetLength(0); num2++)
			{
				colors[num2, 1].a = (float)tweenArguments["alpha"];
			}
		}
	}

	public void GenerateAudioToTargets()
	{
		vector2s = new Vector2[3];
		if (tweenArguments.Contains("audiosource"))
		{
			audioSource = (AudioSource)tweenArguments["audiosource"];
		}
		else if ((bool)GetComponent(typeof(AudioSource)))
		{
			audioSource = GetComponent<AudioSource>();
		}
		else
		{
			Debug.LogError("iTween Error: AudioTo requires an AudioSource.");
			Dispose();
		}
		vector2s[0] = (vector2s[1] = new Vector2(audioSource.volume, audioSource.pitch));
		if (tweenArguments.Contains("volume"))
		{
			vector2s[1].x = (float)tweenArguments["volume"];
		}
		if (tweenArguments.Contains("pitch"))
		{
			vector2s[1].y = (float)tweenArguments["pitch"];
		}
	}

	public void GenerateStabTargets()
	{
		if (tweenArguments.Contains("audiosource"))
		{
			audioSource = (AudioSource)tweenArguments["audiosource"];
		}
		else if ((bool)GetComponent(typeof(AudioSource)))
		{
			audioSource = GetComponent<AudioSource>();
		}
		else
		{
			base.gameObject.AddComponent(typeof(AudioSource));
			audioSource = GetComponent<AudioSource>();
			audioSource.playOnAwake = false;
		}
		audioSource.clip = (AudioClip)tweenArguments["audioclip"];
		if (tweenArguments.Contains("pitch"))
		{
			audioSource.pitch = (float)tweenArguments["pitch"];
		}
		if (tweenArguments.Contains("volume"))
		{
			audioSource.volume = (float)tweenArguments["volume"];
		}
		time = audioSource.clip.length / audioSource.pitch;
	}

	public void GenerateLookToTargets()
	{
		vector3s = new Vector3[3];
		vector3s[0] = base.transform.eulerAngles;
		if (tweenArguments.Contains("looktarget"))
		{
			if (tweenArguments["looktarget"].GetType() == typeof(Transform))
			{
				base.transform.LookAt((Transform)tweenArguments["looktarget"], ((Vector3?)tweenArguments["up"]) ?? Defaults.up);
			}
			else if (tweenArguments["looktarget"].GetType() == typeof(Vector3))
			{
				base.transform.LookAt((Vector3)tweenArguments["looktarget"], ((Vector3?)tweenArguments["up"]) ?? Defaults.up);
			}
		}
		else
		{
			Debug.LogError("iTween Error: LookTo needs a 'looktarget' property!");
			Dispose();
		}
		vector3s[1] = base.transform.eulerAngles;
		base.transform.eulerAngles = vector3s[0];
		if (tweenArguments.Contains("axis"))
		{
			switch ((string)tweenArguments["axis"])
			{
			case "z":
				vector3s[1].x = vector3s[0].x;
				vector3s[1].y = vector3s[0].y;
				break;
			case "y":
				vector3s[1].x = vector3s[0].x;
				vector3s[1].z = vector3s[0].z;
				break;
			case "x":
				vector3s[1].y = vector3s[0].y;
				vector3s[1].z = vector3s[0].z;
				break;
			}
		}
		vector3s[1] = new Vector3(clerp(vector3s[0].x, vector3s[1].x, 1f), clerp(vector3s[0].y, vector3s[1].y, 1f), clerp(vector3s[0].z, vector3s[1].z, 1f));
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	public void GenerateMoveToPathTargets()
	{
		Vector3[] array2;
		if (tweenArguments["path"].GetType() == typeof(Vector3[]))
		{
			Vector3[] array = (Vector3[])tweenArguments["path"];
			if (array.Length == 1)
			{
				Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
				Dispose();
			}
			array2 = new Vector3[array.Length];
			Array.Copy(array, array2, array.Length);
		}
		else
		{
			Transform[] array3 = (Transform[])tweenArguments["path"];
			if (array3.Length == 1)
			{
				Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
				Dispose();
			}
			array2 = new Vector3[array3.Length];
			for (int i = 0; i < array3.Length; i++)
			{
				array2[i] = array3[i].position;
			}
		}
		bool flag;
		int num;
		if (base.transform.position != array2[0])
		{
			if (tweenArguments.Contains("movetopath") && !(bool)tweenArguments["movetopath"])
			{
				flag = false;
				num = 2;
			}
			else
			{
				flag = true;
				num = 3;
			}
		}
		else
		{
			flag = false;
			num = 2;
		}
		vector3s = new Vector3[array2.Length + num];
		if (flag)
		{
			vector3s[1] = base.transform.position;
			num = 2;
		}
		else
		{
			num = 1;
		}
		Array.Copy(array2, 0, vector3s, num, array2.Length);
		vector3s[0] = vector3s[1] + (vector3s[1] - vector3s[2]);
		vector3s[vector3s.Length - 1] = vector3s[vector3s.Length - 2] + (vector3s[vector3s.Length - 2] - vector3s[vector3s.Length - 3]);
		if (vector3s[1] == vector3s[vector3s.Length - 2])
		{
			Vector3[] array4 = new Vector3[vector3s.Length];
			Array.Copy(vector3s, array4, vector3s.Length);
			array4[0] = array4[array4.Length - 3];
			array4[array4.Length - 1] = array4[2];
			vector3s = new Vector3[array4.Length];
			Array.Copy(array4, vector3s, array4.Length);
		}
		path = new CRSpline(vector3s);
		if (tweenArguments.Contains("speed"))
		{
			float num2 = PathLength(vector3s);
			time = num2 / (float)tweenArguments["speed"];
		}
	}

	public void GenerateMoveToTargets()
	{
		vector3s = new Vector3[3];
		if (isLocal)
		{
			vector3s[0] = (vector3s[1] = base.transform.localPosition);
		}
		else
		{
			vector3s[0] = (vector3s[1] = base.transform.position);
		}
		if (tweenArguments.Contains("position"))
		{
			if (tweenArguments["position"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)tweenArguments["position"];
				vector3s[1] = transform.position;
			}
			else if (tweenArguments["position"].GetType() == typeof(Vector3))
			{
				vector3s[1] = (Vector3)tweenArguments["position"];
			}
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x = (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y = (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z = (float)tweenArguments["z"];
			}
		}
		if (tweenArguments.Contains("orienttopath") && (bool)tweenArguments["orienttopath"])
		{
			tweenArguments["looktarget"] = vector3s[1];
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	public void GenerateMoveByTargets()
	{
		vector3s = new Vector3[6];
		vector3s[4] = base.transform.eulerAngles;
		vector3s[0] = (vector3s[1] = (vector3s[3] = base.transform.position));
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = vector3s[0] + (Vector3)tweenArguments["amount"];
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x = vector3s[0].x + (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y = vector3s[0].y + (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z = vector3s[0].z + (float)tweenArguments["z"];
			}
		}
		base.transform.Translate(vector3s[1], space);
		vector3s[5] = base.transform.position;
		base.transform.position = vector3s[0];
		if (tweenArguments.Contains("orienttopath") && (bool)tweenArguments["orienttopath"])
		{
			tweenArguments["looktarget"] = vector3s[1];
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	public void GenerateScaleToTargets()
	{
		vector3s = new Vector3[3];
		vector3s[0] = (vector3s[1] = base.transform.localScale);
		if (tweenArguments.Contains("scale"))
		{
			if (tweenArguments["scale"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)tweenArguments["scale"];
				vector3s[1] = transform.localScale;
			}
			else if (tweenArguments["scale"].GetType() == typeof(Vector3))
			{
				vector3s[1] = (Vector3)tweenArguments["scale"];
			}
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x = (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y = (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z = (float)tweenArguments["z"];
			}
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	public void GenerateScaleByTargets()
	{
		vector3s = new Vector3[3];
		vector3s[0] = (vector3s[1] = base.transform.localScale);
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = Vector3.Scale(vector3s[1], (Vector3)tweenArguments["amount"]);
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x *= (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y *= (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z *= (float)tweenArguments["z"];
			}
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	public void GenerateScaleAddTargets()
	{
		vector3s = new Vector3[3];
		vector3s[0] = (vector3s[1] = base.transform.localScale);
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] += (Vector3)tweenArguments["amount"];
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x += (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y += (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z += (float)tweenArguments["z"];
			}
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	public void GenerateRotateToTargets()
	{
		vector3s = new Vector3[3];
		if (isLocal)
		{
			vector3s[0] = (vector3s[1] = base.transform.localEulerAngles);
		}
		else
		{
			vector3s[0] = (vector3s[1] = base.transform.eulerAngles);
		}
		if (tweenArguments.Contains("rotation"))
		{
			if (tweenArguments["rotation"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)tweenArguments["rotation"];
				vector3s[1] = transform.eulerAngles;
			}
			else if (tweenArguments["rotation"].GetType() == typeof(Vector3))
			{
				vector3s[1] = (Vector3)tweenArguments["rotation"];
			}
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x = (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y = (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z = (float)tweenArguments["z"];
			}
		}
		vector3s[1] = new Vector3(clerp(vector3s[0].x, vector3s[1].x, 1f), clerp(vector3s[0].y, vector3s[1].y, 1f), clerp(vector3s[0].z, vector3s[1].z, 1f));
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	public void GenerateRotateAddTargets()
	{
		vector3s = new Vector3[5];
		vector3s[0] = (vector3s[1] = (vector3s[3] = base.transform.eulerAngles));
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] += (Vector3)tweenArguments["amount"];
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x += (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y += (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z += (float)tweenArguments["z"];
			}
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	public void GenerateRotateByTargets()
	{
		vector3s = new Vector3[4];
		vector3s[0] = (vector3s[1] = (vector3s[3] = base.transform.eulerAngles));
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] += Vector3.Scale((Vector3)tweenArguments["amount"], new Vector3(360f, 360f, 360f));
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x += 360f * (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y += 360f * (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z += 360f * (float)tweenArguments["z"];
			}
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	public void GenerateShakePositionTargets()
	{
		vector3s = new Vector3[4];
		vector3s[3] = base.transform.eulerAngles;
		vector3s[0] = base.transform.position;
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = (Vector3)tweenArguments["amount"];
			return;
		}
		if (tweenArguments.Contains("x"))
		{
			vector3s[1].x = (float)tweenArguments["x"];
		}
		if (tweenArguments.Contains("y"))
		{
			vector3s[1].y = (float)tweenArguments["y"];
		}
		if (tweenArguments.Contains("z"))
		{
			vector3s[1].z = (float)tweenArguments["z"];
		}
	}

	public void GenerateShakeScaleTargets()
	{
		vector3s = new Vector3[3];
		vector3s[0] = base.transform.localScale;
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = (Vector3)tweenArguments["amount"];
			return;
		}
		if (tweenArguments.Contains("x"))
		{
			vector3s[1].x = (float)tweenArguments["x"];
		}
		if (tweenArguments.Contains("y"))
		{
			vector3s[1].y = (float)tweenArguments["y"];
		}
		if (tweenArguments.Contains("z"))
		{
			vector3s[1].z = (float)tweenArguments["z"];
		}
	}

	public void GenerateShakeRotationTargets()
	{
		vector3s = new Vector3[3];
		vector3s[0] = base.transform.eulerAngles;
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = (Vector3)tweenArguments["amount"];
			return;
		}
		if (tweenArguments.Contains("x"))
		{
			vector3s[1].x = (float)tweenArguments["x"];
		}
		if (tweenArguments.Contains("y"))
		{
			vector3s[1].y = (float)tweenArguments["y"];
		}
		if (tweenArguments.Contains("z"))
		{
			vector3s[1].z = (float)tweenArguments["z"];
		}
	}

	public void GeneratePunchPositionTargets()
	{
		vector3s = new Vector3[5];
		vector3s[4] = base.transform.eulerAngles;
		vector3s[0] = base.transform.position;
		vector3s[1] = (vector3s[3] = Vector3.zero);
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = (Vector3)tweenArguments["amount"];
			return;
		}
		if (tweenArguments.Contains("x"))
		{
			vector3s[1].x = (float)tweenArguments["x"];
		}
		if (tweenArguments.Contains("y"))
		{
			vector3s[1].y = (float)tweenArguments["y"];
		}
		if (tweenArguments.Contains("z"))
		{
			vector3s[1].z = (float)tweenArguments["z"];
		}
	}

	public void GeneratePunchRotationTargets()
	{
		vector3s = new Vector3[4];
		vector3s[0] = base.transform.eulerAngles;
		vector3s[1] = (vector3s[3] = Vector3.zero);
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = (Vector3)tweenArguments["amount"];
			return;
		}
		if (tweenArguments.Contains("x"))
		{
			vector3s[1].x = (float)tweenArguments["x"];
		}
		if (tweenArguments.Contains("y"))
		{
			vector3s[1].y = (float)tweenArguments["y"];
		}
		if (tweenArguments.Contains("z"))
		{
			vector3s[1].z = (float)tweenArguments["z"];
		}
	}

	public void GeneratePunchScaleTargets()
	{
		vector3s = new Vector3[3];
		vector3s[0] = base.transform.localScale;
		vector3s[1] = Vector3.zero;
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = (Vector3)tweenArguments["amount"];
			return;
		}
		if (tweenArguments.Contains("x"))
		{
			vector3s[1].x = (float)tweenArguments["x"];
		}
		if (tweenArguments.Contains("y"))
		{
			vector3s[1].y = (float)tweenArguments["y"];
		}
		if (tweenArguments.Contains("z"))
		{
			vector3s[1].z = (float)tweenArguments["z"];
		}
	}

	public void ApplyRectTargets()
	{
		rects[2].x = ease(rects[0].x, rects[1].x, percentage);
		rects[2].y = ease(rects[0].y, rects[1].y, percentage);
		rects[2].width = ease(rects[0].width, rects[1].width, percentage);
		rects[2].height = ease(rects[0].height, rects[1].height, percentage);
		tweenArguments["onupdateparams"] = rects[2];
		if (percentage == 1f)
		{
			tweenArguments["onupdateparams"] = rects[1];
		}
	}

	public void ApplyColorTargets()
	{
		colors[0, 2].r = ease(colors[0, 0].r, colors[0, 1].r, percentage);
		colors[0, 2].g = ease(colors[0, 0].g, colors[0, 1].g, percentage);
		colors[0, 2].b = ease(colors[0, 0].b, colors[0, 1].b, percentage);
		colors[0, 2].a = ease(colors[0, 0].a, colors[0, 1].a, percentage);
		tweenArguments["onupdateparams"] = colors[0, 2];
		if (percentage == 1f)
		{
			tweenArguments["onupdateparams"] = colors[0, 1];
		}
	}

	public void ApplyVector3Targets()
	{
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		tweenArguments["onupdateparams"] = vector3s[2];
		if (percentage == 1f)
		{
			tweenArguments["onupdateparams"] = vector3s[1];
		}
	}

	public void ApplyVector2Targets()
	{
		vector2s[2].x = ease(vector2s[0].x, vector2s[1].x, percentage);
		vector2s[2].y = ease(vector2s[0].y, vector2s[1].y, percentage);
		tweenArguments["onupdateparams"] = vector2s[2];
		if (percentage == 1f)
		{
			tweenArguments["onupdateparams"] = vector2s[1];
		}
	}

	public void ApplyFloatTargets()
	{
		floats[2] = ease(floats[0], floats[1], percentage);
		tweenArguments["onupdateparams"] = floats[2];
		if (percentage == 1f)
		{
			tweenArguments["onupdateparams"] = floats[1];
		}
	}

	public void ApplyColorToTargets()
	{
		for (int i = 0; i < colors.GetLength(0); i++)
		{
			colors[i, 2].r = ease(colors[i, 0].r, colors[i, 1].r, percentage);
			colors[i, 2].g = ease(colors[i, 0].g, colors[i, 1].g, percentage);
			colors[i, 2].b = ease(colors[i, 0].b, colors[i, 1].b, percentage);
			colors[i, 2].a = ease(colors[i, 0].a, colors[i, 1].a, percentage);
		}
		if ((bool)GetComponent(typeof(Image)))
		{
			GetComponent<Image>().color = colors[0, 2];
		}
		else if ((bool)GetComponent(typeof(Text)))
		{
			GetComponent<Text>().material.color = colors[0, 2];
		}
		else if ((bool)GetComponent<Renderer>())
		{
			for (int j = 0; j < colors.GetLength(0); j++)
			{
				GetComponent<Renderer>().materials[j].SetColor(namedcolorvalue.ToString(), colors[j, 2]);
			}
		}
		else if ((bool)GetComponent<Light>())
		{
			GetComponent<Light>().color = colors[0, 2];
		}
		if (percentage != 1f)
		{
			return;
		}
		if ((bool)GetComponent(typeof(Image)))
		{
			GetComponent<Image>().color = colors[0, 1];
		}
		else if ((bool)GetComponent(typeof(Text)))
		{
			GetComponent<Text>().material.color = colors[0, 1];
		}
		else if ((bool)GetComponent<Renderer>())
		{
			for (int k = 0; k < colors.GetLength(0); k++)
			{
				GetComponent<Renderer>().materials[k].SetColor(namedcolorvalue.ToString(), colors[k, 1]);
			}
		}
		else if ((bool)GetComponent<Light>())
		{
			GetComponent<Light>().color = colors[0, 1];
		}
	}

	public void ApplyAudioToTargets()
	{
		vector2s[2].x = ease(vector2s[0].x, vector2s[1].x, percentage);
		vector2s[2].y = ease(vector2s[0].y, vector2s[1].y, percentage);
		audioSource.volume = vector2s[2].x;
		audioSource.pitch = vector2s[2].y;
		if (percentage == 1f)
		{
			audioSource.volume = vector2s[1].x;
			audioSource.pitch = vector2s[1].y;
		}
	}

	public void ApplyStabTargets()
	{
	}

	public void ApplyMoveToPathTargets()
	{
		preUpdate = base.transform.position;
		float value = ease(0f, 1f, percentage);
		if (isLocal)
		{
			base.transform.localPosition = path.Interp(Mathf.Clamp(value, 0f, 1f));
		}
		else
		{
			base.transform.position = path.Interp(Mathf.Clamp(value, 0f, 1f));
		}
		if (tweenArguments.Contains("orienttopath") && (bool)tweenArguments["orienttopath"])
		{
			float num = ((!tweenArguments.Contains("lookahead")) ? Defaults.lookAhead : ((float)tweenArguments["lookahead"]));
			float value2 = ease(0f, 1f, Mathf.Min(1f, percentage + num));
			tweenArguments["looktarget"] = path.Interp(Mathf.Clamp(value2, 0f, 1f));
		}
		postUpdate = base.transform.position;
		if (physics)
		{
			base.transform.position = preUpdate;
			GetComponent<Rigidbody>().MovePosition(postUpdate);
		}
	}

	public void ApplyMoveToTargets()
	{
		preUpdate = base.transform.position;
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		if (isLocal)
		{
			base.transform.localPosition = vector3s[2];
		}
		else
		{
			base.transform.position = vector3s[2];
		}
		if (percentage == 1f)
		{
			if (isLocal)
			{
				base.transform.localPosition = vector3s[1];
			}
			else
			{
				base.transform.position = vector3s[1];
			}
		}
		postUpdate = base.transform.position;
		if (physics)
		{
			base.transform.position = preUpdate;
			GetComponent<Rigidbody>().MovePosition(postUpdate);
		}
	}

	public void ApplyMoveByTargets()
	{
		preUpdate = base.transform.position;
		Vector3 eulerAngles = default(Vector3);
		if (tweenArguments.Contains("looktarget"))
		{
			eulerAngles = base.transform.eulerAngles;
			base.transform.eulerAngles = vector3s[4];
		}
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		base.transform.Translate(vector3s[2] - vector3s[3], space);
		vector3s[3] = vector3s[2];
		if (tweenArguments.Contains("looktarget"))
		{
			base.transform.eulerAngles = eulerAngles;
		}
		postUpdate = base.transform.position;
		if (physics)
		{
			base.transform.position = preUpdate;
			GetComponent<Rigidbody>().MovePosition(postUpdate);
		}
	}

	public void ApplyScaleToTargets()
	{
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		base.transform.localScale = vector3s[2];
		if (percentage == 1f)
		{
			base.transform.localScale = vector3s[1];
		}
	}

	public void ApplyLookToTargets()
	{
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		if (isLocal)
		{
			base.transform.localRotation = Quaternion.Euler(vector3s[2]);
		}
		else
		{
			base.transform.rotation = Quaternion.Euler(vector3s[2]);
		}
	}

	public void ApplyRotateToTargets()
	{
		preUpdate = base.transform.eulerAngles;
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		if (isLocal)
		{
			base.transform.localRotation = Quaternion.Euler(vector3s[2]);
		}
		else
		{
			base.transform.rotation = Quaternion.Euler(vector3s[2]);
		}
		if (percentage == 1f)
		{
			if (isLocal)
			{
				base.transform.localRotation = Quaternion.Euler(vector3s[1]);
			}
			else
			{
				base.transform.rotation = Quaternion.Euler(vector3s[1]);
			}
		}
		postUpdate = base.transform.eulerAngles;
		if (physics)
		{
			base.transform.eulerAngles = preUpdate;
			GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(postUpdate));
		}
	}

	public void ApplyRotateAddTargets()
	{
		preUpdate = base.transform.eulerAngles;
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		base.transform.Rotate(vector3s[2] - vector3s[3], space);
		vector3s[3] = vector3s[2];
		postUpdate = base.transform.eulerAngles;
		if (physics)
		{
			base.transform.eulerAngles = preUpdate;
			GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(postUpdate));
		}
	}

	public void ApplyShakePositionTargets()
	{
		preUpdate = base.transform.position;
		Vector3 eulerAngles = default(Vector3);
		if (tweenArguments.Contains("looktarget"))
		{
			eulerAngles = base.transform.eulerAngles;
			base.transform.eulerAngles = vector3s[3];
		}
		if (percentage == 0f)
		{
			base.transform.Translate(vector3s[1], space);
		}
		base.transform.position = vector3s[0];
		float num = 1f - percentage;
		vector3s[2].x = UnityEngine.Random.Range((0f - vector3s[1].x) * num, vector3s[1].x * num);
		vector3s[2].y = UnityEngine.Random.Range((0f - vector3s[1].y) * num, vector3s[1].y * num);
		vector3s[2].z = UnityEngine.Random.Range((0f - vector3s[1].z) * num, vector3s[1].z * num);
		base.transform.Translate(vector3s[2], space);
		if (tweenArguments.Contains("looktarget"))
		{
			base.transform.eulerAngles = eulerAngles;
		}
		postUpdate = base.transform.position;
		if (physics)
		{
			base.transform.position = preUpdate;
			GetComponent<Rigidbody>().MovePosition(postUpdate);
		}
	}

	public void ApplyShakeScaleTargets()
	{
		if (percentage == 0f)
		{
			base.transform.localScale = vector3s[1];
		}
		base.transform.localScale = vector3s[0];
		float num = 1f - percentage;
		vector3s[2].x = UnityEngine.Random.Range((0f - vector3s[1].x) * num, vector3s[1].x * num);
		vector3s[2].y = UnityEngine.Random.Range((0f - vector3s[1].y) * num, vector3s[1].y * num);
		vector3s[2].z = UnityEngine.Random.Range((0f - vector3s[1].z) * num, vector3s[1].z * num);
		base.transform.localScale += vector3s[2];
	}

	public void ApplyShakeRotationTargets()
	{
		preUpdate = base.transform.eulerAngles;
		if (percentage == 0f)
		{
			base.transform.Rotate(vector3s[1], space);
		}
		base.transform.eulerAngles = vector3s[0];
		float num = 1f - percentage;
		vector3s[2].x = UnityEngine.Random.Range((0f - vector3s[1].x) * num, vector3s[1].x * num);
		vector3s[2].y = UnityEngine.Random.Range((0f - vector3s[1].y) * num, vector3s[1].y * num);
		vector3s[2].z = UnityEngine.Random.Range((0f - vector3s[1].z) * num, vector3s[1].z * num);
		base.transform.Rotate(vector3s[2], space);
		postUpdate = base.transform.eulerAngles;
		if (physics)
		{
			base.transform.eulerAngles = preUpdate;
			GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(postUpdate));
		}
	}

	public void ApplyPunchPositionTargets()
	{
		preUpdate = base.transform.position;
		Vector3 eulerAngles = default(Vector3);
		if (tweenArguments.Contains("looktarget"))
		{
			eulerAngles = base.transform.eulerAngles;
			base.transform.eulerAngles = vector3s[4];
		}
		if (vector3s[1].x > 0f)
		{
			vector3s[2].x = punch(vector3s[1].x, percentage);
		}
		else if (vector3s[1].x < 0f)
		{
			vector3s[2].x = 0f - punch(Mathf.Abs(vector3s[1].x), percentage);
		}
		if (vector3s[1].y > 0f)
		{
			vector3s[2].y = punch(vector3s[1].y, percentage);
		}
		else if (vector3s[1].y < 0f)
		{
			vector3s[2].y = 0f - punch(Mathf.Abs(vector3s[1].y), percentage);
		}
		if (vector3s[1].z > 0f)
		{
			vector3s[2].z = punch(vector3s[1].z, percentage);
		}
		else if (vector3s[1].z < 0f)
		{
			vector3s[2].z = 0f - punch(Mathf.Abs(vector3s[1].z), percentage);
		}
		base.transform.Translate(vector3s[2] - vector3s[3], space);
		vector3s[3] = vector3s[2];
		if (tweenArguments.Contains("looktarget"))
		{
			base.transform.eulerAngles = eulerAngles;
		}
		postUpdate = base.transform.position;
		if (physics)
		{
			base.transform.position = preUpdate;
			GetComponent<Rigidbody>().MovePosition(postUpdate);
		}
	}

	public void ApplyPunchRotationTargets()
	{
		preUpdate = base.transform.eulerAngles;
		if (vector3s[1].x > 0f)
		{
			vector3s[2].x = punch(vector3s[1].x, percentage);
		}
		else if (vector3s[1].x < 0f)
		{
			vector3s[2].x = 0f - punch(Mathf.Abs(vector3s[1].x), percentage);
		}
		if (vector3s[1].y > 0f)
		{
			vector3s[2].y = punch(vector3s[1].y, percentage);
		}
		else if (vector3s[1].y < 0f)
		{
			vector3s[2].y = 0f - punch(Mathf.Abs(vector3s[1].y), percentage);
		}
		if (vector3s[1].z > 0f)
		{
			vector3s[2].z = punch(vector3s[1].z, percentage);
		}
		else if (vector3s[1].z < 0f)
		{
			vector3s[2].z = 0f - punch(Mathf.Abs(vector3s[1].z), percentage);
		}
		base.transform.Rotate(vector3s[2] - vector3s[3], space);
		vector3s[3] = vector3s[2];
		postUpdate = base.transform.eulerAngles;
		if (physics)
		{
			base.transform.eulerAngles = preUpdate;
			GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(postUpdate));
		}
	}

	public void ApplyPunchScaleTargets()
	{
		if (vector3s[1].x > 0f)
		{
			vector3s[2].x = punch(vector3s[1].x, percentage);
		}
		else if (vector3s[1].x < 0f)
		{
			vector3s[2].x = 0f - punch(Mathf.Abs(vector3s[1].x), percentage);
		}
		if (vector3s[1].y > 0f)
		{
			vector3s[2].y = punch(vector3s[1].y, percentage);
		}
		else if (vector3s[1].y < 0f)
		{
			vector3s[2].y = 0f - punch(Mathf.Abs(vector3s[1].y), percentage);
		}
		if (vector3s[1].z > 0f)
		{
			vector3s[2].z = punch(vector3s[1].z, percentage);
		}
		else if (vector3s[1].z < 0f)
		{
			vector3s[2].z = 0f - punch(Mathf.Abs(vector3s[1].z), percentage);
		}
		base.transform.localScale = vector3s[0] + vector3s[2];
	}

	public IEnumerator TweenDelay()
	{
		delayStarted = Time.time;
		yield return new WaitForSeconds(delay);
		if (wasPaused)
		{
			wasPaused = false;
			TweenStart();
		}
	}

	public void TweenStart()
	{
		CallBack("onstart");
		if (!loop)
		{
			ConflictCheck();
			GenerateTargets();
		}
		if (type == "stab")
		{
			audioSource.PlayOneShot(audioSource.clip);
		}
		if (type == "move" || type == "scale" || type == "rotate" || type == "punch" || type == "shake" || type == "curve" || type == "look")
		{
			EnableKinematic();
		}
		isRunning = true;
	}

	public IEnumerator TweenRestart()
	{
		if (delay > 0f)
		{
			delayStarted = Time.time;
			yield return new WaitForSeconds(delay);
		}
		loop = true;
		TweenStart();
	}

	public void TweenUpdate()
	{
		apply();
		CallBack("onupdate");
		UpdatePercentage();
	}

	public void TweenComplete()
	{
		isRunning = false;
		if (percentage > 0.5f)
		{
			percentage = 1f;
		}
		else
		{
			percentage = 0f;
		}
		apply();
		if (type == "value")
		{
			CallBack("onupdate");
		}
		if (loopType == LoopType.none)
		{
			Dispose();
		}
		else
		{
			TweenLoop();
		}
		CallBack("oncomplete");
	}

	public void TweenLoop()
	{
		DisableKinematic();
		switch (loopType)
		{
		case LoopType.pingPong:
			reverse = !reverse;
			runningTime = 0f;
			StartCoroutine("TweenRestart");
			break;
		case LoopType.loop:
			percentage = 0f;
			runningTime = 0f;
			apply();
			StartCoroutine("TweenRestart");
			break;
		}
	}

	public static Rect RectUpdate(Rect currentValue, Rect targetValue, float speed)
	{
		return new Rect(FloatUpdate(currentValue.x, targetValue.x, speed), FloatUpdate(currentValue.y, targetValue.y, speed), FloatUpdate(currentValue.width, targetValue.width, speed), FloatUpdate(currentValue.height, targetValue.height, speed));
	}

	public static Vector3 Vector3Update(Vector3 currentValue, Vector3 targetValue, float speed)
	{
		Vector3 vector = targetValue - currentValue;
		currentValue += vector * speed * Time.deltaTime;
		return currentValue;
	}

	public static Vector2 Vector2Update(Vector2 currentValue, Vector2 targetValue, float speed)
	{
		Vector2 vector = targetValue - currentValue;
		currentValue += vector * speed * Time.deltaTime;
		return currentValue;
	}

	public static float FloatUpdate(float currentValue, float targetValue, float speed)
	{
		float num = targetValue - currentValue;
		currentValue += num * speed * Time.deltaTime;
		return currentValue;
	}

	public static void FadeUpdate(GameObject target, Hashtable args)
	{
		args["a"] = args["alpha"];
		ColorUpdate(target, args);
	}

	public static void FadeUpdate(GameObject target, float alpha, float time)
	{
		FadeUpdate(target, Hash("alpha", alpha, "time", time));
	}

	public static void ColorUpdate(GameObject target, Hashtable args)
	{
		CleanArgs(args);
		Color[] array = new Color[4];
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			foreach (Transform item in target.transform)
			{
				ColorUpdate(item.gameObject, args);
			}
		}
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		if ((bool)target.GetComponent(typeof(Image)))
		{
			array[0] = (array[1] = target.GetComponent<Image>().color);
		}
		else if ((bool)target.GetComponent(typeof(Text)))
		{
			array[0] = (array[1] = target.GetComponent<Text>().material.color);
		}
		else if ((bool)target.GetComponent<Renderer>())
		{
			array[0] = (array[1] = target.GetComponent<Renderer>().material.color);
		}
		else if ((bool)target.GetComponent<Light>())
		{
			array[0] = (array[1] = target.GetComponent<Light>().color);
		}
		if (args.Contains("color"))
		{
			array[1] = (Color)args["color"];
		}
		else
		{
			if (args.Contains("r"))
			{
				array[1].r = (float)args["r"];
			}
			if (args.Contains("g"))
			{
				array[1].g = (float)args["g"];
			}
			if (args.Contains("b"))
			{
				array[1].b = (float)args["b"];
			}
			if (args.Contains("a"))
			{
				array[1].a = (float)args["a"];
			}
		}
		array[3].r = Mathf.SmoothDamp(array[0].r, array[1].r, ref array[2].r, num);
		array[3].g = Mathf.SmoothDamp(array[0].g, array[1].g, ref array[2].g, num);
		array[3].b = Mathf.SmoothDamp(array[0].b, array[1].b, ref array[2].b, num);
		array[3].a = Mathf.SmoothDamp(array[0].a, array[1].a, ref array[2].a, num);
		if ((bool)target.GetComponent(typeof(Image)))
		{
			target.GetComponent<Image>().color = array[3];
		}
		else if ((bool)target.GetComponent(typeof(Text)))
		{
			target.GetComponent<Text>().material.color = array[3];
		}
		else if ((bool)target.GetComponent<Renderer>())
		{
			target.GetComponent<Renderer>().material.color = array[3];
		}
		else if ((bool)target.GetComponent<Light>())
		{
			target.GetComponent<Light>().color = array[3];
		}
	}

	public static void ColorUpdate(GameObject target, Color color, float time)
	{
		ColorUpdate(target, Hash("color", color, "time", time));
	}

	public static void AudioUpdate(GameObject target, Hashtable args)
	{
		CleanArgs(args);
		Vector2[] array = new Vector2[4];
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		AudioSource audioSource;
		if (args.Contains("audiosource"))
		{
			audioSource = (AudioSource)args["audiosource"];
		}
		else
		{
			if (!target.GetComponent(typeof(AudioSource)))
			{
				Debug.LogError("iTween Error: AudioUpdate requires an AudioSource.");
				return;
			}
			audioSource = target.GetComponent<AudioSource>();
		}
		array[0] = (array[1] = new Vector2(audioSource.volume, audioSource.pitch));
		if (args.Contains("volume"))
		{
			array[1].x = (float)args["volume"];
		}
		if (args.Contains("pitch"))
		{
			array[1].y = (float)args["pitch"];
		}
		array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
		audioSource.volume = array[3].x;
		audioSource.pitch = array[3].y;
	}

	public static void AudioUpdate(GameObject target, float volume, float pitch, float time)
	{
		AudioUpdate(target, Hash("volume", volume, "pitch", pitch, "time", time));
	}

	public static void RotateUpdate(GameObject target, Hashtable args)
	{
		CleanArgs(args);
		Vector3[] array = new Vector3[4];
		Vector3 eulerAngles = target.transform.eulerAngles;
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		bool flag = ((!args.Contains("islocal")) ? Defaults.isLocal : ((bool)args["islocal"]));
		if (flag)
		{
			array[0] = target.transform.localEulerAngles;
		}
		else
		{
			array[0] = target.transform.eulerAngles;
		}
		if (args.Contains("rotation"))
		{
			if (args["rotation"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["rotation"];
				array[1] = transform.eulerAngles;
			}
			else if (args["rotation"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["rotation"];
			}
		}
		array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDampAngle(array[0].z, array[1].z, ref array[2].z, num);
		if (flag)
		{
			target.transform.localEulerAngles = array[3];
		}
		else
		{
			target.transform.eulerAngles = array[3];
		}
		if (target.GetComponent<Rigidbody>() != null)
		{
			Vector3 eulerAngles2 = target.transform.eulerAngles;
			target.transform.eulerAngles = eulerAngles;
			target.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(eulerAngles2));
		}
	}

	public static void RotateUpdate(GameObject target, Vector3 rotation, float time)
	{
		RotateUpdate(target, Hash("rotation", rotation, "time", time));
	}

	public static void ScaleUpdate(GameObject target, Hashtable args)
	{
		CleanArgs(args);
		Vector3[] array = new Vector3[4];
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		array[0] = (array[1] = target.transform.localScale);
		if (args.Contains("scale"))
		{
			if (args["scale"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["scale"];
				array[1] = transform.localScale;
			}
			else if (args["scale"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["scale"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				array[1].x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				array[1].y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				array[1].z = (float)args["z"];
			}
		}
		array[3].x = Mathf.SmoothDamp(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDamp(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDamp(array[0].z, array[1].z, ref array[2].z, num);
		target.transform.localScale = array[3];
	}

	public static void ScaleUpdate(GameObject target, Vector3 scale, float time)
	{
		ScaleUpdate(target, Hash("scale", scale, "time", time));
	}

	public static void MoveUpdate(GameObject target, Hashtable args)
	{
		CleanArgs(args);
		Vector3[] array = new Vector3[4];
		Vector3 position = target.transform.position;
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		bool flag = ((!args.Contains("islocal")) ? Defaults.isLocal : ((bool)args["islocal"]));
		if (flag)
		{
			array[0] = (array[1] = target.transform.localPosition);
		}
		else
		{
			array[0] = (array[1] = target.transform.position);
		}
		if (args.Contains("position"))
		{
			if (args["position"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["position"];
				array[1] = transform.position;
			}
			else if (args["position"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["position"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				array[1].x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				array[1].y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				array[1].z = (float)args["z"];
			}
		}
		array[3].x = Mathf.SmoothDamp(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDamp(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDamp(array[0].z, array[1].z, ref array[2].z, num);
		if (args.Contains("orienttopath") && (bool)args["orienttopath"])
		{
			args["looktarget"] = array[3];
		}
		if (args.Contains("looktarget"))
		{
			LookUpdate(target, args);
		}
		if (flag)
		{
			target.transform.localPosition = array[3];
		}
		else
		{
			target.transform.position = array[3];
		}
		if (target.GetComponent<Rigidbody>() != null)
		{
			Vector3 position2 = target.transform.position;
			target.transform.position = position;
			target.GetComponent<Rigidbody>().MovePosition(position2);
		}
	}

	public static void MoveUpdate(GameObject target, Vector3 position, float time)
	{
		MoveUpdate(target, Hash("position", position, "time", time));
	}

	public static void LookUpdate(GameObject target, Hashtable args)
	{
		CleanArgs(args);
		Vector3[] array = new Vector3[5];
		float num;
		if (args.Contains("looktime"))
		{
			num = (float)args["looktime"];
			num *= Defaults.updateTimePercentage;
		}
		else if (args.Contains("time"))
		{
			num = (float)args["time"] * 0.15f;
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		array[0] = target.transform.eulerAngles;
		if (args.Contains("looktarget"))
		{
			if (args["looktarget"].GetType() == typeof(Transform))
			{
				target.transform.LookAt((Transform)args["looktarget"], ((Vector3?)args["up"]) ?? Defaults.up);
			}
			else if (args["looktarget"].GetType() == typeof(Vector3))
			{
				target.transform.LookAt((Vector3)args["looktarget"], ((Vector3?)args["up"]) ?? Defaults.up);
			}
			array[1] = target.transform.eulerAngles;
			target.transform.eulerAngles = array[0];
			array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
			array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
			array[3].z = Mathf.SmoothDampAngle(array[0].z, array[1].z, ref array[2].z, num);
			target.transform.eulerAngles = array[3];
			if (args.Contains("axis"))
			{
				array[4] = target.transform.eulerAngles;
				switch ((string)args["axis"])
				{
				case "z":
					array[4].x = array[0].x;
					array[4].y = array[0].y;
					break;
				case "y":
					array[4].x = array[0].x;
					array[4].z = array[0].z;
					break;
				case "x":
					array[4].y = array[0].y;
					array[4].z = array[0].z;
					break;
				}
				target.transform.eulerAngles = array[4];
			}
		}
		else
		{
			Debug.LogError("iTween Error: LookUpdate needs a 'looktarget' property!");
		}
	}

	public static void LookUpdate(GameObject target, Vector3 looktarget, float time)
	{
		LookUpdate(target, Hash("looktarget", looktarget, "time", time));
	}

	public static float PathLength(Transform[] path)
	{
		Vector3[] array = new Vector3[path.Length];
		float num = 0f;
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		Vector3[] pts = PathControlPointGenerator(array);
		Vector3 a = Interp(pts, 0f);
		int num2 = path.Length * 20;
		for (int j = 1; j <= num2; j++)
		{
			float t = (float)j / (float)num2;
			Vector3 vector = Interp(pts, t);
			num += Vector3.Distance(a, vector);
			a = vector;
		}
		return num;
	}

	public static float PathLength(Vector3[] path)
	{
		float num = 0f;
		Vector3[] pts = PathControlPointGenerator(path);
		Vector3 a = Interp(pts, 0f);
		int num2 = path.Length * 20;
		for (int i = 1; i <= num2; i++)
		{
			float t = (float)i / (float)num2;
			Vector3 vector = Interp(pts, t);
			num += Vector3.Distance(a, vector);
			a = vector;
		}
		return num;
	}

	public static Texture2D CameraTexture(Color color)
	{
		Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, mipChain: false);
		Color[] array = new Color[Screen.width * Screen.height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = color;
		}
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	public static void PutOnPath(GameObject target, Vector3[] path, float percent)
	{
		target.transform.position = Interp(PathControlPointGenerator(path), percent);
	}

	public static void PutOnPath(Transform target, Vector3[] path, float percent)
	{
		target.position = Interp(PathControlPointGenerator(path), percent);
	}

	public static void PutOnPath(GameObject target, Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		target.transform.position = Interp(PathControlPointGenerator(array), percent);
	}

	public static void PutOnPath(Transform target, Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		target.position = Interp(PathControlPointGenerator(array), percent);
	}

	public static Vector3 PointOnPath(Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		return Interp(PathControlPointGenerator(array), percent);
	}

	public static void DrawLine(Vector3[] line)
	{
		if (line.Length != 0)
		{
			DrawLineHelper(line, Defaults.color, "gizmos");
		}
	}

	public static void DrawLine(Vector3[] line, Color color)
	{
		if (line.Length != 0)
		{
			DrawLineHelper(line, color, "gizmos");
		}
	}

	public static void DrawLine(Transform[] line)
	{
		if (line.Length != 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DrawLineHelper(array, Defaults.color, "gizmos");
		}
	}

	public static void DrawLine(Transform[] line, Color color)
	{
		if (line.Length != 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DrawLineHelper(array, color, "gizmos");
		}
	}

	public static void DrawLineGizmos(Vector3[] line)
	{
		if (line.Length != 0)
		{
			DrawLineHelper(line, Defaults.color, "gizmos");
		}
	}

	public static void DrawLineGizmos(Vector3[] line, Color color)
	{
		if (line.Length != 0)
		{
			DrawLineHelper(line, color, "gizmos");
		}
	}

	public static void DrawLineGizmos(Transform[] line)
	{
		if (line.Length != 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DrawLineHelper(array, Defaults.color, "gizmos");
		}
	}

	public static void DrawLineGizmos(Transform[] line, Color color)
	{
		if (line.Length != 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DrawLineHelper(array, color, "gizmos");
		}
	}

	public static void DrawLineHandles(Vector3[] line)
	{
		if (line.Length != 0)
		{
			DrawLineHelper(line, Defaults.color, "handles");
		}
	}

	public static void DrawLineHandles(Vector3[] line, Color color)
	{
		if (line.Length != 0)
		{
			DrawLineHelper(line, color, "handles");
		}
	}

	public static void DrawLineHandles(Transform[] line)
	{
		if (line.Length != 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DrawLineHelper(array, Defaults.color, "handles");
		}
	}

	public static void DrawLineHandles(Transform[] line, Color color)
	{
		if (line.Length != 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DrawLineHelper(array, color, "handles");
		}
	}

	public static Vector3 PointOnPath(Vector3[] path, float percent)
	{
		return Interp(PathControlPointGenerator(path), percent);
	}

	public static void DrawPath(Vector3[] path)
	{
		if (path.Length != 0)
		{
			DrawPathHelper(path, Defaults.color, "gizmos");
		}
	}

	public static void DrawPath(Vector3[] path, Color color)
	{
		if (path.Length != 0)
		{
			DrawPathHelper(path, color, "gizmos");
		}
	}

	public static void DrawPath(Transform[] path)
	{
		if (path.Length != 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DrawPathHelper(array, Defaults.color, "gizmos");
		}
	}

	public static void DrawPath(Transform[] path, Color color)
	{
		if (path.Length != 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DrawPathHelper(array, color, "gizmos");
		}
	}

	public static void DrawPathGizmos(Vector3[] path)
	{
		if (path.Length != 0)
		{
			DrawPathHelper(path, Defaults.color, "gizmos");
		}
	}

	public static void DrawPathGizmos(Vector3[] path, Color color)
	{
		if (path.Length != 0)
		{
			DrawPathHelper(path, color, "gizmos");
		}
	}

	public static void DrawPathGizmos(Transform[] path)
	{
		if (path.Length != 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DrawPathHelper(array, Defaults.color, "gizmos");
		}
	}

	public static void DrawPathGizmos(Transform[] path, Color color)
	{
		if (path.Length != 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DrawPathHelper(array, color, "gizmos");
		}
	}

	public static void DrawPathHandles(Vector3[] path)
	{
		if (path.Length != 0)
		{
			DrawPathHelper(path, Defaults.color, "handles");
		}
	}

	public static void DrawPathHandles(Vector3[] path, Color color)
	{
		if (path.Length != 0)
		{
			DrawPathHelper(path, color, "handles");
		}
	}

	public static void DrawPathHandles(Transform[] path)
	{
		if (path.Length != 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DrawPathHelper(array, Defaults.color, "handles");
		}
	}

	public static void DrawPathHandles(Transform[] path, Color color)
	{
		if (path.Length != 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DrawPathHelper(array, color, "handles");
		}
	}

	public static void CameraFadeDepth(int depth)
	{
		if ((bool)cameraFade)
		{
			cameraFade.transform.position = new Vector3(cameraFade.transform.position.x, cameraFade.transform.position.y, depth);
		}
	}

	public static void CameraFadeDestroy()
	{
		if ((bool)cameraFade)
		{
			UnityEngine.Object.Destroy(cameraFade);
		}
	}

	public static void Resume(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			((iTween)components[i]).enabled = true;
		}
	}

	public static void Resume(GameObject target, bool includechildren)
	{
		Resume(target);
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.transform)
		{
			Resume(item.gameObject, includechildren: true);
		}
	}

	public static void Resume(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			iTween iTween2 = (iTween)components[i];
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				iTween2.enabled = true;
			}
		}
	}

	public static void Resume(GameObject target, string type, bool includechildren)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			iTween iTween2 = (iTween)components[i];
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				iTween2.enabled = true;
			}
		}
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.transform)
		{
			Resume(item.gameObject, type, includechildren: true);
		}
	}

	public static void Resume()
	{
		for (int i = 0; i < tweens.Count; i++)
		{
			Resume((GameObject)((Hashtable)tweens[i])["target"]);
		}
	}

	public static void Resume(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < tweens.Count; i++)
		{
			GameObject value = (GameObject)((Hashtable)tweens[i])["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			Resume((GameObject)arrayList[j], type);
		}
	}

	public static void Pause(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			iTween iTween2 = (iTween)components[i];
			if (iTween2.delay > 0f)
			{
				iTween2.delay -= Time.time - iTween2.delayStarted;
				iTween2.StopCoroutine("TweenDelay");
			}
			iTween2.isPaused = true;
			iTween2.enabled = false;
		}
	}

	public static void Pause(GameObject target, bool includechildren)
	{
		Pause(target);
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.transform)
		{
			Pause(item.gameObject, includechildren: true);
		}
	}

	public static void Pause(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			iTween iTween2 = (iTween)components[i];
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				if (iTween2.delay > 0f)
				{
					iTween2.delay -= Time.time - iTween2.delayStarted;
					iTween2.StopCoroutine("TweenDelay");
				}
				iTween2.isPaused = true;
				iTween2.enabled = false;
			}
		}
	}

	public static void Pause(GameObject target, string type, bool includechildren)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			iTween iTween2 = (iTween)components[i];
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				if (iTween2.delay > 0f)
				{
					iTween2.delay -= Time.time - iTween2.delayStarted;
					iTween2.StopCoroutine("TweenDelay");
				}
				iTween2.isPaused = true;
				iTween2.enabled = false;
			}
		}
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.transform)
		{
			Pause(item.gameObject, type, includechildren: true);
		}
	}

	public static void Pause()
	{
		for (int i = 0; i < tweens.Count; i++)
		{
			Pause((GameObject)((Hashtable)tweens[i])["target"]);
		}
	}

	public static void Pause(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < tweens.Count; i++)
		{
			GameObject value = (GameObject)((Hashtable)tweens[i])["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			Pause((GameObject)arrayList[j], type);
		}
	}

	public static int Count()
	{
		return tweens.Count;
	}

	public static int Count(string type)
	{
		int num = 0;
		for (int i = 0; i < tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)tweens[i];
			if (((string)hashtable["type"] + (string)hashtable["method"]).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				num++;
			}
		}
		return num;
	}

	public static int Count(GameObject target)
	{
		return target.GetComponents(typeof(iTween)).Length;
	}

	public static int Count(GameObject target, string type)
	{
		int num = 0;
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			iTween iTween2 = (iTween)components[i];
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				num++;
			}
		}
		return num;
	}

	public static void Stop()
	{
		for (int i = 0; i < tweens.Count; i++)
		{
			Stop((GameObject)((Hashtable)tweens[i])["target"]);
		}
		tweens.Clear();
	}

	public static void Stop(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < tweens.Count; i++)
		{
			GameObject value = (GameObject)((Hashtable)tweens[i])["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			Stop((GameObject)arrayList[j], type);
		}
	}

	public static void Stop(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			((iTween)components[i]).Dispose();
		}
	}

	public static void Stop(GameObject target, bool includechildren)
	{
		Stop(target);
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.transform)
		{
			Stop(item.gameObject, includechildren: true);
		}
	}

	public static void Stop(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			iTween iTween2 = (iTween)components[i];
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				iTween2.Dispose();
			}
		}
	}

	public static void Stop(GameObject target, string type, bool includechildren)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			iTween iTween2 = (iTween)components[i];
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				iTween2.Dispose();
			}
		}
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.transform)
		{
			Stop(item.gameObject, type, includechildren: true);
		}
	}

	public static Hashtable Hash(params object[] args)
	{
		Hashtable hashtable = new Hashtable(args.Length / 2);
		if (args.Length % 2 != 0)
		{
			Debug.LogError("Tween Error: Hash requires an even number of arguments!");
			return null;
		}
		for (int i = 0; i < args.Length - 1; i += 2)
		{
			hashtable.Add(args[i], args[i + 1]);
		}
		return hashtable;
	}

	public void Awake()
	{
		RetrieveArgs();
		lastRealTime = Time.realtimeSinceStartup;
	}

	public IEnumerator Start()
	{
		if (delay > 0f)
		{
			yield return StartCoroutine("TweenDelay");
		}
		TweenStart();
	}

	public void Update()
	{
		if (!isRunning || physics)
		{
			return;
		}
		if (!reverse)
		{
			if (percentage < 1f)
			{
				TweenUpdate();
			}
			else
			{
				TweenComplete();
			}
		}
		else if (percentage > 0f)
		{
			TweenUpdate();
		}
		else
		{
			TweenComplete();
		}
	}

	public void FixedUpdate()
	{
		if (!isRunning || !physics)
		{
			return;
		}
		if (!reverse)
		{
			if (percentage < 1f)
			{
				TweenUpdate();
			}
			else
			{
				TweenComplete();
			}
		}
		else if (percentage > 0f)
		{
			TweenUpdate();
		}
		else
		{
			TweenComplete();
		}
	}

	public void LateUpdate()
	{
		if (tweenArguments.Contains("looktarget") && isRunning && (type == "move" || type == "shake" || type == "punch"))
		{
			LookUpdate(base.gameObject, tweenArguments);
		}
	}

	public void OnEnable()
	{
		if (isRunning)
		{
			EnableKinematic();
		}
		if (isPaused)
		{
			isPaused = false;
			if (delay > 0f)
			{
				wasPaused = true;
				ResumeDelay();
			}
		}
	}

	public void OnDisable()
	{
		DisableKinematic();
	}

	public static void DrawLineHelper(Vector3[] line, Color color, string method)
	{
		Gizmos.color = color;
		for (int i = 0; i < line.Length - 1; i++)
		{
			if (method == "gizmos")
			{
				Gizmos.DrawLine(line[i], line[i + 1]);
			}
			else if (method == "handles")
			{
				Debug.LogError("iTween Error: Drawing a line with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
			}
		}
	}

	public static void DrawPathHelper(Vector3[] path, Color color, string method)
	{
		Vector3[] pts = PathControlPointGenerator(path);
		Vector3 to = Interp(pts, 0f);
		Gizmos.color = color;
		int num = path.Length * 20;
		for (int i = 1; i <= num; i++)
		{
			float t = (float)i / (float)num;
			Vector3 vector = Interp(pts, t);
			if (method == "gizmos")
			{
				Gizmos.DrawLine(vector, to);
			}
			else if (method == "handles")
			{
				Debug.LogError("iTween Error: Drawing a path with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
			}
			to = vector;
		}
	}

	public static Vector3[] PathControlPointGenerator(Vector3[] path)
	{
		Vector3[] array = new Vector3[path.Length + 2];
		Array.Copy(path, 0, array, 1, path.Length);
		array[0] = array[1] + (array[1] - array[2]);
		array[array.Length - 1] = array[array.Length - 2] + (array[array.Length - 2] - array[array.Length - 3]);
		if (array[1] == array[array.Length - 2])
		{
			Vector3[] array2 = new Vector3[array.Length];
			Array.Copy(array, array2, array.Length);
			array2[0] = array2[array2.Length - 3];
			array2[array2.Length - 1] = array2[2];
			array = new Vector3[array2.Length];
			Array.Copy(array2, array, array2.Length);
		}
		return array;
	}

	public static Vector3 Interp(Vector3[] pts, float t)
	{
		int num = pts.Length - 3;
		int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		float num3 = t * (float)num - (float)num2;
		Vector3 vector = pts[num2];
		Vector3 vector2 = pts[num2 + 1];
		Vector3 vector3 = pts[num2 + 2];
		Vector3 vector4 = pts[num2 + 3];
		return 0.5f * ((-vector + 3f * vector2 - 3f * vector3 + vector4) * (num3 * num3 * num3) + (2f * vector - 5f * vector2 + 4f * vector3 - vector4) * (num3 * num3) + (-vector + vector3) * num3 + 2f * vector2);
	}

	public static void Launch(GameObject target, Hashtable args)
	{
		if (!args.Contains("id"))
		{
			args["id"] = GenerateID();
		}
		if (!args.Contains("target"))
		{
			args["target"] = target;
		}
		tweens.Insert(0, args);
		target.AddComponent<iTween>();
	}

	public static Hashtable CleanArgs(Hashtable args)
	{
		Hashtable hashtable = new Hashtable(args.Count);
		Hashtable hashtable2 = new Hashtable(args.Count);
		foreach (DictionaryEntry arg in args)
		{
			hashtable.Add(arg.Key, arg.Value);
		}
		foreach (DictionaryEntry item in hashtable)
		{
			if (item.Value.GetType() == typeof(int))
			{
				float num = (int)item.Value;
				args[item.Key] = num;
			}
			if (item.Value.GetType() == typeof(double))
			{
				float num2 = (float)(double)item.Value;
				args[item.Key] = num2;
			}
		}
		foreach (DictionaryEntry arg2 in args)
		{
			hashtable2.Add(arg2.Key.ToString().ToLower(), arg2.Value);
		}
		args = hashtable2;
		return args;
	}

	public static string GenerateID()
	{
		int num = 15;
		char[] array = new char[61]
		{
			'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
			'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
			'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D',
			'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
			'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
			'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7',
			'8'
		};
		int max = array.Length - 1;
		string text = "";
		for (int i = 0; i < num; i++)
		{
			text += array[(int)Mathf.Floor(UnityEngine.Random.Range(0, max))];
		}
		return text;
	}

	public void RetrieveArgs()
	{
		foreach (Hashtable tween in tweens)
		{
			if ((GameObject)tween["target"] == base.gameObject)
			{
				tweenArguments = tween;
				break;
			}
		}
		id = (string)tweenArguments["id"];
		type = (string)tweenArguments["type"];
		method = (string)tweenArguments["method"];
		if (tweenArguments.Contains("time"))
		{
			time = (float)tweenArguments["time"];
		}
		else
		{
			time = Defaults.time;
		}
		if (GetComponent<Rigidbody>() != null)
		{
			physics = true;
		}
		if (tweenArguments.Contains("delay"))
		{
			delay = (float)tweenArguments["delay"];
		}
		else
		{
			delay = Defaults.delay;
		}
		if (tweenArguments.Contains("namedcolorvalue"))
		{
			if (tweenArguments["namedcolorvalue"].GetType() == typeof(NamedValueColor))
			{
				namedcolorvalue = (NamedValueColor)tweenArguments["namedcolorvalue"];
			}
			else
			{
				try
				{
					namedcolorvalue = (NamedValueColor)Enum.Parse(typeof(NamedValueColor), (string)tweenArguments["namedcolorvalue"], ignoreCase: true);
				}
				catch
				{
					Debug.LogWarning("iTween: Unsupported namedcolorvalue supplied! Default will be used.");
					namedcolorvalue = NamedValueColor._Color;
				}
			}
		}
		else
		{
			namedcolorvalue = Defaults.namedColorValue;
		}
		if (tweenArguments.Contains("looptype"))
		{
			if (tweenArguments["looptype"].GetType() == typeof(LoopType))
			{
				loopType = (LoopType)tweenArguments["looptype"];
			}
			else
			{
				try
				{
					loopType = (LoopType)Enum.Parse(typeof(LoopType), (string)tweenArguments["looptype"], ignoreCase: true);
				}
				catch
				{
					Debug.LogWarning("iTween: Unsupported loopType supplied! Default will be used.");
					loopType = LoopType.none;
				}
			}
		}
		else
		{
			loopType = LoopType.none;
		}
		if (tweenArguments.Contains("easetype"))
		{
			if (tweenArguments["easetype"].GetType() == typeof(EaseType))
			{
				easeType = (EaseType)tweenArguments["easetype"];
			}
			else
			{
				try
				{
					easeType = (EaseType)Enum.Parse(typeof(EaseType), (string)tweenArguments["easetype"], ignoreCase: true);
				}
				catch
				{
					Debug.LogWarning("iTween: Unsupported easeType supplied! Default will be used.");
					easeType = Defaults.easeType;
				}
			}
		}
		else
		{
			easeType = Defaults.easeType;
		}
		if (tweenArguments.Contains("space"))
		{
			if (tweenArguments["space"].GetType() == typeof(Space))
			{
				space = (Space)tweenArguments["space"];
			}
			else
			{
				try
				{
					space = (Space)Enum.Parse(typeof(Space), (string)tweenArguments["space"], ignoreCase: true);
				}
				catch
				{
					Debug.LogWarning("iTween: Unsupported space supplied! Default will be used.");
					space = Defaults.space;
				}
			}
		}
		else
		{
			space = Defaults.space;
		}
		if (tweenArguments.Contains("islocal"))
		{
			isLocal = (bool)tweenArguments["islocal"];
		}
		else
		{
			isLocal = Defaults.isLocal;
		}
		if (tweenArguments.Contains("ignoretimescale"))
		{
			useRealTime = (bool)tweenArguments["ignoretimescale"];
		}
		else
		{
			useRealTime = Defaults.useRealTime;
		}
		GetEasingFunction();
	}

	public void GetEasingFunction()
	{
		switch (easeType)
		{
		case EaseType.easeInQuad:
			ease = easeInQuad;
			break;
		case EaseType.easeOutQuad:
			ease = easeOutQuad;
			break;
		case EaseType.easeInOutQuad:
			ease = easeInOutQuad;
			break;
		case EaseType.easeInCubic:
			ease = easeInCubic;
			break;
		case EaseType.easeOutCubic:
			ease = easeOutCubic;
			break;
		case EaseType.easeInOutCubic:
			ease = easeInOutCubic;
			break;
		case EaseType.easeInQuart:
			ease = easeInQuart;
			break;
		case EaseType.easeOutQuart:
			ease = easeOutQuart;
			break;
		case EaseType.easeInOutQuart:
			ease = easeInOutQuart;
			break;
		case EaseType.easeInQuint:
			ease = easeInQuint;
			break;
		case EaseType.easeOutQuint:
			ease = easeOutQuint;
			break;
		case EaseType.easeInOutQuint:
			ease = easeInOutQuint;
			break;
		case EaseType.easeInSine:
			ease = easeInSine;
			break;
		case EaseType.easeOutSine:
			ease = easeOutSine;
			break;
		case EaseType.easeInOutSine:
			ease = easeInOutSine;
			break;
		case EaseType.easeInExpo:
			ease = easeInExpo;
			break;
		case EaseType.easeOutExpo:
			ease = easeOutExpo;
			break;
		case EaseType.easeInOutExpo:
			ease = easeInOutExpo;
			break;
		case EaseType.easeInCirc:
			ease = easeInCirc;
			break;
		case EaseType.easeOutCirc:
			ease = easeOutCirc;
			break;
		case EaseType.easeInOutCirc:
			ease = easeInOutCirc;
			break;
		case EaseType.linear:
			ease = linear;
			break;
		case EaseType.spring:
			ease = spring;
			break;
		case EaseType.bounce:
			ease = bounce;
			break;
		case EaseType.easeInBack:
			ease = easeInBack;
			break;
		case EaseType.easeOutBack:
			ease = easeOutBack;
			break;
		case EaseType.easeInOutBack:
			ease = easeInOutBack;
			break;
		case EaseType.elastic:
			ease = elastic;
			break;
		}
	}

	public void UpdatePercentage()
	{
		if (useRealTime)
		{
			runningTime += Time.realtimeSinceStartup - lastRealTime;
		}
		else
		{
			runningTime += Time.deltaTime;
		}
		if (reverse)
		{
			percentage = 1f - runningTime / time;
		}
		else
		{
			percentage = runningTime / time;
		}
		lastRealTime = Time.realtimeSinceStartup;
	}

	public void CallBack(string callbackType)
	{
		if (tweenArguments.Contains(callbackType) && !tweenArguments.Contains("ischild"))
		{
			GameObject gameObject = ((!tweenArguments.Contains(callbackType + "target")) ? base.gameObject : ((GameObject)tweenArguments[callbackType + "target"]));
			if (tweenArguments[callbackType].GetType() == typeof(string))
			{
				gameObject.SendMessage((string)tweenArguments[callbackType], tweenArguments[callbackType + "params"], SendMessageOptions.DontRequireReceiver);
				return;
			}
			Debug.LogError("iTween Error: Callback method references must be passed as a String!");
			UnityEngine.Object.Destroy(this);
		}
	}

	public void Dispose()
	{
		for (int i = 0; i < tweens.Count; i++)
		{
			if ((string)((Hashtable)tweens[i])["id"] == id)
			{
				tweens.RemoveAt(i);
				break;
			}
		}
		UnityEngine.Object.Destroy(this);
	}

	public void ConflictCheck()
	{
		Component[] components = GetComponents(typeof(iTween));
		int num = 0;
		iTween iTween2;
		while (true)
		{
			if (num >= components.Length)
			{
				return;
			}
			iTween2 = (iTween)components[num];
			if (iTween2.type == "value")
			{
				return;
			}
			if (iTween2.isRunning && iTween2.type == type)
			{
				if (iTween2.tweenArguments.Count != tweenArguments.Count)
				{
					break;
				}
				foreach (DictionaryEntry tweenArgument in tweenArguments)
				{
					if (iTween2.tweenArguments.Contains(tweenArgument.Key))
					{
						if (!iTween2.tweenArguments[tweenArgument.Key].Equals(tweenArguments[tweenArgument.Key]) && (string)tweenArgument.Key != "id")
						{
							iTween2.Dispose();
							return;
						}
						continue;
					}
					iTween2.Dispose();
					return;
				}
				Dispose();
			}
			num++;
		}
		iTween2.Dispose();
	}

	public void EnableKinematic()
	{
	}

	public void DisableKinematic()
	{
	}

	public void ResumeDelay()
	{
		StartCoroutine("TweenDelay");
	}

	public float linear(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value);
	}

	public float clerp(float start, float end, float value)
	{
		float num = 360f;
		float num2 = Mathf.Abs(180f);
		float num3 = 0f;
		float num4 = 0f;
		if (end - start < 0f - num2)
		{
			num4 = (num - start + end) * value;
			return start + num4;
		}
		if (end - start > num2)
		{
			num4 = (0f - (num - end + start)) * value;
			return start + num4;
		}
		return start + (end - start) * value;
	}

	public float spring(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * (float)Math.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	public float easeInQuad(float start, float end, float value)
	{
		end -= start;
		return end * value * value + start;
	}

	public float easeOutQuad(float start, float end, float value)
	{
		end -= start;
		return (0f - end) * value * (value - 2f) + start;
	}

	public float easeInOutQuad(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value + start;
		}
		value -= 1f;
		return (0f - end) / 2f * (value * (value - 2f) - 1f) + start;
	}

	public float easeInCubic(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value + start;
	}

	public float easeOutCubic(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value + 1f) + start;
	}

	public float easeInOutCubic(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value + start;
		}
		value -= 2f;
		return end / 2f * (value * value * value + 2f) + start;
	}

	public float easeInQuart(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value + start;
	}

	public float easeOutQuart(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return (0f - end) * (value * value * value * value - 1f) + start;
	}

	public float easeInOutQuart(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value * value + start;
		}
		value -= 2f;
		return (0f - end) / 2f * (value * value * value * value - 2f) + start;
	}

	public float easeInQuint(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value * value + start;
	}

	public float easeOutQuint(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value * value * value + 1f) + start;
	}

	public float easeInOutQuint(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value * value * value + start;
		}
		value -= 2f;
		return end / 2f * (value * value * value * value * value + 2f) + start;
	}

	public float easeInSine(float start, float end, float value)
	{
		end -= start;
		return (0f - end) * Mathf.Cos(value / 1f * ((float)Math.PI / 2f)) + end + start;
	}

	public float easeOutSine(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Sin(value / 1f * ((float)Math.PI / 2f)) + start;
	}

	public float easeInOutSine(float start, float end, float value)
	{
		end -= start;
		return (0f - end) / 2f * (Mathf.Cos((float)Math.PI * value / 1f) - 1f) + start;
	}

	public float easeInExpo(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (value / 1f - 1f)) + start;
	}

	public float easeOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (0f - Mathf.Pow(2f, -10f * value / 1f) + 1f) + start;
	}

	public float easeInOutExpo(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
		}
		value -= 1f;
		return end / 2f * (0f - Mathf.Pow(2f, -10f * value) + 2f) + start;
	}

	public float easeInCirc(float start, float end, float value)
	{
		end -= start;
		return (0f - end) * (Mathf.Sqrt(1f - value * value) - 1f) + start;
	}

	public float easeOutCirc(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - value * value) + start;
	}

	public float easeInOutCirc(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return (0f - end) / 2f * (Mathf.Sqrt(1f - value * value) - 1f) + start;
		}
		value -= 2f;
		return end / 2f * (Mathf.Sqrt(1f - value * value) + 1f) + start;
	}

	public float bounce(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if (value < 0.36363637f)
		{
			return end * (7.5625f * value * value) + start;
		}
		if (value < 0.72727275f)
		{
			value -= 0.54545456f;
			return end * (7.5625f * value * value + 0.75f) + start;
		}
		if ((double)value < 0.9090909090909091)
		{
			value -= 0.8181818f;
			return end * (7.5625f * value * value + 0.9375f) + start;
		}
		value -= 21f / 22f;
		return end * (7.5625f * value * value + 63f / 64f) + start;
	}

	public float easeInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * (2.70158f * value - num) + start;
	}

	public float easeOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value = value / 1f - 1f;
		return end * (value * value * (2.70158f * value + num) + 1f) + start;
	}

	public float easeInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end / 2f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end / 2f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	public float punch(float amplitude, float value)
	{
		float num = 9f;
		if (value == 0f)
		{
			return 0f;
		}
		if (value == 1f)
		{
			return 0f;
		}
		float num2 = 0.3f;
		num = (float)(0.047746483496176426 * (double)Mathf.Asin(0f));
		return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * 1f - num) * ((float)Math.PI * 2f) / num2);
	}

	public float elastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = 0.3f;
		float num3 = 0f;
		float num4 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		if (num4 != 0f && num4 >= Mathf.Abs(end))
		{
			num3 = num2 / ((float)Math.PI * 2f) * Mathf.Asin(end / num4);
		}
		else
		{
			num4 = end;
			num3 = num2 / 4f;
		}
		return num4 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * num - num3) * ((float)Math.PI * 2f) / num2) + end + start;
	}
}