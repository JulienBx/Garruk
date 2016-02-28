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
		references.Add(new string[]{"Malheureusement vous n'avez pas infligé le moindre dégât à votre adversaire, vous ne gagnez ni points d'expérience ni cristaux",""}); //2
		references.Add(new string[]{"Vous avez été le plus fort !\nmais comme il s'agit d'un défi, vous ne gagnez ni cristaux, ni points d'expérience",""}); //3
		references.Add(new string[]{"Vous avez été le plus fort !\n vous obtenez + ",""}); //4
		references.Add(new string[]{" cristaux (",""}); //5
		references.Add(new string[]{" cristaux)",""}); //6
		references.Add(new string[]{"\n\nVos cartes gagnent chacune ",""}); //7
		references.Add(new string[]{" points d'expérience",""}); //8
		references.Add(new string[]{"Votre ami a été le plus fort !\nmais comme il s'agit d'un défi, vous ne gagnez ni cristaux, ni points d'expérience (votre amil non plus d'ailleurs)",""}); //9
		references.Add(new string[]{"Votre adversaire a été le plus fort !\n vous obtenez + ",""}); //10
	}
}