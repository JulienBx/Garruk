using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingProfile
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingProfile()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Modifier",""}); //0
		references.Add(new string[]{"- Vider -",""}); //1
		references.Add(new string[]{"Victoires",""}); //2
		references.Add(new string[]{"Défaites",""}); //3
		references.Add(new string[]{"Classement combattant",""}); //4
		references.Add(new string[]{"Classement collectionneur",""}); //5
		references.Add(new string[]{"Rechercher",""}); //6
		references.Add(new string[]{"Trouver un ami à l'aide de son pseudo",""}); //7
		references.Add(new string[]{"Entrez un pseudo",""}); //8
		references.Add(new string[]{"OK",""}); //9
		references.Add(new string[]{"Conquêtes",""}); //10
		references.Add(new string[]{"Défis",""}); //11
		references.Add(new string[]{"Confrontations",""}); //12
		references.Add(new string[]{"Amis",""}); //13
		references.Add(new string[]{"Invitations",""}); //14
		references.Add(new string[]{"Votre ami mène avec ",""}); //15
		references.Add(new string[]{" Victoire(s) contre ",""}); //16
		references.Add(new string[]{" défaite(s)",""}); //17
		references.Add(new string[]{"Vous menez avec ",""}); //18
		references.Add(new string[]{" Victoire(s) contre ",""}); //19
		references.Add(new string[]{" défaite(s)",""}); //20
		references.Add(new string[]{"Ex eaquo ! vous avez gagné chacun ",""}); //21
		references.Add(new string[]{" victoire(s)",""}); //22
		references.Add(new string[]{"Hégémonie atteinte le ",""}); //23
		references.Add(new string[]{"(",""}); //24
		references.Add(new string[]{" pts",""}); //25
		references.Add(new string[]{"prénom : ",""}); //26
		references.Add(new string[]{"nom : ",""}); //27
		references.Add(new string[]{"mail : ",""}); //28
		references.Add(new string[]{"Le mot de passe doit comporter au moins 5 caractères",""}); //29
		references.Add(new string[]{"Le mot de passe ne peut comporter de caractères spéciaux hormis @ _ et .",""}); //30
		references.Add(new string[]{"Veuillez saisir un mot de passe",""}); //31
		references.Add(new string[]{"Veuillez confirmer votre mot de passe",""}); //32
		references.Add(new string[]{"Les deux mots de passes doivent être identiques",""}); //33
		references.Add(new string[]{"Vous ne pouvez pas utiliser de caractères spéciaux",""}); //34
		references.Add(new string[]{"Veuillez saisir une adresse email valide",""}); //35
	}
}