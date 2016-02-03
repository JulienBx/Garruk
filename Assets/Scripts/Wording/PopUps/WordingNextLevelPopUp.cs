using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingNextLevelPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingNextLevelPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"NOUVEAU NIVEAU !",""}); //0
		references.Add(new string[]{" est passé au niveau ",""}); //1
		references.Add(new string[]{".\nChoisissez une caractéristique à augmenter",""}); //2
		references.Add(new string[]{"Cette compétence sera accessible à partir du niveau 4",""}); //3
		references.Add(new string[]{"Cette compétence sera accessible à partir du niveau 8",""}); //4
		references.Add(new string[]{"Augmenter ",""}); //5
		references.Add(new string[]{"Passage au niveau ",""}); //6
		references.Add(new string[]{". P : ",""}); //7
		references.Add(new string[]{"%",""}); //8
		references.Add(new string[]{"Cette compétence a déjà été augmentée 3x, vous ne pouvez plus l'augmenter",""}); //9
		references.Add(new string[]{"Niveau maximum atteint pour cette compétence.",""}); //10
		references.Add(new string[]{"Augmenter l'attaque",""}); //11
		references.Add(new string[]{"Augmenter la vie",""}); //12
		references.Add(new string[]{"Augmenter la vitesse",""}); //13
		references.Add(new string[]{"Niveau maximum atteint pour cette caractéristique",""}); //14
		references.Add(new string[]{"Niv ",""}); //15
	}
}