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
		references.Add(new string[]{"Veuillez saisir le nom que vous souhaitez donner à votre unité","Please give your unit a new name"}); //0
		references.Add(new string[]{"Le nom saisi est identique à l'ancien","Chosen name is the same than the previous one"}); //1
		references.Add(new string[]{"Le nom doit comporter au moins 4 caractères","The name must be at least 4 characters long"}); //2
		references.Add(new string[]{"Le nom ne doit pas dépasser 12 caractères","The name's length can not exceed 12 characters"}); //3
		references.Add(new string[]{"Vous ne pouvez pas utiliser de caractères spéciaux","You can not use special characters"}); //4
		references.Add(new string[]{"Merci de bien vouloir saisir un prix","Please choose a selling price"}); //5
		references.Add(new string[]{"Probabilité de succès","Success rate"}); //6
		references.Add(new string[]{"Cette compétence a une probabilité de succès de : ","This skill has a success rate of "}); //7
		references.Add(new string[]{" %.","%."}); //8
		references.Add(new string[]{" victoires\n","Win\n"}); //9
		references.Add(new string[]{" défaites","Loss"}); //10
		references.Add(new string[]{"Niv. ","Lev. "}); //11
		references.Add(new string[]{"Niv. ","Lev. "}); //12
		references.Add(new string[]{"Expérience",""}); //13
		references.Add(new string[]{"L'unité accumule de l'expérience au fil des combats. Cette expérience lui permet de monter de niveau et de faire évoluer ses caractéristiques et compétences",""}); //14
		references.Add(new string[]{"Compétence active",""}); //15
		references.Add(new string[]{"Vous pourrez utiliser les compétences pendant le tour de l'unité, dans l'arène",""}); //16
		references.Add(new string[]{"Compétence passive",""}); //17
		references.Add(new string[]{"les compétences passives se déclenchent automatiquement dans l'arène, et confèrent des bonus aux unités",""}); //18
		references.Add(new string[]{"Nom de l'unité",""}); //19
		references.Add(new string[]{"Le nom de l'unité est visible de vos adversaires et peut être modifié moyennant quelques cristaux",""}); //20
		references.Add(new string[]{"Renommer une unité",""}); //21
		references.Add(new string[]{"Vous pouvez renommer vos unités, en leur payant une petite compensation",""}); //22
		references.Add(new string[]{"Licencier une unité",""}); //23
		references.Add(new string[]{"Vos unités peuvent être licenciées. Leur équipement est dès lors vendu et vous récupérez quelques cristaux",""}); //24
		references.Add(new string[]{"Vendre",""}); //25
		references.Add(new string[]{"Vos unités peuvent être mises en vente sur le marché",""}); //26
		references.Add(new string[]{"Acheter",""}); //27
		references.Add(new string[]{"Vous pouvez acheter les unités mises en vente sur le marché",""}); //28
		references.Add(new string[]{"Modifier le prix de vente",""}); //29
		references.Add(new string[]{"Le prix de vente peut être modifié à tout moment",""}); //30
		references.Add(new string[]{"Retour",""}); //31
		references.Add(new string[]{"Revenir à l'écran précédent",""}); //32
		references.Add(new string[]{"Entrainer l'unité",""}); //33
		references.Add(new string[]{"Faites gagner à votre unité un niveau d'expérience",""}); //34
		references.Add(new string[]{"Points d'attaque",""}); //35
		references.Add(new string[]{"Les points d'attaque représentent les dégats de base infligés par l'unité quand elle attaque un ennemi",""}); //36
		references.Add(new string[]{"Points de vie",""}); //37
		references.Add(new string[]{"Les points de vie représentent la résistance de la carte pendant un combat",""}); //38
		references.Add(new string[]{"Statistiques",""}); //39
		references.Add(new string[]{"Les victoires et défaites de l'unité sont affichées ici",""}); //40
	}
}