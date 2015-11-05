//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//
//
//public class SkillBookView : MonoBehaviour
//{
//	
//	public SkillBookScreenViewModel screenVM;
//	public SkillBookCTypeSelectedViewModel cTypeSelectedVM;
//	public SkillBookCTypesViewModel cTypesVM;
//	public SkillBookStatsViewModel statsVM;
//	public SkillBookSkillsViewModel skillsVM;
//	public SkillBookBookViewModel bookVM;
//
//	
//	public SkillBookView ()
//	{
//		this.screenVM = new SkillBookScreenViewModel ();
//		this.cTypeSelectedVM = new SkillBookCTypeSelectedViewModel ();
//		this.cTypesVM = new SkillBookCTypesViewModel ();
//		this.statsVM = new SkillBookStatsViewModel ();
//		this.skillsVM = new SkillBookSkillsViewModel ();
//		this.bookVM = new SkillBookBookViewModel ();
//	}
//	void Update()
//	{
//		if (Screen.width != screenVM.widthScreen || Screen.height != screenVM.heightScreen) 
//		{
//			SkillBookController.instance.loadAll();
//		}
//	}
//	void OnGUI()
//	{
//		GUILayout.BeginArea (screenVM.blockBook, bookVM.bookBackgroundStyle);
//		{
//		}
//		GUILayout.EndArea();
//		GUILayout.BeginArea (screenVM.blockCTypeSelected);
//		{
//			GUILayout.Space (screenVM.blockCTypeSelectedHeight*0.4f);
//			GUILayout.BeginHorizontal(GUILayout.Height(screenVM.blockCTypeSelectedHeight*0.6f));
//			{
//				GUILayout.Label("",cTypeSelectedVM.pictureStyle,GUILayout.Height(screenVM.blockCTypeSelectedHeight*0.6f),GUILayout.Width(screenVM.blockCTypeSelectedHeight*0.6f));
//				GUILayout.Space (screenVM.blockCTypeSelectedWidth*0.02f);
//				GUILayout.BeginVertical(GUILayout.Width(screenVM.blockCTypeSelectedWidth*0.5f));
//				{
//					GUILayout.Label(cTypeSelectedVM.name,cTypeSelectedVM.nameStyle);
//					GUILayout.BeginHorizontal();
//					{
//						GUILayout.Label("",cTypeSelectedVM.gaugeStyle,GUILayout.Width(cTypeSelectedVM.gaugeWidth*screenVM.blockCTypeSelectedWidth*0.35f));
//						GUILayout.Label("",cTypeSelectedVM.gaugeBackgroundStyle,GUILayout.Width(cTypeSelectedVM.gaugeBackgroundWidth*screenVM.blockCTypeSelectedWidth*0.35f));
//					}
//					GUILayout.EndHorizontal();
//					GUILayout.Label(cTypeSelectedVM.percentage,cTypeSelectedVM.percentageStyle);
//				}
//				GUILayout.EndVertical();
//				GUILayout.FlexibleSpace();
//				GUILayout.BeginVertical(GUILayout.Width(screenVM.blockCTypeSelectedWidth*0.35f));
//				{
//					GUILayout.Label(cTypeSelectedVM.nbCards,cTypeSelectedVM.nbCardsStyle);
//					if(cTypeSelectedVM.displayButton)
//					{
//						GUILayout.BeginHorizontal();
//						{
//							GUILayout.FlexibleSpace();
//							GUI.enabled=bookVM.buttonsEnabled;
//							if (GUILayout.Button("\u00A0Afficher\u00A0",cTypeSelectedVM.buttonStyle))
//							{
//								SkillBookController.instance.displayCardsWidhCardType();
//							}
//							GUI.enabled=true;
//							GUILayout.FlexibleSpace();
//						}
//						GUILayout.EndHorizontal();
//					}
//					GUILayout.FlexibleSpace();
//				}
//				GUILayout.EndVertical();
//			}
//			GUILayout.EndHorizontal();
//			GUILayout.Space (screenVM.blockCTypeSelectedHeight*0.1f);
//		}
//		GUILayout.EndArea();
//		for (int i=0;i<skillsVM.finish-skillsVM.start;i++)
//		{
//			GUILayout.BeginArea(skillsVM.blocks[i]);
//			{
//				GUILayout.BeginHorizontal(GUILayout.Height(skillsVM.blocksHeight*0.4f));
//				{
//					GUILayout.Label("",skillsVM.pictures[i],GUILayout.Height(skillsVM.blocksHeight*0.4f),GUILayout.Width(skillsVM.blocksHeight*0.5f));
//					GUILayout.BeginVertical(GUILayout.Width(skillsVM.blocksWidth*0.5f));
//					{
//						GUILayout.Label(skillsVM.names[i],skillsVM.nameStyle,GUILayout.Height(skillsVM.blocksHeight*0.2f));
//						GUILayout.BeginHorizontal();
//						{
//							GUILayout.Label("",skillsVM.gauges[i],GUILayout.Width (skillsVM.gaugesWidth[i]*screenVM.blockCTypeSelectedWidth*0.35f),GUILayout.Height(skillsVM.blocksHeight*0.1f));
//							GUILayout.Label("",skillsVM.gaugeBackgroundStyle,GUILayout.Width(skillsVM.gaugesBackgroundWidth[i]*screenVM.blockCTypeSelectedWidth*0.35f),GUILayout.Height(skillsVM.blocksHeight*0.1f));
//						}
//						GUILayout.EndHorizontal();
//						GUILayout.Label("Niveau Max atteint : "+skillsVM.percentages[i].ToString(),skillsVM.percentageStyle,GUILayout.Height(skillsVM.blocksHeight*0.1f));
//					}
//					GUILayout.EndVertical();
//					GUILayout.FlexibleSpace();
//					GUILayout.BeginVertical(GUILayout.Width(skillsVM.blocksWidth*0.35f));
//					{
//						GUILayout.Label(skillsVM.nbCards[i],skillsVM.nbCardsStyle);
//						if(skillsVM.displayButtons[i])
//						{
//							GUILayout.BeginHorizontal();
//							{
//								GUILayout.FlexibleSpace();
//								GUI.enabled=bookVM.buttonsEnabled;
//								if (GUILayout.Button("\u00A0Afficher\u00A0",skillsVM.buttonStyle))
//								{
//									SkillBookController.instance.displayCardsWidhSkill(i);
//								}
//								GUI.enabled=true;
//								GUILayout.FlexibleSpace();
//							}
//							GUILayout.EndHorizontal();
//						}
//						GUILayout.FlexibleSpace();
//					}
//					GUILayout.EndVertical();
//				}
//				GUILayout.EndHorizontal();
//				GUILayout.Space (skillsVM.blocksHeight*0.05f);
//				GUILayout.Label(skillsVM.descriptions[i],skillsVM.descriptionStyle);
//			}
//			GUILayout.EndArea();
//		}
//
//		GUILayout.BeginArea (screenVM.blockCTypes);
//		{
//			for(int i =0;i<cTypesVM.tabButtonsStyles.Count;i++)
//			{
//				GUI.enabled=bookVM.buttonsEnabled;
//				if(GUILayout.Button("",cTypesVM.tabButtonsStyles[i],GUILayout.Height(screenVM.blockCTypesHeight/10f),GUILayout.Width(screenVM.blockCTypesHeight/10f)))
//				{
//					SkillBookController.instance.selectCardType(i);
//				}
//				GUI.enabled=true;
//			}
//			GUILayout.FlexibleSpace();
//		}
//		GUILayout.EndArea ();
//		if(skillsVM.chosenPage!=0)
//		{
//			GUILayout.BeginArea (screenVM.blockBackButton);
//			{
//				GUI.enabled=bookVM.buttonsEnabled;
//				if(GUILayout.Button("<",bookVM.backButtonStyle))
//				{
//					SkillBookController.instance.selectBackPage();
//				}
//				GUI.enabled=true;
//			}
//			GUILayout.EndArea ();
//		}
//		if(skillsVM.nbPages>skillsVM.chosenPage+1)
//		{
//			GUILayout.BeginArea (screenVM.blockNextButton);
//			{
//				GUI.enabled=bookVM.buttonsEnabled;
//				if(GUILayout.Button(">",bookVM.nextButtonStyle))
//				{
//					SkillBookController.instance.selectNextPage();
//				}
//				GUI.enabled=true;
//			}
//			GUILayout.EndArea ();
//		}
//		GUILayout.BeginArea (screenVM.blockStats);
//		{
//			GUILayout.FlexibleSpace();
//			GUILayout.BeginHorizontal();
//			{
//				GUILayout.FlexibleSpace();
//				GUILayout.Label("Niveau de la collection :",statsVM.titleStyle);
//				for(int i=0;i<5;i++)
//				{
//					GUILayout.Label("",statsVM.stars[i],GUILayout.Height(screenVM.blockStatsHeight*0.8f),GUILayout.Width(screenVM.blockStatsHeight*0.8f));
//				}
//				GUILayout.FlexibleSpace();
//			}
//			GUILayout.EndHorizontal();
//			GUILayout.FlexibleSpace();
//		}
//		GUILayout.EndArea();
//	}
//}
//
