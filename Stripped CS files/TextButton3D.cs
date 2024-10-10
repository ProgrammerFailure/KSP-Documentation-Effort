using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TextMesh), typeof(Collider))]
public class TextButton3D : MonoBehaviour, IMouseEvents
{
	public Color normalColor;

	public Color hoverColor;

	public Color downColor;

	public Callback onPressed = delegate
	{
	};

	public Callback onReleased = delegate
	{
	};

	public Callback onTap = delegate
	{
	};

	public bool lockButton;

	public bool isHover;

	public Renderer rendrer;

	public TextMesh text;

	public bool tapping;

	public bool tapped;

	public Renderer Rendrer
	{
		get
		{
			if (rendrer == null)
			{
				rendrer = GetComponent<Renderer>();
			}
			return rendrer;
		}
	}

	public TextMesh Text
	{
		get
		{
			if (text == null)
			{
				text = GetComponent<TextMesh>();
			}
			return text;
		}
	}

	public void OnMouseEnter()
	{
		if (!lockButton)
		{
			Rendrer.material.color = hoverColor;
			onPressed();
		}
	}

	public void OnMouseDown()
	{
		if (!lockButton)
		{
			Rendrer.material.color = downColor;
			onReleased();
			if (!tapping)
			{
				StartCoroutine(OnMouseTap());
			}
		}
	}

	public void OnMouseUp()
	{
		if (!lockButton)
		{
			if (tapping)
			{
				tapped = true;
			}
			Rendrer.material.color = hoverColor;
		}
	}

	public void OnMouseExit()
	{
		if (!lockButton)
		{
			Rendrer.material.color = normalColor;
		}
	}

	public IEnumerator OnMouseTap()
	{
		float delay = 0.5f;
		tapping = true;
		tapped = false;
		while (delay >= 0f && !tapped)
		{
			delay -= Time.deltaTime;
			yield return null;
		}
		if (tapped)
		{
			onTap();
		}
		tapping = false;
		tapped = false;
	}

	public void Lock()
	{
		lockButton = true;
		Rendrer.material.color = downColor;
	}

	public void Unlock()
	{
		lockButton = false;
		Rendrer.material.color = normalColor;
	}

	public void OnMouseDrag()
	{
	}

	public MonoBehaviour GetInstance()
	{
		return this;
	}
}
