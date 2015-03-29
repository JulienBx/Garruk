using UnityEngine;
using System.Collections;

public class CharacterScript : MonoBehaviour {
	
	Vector3 CharacterPositionTopLeft;
	Vector3 CharacterPositionBottomRight;

	public GUIStyle NamePoliceStyle;
	public GUIStyle LifePoliceStyle;
	public GUIStyle SpeedPoliceStyle;
	public GUIStyle AttackPoliceStyle;
	public GUIStyle MovePoliceStyle;
	public GUIStyle QuicknessPoliceStyle;
	public GUIStyle iconStyle;

	public Texture attackIcon;
	public Texture moveIcon;
	public Texture quicknessIcon;

	int widthScreen = Screen.width; 
	int heightScreen = Screen.height;
	Bounds bounds;

	Animator animator;

	string Name;
	int Life;
	int Quickness;
	int Move;
	int Attack;

	bool toHide=true;

	// Use this for initialization
	void Start () {
		animator = transform.parent.GetComponent<Animator> ();
		setStyles();
	}
	
	// Update is called once per frame
	void Update () {
		if (Screen.width != widthScreen || Screen.height != heightScreen) {
			this.setStyles();
		}
	}

	void OnGUI ()
	{
		if(!toHide){

			bounds = renderer.bounds;
			foreach(var r in GetComponentsInChildren<Renderer>())
			{
				bounds.Encapsulate(r.bounds);
			}
			CharacterPositionTopLeft=Camera.main.WorldToScreenPoint(new Vector3(bounds.min.x,bounds.max.y,0));
			CharacterPositionBottomRight=Camera.main.WorldToScreenPoint(new Vector3(bounds.max.x,bounds.min.y,0));
			GUILayout.BeginArea (new Rect (CharacterPositionBottomRight.x-(CharacterPositionBottomRight.x-CharacterPositionTopLeft.x)/2-(int)widthScreen*75/1000, Screen.height-CharacterPositionTopLeft.y-(int)heightScreen*4/100, (int)widthScreen*15/100, (int)heightScreen*4/100));
			{
				GUILayout.BeginVertical();
				{
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						GUILayout.Label (Name+ "\u00A0" ,NamePoliceStyle);
						GUILayout.Label (Life+ "\u00A0",LifePoliceStyle);
						GUILayout.FlexibleSpace();
					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.BeginHorizontal();
					{
						GUILayout.FlexibleSpace();
						GUILayout.Box(attackIcon,iconStyle);
						GUILayout.Label ("\u00A0"+Attack+ "\u00A0",AttackPoliceStyle);
						GUILayout.Box(quicknessIcon,iconStyle);
						GUILayout.Label ("\u00A0"+Quickness+ "\u00A0",QuicknessPoliceStyle);
						GUILayout.Box(moveIcon,iconStyle);
						GUILayout.Label ("\u00A0"+Move+ "\u00A0",MovePoliceStyle);
						GUILayout.FlexibleSpace();

					}
					GUILayout.EndHorizontal();
					GUILayout.FlexibleSpace();
				}
				GUILayout.EndVertical();
			}
			GUILayout.EndArea ();
		}

	}

	private void setStyles() {
		heightScreen = Screen.height;
		widthScreen = Screen.width;
		this.NamePoliceStyle.fontSize = heightScreen*15/1000;
		this.NamePoliceStyle.fixedHeight = (int)heightScreen*2/100;
		this.SpeedPoliceStyle.fontSize = heightScreen*15/1000;
		this.SpeedPoliceStyle.fixedHeight = (int)heightScreen*2/100;
		this.AttackPoliceStyle.fontSize = heightScreen*15/1000;
		this.AttackPoliceStyle.fixedHeight = (int)heightScreen*2/100;
		this.MovePoliceStyle.fontSize = heightScreen*15/1000;
		this.MovePoliceStyle.fixedHeight = (int)heightScreen*2/100;
		this.QuicknessPoliceStyle.fontSize = heightScreen*15/1000;
		this.QuicknessPoliceStyle.fixedHeight = (int)heightScreen*2/100;
		this.LifePoliceStyle.fontSize = heightScreen*15/1000;
		this.LifePoliceStyle.fixedHeight = (int)heightScreen*2/100;
		this.iconStyle.fixedHeight=(int)heightScreen*2/100;
	}

	public void hideInformations(){
		toHide=true;
	}

	public void showInformations(){
		toHide=false;
	}

	public void setName(string name){
		this.Name = name;
	}

	public void setLife(int life){
		this.Life = life;
	}

	public void setAttack(int attack){
		this.Attack = attack;
	}

	public void setMove(int move){
		this.Move = move;
	}

	public void setQuickness(int quickness){
		this.Quickness = quickness;
	}

	public void toWalk(){
		animator.SetBool("isWalking",true );
	}

	public void stopWalking(){
		animator.SetBool("isWalking",false );
	}
}
