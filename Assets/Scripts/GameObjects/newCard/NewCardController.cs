using UnityEngine;

public class NewCardController : MonoBehaviour 
{
	
	public GameObject skillObject;
	public Sprite[] faces;
	public Color[] colors;
	private int id;

	private Card c;

	private GameObject[] skills;

	void Awake()
	{
		this.skills=new GameObject[0];
		this.initializeCard ();
	}
	public void setCard(Card c)
	{
		this.c = c;
		this.show ();
	}
	void OnMouseDrag()
	{
		newMyGameController.instance.isDraggingCard ();
		//print ("carte en drag");
	}
	void OnMouseOver()
	{
		newMyGameController.instance.isHoveringCard ();
		print ("enter this" + id);
	}
	void OnMouseExit()
	{
		newMyGameController.instance.endHoveringCard ();
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
		print ("exit this" + id);
	}
	void OnMouseDown()
	{
		newMyGameController.instance.leftClickedHandler (id);
	}
	void OnMouseUp()
	{
		newMyGameController.instance.leftClickReleaseHandler ();
	}
	void show()
	{
		this.gameObject.transform.FindChild ("Face").GetComponent<SpriteRenderer> ().sprite = this.faces[0];
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMesh> ().text = this.c.TitleClass;
		this.gameObject.transform.FindChild("Power").FindChild("Text").GetComponent<TextMesh>().text = this.c.Power.ToString();
		this.gameObject.transform.FindChild ("Power").FindChild ("Text").GetComponent<TextMesh> ().color = this.colors [this.c.PowerLevel - 1];
		this.gameObject.transform.FindChild("Life").FindChild("Text").GetComponent<TextMesh>().text = this.c.Life.ToString();
		this.gameObject.transform.FindChild ("Life").FindChild ("Text").GetComponent<TextMesh> ().color = this.colors [this.c.LifeLevel - 1];
		this.gameObject.transform.FindChild("Move").FindChild("Text").GetComponent<TextMesh>().text = this.c.Move.ToString();
		this.gameObject.transform.FindChild("Attack").FindChild("Text").GetComponent<TextMesh>().text = this.c.Attack.ToString();
		this.gameObject.transform.FindChild ("Attack").FindChild ("Text").GetComponent<TextMesh> ().color = this.colors [this.c.AttackLevel - 1];
		this.gameObject.transform.FindChild("Quickness").FindChild("Text").GetComponent<TextMesh>().text = this.c.Speed.ToString();
		this.gameObject.transform.FindChild ("Quickness").FindChild ("Text").GetComponent<TextMesh> ().color = this.colors [this.c.SpeedLevel - 1];
		this.gameObject.transform.FindChild ("ExperienceGauge").localPosition = new Vector3 (this.c.PercentageToNextLevel * 1.14f, 0.855f, 0);
		this.gameObject.transform.FindChild ("ExperienceGauge").localScale = new Vector3 (this.c.PercentageToNextLevel * (-0.5f), 1.541f, 0);
		this.gameObject.transform.FindChild("ExperienceLevel").GetComponent<TextMesh>().text = this.c.ExperienceLevel.ToString();
		this.cleanSkills ();
		this.skills = new GameObject[this.c.Skills.Count];
		for(int i=0;i<c.Skills.Count;i++)
		{
			this.skills[i]= Instantiate(this.skillObject) as GameObject;
			this.skills[i].transform.parent=this.gameObject.transform;
			this.skills[i].transform.localPosition=new Vector3(-0.4f,-0.5f+i*0.2f,0);
			this.skills[i].transform.GetComponent<NewCardSkillController>().setSkill(c.Skills[i]);
		}
	}
	void initializeCard()
	{
		this.gameObject.transform.FindChild("Name").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.gameObject.transform.FindChild("Power").FindChild("Text").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.gameObject.transform.FindChild("Life").FindChild("Text").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.gameObject.transform.FindChild("Move").FindChild("Text").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.gameObject.transform.FindChild("Attack").FindChild("Text").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.gameObject.transform.FindChild("Quickness").FindChild("Text").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.gameObject.transform.FindChild("ExperienceLevel").GetComponent<MeshRenderer> ().sortingLayerName = "UI";

	}
	void cleanSkills()
	{
		for(int i=0;i<this.skills.Length;i++)
		{
			Destroy (this.skills[i]);
		}
	}
	public void setId(int value)
	{
		this.id = value;
	}
}

