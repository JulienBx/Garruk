using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingLoginPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingLoginPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Pseudo ou email","Player name or email"}); //0
		references.Add(new string[]{"Mot de passe","Password"}); //1
		references.Add(new string[]{"Conserver mes informations","Keep me logged in"}); //2
		references.Add(new string[]{"Se connecter","Log in"}); //3
		references.Add(new string[]{"Pas encore de compte ? Inscrivez-vous","Not registered yet ? Create your account"}); //4
		references.Add(new string[]{"Pseudo ou mot de passe oublié ?","Have you forgotten your player name or password ?"}); //5
		references.Add(new string[]{"Connectez-vous","Please log in"});//6
		references.Add(new string[]{"Ou","Or"});//7
	}
}