using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingLobby
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.idLanguage];
	}
	static WordingLobby()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Statistiques",""}); //0
		references.Add(new string[]{"Résultats",""}); //1
		references.Add(new string[]{"Récompenses",""}); //2
		references.Add(new string[]{"Victoires",""}); //3
		references.Add(new string[]{"Défaites",""}); //4
		references.Add(new string[]{"Classement combattant",""}); //5
		references.Add(new string[]{"Classement collectionneur",""}); //6
		references.Add(new string[]{"Continuer",""}); //7
		references.Add(new string[]{"(",""}); //8
		references.Add(new string[]{" pts)",""}); //9
		references.Add(new string[]{"Victoire le ",""}); //10
		references.Add(new string[]{"Défaite le ",""}); //11
		references.Add(new string[]{"Hégémonie : ",""}); //12
		references.Add(new string[]{"\nColonisation : ",""}); //13
		references.Add(new string[]{" cristaux",""}); //14
		references.Add(new string[]{"Victoire : ",""}); //15
		references.Add(new string[]{"Bravo ! Votre domination sur la planète est sans limite ! Commencez dès maintenant l'exploration d'une nouvelle planète !",""}); //16
		references.Add(new string[]{"Bravo ! Votre domination sur la planète est sans limite ! Prêt à recommencer ?",""}); //17
		references.Add(new string[]{"Bravo ! Grâce à cette victoire, vous pouvez dès maintenant commencer l'exploration d'une nouvelle planète !",""}); //18
		references.Add(new string[]{"Bravo ! Vous pouvez dès maintenant commencer l'exploration d'une nouvelle planète !",""}); //19
		references.Add(new string[]{"Vous pourrez prochainement explorer une nouvelle planète !",""}); //20
		references.Add(new string[]{"Bravo grâce à cette victoire vous pourrez continuer l'exploration de cette planète !",""}); //21
		references.Add(new string[]{"Vos efforts ont payé et vous permettent de maintenir votre présence sur cette planète !",""}); //22
		references.Add(new string[]{"Votre victoire consolide votre présence sur cette planète !",""}); //23
		references.Add(new string[]{"Malheureusement vos efforts seront insuffisants pour vous maintenir.",""}); //24
		references.Add(new string[]{"Continuer",""}); //25
		references.Add(new string[]{"Jouer",""}); //26
		references.Add(new string[]{" Victoires",""}); //27
		references.Add(new string[]{"Hégémonie",""}); //28
		references.Add(new string[]{"Colonisation",""}); //29
		references.Add(new string[]{"Stabilisation",""}); //30
		references.Add(new string[]{" combats restants",""}); //31
		references.Add(new string[]{" combat restant",""}); //32
		references.Add(new string[]{" victoires",""}); //33
		references.Add(new string[]{" victoire",""}); //34
		references.Add(new string[]{"Hégémonie atteinte\n",""}); //35
		references.Add(new string[]{"Colonisation atteinte\n",""}); //36
		references.Add(new string[]{"Stabilisation atteinte\n",""}); //37
		references.Add(new string[]{"Stabilisation en cours\n",""}); //38

	}
}