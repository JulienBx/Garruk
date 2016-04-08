using UnityEngine;
using TMPro;

public class NewCardController : NewFocusedCardController 
{
	private NewCardRessources cardRessources;
	private bool isCardSkillPopUpDisplayed;
	private int skillDisplayed;

	private int layerVariation;
	private string layerName;

	public override void Update()
	{
		base.Update ();
	}
	public override void Awake()
	{
		base.Awake ();
		this.skillDisplayed = -1;
	}
	public override void getCardsComponents()
	{
		this.experience = this.gameObject.transform.FindChild ("Experience").gameObject;
		for(int i=0;i<this.skills.Length;i++)
		{
			this.skills[i]=this.gameObject.transform.FindChild("Skill"+i).gameObject;
		}
		this.attack = this.gameObject.transform.FindChild ("Attack").gameObject;
		this.life = this.gameObject.transform.FindChild ("Life").gameObject;
		this.face = this.gameObject.transform.FindChild ("Face").gameObject;
		this.caracter = this.gameObject.transform.FindChild ("Caracter").gameObject;
		this.cardType = this.gameObject.transform.FindChild("CardType").gameObject;
		this.card = this.gameObject;
	}
	public override void getRessources()
	{
		this.cardRessources = this.gameObject.GetComponent<NewCardRessources> ();
	}
	public override void show()
	{
		this.applyFrontTexture ();
		this.life.transform.FindChild("Text").GetComponent<TextMeshPro>().text = this.c.GetLifeString();
		this.life.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.returnCardColor (this.c.LifeLevel);
		this.attack.transform.FindChild("Text").GetComponent<TextMeshPro>().text = this.c.GetAttackString();
		this.attack.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().color = ApplicationDesignRules.returnCardColor (this.c.AttackLevel);
		this.cardType.transform.GetComponent<NewCardCardTypeController>().setCardType(this.c.CardType);

		for(int i=0;i<this.skills.Length;i++)
		{
			this.skills[i].transform.GetComponent<NewCardSkillController>().setId(i);
			if(i<this.c.Skills.Count && this.c.Skills[i].IsActivated==1)
			{
				this.skills[i].transform.GetComponent<NewCardSkillController>().setSkill(this.c.Skills[i]);
				this.skills[i].transform.GetComponent<NewCardSkillController>().setDescription(this.c.getSkillText(WordingSkills.getDescription(this.c.Skills[i].Id,this.c.Skills[i].Power-1)));
				this.skills[i].SetActive(true);
			}
			else
			{
				this.skills[i].SetActive(false);
			}
		}
	}
	public override void setExperience()
	{
		this.experience.GetComponent<NewCardExperienceController> ().setExperience (this.c.ExperienceLevel, this.c.PercentageToNextLevel);
	}
	public override void applyFrontTexture()
	{
		this.caracter.GetComponent<SpriteRenderer> ().sprite = BackOfficeController.instance.returnSmallCardsCaracter(this.c.Skills[0].getPictureId());
		this.face.GetComponent<SpriteRenderer> ().sprite = cardRessources.faces [this.c.PowerLevel-1];
	}
	public override void displayPanelSold()
	{
		this.panelSold.SetActive (true);
	}
	public override void hidePanelSold()
	{
		this.panelSold.SetActive (false);
	}
	public void changeLayer(int layerVariation, string layerName)
	{

		this.face.GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.face.GetComponent<SpriteRenderer> ().sortingLayerName = layerName;

		int sortingLayerID = this.gameObject.transform.FindChild ("Face").GetComponent<SpriteRenderer> ().sortingLayerID;

		this.caracter.GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.caracter.GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerID;
		this.card.transform.FindChild("caracteristicsBackground").GetComponent<SpriteRenderer>().sortingOrder+= layerVariation;
		this.card.transform.FindChild("caracteristicsBackground").GetComponent<SpriteRenderer>().sortingLayerID=sortingLayerID;
		this.life.transform.FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.life.transform.FindChild("Text").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		this.life.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.life.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerID;
		this.attack.transform.FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.attack.transform.FindChild("Text").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		this.attack.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.attack.transform.FindChild("Picto").GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerID;
		this.experience.transform.FindChild ("ExperienceGauge").GetComponent<SpriteRenderer>().sortingOrder += layerVariation;
		this.experience.transform.FindChild ("ExperienceGauge").GetComponent<SpriteRenderer>().sortingLayerID = sortingLayerID;
		this.cardUpgrade.transform.FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.cardUpgrade.transform.FindChild("Text").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		this.cardUpgrade.GetComponent<SpriteRenderer>().sortingOrder += layerVariation;
		this.cardUpgrade.GetComponent<SpriteRenderer>().sortingLayerID = sortingLayerID;
		this.cardType.GetComponent<SpriteRenderer>().sortingOrder+=layerVariation;
		this.cardType.GetComponent<SpriteRenderer>().sortingLayerID=sortingLayerID;

		for(int i=0;i<this.skills.Length;i++)
		{
			this.skills[i].transform.GetComponent<NewCardSkillController>().changeLayer(layerVariation,sortingLayerID);
		}
	} 
	public override void applyBackTexture()
	{
		this.face.GetComponent<SpriteRenderer> ().sprite = cardRessources.backFace;
	}
	public override void exitCard ()
	{
	}
	public override void animateExperience()
	{
		this.experience.GetComponent<NewCardExperienceController>().startUpdatingXp(this.c.ExperienceLevel,this.c.PercentageToNextLevel);
	}
	public override void endUpdatingXp(bool hasChangedLevel)
	{
		this.show ();
		if(this.c.GetNewSkill)
		{
			base.setHighlightedSkills();
		}
	}
	public override void endUpdatingCardToNextLevel()
	{
		if(this.c.GetNewSkill)
		{
			this.c.GetNewSkill=false;
		}
		if(this.caracteristicUpgraded>-1&&this.caracteristicIncrease>0)
		{
			base.setCardUpgrade();
		}
	}
	public override Vector3 getCardUpgradePosition (int caracteristicUpgraded)
	{
		GameObject refObject = new GameObject ();
		float refSizeX=0f;
		float gap = 0.165f;
		switch(caracteristicUpgraded)
		{
		case 0:
			refObject = this.attack.transform.FindChild("Text").gameObject;
			refSizeX = refObject.transform.GetComponent<MeshRenderer> ().bounds.size.x;
			break;
		case 1:
			refObject=this.life.transform.FindChild("Text").gameObject.gameObject;
			refSizeX = refObject.transform.GetComponent<MeshRenderer> ().bounds.size.x;
			break;
		case 3:
			refObject=this.skills[0].gameObject;
			refSizeX = refObject.transform.GetComponent<SpriteRenderer> ().bounds.size.x;
			break;
		case 4:
			refObject=this.skills[1].gameObject;
			refSizeX = refObject.transform.GetComponent<SpriteRenderer> ().bounds.size.x;
			break;
		case 5:
			refObject=this.skills[2].gameObject;
			refSizeX = refObject.transform.GetComponent<SpriteRenderer> ().bounds.size.x;
			break;
		case 6:
			refObject=this.skills[3].gameObject;
			refSizeX = refObject.transform.GetComponent<SpriteRenderer> ().bounds.size.x;
			break;
		}
		Vector3 refPosition =refObject.transform.position;
		return new Vector3 (refPosition.x+gap+refSizeX/2f,refPosition.y,0f);
	}
	new public virtual Sprite getSkillSprite(int id)
	{
		return this.cardRessources.skills [id];
	}
	public virtual Camera getCurrentCamera()
	{
		return new Camera();
	}
	public virtual void OnMouseOver()
	{
	}
	public virtual void OnMouseExit()
	{
	}

	#region Help Functions

	public Vector3 getCardTypePosition()
	{
		return this.cardType.transform.position;
	}
	public Vector3 getLifePosition()
	{
		return this.life.transform.FindChild("Text").position;
	}
	public Vector3 getSkillsPosition()
	{
		Vector3 skillsPosition = new Vector3();
		skillsPosition.x = this.skills[1].transform.position.x;
		skillsPosition.y = (this.skills[1].transform.position.y +this.skills[0].transform.position.y)/2;
		return skillsPosition;
	}

	#endregion


}

