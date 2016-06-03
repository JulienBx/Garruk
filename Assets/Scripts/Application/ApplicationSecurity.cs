using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.IO;
using System.Text;
/**
 ** Classe permettant de stocker certaines informations lors des chargements de niveaux
*/
public class ApplicationSecurity
{
    static public readonly string PasswordHash;
    static public readonly string SaltKey;
    static public readonly string VIKey;
	static public readonly string hash;

	static ApplicationSecurity()
	{
		PasswordHash = "4sA4rQtdpgLMxCGJ";
    	SaltKey = "7MScM011s1C07n77";
    	VIKey = "ijA55x1s4mH2X792";
		hash="J8xy9Uz4";

	}

    
}