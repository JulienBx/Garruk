using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingEndGame
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingEndGame()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Continuer",""}); //0
		references.Add(new string[]{"Récompenses",""}); //1
		references.Add(new string[]{"Vous n'avez pas infligé le moindre dégât à votre adversaire, vous ne gagnez ni points d'expérience ni cristaux",""}); //2
		references.Add(new string[]{"Les défis entre amis ne rapportent ni cristaux, ni points d'expérience",""}); //3
		references.Add(new string[]{"Vous gagnez + ",""}); //4
		references.Add(new string[]{" cristaux (",""}); //5
		references.Add(new string[]{" cristaux)",""}); //6
		references.Add(new string[]{"\n\nVos cartes gagnent chacune ",""}); //7
		references.Add(new string[]{" points d'expérience",""}); //8
	}
}