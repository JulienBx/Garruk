using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NewsList{

	public List<News> newsList ;

	public NewsList()
	{
		this.newsList = new List<News>();
	}

	public News getNews(int index)
	{
		return this.newsList [index];
	}

	public int getCount()
	{
		return this.newsList.Count;
	}
	public void add()
	{
		this.newsList.Add(new News());
	}

	public void parseNews(string s, Player p)
	{
		string[] array = s.Split(new string[] { "#N#" }, System.StringSplitOptions.None);
		for (int i=0;i<array.Length-1;i++)
		{
			string[] newsData =array[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			newsList.Add(new News());
			newsList[i].IdNewsType=System.Convert.ToInt32(newsData[0]);
			newsList[i].Date=DateTime.ParseExact(newsData[1], "yyyy-MM-dd HH:mm:ss", null);
			newsList[i].User=p.Users.returnUsersIndex(System.Convert.ToInt32(newsData[2]));
			for(int j=3;j<newsData.Length-1;j++)
			{
				string[] newsObjectData = newsData[j].Split (new char[] {':'},System.StringSplitOptions.None);
				switch (newsObjectData[0])
				{
				case "user":
					newsList[i].Users.Add(p.Users.returnUsersIndex(System.Convert.ToInt32(newsObjectData[1])));
					break;
				case "card":
					newsList[i].Cards.add ();
					newsList[i].Cards.getCard(newsList[i].Cards.getCount()-1).Title=newsObjectData[1];
					newsList[i].Cards.getCard(newsList[i].Cards.getCount()-1).CardType.Id=System.Convert.ToInt32(newsObjectData[2]);
					break;
				case "value":
					newsList[i].Values.Add (newsObjectData[1]);
					break;
				case "trophy":
					newsList[i].Trophies.add();
					newsList[i].Trophies.getTrophy(newsList[i].Trophies.getCount()-1).Id=System.Convert.ToInt32(newsObjectData[2]);
					break;
				}
			}
		}
	}
	public void writeNews()
	{
		for (int i=0;i<newsList.Count;i++)
		{
			string tempContent=WordingNews.getContent(newsList[i].IdNewsType-1);
			string newTempContent=tempContent;
			bool toReplace=true;
			int j=0;
			while(toReplace)
			{
				tempContent=newTempContent;
				if(newsList[i].Users.Count>j)
				{
					newTempContent=ApplicationModel.ReplaceFirst(newTempContent,"#*user*#",ApplicationModel.player.Users.getUser(newsList[i].Users[j]).Username);
				}
				if(newsList[i].Cards.getCount()>j)
				{
					if(newsList[i].Cards.getCard(j).Title=="")
					{
						newTempContent=ApplicationModel.ReplaceFirst(newTempContent,"#*card*#",WordingCardTypes.getName(System.Convert.ToInt32(newsList[i].Cards.getCard(j).Title)));
					}
					else
					{
						newTempContent=ApplicationModel.ReplaceFirst(newTempContent,"#*card*#",newsList[i].Cards.getCard(j).Title);
					}
				}
				if(newsList[i].Values.Count>j)
				{
					newTempContent=ApplicationModel.ReplaceFirst(newTempContent,"#*value*#",newsList[i].Values[j]);
				}
				if(newsList[i].Trophies.getCount()>j)
				{
					newTempContent=ApplicationModel.ReplaceFirst(newTempContent,"#*trophy*#",WordingGameModes.getName(newsList[i].Trophies.getTrophy(j).Id));	
				}
				if(newTempContent==tempContent)
				{
					toReplace=false;
				}
				j++;
			}
			newsList[i].Content=tempContent;
		}		
	}

	public void filterNews(int playerId)
	{
		NewsList filterednews = new NewsList();
		bool toAdd = true;
		for (int i=0;i<this.newsList.Count;i++)
		{
			if(newsList[i].IdNewsType==1)
			{
				if(ApplicationModel.player.Users.getUser(newsList[i].Users[0]).Id!=playerId)
				{
					toAdd=true;
					for(int j=0;j<filterednews.getCount();j++)
					{
						if (filterednews.getNews(j).IdNewsType==1 &&
						    ApplicationModel.player.Users.getUser(filterednews.getNews(j).Users[0]).Id==ApplicationModel.player.Users.getUser(newsList[i].User).Id &&
						    ApplicationModel.player.Users.getUser(filterednews.getNews(j).Users[0]).Id==ApplicationModel.player.Users.getUser(newsList[i].Users[0]).Id)
						{
							toAdd=false;
							break;
						}
					}
					if(toAdd)
					{
						filterednews.add ();
						filterednews.newsList[filterednews.getCount()-1]=newsList[i];
					}
				}
			}
			else if(newsList[i].IdNewsType!=1)
			{
				filterednews.add ();
				filterednews.newsList[filterednews.getCount()-1]=newsList[i];
			}
		}
		this.newsList=filterednews.newsList;
	}
}