using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingStore
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingStore()
	{
		references=new List<string[]>();
		references.Add(new string[]{"Acheter","Buy"}); //0
		references.Add(new string[]{"Retour","Go back"}); //1
		references.Add(new string[]{"le centre de recrutement","Enlistment"}); //2
		references.Add(new string[]{"Accédez aux offres du centre de recrutement pour trouver les unités qui vous intéressent. Chaque unité possède ses propres compétences et caractéristiques, que vous ne découvrirez qu'après les avoir recrutées!","Click on the pack you want to enlist new units"}); //3
		references.Add(new string[]{"La boutique vous permet d'échanger de l'argent terrien contre des cristaux, selon le taux de change Terra-Cristalien en vigueur","Use the Cristalian foreign exchange office to buy cristals"}); //4
		references.Add(new string[]{"Acheter des cristaux","Buy cristals"}); //5
		references.Add(new string[]{"Boutique","Shop"}); //6
		references.Add(new string[]{"Merci de bien vouloir saisir une valeur","Please type a number"}); //7
		references.Add(new string[]{"Vendre","Sell"}); //8
		references.Add(new string[]{"Acheter des cristaux","Buy cristals"}); //9
		references.Add(new string[]{"La boutique vous permet d'acheter des cristaux supplémentaires et d'accélérer votre progression dans le jeu","In the shop you can buy some cristals with your terran money. Cristals help you to get better units and to train them before fighting"}); //10
		references.Add(new string[]{"Retour","Back"}); //11
		references.Add(new string[]{"Revenir au centre de recrutement","Back to the store"}); //12
	}
}