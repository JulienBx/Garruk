using UnityEngine;

public class NewCardSkillController : MonoBehaviour 
{

	public Sprite[] skillsPicto;
	private Skill s;
	public Color[] colors;

	private void Awake()
	{
		this.initializeSkill ();
	}
	private void show()
	{
		//this.gameObject.transform.FindChild ("Picto").GetComponent<SpriteRenderer> ().sprite = this.skillsPicto [0];
		this.gameObject.transform.FindChild ("Name").GetComponent<TextMesh> ().text = this.s.Name;
		this.gameObject.transform.FindChild ("Power").GetComponent<TextMesh> ().text = this.s.Power.ToString();
		this.gameObject.transform.FindChild ("Power").GetComponent<TextMesh> ().color = this.colors [this.s.Level - 1];
		if(s.Name.Length>15)
		{
			float scale = this.gameObject.transform.FindChild ("Name").localScale.x-0.004f*(s.Name.Length-15f);
			this.gameObject.transform.FindChild ("Name").localScale=new Vector3(scale,scale,scale);
		}
	}
	public void setSkill(Skill s)
	{
		this.s = s;
		this.show ();
	}
	private void initializeSkill()
	{
		this.gameObject.transform.FindChild("Name").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		this.gameObject.transform.FindChild("Power").GetComponent<MeshRenderer> ().sortingLayerName = "UI";
		
	}
}

