using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingAuthentication
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingAuthentication()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Connexion au lobby ...",""}); //0
		references.Add(new string[]{"Veuillez saisir un pseudo",""}); //1
		references.Add(new string[]{"Le pseudo doit comporter au moins 3 caractères",""}); //2
		references.Add(new string[]{"Vous ne pouvez pas utiliser de caractères spéciaux",""}); //3
		references.Add(new string[]{"Veuillez saisir un mot de passe",""}); //4
		references.Add(new string[]{"Veuillez confirmer votre mot de passe",""}); //5
		references.Add(new string[]{"Les deux mots de passes doivent être identiques",""});//6
		references.Add(new string[]{"Le mot de passe doit comporter au moins 5 caractères",""});//7
		references.Add(new string[]{"Le mot de passe ne peut comporter de caractères spéciaux hormis @ _ et .",""});//8
		references.Add(new string[]{"Veuillez saisir une adresse email valide",""});//9
		references.Add(new string[]{"Connexion avec Facebook",""});//10
		references.Add(new string[]{"Inscription avec Facebook",""});//11
		references.Add(new string[]{"Le pseudo ne doit pas comporter plus de 12 caractères",""}); //12
	}
}