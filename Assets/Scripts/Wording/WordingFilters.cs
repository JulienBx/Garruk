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
		references.Add(new string[]{"Filtres","Filters"}); //0
		references.Add(new string[]{"Compétence","Skill"}); //1
		references.Add(new string[]{"Rechercher","Skill search"}); //2
		references.Add(new string[]{"Factions","Factions"}); //3
		references.Add(new string[]{"Attributs","Stats"}); //4
		references.Add(new string[]{"Puissantes","Uncommon"}); //5
		references.Add(new string[]{"Terribles","Rare"}); //6
		references.Add(new string[]{"Toutes","All"}); //7
		references.Add(new string[]{"Prix","Price"}); //8
		references.Add(new string[]{"Découvertes","Owned"}); //9
		references.Add(new string[]{"A découvrir","Missing"}); //10
		references.Add(new string[]{"Disciplines","Skill type"}); //11
		references.Add(new string[]{"Statut","Availability"}); //12
		references.Add(new string[]{"Factions","Factions"}); //13
		references.Add(new string[]{"Chaque unité appartient à une faction, déterminant les compétences auxquelles elle a accès","Each unit belongs to a faction and can only learn its skills"}); //14
		references.Add(new string[]{"Puissance","Power"}); //15
		references.Add(new string[]{"La puissance de l'unité est symbolisée par sa couleur de fond (bleue pour les puissantes, rouges pour les terribles)","Background color shows the power of your unit. Blue background means strong unit, and some folks claim to have seen red background units with incredible power"}); //16
		references.Add(new string[]{"Points d'attaque (ATK)","Attack power (ATK)"}); //17
		references.Add(new string[]{"Les points d'attaque (ATK) représentent les dégats de base infligés par l'unité quand elle attaque un ennemi","Attack power (ATK) determines how many health points are withdrawn from your enemy when you attack"}); //18
		references.Add(new string[]{"Points de vie (PV)","Health points (HP)"}); //19
		references.Add(new string[]{"Les points de vie représentent la résistance de l'unité pendant un combat","Health points determine your unit strenght. When the total reaches 0, your unit quit the fight"}); //20
		references.Add(new string[]{"Stats","Stats"}); //21
		references.Add(new string[]{"Les stats des unités permettent de mesurer leur force, et leur résistance","Stats measure unit strenght and power"}); //22
		references.Add(new string[]{"Compétence","Skill"}); //23
		references.Add(new string[]{"Vous pouvez ici chercher toutes les unités possédant la compétence choisie","You can look for every unit you own using a specific skill"}); //24
		references.Add(new string[]{"Tri croissant","Ascending sort"}); //25
		references.Add(new string[]{"Les unités seront triés selon la stat choisie, par valeur croissante","Units will be ascending sorted depending on the chosen stat"}); //26
		references.Add(new string[]{"Tri décroissant","Descending sort"}); //27
		references.Add(new string[]{"Les cartes seront triés selon l'attribut choisi, par valeur décroissante","Units will be descending sorted depending on the chosen stat"}); //28
		references.Add(new string[]{"Puissance","Power"}); //29
		references.Add(new string[]{"La puissance de l'unité est symbolisée par sa couleur de fond (bleue pour les puissantes, rouges pour les terribles)","Background color shows the power of your unit. Blue background means strong unit, and some folks claim to have seen red background units with incredible power"}); //30
		references.Add(new string[]{"Prix","Price"}); //31
		references.Add(new string[]{"Les unités peuvent être filtrées selon un prix minimum et un prix maximum","Units can be filtered through a minimum price and/or a maximum price"}); //32
		references.Add(new string[]{"Disciplines","Skill type"}); //33
		references.Add(new string[]{"Vous pouvez trier les compétences selon leur discipline (description complète disponible dans la Cristalopedia)","You can filter the skills depending on their types (full description available in the cristalopedia)"}); //34
		references.Add(new string[]{"Recherche","Research"}); //35
		references.Add(new string[]{"140 compétences existent sur Cristalia. Vous pouvez afficher celles que vous avez déjà découvertes ou celles à découvrir","You can display the skills you have already discovered ot the ones you have not"}); //36
	}
}