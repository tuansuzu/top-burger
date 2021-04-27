using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

///<summary>
///<para>Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...</para>
///<para>Object:N/A</para>
///<para>Description: Sample Description </para>
///</summary>

public class LevelsParser : MonoBehaviour {

	public struct LevelStruct
	{
		public int levelNumber;
		public int levelGoal;
		public int levelUnlockedItem;
		public string levelUnlockedTitleWorld1;
		public string levelUnlockedMessageWorld1;
		public string levelUnlockedTitleWorld2;
		public string levelUnlockedMessageWorld2;
		public string levelUnlockedTitleWorld3;
		public string levelUnlockedMessageWorld3;
	}
	public static List<LevelStruct> ListOfLevels=new List<LevelStruct>();
	// Use this for initialization
	void Start () {
		ParseXML();
		
	}

	void ParseXML()
	{
		TextAsset aset =(TextAsset)Resources.Load("Levels/Levels");
		XmlDocument xml= new XmlDocument();
		xml.LoadXml(aset.ToString());

		
		XmlNodeList appNodes = xml.SelectNodes("/xml/level");
		
		int number=appNodes.Count;

		foreach (XmlNode node in appNodes)
		{
			LevelStruct SingleLevel=new LevelStruct
			{
				levelNumber = int.Parse(node.Attributes.GetNamedItem("number").Value)
					
			};
			SingleLevel.levelGoal = int.Parse(node.SelectSingleNode("goal").InnerText);
			SingleLevel.levelUnlockedItem = int.Parse(node.SelectSingleNode("unlockedItem").InnerText);
			SingleLevel.levelUnlockedTitleWorld1 = node.SelectSingleNode("unlockedTitleWorld1").InnerText;
			SingleLevel.levelUnlockedMessageWorld1 = node.SelectSingleNode("unlockedMessageWorld1").InnerText;
			SingleLevel.levelUnlockedTitleWorld2 = node.SelectSingleNode("unlockedTitleWorld2").InnerText;
			SingleLevel.levelUnlockedMessageWorld2 = node.SelectSingleNode("unlockedMessageWorld2").InnerText;
			SingleLevel.levelUnlockedTitleWorld3 = node.SelectSingleNode("unlockedTitleWorld3").InnerText;
			SingleLevel.levelUnlockedMessageWorld3 = node.SelectSingleNode("unlockedMessageWorld3").InnerText;
			ListOfLevels.Add(SingleLevel);
		}
	}
}
