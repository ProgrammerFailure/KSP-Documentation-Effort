public interface IModular
{
	string ClassName { get; }

	int ClassID { get; }

	BaseEventList Events { get; }

	BaseFieldList Fields { get; }
}
