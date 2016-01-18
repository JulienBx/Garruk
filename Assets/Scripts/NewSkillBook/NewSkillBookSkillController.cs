using UnityEngine;
using TMPro;

public class NewSkillBookSkillController : MonoBehaviour 
{
	public Skill s;
	public Sprite[] backgrounds;


	private int selectedPower;
	
	private GameObject picto;
	private GameObject cardType;
	private GameObject description;
	private GameObject title;
	private GameObject skillType;
	private GameObject proba;
	private GameObject background;
	//private GameObject cardTypeDescription;
	//private GameObject skillTypeDescription;
	private GameObject level;
	private GameObject nextLevelButton;
	private GameObject previousLevelButton;
	private GameObject descriptionBackground;

	public void initialize()
	{
		this.selectedPower = 0;
		this.picto = gameObject.transform.FindChild ("Picto").gameObject;
		this.cardType = gameObject.transform.FindChild("CardType").gameObject;
		this.description = gameObject.transform.FindChild ("Description").gameObject;
		this.title = gameObject.transform.FindChild ("Title").gameObject;
		this.skillType = gameObject.transform.FindChild ("SkillType").gameObject;
		this.proba = gameObject.transform.FindChild ("Proba").gameObject;
		this.level = gameObject.transform.FindChild ("Level").gameObject;
		this.background = gameObject.transform.FindChild ("Background").gameObject;
		this.descriptionBackground = gameObject.transform.FindChild ("DescriptionBackground").gameObject;
		this.nextLevelButton = gameObject.transform.FindChild ("NextLevelButton").gameObject;
		this.previousLevelButton = gameObject.transform.FindChild ("PreviousLevelButton").gameObject;
		//this.cardTypeDescription = gameObject.transform.FindChild ("CardTypeDescription").gameObject;
		//this.skillTypeDescription = gameObject.transform.FindChild ("SkillTypeDescription").gameObject;
	}

	public void resize(float worldWidth)
	{
		float skillScale = 0.75f;
		float skillBackgroundWidth = 931f;
		float originalWorldWidth = skillScale*(skillBackgroundWidth / ApplicationDesignRules.pixelPerUnit);
		float scale = (worldWidth /originalWorldWidth);
		float worldIncrease = worldWidth-originalWorldWidth;

		this.background.transform.localScale =new Vector3(scale, 0.86f, 1f);

		float descriptionBackgroundWorldWidth = worldWidth - 1f;
		float descriptionBackgroundWidth = 1046f;
		float descriptionOriginalWorldWidth = skillScale*(descriptionBackgroundWidth / ApplicationDesignRules.pixelPerUnit);
		float descriptionScale = descriptionBackgroundWorldWidth / descriptionOriginalWorldWidth;

		this.descriptionBackground.transform.localScale = new Vector3 (descriptionScale, 0.7584848f, 0.727804f);

		Vector3 pictoPosition = new Vector3 (-3.7f, 0.99f, 0f);
		pictoPosition.x = pictoPosition.x - (worldIncrease / 2f)*(1/skillScale);
		this.picto.transform.localPosition = pictoPosition;
		Vector3 cardTypePosition = new Vector3 (2.89f, 1f, 0f);
		cardTypePosition.x = cardTypePosition.x + (worldIncrease / 2f)*(1/skillScale);
		this.cardType.transform.localPosition = cardTypePosition;
		Vector3 descriptionPosition = new Vector3 (-3f, -0.18f, 0f);
		descriptionPosition.x = descriptionPosition.x - (worldIncrease / 2f)*(1/skillScale);
		this.description.transform.localPosition = descriptionPosition;
		Vector3 titlePosition = new Vector3 (-2.97f, 1.17f, 0f);
		titlePosition.x = titlePosition.x - (worldIncrease / 2f)*(1/skillScale);
		this.title.transform.localPosition = titlePosition;
		Vector3 skillTypePosition = new Vector3 (3.67f, 0.97f, 0f);
		skillTypePosition.x = skillTypePosition.x +(worldIncrease / 2f)*(1/skillScale);
		this.skillType.transform.localPosition = skillTypePosition;
		Vector3 probaPosition = new Vector3 (3.66f, -0.18f, 0f);
		probaPosition.x = probaPosition.x + (worldIncrease / 2f)*(1/skillScale);
		this.proba.transform.localPosition = probaPosition;
		Vector3 levelPosition = new Vector3 (-2.95f, 0.8f, 0f);
		levelPosition.x = levelPosition.x - (worldIncrease / 2f)*(1/skillScale);
		this.level.transform.localPosition = levelPosition;
		Vector3 nextLevelButtonPosition = new Vector3 (-3.7f, 0.23f, 0f);
		nextLevelButtonPosition.x = nextLevelButtonPosition.x - (worldIncrease / 2f)*(1/skillScale);
		this.nextLevelButton.transform.localPosition = nextLevelButtonPosition;
		Vector3 previousLevelButtonPosition = new Vector3 (-3.72f, -0.54f, 0f);
		previousLevelButtonPosition.x = previousLevelButtonPosition.x - (worldIncrease / 2f)*(1/skillScale);
		this.previousLevelButton.transform.localPosition = previousLevelButtonPosition;
		Vector3 descriptionBackgroundPosition = this.descriptionBackground.transform.position;
		descriptionBackgroundPosition.x = description.transform.position.x - 0.075f + descriptionBackgroundWorldWidth / 2f;
		this.descriptionBackground.transform.position = descriptionBackgroundPosition;

		this.description.transform.GetComponent<TextContainer> ().width = 7.63f + worldIncrease;


	}
	public void show()
	{
		this.title.GetComponent<TextMeshPro> ().text = s.Name.ToUpper();
		this.cardType.GetComponent<SpriteRenderer> ().sprite = MenuController.instance.returnCardTypePicture (s.CardType.IdPicture);
		this.skillType.GetComponent<SpriteRenderer> ().sprite = MenuController.instance.returnSkillTypePicture (s.SkillType.IdPicture);
		this.skillType.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = s.SkillType.Name.Substring (0, 1).ToUpper();
		this.picto.GetComponent<SpriteRenderer> ().sprite = MenuController.instance.returnSkillPicture (s.IdPicture);
		this.background.GetComponent<SpriteRenderer> ().sprite = this.backgrounds [s.Level];
		if(s.Power!=0)
		{
			this.selectedPower = s.Power-1;
		}
		else
		{
			this.selectedPower=0;
		}
		if(s.Level==0)
		{
			this.level.transform.GetComponent<TextMeshPro>().text="Non acquise";
		}
		else
		{
			this.level.transform.GetComponent<TextMeshPro>().text="Acquise (niveau "+(s.Power).ToString()+")";
		}
		this.showDescription ();
	}
	public void showDescription()
	{
		this.description.GetComponent<TextMeshPro> ().text = s.AllDescriptions [this.selectedPower];
		if(s.AllProbas[0]==0)
		{
			this.proba.SetActive(false);
		}
		else
		{
			this.proba.SetActive(true);
			this.proba.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = s.AllProbas[this.selectedPower].ToString();
		}
		this.drawButtons ();
		if(s.AllProbas[this.selectedPower]<50)
		{
			this.proba.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.greyTextColor;
		}
		else if(s.AllProbas[this.selectedPower]<80)
		{
			this.proba.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.blueColor;
		}
		else
		{
			this.proba.transform.FindChild("Title").GetComponent<TextMeshPro>().color=ApplicationDesignRules.redColor;
		}
	}
	public void nextLevelHandler()
	{
		this.selectedPower++;
		this.showDescription ();
	}
	public void previousLevelHandler()
	{
		this.selectedPower--;
		this.showDescription ();
	}
	public void drawButtons()
	{
		if(this.selectedPower<9)
		{
			this.nextLevelButton.SetActive(true);
			this.nextLevelButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Nv " + this.selectedPower.ToString ()+2;
		}
		else
		{
			this.nextLevelButton.SetActive(false);
		}
		if(this.selectedPower>0)
		{
			this.previousLevelButton.SetActive(true);
			this.previousLevelButton.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = "Nv " + this.selectedPower.ToString ();
		}
		else
		{
			this.previousLevelButton.SetActive(false);
		}
	}
}

