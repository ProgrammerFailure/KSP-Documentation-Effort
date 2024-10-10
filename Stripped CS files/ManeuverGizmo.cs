using ns2;
using ns9;
using UnityEngine;
using UnityEngine.UI;

public class ManeuverGizmo : ManeuverGizmoBase
{
	public delegate void HandlesUpdatedCallback(Vector3d dV, double double_0);

	public Canvas canvas;

	public Button deleteBtn;

	public Button plusOrbitBtn;

	public Button minusOrbitbtn;

	public PointerEnterExitHandler plusBtnHover;

	public PointerEnterExitHandler minusBtnHover;

	public int orbitsAdded;

	public HandlesUpdatedCallback OnGizmoUpdated = delegate
	{
	};

	public Orbit patchBefore;

	public Orbit patchAhead;

	public PatchedConicRenderer renderer;

	public Vector3 mouseOffset;

	public PatchedConics.PatchCastHit scHit;

	public bool goodHover;

	public bool startDrag;

	public bool justTweaked;

	public double prevUT = -1.0;

	public double prevprevUT = -1.0;

	public bool wasLocked;

	public Vector3 lastMousePosition;

	public override void SetMouseOverGizmo(bool h)
	{
		if (h)
		{
			mouseOverGizmo = true;
			ManeuverGizmoBase.HasMouseFocus = true;
			if ((bool)renderer)
			{
				renderer.SetMouseOverGizmo(h: true);
			}
		}
		else if (!handlePrograde.Hover && !handleRetrograde.Hover && !handleNormal.Hover && !handleAntiNormal.Hover && !handleRadialIn.Hover && !handleRadialOut.Hover && !minusBtnHover.IsOver && !plusBtnHover.IsOver && !hover)
		{
			mouseOverGizmo = false;
			ManeuverGizmoBase.HasMouseFocus = false;
			if ((bool)renderer)
			{
				renderer.SetMouseOverGizmo(h: false);
			}
		}
	}

	public override void Start()
	{
		base.Start();
		camera = PlanetariumCamera.Camera ?? Camera.main;
		canvas.worldCamera = camera;
		willDelete = XKCDColors.BurntSienna;
		willDelete.a = 0.5f;
		deleteBtn.onClick.AddListener(OnDeletePress);
		plusOrbitBtn.onClick.AddListener(OnPlusOrbitPress);
		minusOrbitbtn.onClick.AddListener(OnMinusOrbitPress);
		plusBtnHover = plusOrbitBtn.GetComponent<PointerEnterExitHandler>();
		minusBtnHover = minusOrbitbtn.GetComponent<PointerEnterExitHandler>();
		orbitsAdded = 0;
		if (patchBefore.eccentricity < 1.0)
		{
			orbitsAdded = (int)((double_0 - patchBefore.StartUT) / patchBefore.period);
		}
		justTweaked = true;
	}

	public void Setup(ManeuverNode node, PatchedConicRenderer rnd)
	{
		patchBefore = node.patch;
		patchAhead = node.nextPatch;
		renderer = rnd;
		UpdateOrbitButtons();
	}

	public void SetPatches(Orbit patch, Orbit nextPatch, bool reset)
	{
		patchBefore = patch;
		patchAhead = nextPatch;
		if (reset)
		{
			prevUT = -1.0;
			prevprevUT = -1.0;
		}
	}

	public override void LateUpdate()
	{
		base.LateUpdate();
		if (InputLockManager.IsLocked(ControlTypes.MANNODE_ADDEDIT) && base.MouseOverGizmo && !ManeuverNodeEditorManager.Instance.MouseWithinTool && !ManeuverNodeEditorManager.Instance.JustSelectedNode)
		{
			buttonMode = true;
			if (!wasLocked)
			{
				UpdateOrbitButtons();
			}
			wasLocked = true;
		}
		else
		{
			if (wasLocked)
			{
				UpdateOrbitButtons();
			}
			wasLocked = false;
		}
		if (Input.GetMouseButtonUp(0) && !mouseOverGizmo)
		{
			if (!justTweaked)
			{
				screenSize = 0f;
			}
			else
			{
				justTweaked = false;
			}
		}
		if (Input.GetMouseButtonDown(0) && !mouseOverGizmo)
		{
			justTweaked = false;
		}
		if (double_0 > Planetarium.GetUniversalTime())
		{
			if (Input.GetMouseButtonDown(1) && mouseOverGizmo && InputLockManager.IsUnlocked(ControlTypes.MANNODE_ADDEDIT) && !ManeuverNodeEditorManager.Instance.MouseWithinTool)
			{
				buttonMode = !buttonMode;
			}
		}
		else
		{
			buttonMode = true;
		}
	}

	public void OnDestroy()
	{
		ManeuverGizmoBase.HasMouseFocus = false;
		if ((bool)renderer)
		{
			renderer.SetMouseOverGizmo(h: false);
		}
	}

	public override void OnMouseDown()
	{
		base.OnMouseDown();
		mouseOffset = camera.WorldToScreenPoint(base.transform.position) - Input.mousePosition;
		mouseOffset = new Vector3(mouseOffset.x, mouseOffset.y, 0f);
		goodHover = true;
		startDrag = true;
	}

	public override void OnMouseDrag()
	{
		base.OnMouseDrag();
		if (!InputLockManager.IsUnlocked(ControlTypes.MANNODE_ADDEDIT) || renderer == null)
		{
			return;
		}
		bool flag = double_0 < Planetarium.GetUniversalTime();
		if (startDrag)
		{
			lastMousePosition = Input.mousePosition;
			startDrag = false;
		}
		if (lastMousePosition == Input.mousePosition)
		{
			return;
		}
		PatchRendering patchRendering = renderer.FindRenderingForPatch(patchBefore);
		if (patchRendering == null)
		{
			goodHover = false;
		}
		else
		{
			goodHover = PatchedConics.ScreenCastWorker(Input.mousePosition + mouseOffset, patchRendering, out scHit, 100f);
		}
		if (goodHover)
		{
			double num = double_0;
			double_0 = scHit.UTatTA;
			if (patchBefore.eccentricity < 1.0)
			{
				double_0 += (double)orbitsAdded * patchBefore.period;
			}
			if (flag && !DraggingForwardInTime())
			{
				double_0 = num;
				prevprevUT = prevUT;
				prevUT = double_0;
				return;
			}
			prevprevUT = prevUT;
			prevUT = double_0;
			if (double_0 <= patchBefore.StartUT)
			{
				double_0 -= 1.0;
			}
			if (!double.IsNaN(double_0))
			{
				onGizmoUpdate(DeltaV, double_0);
			}
		}
		else
		{
			grabMat.color = willDelete;
		}
	}

	public bool DraggingForwardInTime()
	{
		if (prevUT >= 0.0 && prevprevUT >= 0.0 && double_0 > prevUT)
		{
			return prevUT >= prevprevUT;
		}
		return false;
	}

	public override void OnMouseUp()
	{
		base.OnMouseUp();
		if (!goodHover)
		{
			OnGizmoDraggedOff();
		}
	}

	public void NextOrbit()
	{
		OnPlusOrbitPress();
	}

	public void PreviousOrbit()
	{
		OnMinusOrbitPress();
	}

	public override void OnProgradeUpdate(float value)
	{
		base.OnProgradeUpdate(value);
		onGizmoUpdate(DeltaV, double_0);
	}

	public override void OnRetrogradeUpdate(float value)
	{
		base.OnRetrogradeUpdate(value);
		onGizmoUpdate(DeltaV, double_0);
	}

	public override void OnNormalUpdate(float value)
	{
		base.OnNormalUpdate(value);
		onGizmoUpdate(DeltaV, double_0);
	}

	public override void OnAntinormalUpdate(float value)
	{
		base.OnAntinormalUpdate(value);
		onGizmoUpdate(DeltaV, double_0);
	}

	public override void OnRadialInUpdate(float value)
	{
		base.OnRadialInUpdate(value);
		onGizmoUpdate(DeltaV, double_0);
	}

	public override void OnRadialOutUpdate(float value)
	{
		base.OnRadialOutUpdate(value);
		onGizmoUpdate(DeltaV, double_0);
	}

	public void OnPlusOrbitPress()
	{
		if (patchBefore.eccentricity < 1.0)
		{
			orbitsAdded++;
			double_0 += patchBefore.period;
			onGizmoUpdate(DeltaV, double_0);
		}
		Mouse.Left.ClearMouseState();
	}

	public void OnMinusOrbitPress()
	{
		if (patchBefore.eccentricity < 1.0)
		{
			orbitsAdded--;
			double_0 -= patchBefore.period;
			onGizmoUpdate(DeltaV, double_0);
		}
		Mouse.Left.ClearMouseState();
	}

	public void OnDeletePress()
	{
		if (InputLockManager.IsUnlocked(ControlTypes.MANNODE_DELETE))
		{
			OnDelete();
		}
		else
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_193630"), 3f, ScreenMessageStyle.UPPER_CENTER);
		}
		Mouse.Left.ClearMouseState();
	}

	public void onGizmoUpdate(Vector3d dV, double double_1)
	{
		justTweaked = true;
		OnGizmoUpdated(dV, double_1);
		UpdateOrbitButtons();
	}

	public bool PreviousOrbitPossible()
	{
		bool result = false;
		if (InputLockManager.IsUnlocked(ControlTypes.MANNODE_ADDEDIT) && (patchBefore.patchEndTransition == Orbit.PatchTransitionType.MANEUVER || patchBefore.patchEndTransition == Orbit.PatchTransitionType.FINAL || patchBefore.eccentricity <= 1.0) && double_0 - patchBefore.period >= patchBefore.StartUT)
		{
			result = true;
		}
		return result;
	}

	public bool NextOrbitPossible()
	{
		bool result = false;
		if (InputLockManager.IsUnlocked(ControlTypes.MANNODE_ADDEDIT) && (patchBefore.patchEndTransition == Orbit.PatchTransitionType.MANEUVER || patchBefore.patchEndTransition == Orbit.PatchTransitionType.FINAL || patchBefore.eccentricity <= 1.0))
		{
			result = true;
		}
		return result;
	}

	public void UpdateOrbitButtons()
	{
		minusOrbitbtn.interactable = PreviousOrbitPossible();
		plusOrbitBtn.interactable = NextOrbitPossible();
	}
}
