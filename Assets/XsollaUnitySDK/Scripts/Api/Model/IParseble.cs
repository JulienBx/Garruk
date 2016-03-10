using System;
using SimpleJSON;

namespace Xsolla
{
	public interface IParseble
	{

//		public XsollaApiModel(){
//
//		}
//
//		public XsollaApiModel(JSONNode rootNode){
//			Parse (rootNode);
//		}

		IParseble Parse(JSONNode rootNode);
//		{
//			return null;
//		}
	}
}

