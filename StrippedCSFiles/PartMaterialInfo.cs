using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PartMaterialInfo : IConfigNode
{
	private class PartMaterialCommand
	{
		private enum CommandType
		{
			Texture,
			Color,
			Float,
			NONE
		}

		private CommandType commandType;

		private string key;

		private Texture texture;

		private Color color;

		private float number;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PartMaterialCommand(string key, string value)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void ApplyCommand(Material targetMaterial)
		{
			throw null;
		}
	}

	private List<PartMaterialCommand> commands;

	private Shader shader;

	private string materialName;

	public string MaterialName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Shader CustomShader
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartMaterialInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ApplyCommands(Material targetMaterial)
	{
		throw null;
	}
}
