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
		names.Add(new string[]{"Loke","Loke"}); 

		//33 - Fou
		idSkills.Add(33);
		names.Add(new string[]{"Loggs","Loggs"}); 

		//34 - Sanguinaire
		idSkills.Add(34);
		names.Add(new string[]{"Bawen","Bawen"}); 

		//35 - Sniper
		idSkills.Add(35);
		names.Add(new string[]{"Leïane","Leïane"}); 

		//64 - Piégeur
		idSkills.Add(64);
		names.Add(new string[]{"Deegan","Deegan"}); 
		 
		//65 - Lâche
		idSkills.Add(65);
		names.Add(new string[]{"Rolik","Rolik"}); 

		//66 - Agile
		idSkills.Add(66);
		names.Add(new string[]{"Lono","Lono"}); 
		 
		//67 - Ninja
		idSkills.Add(67);
		names.Add(new string[]{"Mudo","Mudo"}); 
		 
		//68 - Aguerri
		idSkills.Add(68);
		names.Add(new string[]{"Garruk","Garruk"}); 
		  
		//69 - Frénétique
		idSkills.Add(69);
		names.Add(new string[]{"Gunno","Gunno"}); 
		  
		//70 - Cuirassé
		idSkills.Add(70);
		names.Add(new string[]{"Dundge","Dundge"}); 
		 
		//71 - Rapide
		idSkills.Add(71);
		names.Add(new string[]{"Thanba","Thanba"}); 

		//72 - Virologue
		idSkills.Add(72);
		names.Add(new string[]{"Rhea","Rhea"}); 

		//73 - Paladin
		idSkills.Add(73);
		names.Add(new string[]{"Vanrell","Vanrell"}); 

		//75 - Infirmier
		idSkills.Add(75);
		names.Add(new string[]{"Aessa","Aessa"}); 

		//76 - Leader
		idSkills.Add(76);
		names.Add(new string[]{"Dacus","Dacus"}); 

		//138 - Mutant
		idSkills.Add(138);
		names.Add(new string[]{"Rhidar","Rhidar"}); 

		//139 - Symbiote
		idSkills.Add(139);
		names.Add(new string[]{"Bopren","Bopren"}); 

		//140 - Alchimiste
		idSkills.Add(140);
		names.Add(new string[]{"Renkin","Renkin"});

		//141 - Human Killer
		idSkills.Add(141);
		names.Add(new string[]{"Deggs","Deggs"});

		//110 - Cristomaitre
		idSkills.Add(110);
		names.Add(new string[]{"Gurwen","Gurwen"}); 

		//111 - Apotre
		idSkills.Add(111);
		names.Add(new string[]{"Bolek","Bolek"}); 

		//112 - Fanatique
		idSkills.Add(112);
		names.Add(new string[]{"Boty","Boty"});

		//113 - Purificateur
		idSkills.Add(113);
		names.Add(new string[]{"Munnar","Munnar"});

		//113 - God
		idSkills.Add(144);
		names.Add(new string[]{"Dieu Cristal","Cristal God"});

		//113 - God
		idSkills.Add(47);
		names.Add(new string[]{"Blob","Blob"});
	}
}