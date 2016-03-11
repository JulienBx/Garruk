
using SimpleJSON;

namespace Xsolla 
{
	public class XsollaApi : IParseble 
	{

		private string version;

		public IParseble Parse (JSONNode apiNode)
		{
			version = apiNode["ver"];
			return this;
		}

		public override string ToString ()
		{
			return string.Format ("[XsollaApi]");
		}
	}
}
