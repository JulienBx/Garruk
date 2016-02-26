using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class WordingCardName
{
	public static IList<string[]> names;
	private static IList<int> idSkills;

	public static string getName(int idSkill)
	{
		return names[idSkills.IndexOf(idSkill)][ApplicationModel.player.IdLanguage];
	}
	static WordingCardName()
	{
		names = new List<string[]>();
		idSkills=new List<int>();

		//32 - Embusqué
		idSkills.Add(32);
		names.Add(new string[]{"Loke",""}); 

		//33 - Fou
		idSkills.Add(33);
		names.Add(new string[]{"Loggs",""}); 

		//34 - Sanguinaire
		idSkills.Add(34);
		names.Add(new string[]{"Bawen",""}); 

		//35 - Sniper
		idSkills.Add(35);
		names.Add(new string[]{"Leïane",""}); 

		//64 - Piégeur
		idSkills.Add(64);
		names.Add(new string[]{"Deegan",""}); 
		 
		//65 - Lâche
		idSkills.Add(65);
		names.Add(new string[]{"Rolik",""}); 

		//66 - Agile
		idSkills.Add(66);
		names.Add(new string[]{"Lono",""}); 
		 
		//67 - Ninja
		idSkills.Add(67);
		names.Add(new string[]{"Mudo",""}); 
		 
		//68 - Aguerri
		idSkills.Add(68);
		names.Add(new string[]{"Garruk",""}); 
		  
		//69 - Frénétique
		idSkills.Add(69);
		names.Add(new string[]{"Gunno",""}); 
		  
		//70 - Cuirassé
		idSkills.Add(70);
		names.Add(new string[]{"Dundge",""}); 
		 
		//71 - Rapide
		idSkills.Add(71);
		names.Add(new string[]{"Thanba",""}); 

		//72 - Virologue
		idSkills.Add(72);
		names.Add(new string[]{"Rheïane",""}); 

		//73 - Paladin
		idSkills.Add(73);
		names.Add(new string[]{"Vanrell",""}); 

		//75 - Infirmier
		idSkills.Add(75);
		names.Add(new string[]{"Aessa",""}); 

		//76 - Leader
		idSkills.Add(76);
		names.Add(new string[]{"Dacus",""}); 

	}
}