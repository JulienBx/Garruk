using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingFocusedCard
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingFocusedCard()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Veuillez saisir le nouveau nom de votre unité","Please give your unit a new name"}); //0
		references.Add(new string[]{"Le nom saisi est identique à l'ancien","Chosen name is the same than the previous one"}); //1
		references.Add(new string[]{"Le nom doit comporter au moins 4 caractères","The name must be at least 4 characters long"}); //2
		references.Add(new string[]{"Le nom ne doit pas dépasser 12 caractères","The name's length can not exceed 12 characters"}); //3
		references.Add(new string[]{"Vous ne pouvez pas utiliser de caractères spéciaux","You can not use special characters"}); //4
		references.Add(new string[]{"Merci de bien vouloir saisir un prix","Please choose a selling price"}); //5
		references.Add(new string[]{"Probabilité de succès","Success rate"}); //6
		references.Add(new string[]{"Cette compétence a un taux de réussite de : ","This skill has a success rate of "}); //7
		references.Add(new string[]{" %.","%."}); //8
		references.Add(new string[]{" Victoires\n","Win\n"}); //9
		references.Add(new string[]{" Défaites","Loss"}); //10
		references.Add(new string[]{"Niv. ","Lev. "}); //11
		references.Add(new string[]{"Niv. ","Lev. "}); //12
		references.Add(new string[]{"Expérience",""}); //13
		references.Add(new string[]{"expérience de la carte ....",""}); //14
		references.Add(new string[]{"Compétence active",""}); //15
		references.Add(new string[]{"Compétence active, cliquez pour avoir les détails",""}); //16
		references.Add(new string[]{"Compétence passive",""}); //17
		references.Add(new string[]{"Compétence passive, cliquez pour avoir les détails",""}); //18
		references.Add(new string[]{"Nom de la carte",""}); //19
		references.Add(new string[]{"Nom de la carte ....",""}); //20
		references.Add(new string[]{"Renommer une carte",""}); //21
		references.Add(new string[]{"Vous pouvez renommer...",""}); //22
		references.Add(new string[]{"Supprimer une carte",""}); //23
		references.Add(new string[]{"Vous pouvez supprimer...",""}); //24
		references.Add(new string[]{"Vendre",""}); //25
		references.Add(new string[]{"Vous pouvez vendre...",""}); //26
		references.Add(new string[]{"Acheter",""}); //27
		references.Add(new string[]{"Vous pouvez vendre...",""}); //28
		references.Add(new string[]{"Editer la vente",""}); //29
		references.Add(new string[]{"Vous pouvez modifier...",""}); //30
		references.Add(new string[]{"Quitter",""}); //31
		references.Add(new string[]{"Cliquez ici pour revenir à l'écran précédent",""}); //32
		references.Add(new string[]{"Augmenter la carte",""}); //33
		references.Add(new string[]{"Cliquez ici pour achter un niveau d'expérience",""}); //34
		references.Add(new string[]{"Attaque",""}); //35
		references.Add(new string[]{"Description de l'attaque",""}); //36
		references.Add(new string[]{"Vie",""}); //37
		references.Add(new string[]{"Description de la vie",""}); //38
		references.Add(new string[]{"Statistiques",""}); //39
		references.Add(new string[]{"Description des statistiques",""}); //40
	}
}