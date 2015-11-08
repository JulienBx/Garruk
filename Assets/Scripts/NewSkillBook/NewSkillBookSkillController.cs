using UnityEngine;
using TMPro;

public class NewSkillBookSkillController : MonoBehaviour 
{
	public Skill s;
	public Sprite[] backgrounds;
	public Sprite[] startColorLevel;
	public Sprite[] startHoverColorLevel;
	public Sprite[] hoverColorLevel;
	public Sprite[] colorLevel;


	private int selectedPower;
	
	private GameObject picto;
	private GameObject cardType;
	private GameObject description;
	private GameObject title;
	private GameObject skillType;
	private GameObject proba;
	private GameObject level;
	private GameObject background;
	private GameObject[] levels;
	private GameObject[] levelsTitles;

	public void initialize()
	{
		this.selectedPower = 0;
		this.picto = gameObject.transform.FindChild ("Picto").gameObject;
		this.cardType = gameObject.transform.FindChild("CardType").gameObject;
		this.description = gameObject.transform.FindChild ("Description").gameObject;
		this.title = gameObject.transform.FindChild ("Title").gameObject;
		this.skillType = gameObject.transform.FindChild ("SkillType").gameObject;
		this.proba = gameObject.transform.FindChild ("Proba").gameObject;
		this.level = gameObject.transform.FindChild ("level").gameObject;
		this.background = gameObject.transform.FindChild ("Background").gameObject;
		this.levels = new GameObject[10];
		for(int i=0;i<this.levels.Length;i++)
		{
			this.levels[i]=gameObject.transform.FindChild("level"+i).gameObject;
			this.levels[i].AddComponent<NewSkillBookSkillPowerSelectionButtonController>();
			this.levels[i].GetComponent<NewSkillBookSkillPowerSelectionButtonController>().setId(i);
		}
		this.levelsTitles = new GameObject[10];
		for(int i=0;i<this.levelsTitles.Length;i++)
		{
			this.levelsTitles[i]=gameObject.transform.FindChild("text"+i).gameObject;
			this.levelsTitles[i].GetComponent<TextMeshPro>().text=(i+1).ToString();
		}
	}

	public void resize(float worldWidth)
	{
		float skillScale = 0.53f;
		float skillBackgroundWidth = 1600f;
		float originalWorldWidth = skillScale*(skillBackgroundWidth / ApplicationDesignRules.pixelPerUnit);
		float scale = (worldWidth /originalWorldWidth);
		float worldIncrease = worldWidth-originalWorldWidth;
		print (worldIncrease);

		this.background.transform.localScale =new Vector3(scale, 1f, 1f);

		Vector3 pictoPosition = new Vector3 (-6.21f, 0.31f, 0f);
		pictoPosition.x = pictoPosition.x - (worldIncrease / 2f)*(1/skillScale);
		this.picto.transform.localPosition = pictoPosition;
		Vector3 cardTypePosition = new Vector3 (-4.7f, 0.86f, 0f);
		cardTypePosition.x = cardTypePosition.x - (worldIncrease / 2f)*(1/skillScale);
		this.cardType.transform.localPosition = cardTypePosition;
		Vector3 descriptionPosition = new Vector3 (-5.26f, -0.35f, 0f);
		descriptionPosition.x = descriptionPosition.x - (worldIncrease / 2f)*(1/skillScale);
		this.description.transform.localPosition = descriptionPosition;
		Vector3 titlePosition = new Vector3 (-2.61f, 0.86f, 0f);
		titlePosition.x = titlePosition.x - (worldIncrease / 2f)*(1/skillScale);
		this.title.transform.localPosition = titlePosition;
		Vector3 skillTypePosition = new Vector3 (-3.44f, 0.86f, 0f);
		skillTypePosition.x = skillTypePosition.x -(worldIncrease / 2f)*(1/skillScale);
		this.skillType.transform.localPosition = skillTypePosition;
		Vector3 probaPosition = new Vector3 (6.46f, 0f, 0f);
		probaPosition.x = probaPosition.x + (worldIncrease / 2f)*(1/skillScale);
		this.proba.transform.localPosition = probaPosition;
		Vector3 levelPosition = new Vector3 (-7.02f, -1.15f, 0f);
		levelPosition.x = levelPosition.x - (worldIncrease / 2f)*(1/skillScale);
		this.level.transform.localPosition = levelPosition;

		this.description.transform.GetComponent<TextContainer> ().width = worldWidth - 2f * (worldWidth / 2f + descriptionPosition.x);

		float levelsSpace = worldWidth - 2f * (worldWidth / 2f + levelPosition.x) - this.level.GetComponent<TextContainer> ().width;
		float origin = -worldWidth / 2f + this.level.GetComponent<TextContainer> ().width + (worldWidth / 2f + levelPosition.x)+0.1f;

		float gapBetweenLevels = -0.15f;
		float levelWidth = 118f;
		float levelWorldWidth = (levelsSpace - 9f * gapBetweenLevels) / 10f;
		float distanceBetweenLevels = gapBetweenLevels + levelWorldWidth;
		float levelScale = (levelWorldWidth * ApplicationDesignRules.pixelPerUnit) / levelWidth;

		for(int i=0;i<this.levels.Length;i++)
		{
			this.levels[i].transform.localScale=new Vector3(levelScale,1f,1f);
			this.levels[i].transform.localPosition=new Vector3(origin+levelWorldWidth/2f+i*(distanceBetweenLevels),-1.15f,0f);
			Vector2 ColliderSize = this.levels[i].GetComponent<BoxCollider2D>().size;
			ColliderSize.x=levelWorldWidth*0.7f;
			this.levels[i].GetComponent<BoxCollider2D>().size=ColliderSize;
		}
		for(int i=0;i<this.levelsTitles.Length;i++)
		{
			this.levelsTitles[i].transform.position=this.levels[i].transform.position;
		}
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
		for(int i=0;i<this.levels.Length;i++)
		{
			if(i==selectedPower)
			{
				this.applyHoverColorLevel(i);
			}
			else
			{
				this.applyColorLevel(i);
			}
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
	public void selectPowerHandler(int id)
	{
		this.applyColorLevel (this.selectedPower);
		this.selectedPower = id;
		this.showDescription ();
	}
	public void startHoverPowerHandler(int id)
	{
		if(this.selectedPower!=id)
		{
			this.applyHoverColorLevel(id);
		}
	}
	public void endHoverPowerHandler(int id)
	{
		if (this.selectedPower != id) 
		{
			this.applyColorLevel (id);
		}
	}
	private void applyColorLevel(int id)
	{
		if(id==0)
		{
			this.levels [0].transform.GetComponent<SpriteRenderer> ().sprite=this.startColorLevel [s.Level];
		}
		else
		{
			if(id<s.Power)
			{
				this.levels[id].transform.GetComponent<SpriteRenderer>().sprite=this.colorLevel[s.Level];
			}
			else
			{
				this.levels[id].transform.GetComponent<SpriteRenderer>().sprite=this.colorLevel[0];
			}
		}
	}
	private void applyHoverColorLevel(int id)
	{
		if(id==0)
		{
			this.levels [0].transform.GetComponent<SpriteRenderer> ().sprite=this.startHoverColorLevel [s.Level];
		}
		else
		{
			if(id<this.s.Power)
			{
				this.levels[id].transform.GetComponent<SpriteRenderer>().sprite=this.hoverColorLevel[s.Level];
			}
			else
			{
				this.levels[id].transform.GetComponent<SpriteRenderer>().sprite=this.hoverColorLevel[0];
			}
		}
	}
}

