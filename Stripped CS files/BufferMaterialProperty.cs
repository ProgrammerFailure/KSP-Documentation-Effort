using UnityEngine;

public class BufferMaterialProperty : MaterialPropertyExtensions.MaterialProperty
{
	public readonly ComputeBuffer Buffer;

	public BufferMaterialProperty(string name, ComputeBuffer buffer)
	{
		Name = name;
		Buffer = buffer;
	}

	public override void Apply(ref MaterialPropertyBlock block)
	{
		block.SetBuffer(Name, Buffer);
	}
}
