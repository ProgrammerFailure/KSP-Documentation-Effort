using System;
using System.Collections;
using UnityEngine;

public class Mouse : MonoBehaviour
{
	public class MouseButton
	{
		public bool button;

		public bool down;

		public bool up;

		public bool tap;

		public bool doubleTap;

		public bool abort;

		public float doubleClickTime;

		public Vector2 dragDelta = Vector2.zero;

		public Vector2 pAtBtnDown = Vector2.zero;

		public Mouse owner;

		public int buttonIndex;

		public static RaycastHit hoveredPartHitInfo;

		public bool isTapStarted;

		public MouseButton(Mouse owner, int button)
		{
			this.owner = owner;
			buttonIndex = button;
		}

		public void Update()
		{
			if (abort)
			{
				if (!isTapStarted)
				{
					abort = false;
				}
				return;
			}
			down = Input.GetMouseButtonDown(buttonIndex);
			up = Input.GetMouseButtonUp(buttonIndex);
			button = Input.GetMouseButton(buttonIndex);
			if (down)
			{
				pAtBtnDown = screenPos;
			}
			else if (button)
			{
				dragDelta = screenPos - pAtBtnDown;
			}
			if (!isTapStarted)
			{
				tap = false;
				doubleTap = false;
				if (down)
				{
					owner.StartCoroutine(TapRoutine());
				}
			}
		}

		public IEnumerator TapRoutine()
		{
			isTapStarted = true;
			float endTime2 = Time.realtimeSinceStartup + GameSettings.DOUBLECLICK_MOUSESPEED;
			bool tapped = false;
			while (!abort && !(Time.realtimeSinceStartup >= endTime2))
			{
				if (Input.GetMouseButtonUp(buttonIndex))
				{
					tap = true;
					tapped = true;
					yield return null;
					tap = false;
					break;
				}
				yield return null;
			}
			if (tapped)
			{
				endTime2 = Time.realtimeSinceStartup + GameSettings.DOUBLECLICK_MOUSESPEED;
				while (!abort && !(Time.realtimeSinceStartup >= endTime2))
				{
					if (Input.GetMouseButtonDown(buttonIndex))
					{
						doubleTap = true;
						tap = false;
						doubleClickTime = Time.realtimeSinceStartup + GameSettings.DOUBLECLICK_MOUSESPEED;
						break;
					}
					yield return null;
				}
			}
			abort = false;
			isTapStarted = false;
		}

		public void ClearMouseState()
		{
			doubleClickTime = Time.realtimeSinceStartup;
			tap = false;
			doubleTap = false;
			down = false;
			up = false;
			button = false;
			abort = true;
		}

		public bool GetButtonDown()
		{
			return down;
		}

		public bool GetButtonUp()
		{
			return up;
		}

		public bool GetButton()
		{
			return button;
		}

		public bool GetClick()
		{
			return tap;
		}

		public bool GetDoubleClick(bool isDelegate = false)
		{
			if (isDelegate && Time.realtimeSinceStartup < doubleClickTime)
			{
				return true;
			}
			return doubleTap;
		}

		public Vector2 GetDragDelta()
		{
			return dragDelta;
		}

		public bool WasDragging(float delta = 400f)
		{
			if (!button && !up)
			{
				return false;
			}
			return dragDelta.sqrMagnitude > delta;
		}
	}

	[Flags]
	public enum Buttons
	{
		None = 0,
		Left = 1,
		Right = 2,
		Middle = 4,
		Btn4 = 8,
		Btn5 = 0x10,
		Any = -1
	}

	public static Mouse fetch;

	public static MouseButton Left;

	public static MouseButton Right;

	public static MouseButton Middle;

	public static Vector2 screenPos;

	public static Vector2 lastPos;

	public static Vector2 delta;

	public static PartPointer partPointer;

	public static Part HoveredPart;

	public static bool IsMoving => delta.sqrMagnitude > 0.01f;

	public void Awake()
	{
		if ((bool)fetch)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			fetch = this;
		}
	}

	public void OnDestroy()
	{
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}

	public void Start()
	{
		Left = new MouseButton(this, 0);
		Right = new MouseButton(this, 1);
		Middle = new MouseButton(this, 2);
	}

	public void Update()
	{
		Left.Update();
		Right.Update();
		Middle.Update();
		screenPos = Input.mousePosition;
		screenPos.y = (float)Screen.height - screenPos.y;
		delta = screenPos - lastPos;
		lastPos = screenPos;
		HoveredPart = CheckHoveredPart();
	}

	public static Buttons GetAllMouseButtons()
	{
		Buttons buttons = Buttons.None;
		if (Input.GetMouseButton(0))
		{
			buttons |= Buttons.Left;
		}
		if (Input.GetMouseButton(1))
		{
			buttons |= Buttons.Right;
		}
		if (Input.GetMouseButton(2))
		{
			buttons |= Buttons.Middle;
		}
		if (Input.GetMouseButton(3))
		{
			buttons |= Buttons.Btn4;
		}
		if (Input.GetMouseButton(4))
		{
			buttons |= Buttons.Btn5;
		}
		return buttons;
	}

	public static Buttons GetAllMouseButtonsUp()
	{
		Buttons buttons = Buttons.None;
		if (Input.GetMouseButtonUp(0))
		{
			buttons |= Buttons.Left;
		}
		if (Input.GetMouseButtonUp(1))
		{
			buttons |= Buttons.Right;
		}
		if (Input.GetMouseButtonUp(2))
		{
			buttons |= Buttons.Middle;
		}
		if (Input.GetMouseButtonUp(3))
		{
			buttons |= Buttons.Btn4;
		}
		if (Input.GetMouseButtonUp(4))
		{
			buttons |= Buttons.Btn5;
		}
		return buttons;
	}

	public static Buttons GetAllMouseButtonsDown()
	{
		Buttons buttons = Buttons.None;
		if (Input.GetMouseButtonDown(0))
		{
			buttons |= Buttons.Left;
		}
		if (Input.GetMouseButtonDown(1))
		{
			buttons |= Buttons.Right;
		}
		if (Input.GetMouseButtonDown(2))
		{
			buttons |= Buttons.Middle;
		}
		if (Input.GetMouseButtonDown(3))
		{
			buttons |= Buttons.Btn4;
		}
		if (Input.GetMouseButtonDown(4))
		{
			buttons |= Buttons.Btn5;
		}
		return buttons;
	}

	public static bool CheckButtons(Buttons buttons, Buttons buttonsToTest, bool strict = true)
	{
		if (strict)
		{
			return (buttons & buttonsToTest) == buttonsToTest;
		}
		return (buttons & buttonsToTest) != 0;
	}

	public static Part CheckHoveredPart()
	{
		if (!HighLogic.LoadedSceneIsGame)
		{
			return null;
		}
		Camera currentCamera = CameraManager.GetCurrentCamera();
		if (!(currentCamera == null) && currentCamera.enabled)
		{
			Vector3 mousePosition = Input.mousePosition;
			if (!(mousePosition.x < 0f) && !(mousePosition.x >= (float)Screen.width) && !(mousePosition.y < 0f) && mousePosition.y < (float)Screen.height)
			{
				Part part = null;
				if (HighLogic.LoadedSceneIsFlight && EVAConstructionModeController.Instance != null && EVAConstructionModeController.Instance.evaEditor != null && EVAConstructionModeController.Instance.evaEditor.SelectedPart != null)
				{
					RaycastHit[] array = Physics.RaycastAll(currentCamera.ScreenPointToRay(mousePosition), float.MaxValue, Part.layerMask.value);
					if (array.Length < 1)
					{
						return null;
					}
					if (array.Length == 1)
					{
						part = GetValidHoverPart(array[0]);
						if (part != null && part.State != PartStates.CARGO)
						{
							MouseButton.hoveredPartHitInfo = array[0];
							return part;
						}
						return null;
					}
					Array.Sort(array, (RaycastHit a, RaycastHit b) => a.distance.CompareTo(b.distance));
					int num = 0;
					while (true)
					{
						if (num < array.Length)
						{
							part = GetValidHoverPart(array[num]);
							if (part != null && part.State != PartStates.CARGO)
							{
								break;
							}
							num++;
							continue;
						}
						return null;
					}
					MouseButton.hoveredPartHitInfo = array[num];
					return part;
				}
				if (!Physics.Raycast(currentCamera.ScreenPointToRay(mousePosition), out MouseButton.hoveredPartHitInfo, float.MaxValue, Part.layerMask.value))
				{
					return null;
				}
				return GetValidHoverPart(MouseButton.hoveredPartHitInfo);
			}
			return null;
		}
		return null;
	}

	public static Part GetValidHoverPart(RaycastHit hitPart)
	{
		Part part = null;
		partPointer = FlightGlobals.GetPartPointerUpwardsCached(hitPart.collider.gameObject);
		part = ((!(partPointer != null) || !(partPointer.PartReference != null)) ? FlightGlobals.GetPartUpwardsCached(hitPart.collider.gameObject) : partPointer.PartReference);
		if (part != null && part.State == PartStates.PLACEMENT)
		{
			ModuleCargoPart moduleCargoPart = part.FindModuleImplementing<ModuleCargoPart>();
			if ((moduleCargoPart != null && moduleCargoPart.IsDeployedSciencePart()) || EditorLogic.SelectedPart == part)
			{
				return null;
			}
		}
		return part;
	}
}
