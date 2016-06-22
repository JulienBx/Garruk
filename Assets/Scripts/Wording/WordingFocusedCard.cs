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
		references.Add(new string[]{"Expérience","Experience"}); //13
		references.Add(new string[]{"L'unité accumule de l'expérience au fil des combats. Cette expérience lui permet de monter de niveau et de faire évoluer ses caractéristiques et compétences","Unit earns experience points during fights. Experience points can then be used to improve the unit stats or skills"}); //14
		references.Add(new string[]{"Compétence active","Active skill"}); //15
		references.Add(new string[]{"Ces compétences sont utilisées par les joueurs dans l'arène","Players can use these skill during fights"}); //16
		references.Add(new string[]{"Compétence passive","Passive skill"}); //17
		references.Add(new string[]{"Les compétences passives se déclenchent automatiquement et confèrent des bonus aux unités","Passive skills give bonus to the units during fights. Players can not control them."}); //18
		references.Add(new string[]{"Nom","Name"}); //19
		references.Add(new string[]{"Chaque unité possède un nom et un visuel unique","Each unit has a unique name and a unique picture"}); //20
		references.Add(new string[]{"",""}); //21
		references.Add(new string[]{"",""}); //22
		references.Add(new string[]{"Licencier une unité","Fire your unit"}); //23
		references.Add(new string[]{"Vos unités peuvent être licenciées. La vente de leur équipement vous rapporte quelques cristaux","Units can be fired. Selling their equipment earns you few cristals"}); //24
		references.Add(new string[]{"Vendre","Sell"}); //25
		references.Add(new string[]{"Vos unités peuvent être mises en vente sur le marché","You can sell your units on the market"}); //26
		references.Add(new string[]{"Acheter","Buy"}); //27
		references.Add(new string[]{"Vous pouvez acheter les unités mises en vente sur le marché","You can buy units from the market"}); //28
		references.Add(new string[]{"Modifier le prix de vente","Change the selling price"}); //29
		references.Add(new string[]{"Le prix de vente peut être modifié à tout moment","Selling price can be changed anytime"}); //30
		references.Add(new string[]{"Retour","Back"}); //31
		references.Add(new string[]{"Revenir à l'écran précédent","Back to previous screen"}); //32
		references.Add(new string[]{"Entrainer l'unité","Training"}); //33
		references.Add(new string[]{"Faites gagner à votre unité un niveau d'expérience","Gain an experience level by training your unit"}); //34
		references.Add(new string[]{"Points d'attaque (ATK)","Attack power (ATK)"}); //35
		references.Add(new string[]{"Les points d'attaque (ATK) représentent les dégats de base infligés par l'unité quand elle attaque un ennemi","Attack power (ATK) determines how many health points are withdrawn from your enemy when you attack"}); //36
		references.Add(new string[]{"Points de vie (PV)","Health points (HP)"}); //37
		references.Add(new string[]{"Les points de vie (PV) représentent la résistance de l'unité pendant un combat","Health points determine your unit strenght. When the total reaches 0, your unit quit the fight"}); //38
		references.Add(new string[]{"Résultats","Résults"}); //39
		references.Add(new string[]{"Les victoires et défaites de l'unité sont affichées ici","Results show the unit wins and losses"}); //40
	}
}