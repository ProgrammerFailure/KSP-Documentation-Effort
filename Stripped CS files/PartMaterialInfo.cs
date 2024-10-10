using System.Collections.Generic;
using UnityEngine;

public class PartMaterialInfo : IConfigNode
{
	public class PartMaterialCommand
	{
		public enum CommandType
		{
			Texture,
			Color,
			Float,
			NONE
		}

		public CommandType commandType;

		public string key;

		public Texture texture;

		public Color color;

		public float number;

		public PartMaterialCommand(string key, string value)
		{
			commandType = CommandType.NONE;
			this.key = key;
			if (value.StartsWith("#") && ColorUtility.TryParseHtmlString(value, out color))
			{
				commandType = CommandType.Color;
				return;
			}
			if (float.TryParse(value, out number))
			{
				commandType = CommandType.Float;
				return;
			}
			GameDatabase.TextureInfo textureInfo = GameDatabase.Instance.GetTextureInfo(value);
			commandType = CommandType.Texture;
			if (textureInfo != null)
			{
				texture = textureInfo.texture;
			}
			else
			{
				texture = null;
			}
		}

		public void ApplyCommand(Material targetMaterial)
		{
			switch (commandType)
			{
			case CommandType.Texture:
				targetMaterial.SetTexture(key, texture);
				break;
			case CommandType.Color:
				targetMaterial.SetColor(key, color);
				break;
			case CommandType.Float:
				targetMaterial.SetFloat(key, number);
				break;
			}
		}
	}

	public List<PartMaterialCommand> commands;

	public Shader shader;

	public string materialName;

	public string MaterialName => materialName;

	public Shader CustomShader => shader;

	public void Load(ConfigNode node)
	{
		commands = new List<PartMaterialCommand>();
		foreach (ConfigNode.Value value in node.values)
		{
			switch (value.name)
			{
			default:
				commands.Add(new PartMaterialCommand(value.name, value.value));
				break;
			case "backColor":
				commands.Add(new PartMaterialCommand("_BackColor", value.value));
				break;
			case "backTextureURL":
				commands.Add(new PartMaterialCommand("_BackTex", value.value));
				break;
			case "color":
				commands.Add(new PartMaterialCommand("_Color", value.value));
				break;
			case "mainTextureURL":
				commands.Add(new PartMaterialCommand("_MainTex", value.value));
				break;
			case "shader":
				shader = Shader.Find(value.value);
				break;
			case "materialName":
				materialName = value.value;
				break;
			}
		}
	}

	public void Save(ConfigNode node)
	{
	}

	public void ApplyCommands(Material targetMaterial)
	{
		for (int i = 0; i < commands.Count; i++)
		{
			commands[i].ApplyCommand(targetMaterial);
		}
	}
}
