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
		references.Add(new string[]{"Pseudo ou email","Login or email"}); //0
		references.Add(new string[]{"Mot de passe","Password"}); //1
		references.Add(new string[]{"Se souvenir de moi","Remember me"}); //2
		references.Add(new string[]{"Se connecter","Go !"}); //3
		references.Add(new string[]{"Pas de compte ? S'inscrire","Not registred yet, create an account"}); //4
		references.Add(new string[]{"Pseudo ou mot de passe oublié ?","Forgot your login or password ?"}); //5
		references.Add(new string[]{"Connectez-vous","Connection"});//6
		references.Add(new string[]{"Ou","Connection"});//7
	}
}