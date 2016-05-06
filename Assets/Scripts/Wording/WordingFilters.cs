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
		references.Add(new string[]{"Factions",""}); //13
		references.Add(new string[]{"Chaque unité appartient à une faction, déterminant les compétences auxquelles elle a accès",""}); //14
		references.Add(new string[]{"Puissance",""}); //15
		references.Add(new string[]{"La puissance de la carte est symbolisée par sa couleur de fond (bleue pour les puissantes, rouges pour les terribles)",""}); //16
		references.Add(new string[]{"Points d'attaque",""}); //17
		references.Add(new string[]{"Les points d'attaque représentent les dégats de base infligés par l'unité quand elle attaque un ennemi",""}); //18
		references.Add(new string[]{"Points de vie",""}); //19
		references.Add(new string[]{"Les points de vie représentent la résistance de la carte pendant un combat",""}); //20
		references.Add(new string[]{"Attributs",""}); //21
		references.Add(new string[]{"les attributs ou caractéristiques des cartes permettent de mesurer leur force, leur résistance et leur puissance",""}); //22
		references.Add(new string[]{"Compétence",""}); //23
		references.Add(new string[]{"Vous pouvez ici chercher toutes les cartes possédant la compétence choisie",""}); //24
		references.Add(new string[]{"Tri croissant",""}); //25
		references.Add(new string[]{"Les cartes seront triés selon l'attribut choisi, par valeur croissante",""}); //26
		references.Add(new string[]{"Tri décroissant",""}); //27
		references.Add(new string[]{"Les cartes seront triés selon l'attribut choisi, par valeur décroissante",""}); //28
		references.Add(new string[]{"Puissance",""}); //29
		references.Add(new string[]{"La puissance de la carte est symbolisée par sa couleur de fond (bleue pour les puissantes, rouges pour les terribles)",""}); //30
		references.Add(new string[]{"Prix",""}); //31
		references.Add(new string[]{"Les cartes peuvent être filtrées selon un prox minimum et un prix maximum",""}); //32
		references.Add(new string[]{"Disciplines",""}); //33
		references.Add(new string[]{"Les disciplines permettent de trier les compétences selon leur type (voir onglet 'disciplines' de la cristalopedia)",""}); //34
		references.Add(new string[]{"Disponibilité",""}); //35
		references.Add(new string[]{"140 compétences existent sur Cristalia. Vous pouvez afficher celles que vous avez déjà découvertes ou celles à découvrir",""}); //36
	}
}