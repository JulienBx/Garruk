using UnityEngine;
using System.Collections;

namespace Xsolla {
	public interface IValidator {
		bool Validate(string s);
		string GetErrorMsg();
	}
}
