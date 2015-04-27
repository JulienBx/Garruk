using UnityEngine ;

public class CardView : MonoBehaviour
{
	public CardViewModel cardVM;

	public CardView ()
	{
		this.cardVM = new CardViewModel();
	}
	public void show()
	{
		transform.Find("texturedGameCard").FindChild("AttackArea").renderer.material.mainTexture = cardVM.attackArea;
		transform.Find ("texturedGameCard").FindChild ("SpeedArea").renderer.material.mainTexture = cardVM.speedArea;
		transform.Find("texturedGameCard").FindChild("MoveArea").renderer.material.mainTexture = cardVM.moveArea;
		for (int i=0;i<6;i++)
		{
			transform.Find("texturedGameCard").renderer.materials[i].mainTexture = cardVM.cardFaces[0]; 
			transform.Find("texturedGameCard").FindChild("PictoMetalLife").renderer.materials[i].mainTexture = cardVM.lifeLevel[i];
			transform.Find("texturedGameCard").FindChild("AttackArea").FindChild("PictoMetalAttack").renderer.materials[i].mainTexture = cardVM.attackLevel[i];
			transform.Find("texturedGameCard").FindChild("MoveArea").FindChild ("PictoMetalMove").renderer.materials[i].mainTexture = cardVM.moveLevel[i];
			transform.Find("texturedGameCard").FindChild("SpeedArea").FindChild("PictoMetalSpeed").renderer.materials[i].mainTexture = cardVM.speedLevel[i];
		}
		transform.Find("texturedGameCard").FindChild("Title").GetComponent<TextMesh>().text = cardVM.title;
		transform.Find("texturedGameCard").FindChild("PictoMetalLife").FindChild("Life").GetComponent<TextMesh> ().text = cardVM.life.ToString();
		transform.Find("texturedGameCard").FindChild("AttackArea").FindChild("PictoMetalAttack").FindChild("Attack").GetComponent<TextMesh> ().text = cardVM.attack.ToString();
		transform.Find("texturedGameCard").FindChild ("MoveArea").FindChild ("PictoMetalMove").FindChild ("Move").GetComponent<TextMesh> ().text = cardVM.move.ToString();
		transform.Find("texturedGameCard").FindChild("SpeedArea").FindChild("PictoMetalSpeed").FindChild("Speed").GetComponent<TextMesh> ().text = cardVM.speed.ToString();
		transform.Find ("texturedGameCard").FindChild ("Class").GetComponent<TextMesh> ().text = cardVM.titleClass;
	}
	public void setTextResolution(float resolution)
	{
		transform.Find("texturedGameCard").Find("Title").GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(resolution * 15);	
		transform.Find("texturedGameCard").Find("Title").localScale = new Vector3(0.07f/resolution,0.054f/resolution,0);
		transform.Find("texturedGameCard").FindChild("PictoMetalLife").FindChild("Life").GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(resolution * 17);	
		transform.Find("texturedGameCard").FindChild("PictoMetalLife").FindChild("Life").localScale = new Vector3(0.3f/resolution,0.6f/resolution,0);
		transform.Find("texturedGameCard").FindChild("AttackArea").FindChild("PictoMetalAttack").FindChild("Attack").GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(resolution * 15);	
		transform.Find("texturedGameCard").FindChild("AttackArea").FindChild("PictoMetalAttack").FindChild("Attack").localScale = new Vector3(0.5f/resolution,0.7f/resolution,0);
		transform.Find("texturedGameCard").FindChild("MoveArea").FindChild("PictoMetalMove").FindChild("Move").GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(resolution * 15);	
		transform.Find("texturedGameCard").FindChild("MoveArea").FindChild("PictoMetalMove").FindChild("Move").localScale = new Vector3(0.5f/resolution,0.7f/resolution,0);
		transform.Find("texturedGameCard").FindChild("SpeedArea").FindChild("PictoMetalSpeed").FindChild("Speed").GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(resolution * 15);	
		transform.Find("texturedGameCard").FindChild("SpeedArea").FindChild("PictoMetalSpeed").FindChild("Speed").localScale = new Vector3(0.5f/resolution,0.7f/resolution,0);
		transform.Find("texturedGameCard").Find("Class").GetComponent<TextMesh>().fontSize = Mathf.RoundToInt(resolution * 15);	
		transform.Find("texturedGameCard").Find("Class").localScale = new Vector3(0.06f/resolution,0.046f/resolution,0);
	}
	public void applySoldTexture()
	{
		transform.Find("texturedGameCard").renderer.materials[0].mainTexture = cardVM.soldCardTexture;
	}
	public void updateName()
	{
		transform.Find("texturedGameCard").FindChild("Title").GetComponent<TextMesh>().text = cardVM.title;
	}
}