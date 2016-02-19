using UnityEngine;
using TMPro;

public class NewSkillBookSkillController : MonoBehaviour 
{
	public Skill s;
	public Sprite[] skillsPictos;
	
	private GameObject picto;
	private GameObject cardType;
	private GameObject title;
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

		Vector3 pictoPosition = new Vector3 (-2.55f, 0f, 0f);
		pictoPosition.x = pictoPosition.x - (worldIncrease / 2f)*(1/skillScale);
		this.picto.transform.localPosition = pictoPosition;
		Vector3 cardTypePosition = new Vector3 (2.55f, 0f, 0f);
		cardTypePosition.x = cardTypePosition.x + (worldIncrease / 2f)*(1/skillScale);
		this.cardType.transform.localPosition = cardTypePosition;
		Vector3 titlePosition = new Vector3 (-2.08f, 0.22f, 0f);
		titlePosition.x = titlePosition.x - (worldIncrease / 2f)*(1/skillScale);
		this.title.transform.localPosition = titlePosition;
		Vector3 levelPosition = new Vector3 (-2.08f, -0.15f, 0f);
		levelPosition.x = levelPosition.x - (worldIncrease / 2f)*(1/skillScale);
		this.level.transform.localPosition = levelPosition;
	}
	public void setSkill(Skill s, bool isPassive)
	{
		
	}
	public void setId(int id)
	{
		this.id = id;
	}
	public void show()
	{
		this.title.GetComponent<TextMeshPro> ().text = WordingSkills.getName(s.Id);
		//this.picto.GetComponent<SpriteRenderer> ().sprite = MenuController.instance.returnSkillPicture (s.IdPicture);
		if(s.Power==0)
		{
			this.level.transform.GetComponent<TextMeshPro>().text=WordingSkillBook.getReference(14);
			this.contour1.SetActive(true);
			this.contour1.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
			this.contour0.SetActive(true);
			this.background.SetActive(false);
		}
		else
		{
			this.background.SetActive(true);
			this.background.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
			this.contour1.SetActive(false);
			this.contour0.SetActive(false);
			this.level.transform.GetComponent<TextMeshPro>().text=WordingSkillBook.getReference(15)+(s.Power).ToString()+WordingSkillBook.getReference(16);
		}
		int level=1;
		if(s.Power>7)
		{
			level=3;
		}
		else if(s.Power>4)
		{
			level=2;
		}
		else
		{
			level=1;
		}
		if(!s.IsActiveSkill)
		{
			this.cardType.SetActive(false);
			this.picto.GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnCardTypePicto(s.CardType.getPictureId(),level);
			this.picto.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.returnCardColor(1);
			this.gameObject.transform.FindChild("Background").GetComponent<SpriteRenderer>().color=new Color(0f,0f,0f);
		}
		else
		{
			this.cardType.SetActive(true);
			this.cardType.GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnCardTypePicto(s.CardType.getPictureId(),1);
			this.picto.GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnSkillPicto(this.s.getPictureId());
			this.picto.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.returnCardColor(level);
			this.gameObject.transform.FindChild("Background").GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
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
				if(this.s.IsActiveSkill)
				{
					this.background.GetComponent<SpriteRenderer>().color=ApplicationDesignRules.whiteSpriteColor;
				}
				else
				{
					this.background.GetComponent<SpriteRenderer>().color=new Color(0f,0f,0f);
				}
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

