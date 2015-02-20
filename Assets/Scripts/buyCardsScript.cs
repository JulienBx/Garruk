using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class buyCardsScript : MonoBehaviour {

	public string scriptGenerateURL = null ;
	private bool isExistingCard = false ;
	public GameObject CardObject;

	// Use this for initialization
	void Start (){
	
	}

	void OnGUI() {
	
		// Deal button
		if (GUI.Button(new Rect(10, 10, 100, 20), "Accueil"))
		{
			Application.LoadLevel("HomePage");
		}

		if (GUI.Button(new Rect(10, 40, 150, 20), "Me créer une carte"))
		{
			StartCoroutine(generateRandomCard());
		}
	}

	private IEnumerator generateRandomCard(){

		if (isExistingCard) {
			Destroy(GameObject.Find("Card"));
		}
		isExistingCard = true;

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		
		WWW w = new WWW("http://54.77.118.214//GarrukServer/buyRandomCard.php", form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			print (w.error); 										// donne l'erreur eventuelle
		} else {
			print (w.text);
		}

		string[] data = w.text.Split(new string[] { "\n" }, System.StringSplitOptions.None);
		string[] cardInformation = data[0].Split(new string[] { "//" }, System.StringSplitOptions.None);
		//print (cardInformation);
		Card myCard = new Card(System.Convert.ToInt32(cardInformation[0]), // id
		                        cardInformation[1], // title
		                        System.Convert.ToInt32(cardInformation[2]), // life
		                        System.Convert.ToInt32(cardInformation[3]), // attack
		                        System.Convert.ToInt32(cardInformation[4]), // speed
		                        System.Convert.ToInt32(cardInformation[5]), // move
		                        System.Convert.ToInt32(cardInformation[6]), // artindex
		                        System.Convert.ToInt32(cardInformation[7]), // idclass
		                        cardInformation[8], // titleclass
		                        System.Convert.ToInt32(cardInformation[9]), // lifelevel
		                        System.Convert.ToInt32(cardInformation[10]), // movelevel
		                        System.Convert.ToInt32(cardInformation[11]), // speedlevel
		                        System.Convert.ToInt32(cardInformation[12])); // attacklevel;


		myCard.Skills = new List<Skill>();

		for (int i = 1; i < 5; i++) {         			// On parcourt toutes les cartes de l'utilisateur
			//print (i);
			//print (data[i]);
			cardInformation = data[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			myCard.Skills.Add (new Skill (cardInformation [0], //skillName
		                                  System.Convert.ToInt32 (cardInformation [1]), // idskill
		                                  System.Convert.ToInt32 (cardInformation [2]), // isactivated
		                                  System.Convert.ToInt32 (cardInformation [3]), // level
		                                  System.Convert.ToInt32 (cardInformation [4]), // power
			                              System.Convert.ToInt32 (cardInformation [5]),
			                              cardInformation [6])); // costmana

		}

		GameObject instance = Instantiate(CardObject) as GameObject;            					// On charge une instance du prefab Card
		instance.transform.localScale = new Vector3(4f, 4f, 4f);               					 // On change ses attributs d'échelle ...                                                                    
		instance.transform.localPosition = new Vector3(1,1,0);                					// ..., de positionnement ...
		instance.GetComponent<GameCard>().Card = myCard;        					// ... et la carte qu'elle représente
		instance.GetComponent<GameCard>().ShowFace();        					// On affiche la carte
		instance.animation.Play ("flipCard");
		instance.gameObject.name = "Card";	
	}
}
