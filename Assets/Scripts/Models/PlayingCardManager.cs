using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayingCardManager : MonoBehaviour
{
	private string URLCard = ApplicationModel.host + "get_card.php";
	private string URLUpdateStats = ApplicationModel.dev + "update_stat.php";

	public static PlayingCardManager instance;

	void Start()
	{
		instance = this;
	}

	public IEnumerator updateStat(int idCard, DiscoveryFeature discoveryFeature)
	{
		WWWForm form = new WWWForm(); 								            // Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		            // hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);	            // user
		form.AddField("myform_idcard", idCard);                                 // ID de la carte
		form.AddField("myform_attack", discoveryFeature.Attack?"1":"0");	    // attaque de la carte
		form.AddField("myform_life", discoveryFeature.Life?"1":"0");	        // attaque de la carte
		form.AddField("myform_move", discoveryFeature.Move?"1":"0");	        // attaque de la carte
		form.AddField("myform_movemin", discoveryFeature.MoveMin.ToString());	// attaque de la carte
		form.AddField("myform_skill1", discoveryFeature.Skills[0]?"1":"0");	    // attaque de la carte
		form.AddField("myform_skill2", discoveryFeature.Skills[1]?"1":"0");	    // attaque de la carte
		form.AddField("myform_skill3", discoveryFeature.Skills[2]?"1":"0");	    // attaque de la carte
		form.AddField("myform_skill4", discoveryFeature.Skills[3]?"1":"0");	    // attaque de la carte
		
		WWW w = new WWW(URLUpdateStats, form); 							        // On envoie le formulaire à l'url sur le serveur 
		yield return w; 	
		if (w.error != null) 
		{
			Debug.Log(w.error);          										// donne l'erreur eventuelle
		} 
		Debug.Log(w.text);
	}

	public IEnumerator RetrieveCard(int idCard, Card card)
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_idcard", idCard);						// ID de la carte
		
		WWW w = new WWW(URLCard, form); 							// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log(w.error); 									// donne l'erreur eventuelle
		} 
		else 
		{
			string[] cardEntries = w.text.Split('\n'); 				// Chaque ligne du serveur correspond à une carte

			for(int i = 0 ; i < cardEntries.Length - 1 ; i++) 		// On boucle sur les attributs d'une carte
			{
				string[] cardData = cardEntries[i].Split('\\'); 	// On découpe les attributs de la carte qu'on place dans un tableau
				if (cardData.Length < 2) 
				{
					break;
				}
				if (i == 0)
				{
					card = new Card(System.Convert.ToInt32(cardData[0]), // id
					                     cardData[1], // title
					                     System.Convert.ToInt32(cardData[2]), // life
					                     System.Convert.ToInt32(cardData[3]), // attack
					                     System.Convert.ToInt32(cardData[4]), // speed
					                     System.Convert.ToInt32(cardData[5]), // move
					                     System.Convert.ToInt32(cardData[6]), // artindex
					                     System.Convert.ToInt32(cardData[7]), // idclass
					                     cardData[8], // titleclass
					                     System.Convert.ToInt32(cardData[9]), // lifelevel
					                     System.Convert.ToInt32(cardData[10]), // movelevel
					                     System.Convert.ToInt32(cardData[11]), // speedlevel
					                     System.Convert.ToInt32(cardData[12])); // attacklevel
					card.Skills = new List<Skill>();
				}
				else
				{
					Skill skill = new Skill(  cardData[1],                         // name
					                        System.Convert.ToInt32(cardData[0]), // idskill
					                        System.Convert.ToInt32(cardData[2]), // isactivated
					                        System.Convert.ToInt32(cardData[3]), // level
					                        System.Convert.ToInt32(cardData[4]), // power
					                        System.Convert.ToInt32(cardData[5]), // costmana
					                        cardData[6],                         // description
					                        cardData[7],                         // Nom de la ressource
					                        System.Convert.ToSingle(cardData[8]),// ponderation
					                        System.Convert.ToInt32(cardData[9]));// xmin
					
					card.Skills.Add(skill);
					
					//					Transform go = transform.Find("texturedGameCard/Skill" + Card.Skills.Count + "Area");
					//					
					//					switch (skill.ResourceName)
					//					{
					//					case "Reflexe": 
					//						Reflexe rx = go.gameObject.AddComponent("Reflexe") as Reflexe;
					//						rx.Skill = skill;
					//						rx.SkillNumber = Card.Skills.Count;
					//						rx.Init();
					//						break;
					//					case "Apathie":
					//						Apathie ap = go.gameObject.AddComponent("Apathie") as Apathie;
					//						ap.Skill = skill;
					//						ap.SkillNumber = Card.Skills.Count;
					//						ap.Init();
					//						break;
					//					case "Renforcement":
					//						Renforcement rf = go.gameObject.AddComponent("Renforcement") as Renforcement;
					//						rf.Skill = skill;
					//						rf.SkillNumber = Card.Skills.Count;
					//						rf.Init();
					//						break;
					//					case "Sape":
					//						Sape sp = go.gameObject.AddComponent("Sape") as Sape;
					//						sp.Skill = skill;
					//						sp.SkillNumber = Card.Skills.Count;
					//						sp.Init();
					//						break;
					//					default: 
					//						GameSkill skillCp = go.gameObject.AddComponent("GameSkill") as GameSkill;
					//						skillCp.Skill = skill;
					//						skillCp.SkillNumber = Card.Skills.Count;
					//						break;
					//					}
				}
				
			}
		}	
	}
}
