using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TutorialController : MonoBehaviour 
{
	public static TutorialController instance;
	public GameObject blockObject;

	private GameObject mainBlock;
	private GameObject description;
	private GameObject nextButton;
	private GameObject picture0;
	private GameObject picture1;
	private GameObject background;
	private GameObject title;
	private GameObject leftMargin;
	private GameObject rightMargin;

	void Update()
	{
		if (Screen.width != ApplicationDesignRules.widthScreen || Screen.height != ApplicationDesignRules.heightScreen) 
		{
			this.resize();
		}
	}
	public void initializeScene()
	{
		this.mainBlock = Instantiate(this.blockObject) as GameObject;
		this.description = GameObject.Find ("Description");
		this.background = GameObject.Find ("Background");
		this.picture0 = GameObject.Find ("picture0");
		this.picture1 = GameObject.Find ("picture1");
		this.nextButton = GameObject.Find ("NextButton");
		this.title = GameObject.Find ("TitleLabel");
		this.nextButton.transform.FindChild("Title").GetComponent<TextMeshPro> ().text = "Suivant";
		this.title.transform.GetComponent<TextMeshPro>().text="Bienvenue à Cristalia !";
		this.description.transform.GetComponent<TextMeshPro>().text="Après un long voyage de 17 années dans l'espace, vous voici enfin arrivé à Cristalia. La planète et ses satellites sont actuellement les proies de nombreuses convoitises. Vous êtes un colon missionné par votre pays pour amasser le plus de richesses et de connaissances sur ce fameux cristal. Cristal qui confèrerait aux habitants de la planète d'étranges capacités... Saurez-vous les mettre à profit pour triompher des colons ennemis ?";
		this.leftMargin = GameObject.Find ("leftMargin");
		this.rightMargin = GameObject.Find ("rightMargin");
	}
	void Start ()
	{
		instance = this;
		this.initializeScene ();
		this.resize ();
	}
	private IEnumerator quitTutorial()
	{
		yield return StartCoroutine(ApplicationModel.player.setTutorialStep (1));
		Application.LoadLevel("NewStore");
	}
	public void resize()
	{
		ApplicationDesignRules.computeDesignRules ();

		float mainBlockLeftMargin;
		float mainBlockUpMargin;
		float mainBlockHeight;
		float mainBlockWidth;

		float caractersYPosition;

		Vector2 backgroundSize = new Vector2 (1913f,1080f);
		Vector2 backgroundWorldSize = new Vector2 ();
		Vector3 backgroundScale = new Vector3 ();
		float backgroundYScale;

		Vector3 picture1Scale;
		Vector3 picture0Scale;
		Vector3 picture0Position;
		Vector3 picture1Position;

		if(ApplicationDesignRules.isMobileScreen)
		{
			mainBlockUpMargin=6f;
			mainBlockHeight=4f;
			mainBlockWidth=ApplicationDesignRules.blockWidth;
			mainBlockLeftMargin=ApplicationDesignRules.leftMargin;
			backgroundWorldSize.y=mainBlockUpMargin;
			this.background.transform.position=new Vector3(0f,2f,0f);
			picture0Scale=new Vector3(0.5f,0.5f,0.5f);
			picture1Scale=new Vector3(0.45f,0.45f,0.45f);

			picture0Position = this.picture0.transform.position;
			picture0Position.x = -5f * mainBlockWidth / 16f;
			picture0Position.y=1.2f;
			
			picture1Position = this.picture1.transform.position;
			picture1Position.x = 5f * mainBlockWidth / 16f;
			picture1Position.y=1.3f;

			Vector2 marginObjectSize=new Vector2(100f,100f);

			if(ApplicationDesignRules.leftMargin>0)
			{
				this.leftMargin.SetActive(true);
				Vector2 leftMarginWorldSize=new Vector2(ApplicationDesignRules.leftMargin,ApplicationDesignRules.worldHeight);
				Vector3 leftMarginScale = new Vector3(leftMarginWorldSize.x/(marginObjectSize.x / ApplicationDesignRules.pixelPerUnit),leftMarginWorldSize.y/(marginObjectSize.y / ApplicationDesignRules.pixelPerUnit),1f);
				Vector3 leftMarginPosition = new Vector3(-ApplicationDesignRules.worldWidth/2f+ApplicationDesignRules.leftMargin/2f,0f,0f);
				this.leftMargin.transform.localScale=leftMarginScale;
				this.leftMargin.transform.position=leftMarginPosition;
			}
			else
			{
				this.leftMargin.SetActive(false);
			}
			if(ApplicationDesignRules.rightMargin>0)
			{
				this.rightMargin.SetActive(true);
				Vector2 rightMarginWorldSize=new Vector2(ApplicationDesignRules.rightMargin,ApplicationDesignRules.worldHeight);
				Vector3 rightMarginScale = new Vector3(rightMarginWorldSize.x/(marginObjectSize.x / ApplicationDesignRules.pixelPerUnit),rightMarginWorldSize.y/(marginObjectSize.y / ApplicationDesignRules.pixelPerUnit),1f);
				Vector3 rightMarginPosition = new Vector3(ApplicationDesignRules.worldWidth/2f-ApplicationDesignRules.rightMargin/2f,0f,0f);
				this.rightMargin.transform.localScale=rightMarginScale;
				this.rightMargin.transform.position=rightMarginPosition;
			}
			else
			{
				this.rightMargin.SetActive(false);
			}
		}
		else
		{
			this.leftMargin.SetActive(false);
			this.rightMargin.SetActive(false);
			mainBlockUpMargin=7.5f;
			mainBlockHeight=2.5f;
			mainBlockLeftMargin=0f;
			mainBlockWidth=ApplicationDesignRules.worldWidth;
			backgroundWorldSize.y=ApplicationDesignRules.worldHeight;
			this.background.transform.position=new Vector3(0f,0f,0f);
			picture0Scale=new Vector3(1f,1f,1f);
			picture1Scale=new Vector3(1f,1f,1f);

			picture0Position = this.picture0.transform.position;
			picture0Position.x = -5f * mainBlockWidth / 16f;
			picture0Position.y=0f;

			picture1Position = this.picture1.transform.position;
			picture1Position.x = 5f * mainBlockWidth / 16f;
			picture1Position.y=-0.52f;
		}

		backgroundYScale = backgroundWorldSize.y/(backgroundSize.y / ApplicationDesignRules.pixelPerUnit);
		backgroundWorldSize.x = backgroundWorldSize.y * (backgroundSize.x / backgroundSize.y);
		backgroundScale = new Vector3 (backgroundYScale, backgroundYScale, backgroundYScale);
		this.background.transform.localScale = backgroundScale;
		this.mainBlock.GetComponent<NewBlockController> ().resize(mainBlockLeftMargin,mainBlockUpMargin,mainBlockWidth,mainBlockHeight);
		Vector2 mainBlockOrigin = this.mainBlock.GetComponent<NewBlockController> ().getOriginPosition ();
		this.description.GetComponent<TextContainer> ().width = mainBlockWidth - 2f*ApplicationDesignRules.blockHorizontalSpacing;
		this.description.GetComponent<TextContainer> ().height = mainBlockHeight - 1f;
		this.description.transform.position = new Vector3 (mainBlockOrigin.x, mainBlockOrigin.y + 0.3f, 0f);
		this.nextButton.transform.position = new Vector3 (0f, -4.5f);
		this.picture0.transform.position = picture0Position;
		this.picture0.transform.localScale = picture0Scale;
		this.picture1.transform.position = picture1Position;
		this.picture1.transform.localScale = picture1Scale;
	}
	public void nextButtonHandler()
	{
		StartCoroutine(this.quitTutorial());
	}
}
