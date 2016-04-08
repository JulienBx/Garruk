﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingTrainingPopUp
{
	public static IList<string[]> references;

	public static string getReference(int idReference)
	{
		return references[idReference][ApplicationModel.player.IdLanguage];
	}
	static WordingTrainingPopUp()
	{
		references=new List<string[]>();
		references.Add(new string[]{"BRAVO !\n\nVous progressez dans l'apprentissage de la faction ",""}); //0
		references.Add(new string[]{"\n\nil vous manque encore ",""}); //1
		references.Add(new string[]{" victoire ",""}); //2
		references.Add(new string[]{" victoires ",""}); //3
		references.Add(new string[]{"pour débloquer une nouvelle faction",""}); //4
		references.Add(new string[]{"pour terminer votre apprentissage et accéder au mode conquête",""}); //5
		references.Add(new string[]{"DOMMAGE !\n\nContinuer à perservérer ",""}); //6
		references.Add(new string[]{"OK!","OK!"}); //7
		references.Add(new string[]{"BRAVO !\n\nVotre apprentissage de la faction ",""}); //8
		references.Add(new string[]{" est désormais terminé  ",""}); //9
		references.Add(new string[]{"Vous pouvez maintenant découvrir la faction ",""}); //10
		references.Add(new string[]{" ,cliquez sur ok pour découvrir de nouvelles unités",""}); //11
		references.Add(new string[]{"Votre apprentissage étant maintenant terminé, allons découvrir le mode conquête !",""}); //12
	}
}