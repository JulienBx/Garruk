using UnityEngine;
using TMPro;

public class NewCardController : NewFocusedCardController 
{
	private NewCardRessources ressources;
	private GameObject[] skills;
	private GameObject panelSold;
	private GameObject skillPopUp;
	private bool isSkillPopUpDisplayed;
	private int skillDisplayed;

	public override void Update()
	{
		base.Update ();
	}
	public override void Awake()
	{
		this.skillDisplayed = -1;
		this.skills=new GameObject[0];
		this.ressources = this.gameObject.GetComponent<NewCardRessources> ();
		base.setPopUpRessources ();
		this.panelSold = this.gameObject.transform.FindChild ("PanelSold").gameObject;
		this.skillPopUp = this.gameObject.transform.FindChild ("SkillPopUp").gameObject;
	}
	public override void show()
	{
		this.applyFrontTexture ();
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().text = this.c.Title;
		this.gameObject.transform.FindChild("Power").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Power.ToString();
		this.gameObject.transform.FindChild ("Power").FindChild ("Text").GetComponent<TextMeshPro> ().color = ressources.colors [this.c.PowerLevel - 1];
		this.gameObject.transform.FindChild("Life").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Life.ToString();
		this.gameObject.transform.FindChild ("Life").FindChild ("Text").GetComponent<TextMeshPro> ().color = ressources.colors [this.c.LifeLevel - 1];
		this.gameObject.transform.FindChild("Move").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Move.ToString();
		this.gameObject.transform.FindChild("Attack").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Attack.ToString();
		this.gameObject.transform.FindChild ("Attack").FindChild ("Text").GetComponent<TextMeshPro> ().color = ressources.colors [this.c.AttackLevel - 1];
		this.gameObject.transform.FindChild("Quickness").FindChild("Text").GetComponent<TextMeshPro>().text = this.c.Speed.ToString();
		this.gameObject.transform.FindChild ("Quickness").FindChild ("Text").GetComponent<TextMeshPro> ().color = ressources.colors [this.c.SpeedLevel - 1];
		this.gameObject.transform.FindChild ("ExperienceGauge").localPosition = new Vector3 (-0.43f+0.43f*this.c.PercentageToNextLevel*0.01f, 0.711f, 0f);
		this.gameObject.transform.FindChild ("ExperienceGauge").localScale = new Vector3 (this.c.PercentageToNextLevel *0.01f* (0.97f), 0.78f, 0f);
		this.gameObject.transform.FindChild("ExperienceLevel").GetComponent<TextMeshPro>().text = this.c.ExperienceLevel.ToString();
		for(int i=0;i<this.skills.Length;i++)
		{
			Destroy (this.skills[i]);
		}
		this.skills = new GameObject[this.c.Skills.Count];
		for(int i=0;i<c.Skills.Count;i++)
		{
			this.skills[i]= Instantiate(ressources.skillObject) as GameObject;
			this.skills[i].transform.parent=this.gameObject.transform;
			this.skills[i].transform.localPosition=new Vector3(-0.4f,-0.3f-i*0.2f,0);
			this.skills[i].transform.GetComponent<NewCardSkillController>().setSkill(c.Skills[i]);
		}
	}
	public override void applyFrontTexture()
	{
		this.gameObject.transform.FindChild ("Face").GetComponent<SpriteRenderer> ().sprite = ressources.faces[this.c.IdClass];
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

		this.gameObject.transform.FindChild ("Face").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild ("Face").GetComponent<SpriteRenderer> ().sortingLayerName = layerName;

		int sortingLayerID = this.gameObject.transform.FindChild ("Face").GetComponent<SpriteRenderer> ().sortingLayerID;

		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMeshPro> ().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild("Power").FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Power").FindChild("Text").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild("Power").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Power").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild("Life").FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Life").FindChild("Text").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild("Life").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Life").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild("Move").FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Move").FindChild("Text").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild("Move").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Move").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild("Attack").FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Attack").FindChild("Text").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild("Attack").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Attack").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild("Quickness").FindChild("Text").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Quickness").FindChild("Text").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild("Quickness").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("Quickness").FindChild("Picto").GetComponent<SpriteRenderer> ().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild ("ExperienceGauge").GetComponent<SpriteRenderer>().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild ("ExperienceGauge").GetComponent<SpriteRenderer>().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild ("ExperienceBar").GetComponent<SpriteRenderer>().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild ("ExperienceBar").GetComponent<SpriteRenderer>().sortingLayerID = sortingLayerID;
		this.gameObject.transform.FindChild("ExperienceLevel").GetComponent<TextMeshPro>().sortingOrder += layerVariation;
		this.gameObject.transform.FindChild("ExperienceLevel").GetComponent<TextMeshPro>().sortingLayerID = sortingLayerID;
		for(int i=0;i<this.skills.Length;i++)
		{
			this.skills[i].transform.GetComponent<NewCardSkillController>().changeLayer(layerVariation,sortingLayerID);
		}
	} 
	public override void applyBackTexture()
	{
		this.gameObject.transform.FindChild ("Face").GetComponent<SpriteRenderer> ().sprite = ressources.backFace;
	}
	public virtual void OnMouseOver()
	{
		if(!Input.GetMouseButton(0))
		{
			int newSkillHovered = this.skillHovered();
			if(newSkillHovered>-1 && newSkillHovered<this.c.Skills.Count)
			{
				if(newSkillHovered!=skillDisplayed)
				{
					this.skillDisplayed=newSkillHovered;
					if(!isSkillPopUpDisplayed)
					{
						this.isSkillPopUpDisplayed=true;
						this.skillPopUp.SetActive(true);
						Vector3 popUpScale = new Vector3(1f/this.gameObject.transform.localScale.x,1f/this.gameObject.transform.localScale.y,1f/this.gameObject.transform.localScale.z);
						this.skillPopUp.transform.localScale=popUpScale;
					}
					this.skillPopUp.transform.position=new Vector3(gameObject.transform.position.x,gameObject.transform.position.y-(skillDisplayed*(0.175f*this.gameObject.transform.localScale.y))+0.25f/this.gameObject.transform.localScale.y+0.45f,-1f);
					this.skillPopUp.transform.FindChild("title").GetComponent<TextMeshPro>().text=c.Skills[skillDisplayed].Name;
					this.skillPopUp.transform.FindChild("description").GetComponent<TextMeshPro>().text=c.Skills[skillDisplayed].Description;
				}
			}
			else
			{
				if(isSkillPopUpDisplayed)
				{
					this.hideSkillPopUp();
				}
			}
		}
		else if(isSkillPopUpDisplayed)
		{
			this.hideSkillPopUp();
		}
	}
	public virtual void OnMouseExit()
	{
		this.hideSkillPopUp ();
	}
	private void hideSkillPopUp()
	{
		if(isSkillPopUpDisplayed)
		{
			this.isSkillPopUpDisplayed=false;
			this.skillPopUp.SetActive(false);
			this.skillDisplayed=-1;
		}
	}
	public int skillHovered()
	{
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
		if(cursorPosition.x>this.gameObject.transform.position.x-(0.35f*this.gameObject.transform.localScale.x) && 
		   cursorPosition.y<this.gameObject.transform.position.y-(0.25f*this.gameObject.transform.localScale.y) &&
		   cursorPosition.y>this.gameObject.transform.position.y-(0.95f*this.gameObject.transform.localScale.y))
		{
			if(cursorPosition.y>this.gameObject.transform.position.y-(0.425f*this.gameObject.transform.localScale.y))
			{
				return 0;
			}
			else if(cursorPosition.y>this.gameObject.transform.position.y-(0.60f*this.gameObject.transform.localScale.y))
			{
				return 1;
			}
			else if(cursorPosition.y>this.gameObject.transform.position.y-(0.775f*this.gameObject.transform.localScale.y))
			{
				return 2;
			}
			else
			{
				return 3;
			}
		}
		else
		{
			return -1;
		}
	}
}

