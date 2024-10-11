using System.Runtime.CompilerServices;

namespace Experience;

public class ExperienceEffect
{
	private ExperienceTrait parent;

	private string name;

	private float[] levelModifiers;

	private int level;

	public ExperienceTrait Parent
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string Description
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string Name
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float[] LevelModifiers
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int Level
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ExperienceEffect(ExperienceTrait parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ExperienceEffect(ExperienceTrait parent, float[] modifiers)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ExperienceEffect(ExperienceTrait parent, int level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Register(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Unregister(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadFromConfig(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string GetDescription()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnRegister(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnUnregister(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnLoadFromConfig(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual float GetDefaultValue()
	{
		throw null;
	}
}
