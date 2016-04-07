using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingFilters
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingFilters()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Filtrer","Filters"}); //0
		references.Add(new string[]{"Compétences","Skills"}); //1
		references.Add(new string[]{"Rechercher","Skill search"}); //2
		references.Add(new string[]{"Factions","Factions"}); //3
		references.Add(new string[]{"Attributs","Stats"}); //4
		references.Add(new string[]{"Rares","Uncommon"}); //5
		references.Add(new string[]{"Très rares","Rare"}); //6
		references.Add(new string[]{"Toutes","All"}); //7
		references.Add(new string[]{"Prix","Price"}); //8
		references.Add(new string[]{"Acquises","Owned"}); //9
		references.Add(new string[]{"Manquantes","Missing"}); //10
		references.Add(new string[]{"Discipline","Skill type"}); //11
		references.Add(new string[]{"Disponibilité","Availability"}); //12
		references.Add(new string[]{"Les factions",""}); //13
		references.Add(new string[]{"Les factions permettent de classer les unités en fonction de leurs caractéristiques (points de vie, attaque et compétences)",""}); //14
		references.Add(new string[]{"Puissance",""}); //15
		references.Add(new string[]{"La puissance correspond à la valeur globale de la carte (elle inclue les points de vie, d'attaque et les compétences)",""}); //16
		references.Add(new string[]{"Vie",""}); //17
		references.Add(new string[]{"Description à rajouter",""}); //18
		references.Add(new string[]{"Attaque",""}); //19
		references.Add(new string[]{"Description à rajouter",""}); //20
		references.Add(new string[]{"Attributs",""}); //21
		references.Add(new string[]{"Description des attributs",""}); //22
		references.Add(new string[]{"Compétences",""}); //23
		references.Add(new string[]{"Description des compétences",""}); //24
		references.Add(new string[]{"Tri croissant",""}); //25
		references.Add(new string[]{"Vous pouvez trier par ordre croissant",""}); //26
		references.Add(new string[]{"Tri décroissant",""}); //27
		references.Add(new string[]{"Vous pouvez trier par ordre décroissant",""}); //28
		references.Add(new string[]{"Niveau de rareté",""}); //29
		references.Add(new string[]{"Le niveau de rareté....",""}); //30
		references.Add(new string[]{"Prix",""}); //31
		references.Add(new string[]{"Vous pouvez filtrer par prix",""}); //32
		references.Add(new string[]{"Disciplines",""}); //33
		references.Add(new string[]{"les disciplines...",""}); //34
		references.Add(new string[]{"Disponibilité",""}); //35
		references.Add(new string[]{"vous pouvez collectionner les compétences ...",""}); //36
	}
}