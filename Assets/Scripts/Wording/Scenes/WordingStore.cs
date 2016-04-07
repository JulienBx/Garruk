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
		references.Add(new string[]{"Recrutement","Enlistment"}); //2
		references.Add(new string[]{"Enrolez de nouvelles unités en sélectionnant le pack que vous souhaitez","Click on the pack you want to enlist new units"}); //3
		references.Add(new string[]{"Echanger l'argent de votre planète au bureau de change de Cristalia","Use the Cristalian foreign exchange office to buy cristals"}); //4
		references.Add(new string[]{"Acheter des cristaux","Buy cristals"}); //5
		references.Add(new string[]{"Boutique","Shop"}); //6
		references.Add(new string[]{"Merci de bien vouloir saisir une valeur","Please type a number"}); //7
		references.Add(new string[]{"Vendre","Sell"}); //8
		references.Add(new string[]{"Acheter des crédits",""}); //9
		references.Add(new string[]{"a compléter",""}); //10
		references.Add(new string[]{"Retour",""}); //11
		references.Add(new string[]{"Cliquez ici pour retourner à l'écran précédent",""}); //12
	}
}