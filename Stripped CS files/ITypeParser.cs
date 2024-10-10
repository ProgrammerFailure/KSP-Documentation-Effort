public interface ITypeParser
{
	object Parse(string s);

	string Convert(object o);
}
