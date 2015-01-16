using UnityEngine; using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyCardsScript : MonoBehaviour
{
	public GameObject CardObject;									// Prefab de la carte vide
	public string URL; 												// L'url de récupération des cartes du joueur sur le serveur
	public string[] cardEntries;									// Tableau pour stocker les informations cartes données par le serveur
	private bool[] classeNew = new bool[10];						// Tableau pour enregistrer un changement d'état sur une checkbox 
	private bool[] classe = new bool[10];							// Tableau pour enregistrer l'état des checkbox
	private string[] listeClasse = new string[10]{"Enchanteur","Roublard","Bersek","Artificier","Mentaliste","Androide","Métamorphe","Prêtre de Garruck","Animiste","Géomancien"};
																	// Tableau avec la liste des classes
	void Start() {

	}

	void Awake()
	{
		StartCoroutine(RetrieveCards()); 							// Appel de la coroutine pour récupérer les cartes de l'utilisateur sur le serveur distant		
	}

	IEnumerator RetrieveCards() {
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 						// Pseudo de l'utilisateur connecté
		form.AddField("myform_deck", ApplicationModel.selectedDeck);// Pseudo de l'utilisateur connecté
		
		WWW w = new WWW(URL, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			print(w.text); 											// donne le retour
			cardEntries = w.text.Split('\n'); 						// Chaque ligne du serveur correspond à une carte
			calcul ();												// Fonction d'affichage des cartes

		}
	}
	
	void OnGUI()
	{

		for (int i=0;i<10;i++)										// Boucle d'initialisation et de contrôle des checkbox
		{

		classeNew[i] = GUI.Toggle (new Rect (740, 80+i*20, 150, 30), classe[i], listeClasse[i]);
		if (classeNew[i] != classe[i]) {							// On vérifie l'état d'une checkbox
				calcul ();											// Si changement, on lance l'affichage des cartes
				classe[i] = classeNew[i];							// On remet à l'équilibre pour éviter le lancement du calcul
			}
		}
	}

	void calcul()
	{

		bool testFilters = false;									// Variable pour tester si tous les filtres sont désélectionnés
		for (int i=0; i<10; i++) {									// Boucle de test sur toutes les checkboxs
			if (classeNew[i]){
				testFilters=true;
			}
				}

		int x=0;												  	// initialisation des coordonnées des cartes
		float y=2.5f;
		int k = 0;
		for(int i = 0 ; i < cardEntries.Length - 1 ; i++)         	// On lit une à une les lignes du tableau renvoyé par le serveur
		{
			Destroy(GameObject.Find("Card" + i + ""));				// On supprime les cartes précédemment affichés
			string[] cardData = cardEntries[i].Split('\\');
			
			for(int j=0;j<10;j++)									// Boucle pour tester la correspondance filtres actifs/classes
			{

				if ((System.Convert.ToInt32 (cardData[4]) == j+1 && classeNew[j]) || (System.Convert.ToInt32 (cardData[4]) == j+1 && !testFilters))
			{
				// test soit le filtre correspond à la classe, soit tous les filtres sont décochés
				x= k % 5;											// On calcule les coordonnées de la prochaine carte
				if (k % 5 == 0 && k>0) 
				{
					y=y-2.5f;
					x=0;
				}
																	// initialisation de la carte
				int cardArt = System.Convert.ToInt32(cardData[1]); 	// l'indice de l'image
				string cardTitle = cardData[2]; 					// le titre de la carte
				int cardLife = System.Convert.ToInt32(cardData[3]);	// le nombre de point de vie
				
				Card card = new Card(cardTitle, cardLife, cardArt);
				
				GameObject instance = 
					Instantiate(CardObject) as GameObject;            // On charge une instance du prefab Card
				instance.transform.localScale = 
					new Vector3(0.15f, 0.02f, 0.20f);                // On change ses attributs d'échelle ...                                                                    
				instance.transform.localPosition = 
					new Vector3(-5 + (2 * x), y, 0);                // ..., de positionnement ...
				instance.GetComponent<GameCard>().Card = card;        // ... et la carte qu'elle représente
				instance.GetComponent<GameCard>().ShowFace();        // On affiche la carte
				instance.gameObject.name = "Card" + k + "";			// On nomme la carte
				k++;
			}
			}
		}
		
	}
	
}