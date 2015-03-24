using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class buyCardsScript : MonoBehaviour {

	public string scriptGenerateURL = null ;
	private bool isExistingCard = false ;
	public GameObject CardObject;
	public Rect windowRect ;
	GameObject instance;

	private string URLGetMoneyByUser = "http://54.77.118.214/GarrukServer/get_money_by_user.php";
	private string URLSellRandomCard = "http://54.77.118.214/GarrukServer/sellRandomCard.php";
	private string URLBuyRandomCard = "http://54.77.118.214/GarrukServer/buyRandomCard.php";
	private string URLPutOnMarket = "http://54.77.118.214/GarrukServer/putonmarket.php";
	private string URLRenameCard = "http://54.77.118.214/GarrukServer/renameCard.php";

	int renameCost = 200;
	int cost;
	string price ="";
	float cardHeight;
	Vector3 sellButtonPos;
	bool displaySellPopUp=false;
	bool displayRenamePopUp=false;

	string newTitle;

	// Use this for initialization
	void Start (){
		StartCoroutine (getUserMoney ());
	}

	void OnGUI() {
	
		// Deal button
		if (GUI.Button(new Rect(10, 10, 100, 20), "Accueil"))
		{
			Application.LoadLevel("HomePage");
		}

		if (GUI.Button(new Rect(10, 40, 150, 20), "Me créer une carte"))
		{
			displaySellPopUp=false;
			displayRenamePopUp=false;
			StartCoroutine(generateRandomCard());
		}

		if (instance!=null){
			if(!instance.transform.FindChild ("texturedGameCard").animation.IsPlaying("flipCard")){

				Vector3 cardTopPosition = new Vector3(0,instance.transform.FindChild ("texturedGameCard").renderer.bounds.max.y,instance.transform.FindChild ("texturedGameCard").renderer.bounds.min.z);
				cardTopPosition = camera.WorldToScreenPoint(cardTopPosition);

				Vector3 cardbottomPosition = new Vector3(0,instance.transform.FindChild ("texturedGameCard").renderer.bounds.min.y,instance.transform.FindChild ("texturedGameCard").renderer.bounds.min.z);
				cardbottomPosition = camera.WorldToScreenPoint(cardbottomPosition);

				cardHeight=cardTopPosition.y-cardbottomPosition.y;

				if (GUI.Button (new Rect(Screen.width/2-325,Screen.height/2+10+cardHeight/2, 210,20), "Vendre pour "+cost+" crédits"))
				{

					StartCoroutine(sellCard());							
					Destroy(GameObject.Find("Card"));

				}

				if (GUI.Button (new Rect(Screen.width/2-105,Screen.height/2+10+cardHeight/2, 210,20), "Vendre sur le marché"))
				{
					displaySellPopUp = true;
				}
				if (GUI.Button (new Rect(Screen.width/2+115,Screen.height/2+10+cardHeight/2, 210,20), "Changer le nom pour "+renameCost+" crédits"))
				{
					newTitle=instance.GetComponent<GameCard>().Card.Title;
					displayRenamePopUp = true;
				}
			
			}
			if (displaySellPopUp){
				windowRect = GUI.Window(0, new Rect(Screen.width/2-100, Screen.height/2-50, 200, 110), SellWindow, "Vendre sur le marché");
			}
			if (displayRenamePopUp){
				windowRect = GUI.Window(0, new Rect(Screen.width/2-100, Screen.height/2-50, 200, 110), RenameWindow, "Changer le nom");
			}
		}
	}



	void SellWindow(int windowID) {

		GUI.Label (new Rect(10,20,180,20),"Votre prix de vente :");
		GUI.SetNextControlName("Price");
		price = GUI.TextField(new Rect(10,50,180,20),price, 14);
		GUI.FocusControl("Price");
		if (GUI.Button(new Rect(10,80,80,20), "Annuler"))
		    displaySellPopUp=false;
		if (GUI.Button(new Rect(110,80,80,20), "Confirmer")){  

			StartCoroutine(putOnMarket());
			displaySellPopUp = false;
			Destroy(GameObject.Find("Card"));
			price="";

		}
		
	}

	void RenameWindow(int windowID) {
		
		GUI.Label (new Rect(10,20,180,20),"Nouveau nom ?");
		GUI.SetNextControlName("Title");
		newTitle = GUI.TextField(new Rect(10,50,180,20),newTitle, 20);
		GUI.FocusControl("Title");
		if (GUI.Button(new Rect(10,80,80,20), "Annuler"))
			displayRenamePopUp=false;
		if (GUI.Button(new Rect(110,80,80,20), "Confirmer")){  
			StartCoroutine(renameCard());
			displayRenamePopUp = false;
		}
		
	}


	private IEnumerator getUserMoney(){
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		
		WWW w = new WWW(URLGetMoneyByUser, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			print (w.error); 	
		else 
		{
			print(w.text); 											// donne le retour
			ApplicationModel.credits = System.Convert.ToInt32(w.text);
		}
	}



	private IEnumerator sellCard(){

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", instance.GetComponent<GameCard>().Card.Id);
		form.AddField("myform_cost", cost);
		
		WWW w = new WWW(URLSellRandomCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			print (w.error); 
		else 
		{
			print(w.text); 											// donne le retour
			ApplicationModel.credits = System.Convert.ToInt32(w.text);
		}

	}

	private IEnumerator renameCard(){
		
		newTitle = newTitle.Replace("\r\n", "");
		newTitle=System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(newTitle.ToLower());

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", instance.GetComponent<GameCard>().Card.Id);
		form.AddField("myform_title", newTitle);
		form.AddField("myform_cost", renameCost.ToString());
		
		WWW w = new WWW(URLRenameCard, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			print (w.error); 
		else 
		{
			print(w.text); 											// donne le retour
			ApplicationModel.credits = System.Convert.ToInt32(w.text);
			instance.GetComponent<GameCard>().Card.Title=newTitle;
			instance.GetComponent<GameCard>().ShowFace();

		}
		
	}


	private IEnumerator putOnMarket(){

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField("myform_idcard", instance.GetComponent<GameCard>().Card.Id);
		form.AddField("myform_price", price);
		form.AddField("myform_date",  System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss").ToString());
		
		WWW w = new WWW(URLPutOnMarket, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
			print (w.error); 
		else 
		{
			print(w.text); 											// donne le retour
		}
		
	}

	
	private IEnumerator generateRandomCard(){

		if (isExistingCard) {
			isExistingCard = false;
			Destroy(GameObject.Find("Card"));

		}
		isExistingCard = true;

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		
		WWW w = new WWW(URLBuyRandomCard, form); 				// On envoie le formulaire à l'url sur le serveur 
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
			                              cardInformation [6])); 

		}

		instance = Instantiate(CardObject) as GameObject;
		Destroy(instance.GetComponent<GameNetworkCard>());
		Destroy(instance.GetComponent<PhotonView>());
		instance.transform.localScale = new Vector3(1f, 1f, 1f);               					 // On change ses attributs d'échelle ...                                                                    
		instance.transform.localPosition = new Vector3(0,0,0);                					// ..., de positionnement ...
		instance.GetComponent<GameCard>().Card = myCard;        					// ... et la carte qu'elle représente
		instance.GetComponent<GameCard>().ShowFace();        					// On affiche la carte
		instance.transform.FindChild("texturedGameCard").animation.Play ("flipCard");
		instance.gameObject.name = "Card";
		cost = cardCost (myCard);


	}


	public int cardCost(Card Card){

		int cost;

		cost = Mathf.RoundToInt (Card.Speed +
		Card.Attack +
		Card.Move * 10 +
		Card.Life +
		Card.Skills [0].Power * (1/Card.Skills [0].ManaCost) +
		Card.Skills [1].Power * (1/Card.Skills [1].ManaCost));

		return cost;
		}
}
