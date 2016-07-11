using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pack 
{
	public int Id;
	public int NbCards;
	public int CardType;
	public int Price;
	public bool OnHomePage;
	public bool New;
	public string Picture;
	public string Name;
	public Cards Cards;
	public int IdPicture;
    public string Error;

    private string URLBuyPack = ApplicationModel.host + "buyPack.php";
	
	public Pack()
	{
	}
    public IEnumerator buy(int cardtype, bool isTutorialPack, bool isTrainingPack)
    {
        string isTutorialPackToString = "0";
        if(isTutorialPack)
        {
            isTutorialPackToString="1";
        }
        string isTrainingPackToString = "0";
        if(isTrainingPack)
        {
            isTrainingPackToString="1";
        }

        this.Cards = new Cards ();
        ApplicationModel.player.NewSkills = new List<int>();
        ApplicationModel.player.CollectionPointsEarned = -1;
        this.Error = "";
        
        WWWForm form = new WWWForm();                                           // Création de la connexion
        form.AddField("myform_hash", ApplicationModel.hash);                    // hashcode de sécurité, doit etre identique à celui sur le serveur
        form.AddField("myform_nick", ApplicationModel.player.Username);
        form.AddField("myform_Id", this.Id);    
        form.AddField("myform_cardtype", cardtype.ToString());  
        form.AddField("myform_istutorialpack", isTutorialPackToString);
        form.AddField("myform_istrainingpack", isTrainingPackToString);

        ServerController.instance.setRequest(URLBuyPack, form);
        yield return ServerController.instance.StartCoroutine("executeRequest");
        this.Error=ServerController.instance.getError();

        if(this.Error=="")
        {
            string result = ServerController.instance.getResult();
            string[] data = result.Split(new string[] { "END" }, System.StringSplitOptions.None);
            this.Cards.parseCards(data[1]);
            ApplicationModel.player.updateMyCollection(this.Cards);
            ApplicationModel.player.MyCards.addCards(this.Cards);
        }
    }
}



