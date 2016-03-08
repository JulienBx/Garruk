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
		references.Add(new string[]{"NOUVEAU NIVEAU!","NEW LEVEL!"}); //0
		references.Add(new string[]{" a atteint le niveau "," has reached level "}); //1
		references.Add(new string[]{".\n\nChoisissez une compétence ou stat à booster",".\n\nChoose a stat or skill to boost"}); //2
		references.Add(new string[]{"L'unité acquerra une nouvelle compétence au niveau 4","Reaching level 4 will unlock a new skill"}); //3
		references.Add(new string[]{"L'unité acquerra une nouvelle compétence au niveau 8","Reaching level 8 will unlock a new skill"}); //4
		references.Add(new string[]{"Augmenter ","Train"}); //5
		references.Add(new string[]{"Passage au niveau ","Reaching level "}); //6
		references.Add(new string[]{". P : ",". P : "}); //7
		references.Add(new string[]{"%","%"}); //8
		references.Add(new string[]{"Une compétence ne peut être améliorée plus de 3 fois","This skill has already been trained 3 times and can not be trained more"}); //9
		references.Add(new string[]{"Cette compétence est déjà au niveau maximum.","This skill has reached its maximum level"}); //10
		references.Add(new string[]{"Augmenter l'attaque","Increase attack power"}); //11
		references.Add(new string[]{"Augmenter la vie","Increase health points"}); //12
		references.Add(new string[]{"Augmenter la vitesse","Increase speed"}); //13
		references.Add(new string[]{"Niveau maximum atteint pour cette caractéristique","This stat has reached its maximum level"}); //14
		references.Add(new string[]{"Niv. ","Lev. "}); //15
	}
}