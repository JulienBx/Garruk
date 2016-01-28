using UnityEngine;
using TMPro;

public class NewSkillBookSkillController : MonoBehaviour 
{
	public Skill s;
	public Sprite[] skillsPictos;
	
	private GameObject picto;
	private GameObject cardType;
	private GameObject title;
	private GameObject skillType;
	private GameObject background;
	private GameObject contour0;
	private GameObject contour1;
	private GameObject level;

	private bool isHovering;
	private int id;

	void Awake()
	{
		this.isHovering = false;
	}
	public void initialize()
	{
		this.background = gameObject.transform.FindChild ("Background").gameObject;
		this.picto = gameObject.transform.FindChild ("Picto").gameObject;
		this.cardType = gameObject.transform.FindChild("CardType").gameObject;
		this.title = gameObject.transform.FindChild ("Title").gameObject;
		this.skillType = gameObject.transform.FindChild ("SkillType").gameObject;
		this.level = gameObject.transform.FindChild ("Level").gameObject;
		this.contour0 = gameObject.transform.FindChild ("Contour0").gameObject;
		this.contour1 = gameObject.transform.FindChild ("Contour1").gameObject;
	}
	public void resize(float worldWidth)
	{
		float skillScale = 0.9f;
		float skillBackgroundWidth = 671f;
		float originalWorldWidth = skillScale*(skillBackgroundWidth / ApplicationDesignRules.pixelPerUnit);
		float scale = ((worldWidth-0.05f) /originalWorldWidth);
		float worldIncrease = worldWidth-originalWorldWidth;

		this.background.transform.localScale =new Vector3(scale, 1f, 1f);

		float contour0WorldWidth = worldWidth;
		float contour0Width = 338f;
		float contour0OriginalWorldWidth = skillScale*(contour0Width / ApplicationDesignRules.pixelPerUnit);
		float contour0Scale = contour0WorldWidth / contour0OriginalWorldWidth;

		this.contour0.transform.localScale = new Vector3 (contour0Scale, 1.9f, 1f);

		float contour1WorldWidth = worldWidth;
		float contour1Width = 334f;
		float contour1OriginalWorldWidth = skillScale*(contour1Width / ApplicationDesignRules.pixelPerUnit);
		float contour1Scale = contour1WorldWidth / contour1OriginalWorldWidth;
		
		this.contour1.transform.localScale = new Vector3 (contour1Scale, 1.9f, 1f);

		Vector3 pictoPosition = new Vector3 (-2.56f, 0f, 0f);
		pictoPosition.x = pictoPosition.x - (worldIncrease / 2f)*(1/skillScale);
		this.picto.transform.localPosition = pictoPosition;
		Vector3 cardTypePosition = new Vector3 (1.85f, 0f, 0f);
		cardTypePosition.x = cardTypePosition.x + (worldIncrease / 2f)*(1/skillScale);
		this.cardType.transform.localPosition = cardTypePosition;
		Vector3 titlePosition = new Vector3 (-2.08f, 0.22f, 0f);
		titlePosition.x = titlePosition.x - (worldIncrease / 2f)*(1/skillScale);
		this.title.transform.localPosition = titlePosition;
		Vector3 skillTypePosition = new Vector3 (2.64f, 0f, 0f);
		skillTypePosition.x = skillTypePosition.x +(worldIncrease / 2f)*(1/skillScale);
		this.skillType.transform.localPosition = skillTypePosition;
		Vector3 levelPosition = new Vector3 (-2.08f, -0.15f, 0f);
		levelPosition.x = levelPosition.x - (worldIncrease / 2f)*(1/skillScale);
		this.level.transform.localPosition = levelPosition;
	}
	public void setId(int id)
	{
		this.id = id;
	}
	public void show()
	{
		this.title.GetComponent<TextMeshPro> ().text = WordingSkills.getName(s.Id);
		this.cardType.GetComponent<SpriteRenderer> ().sprite = MenuController.instance.returnCardTypePicture (s.IdCardType);
		this.skillType.GetComponent<SpriteRenderer> ().sprite = MenuController.instance.returnSkillTypePicture (s.IdSkillType);
		this.skillType.transform.FindChild ("Title").GetComponent<TextMeshPro> ().text = WordingSkillTypes.getName(s.IdSkillType).Substring (0, 1).ToUpper();
		//this.picto.GetComponent<SpriteRenderer> ().sprite = MenuController.instance.returnSkillPicture (s.IdPicture);
		if(s.Power==0)
		{
			this.level.transform.GetComponent<TextMeshPro>().text="Non acquise";
			//this.contour0.transform.GetComponent<SpriteRenderer>().color=new Color(0f,0f,0f);
			this.contour1.SetActive(true);
			this.contour1.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
			this.contour0.SetActive(true);
			//this.picto.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.whiteSpriteColor;
			this.picto.GetComponent<SpriteRenderer>().sprite=this.skillsPictos[0];
			this.background.SetActive(false);
		}
		else
		{
			this.background.SetActive(true);
			this.background.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
			this.contour1.SetActive(false);
			this.contour0.SetActive(false);
			this.level.transform.GetComponent<TextMeshPro>().text="Acquise (niveau "+(s.Power).ToString()+")";
			if(s.Power>7)
			{
				//this.contour0.transform.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.redColor;
				//this.picto.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.redColor;
				this.picto.GetComponent<SpriteRenderer>().sprite=this.skillsPictos[2];
			}
			else if(s.Power>4)
			{
				//this.contour0.transform.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
				//this.picto.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.blueColor;
				this.picto.GetComponent<SpriteRenderer>().sprite=this.skillsPictos[1];
			}
			else
			{
				//this.contour0.transform.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.greySpriteColor;
				//this.picto.GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.greySpriteColor;
				this.picto.GetComponent<SpriteRenderer>().sprite=this.skillsPictos[0];
			}
		}
	}
	void OnMouseOver()
	{
		if(!this.isHovering && !ApplicationDesignRules.isMobileScreen)
		{
			this.isHovering=true;
			if(s.Power==0)
			{
				this.contour1.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
			}
			else
			{
				this.background.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.blueColor;
			}
		}
	}
	void OnMouseExit()
	{
		if(this.isHovering && !ApplicationDesignRules.isMobileScreen)
		{
			this.isHovering=false;
			if(s.Power==0)
			{
				this.contour1.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
			}
			else
			{
				this.background.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
			}
		}
	}
	void OnMouseDown()
	{
		this.isHovering=false;
		NewSkillBookController.instance.leftClickHandler ();
	}
	void OnMouseUp()
	{
		NewSkillBookController.instance.leftClickReleaseHandler (this.id);
	}
}

