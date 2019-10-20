using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace Farming_session
{
	public class GameFile
	{
		//Data location
		SceneGameplay currentScene;

		//Serializer
		XmlGameFile serializableClass;

		//Game data
		public string currentGameData;

		//For the Gameplay
		public Vector2 HeroPosition;
		public int CurrentTime;
		public string CurrentMap;

		//FileName
		string GameFileName = "Content/SaveFile/GameFile";

		public GameFile(SceneGameplay pScene)
		{
			serializableClass = new XmlGameFile();

			currentScene = pScene;
			currentGameData = "";
		}

		public void ImportingGameData() //Get data from the file
		{
			XmlGameFile a = XmlGameFile.OpenFromXml(GameFileName);
			//Hero
			string posX = a.playerFile.PosX;
			string posY = a.playerFile.PosY;
			HeroPosition = new Vector2(float.Parse(posX), float.Parse(posY));

			//GameInfo
			CurrentTime = int.Parse(a.gameInfo.Time);
			CurrentMap = a.gameInfo.CurrentMap;
		}

		public void GetInventory(InventoryManager inventory) //Get inventory from the file
		{
			inventory.Empty();

			XmlGameFile a = XmlGameFile.OpenFromXml(GameFileName);

			//Inventory
			for (int i = 0; i < a.inventoryFile.lstItem.Length; i++) {
				if (a.inventoryFile.lstItem[i].Number != null) {
					inventory.AddObjectAt(int.Parse(a.inventoryFile.lstItem[i].Slot), a.inventoryFile.lstItem[i].ID, 
					                      int.Parse(a.inventoryFile.lstItem[i].Number));
				} else {
					inventory.AddObject(a.inventoryFile.lstItem[i].ID, 0);
				}
			}
		}

		public void SavingGameData() //Save the data into the file
		{
			SetMapFile();

			serializableClass.SaveToXml(GameFileName);
		}

		private void SetMapFile() //Set all the data who will be send to the file
		{
			//XmlFile element
			PlayerFile playerFile = new PlayerFile() {
				PosX = currentScene.myCharacter.Position.X.ToString(),
				PosY = currentScene.myCharacter.Position.Y.ToString(),
			};
			Console.WriteLine(currentScene.myCharacter.Position.X);
			Console.WriteLine(currentScene.myCharacter.Position.Y);

			GameInfo gameInfo = new GameInfo() {
				Time = currentScene.timeManager.CurrentHour.ToString(),
				CurrentMap = currentScene.mapManager.CurrentMap.FileName,
			};

			InventoryFile inventoryFile = new InventoryFile() {
				lstItem = new ItemFile[InventoryManager.MAXITEM],
			};

			for (int i = 0; i < InventoryManager.MAXITEM; i++) {
				Item a = currentScene.myCharacter.inventoryManager.GetItemAt(i);
				if (a != null) {
					string IDName = a.ID;
					string number = a.Quantity.ToString();
					string slot = a.InventorySlot.ToString();
					if (a.Quantity > 0) {
						inventoryFile.lstItem[i] = new ItemFile() { index = i.ToString(), ID = IDName, Number = number, Slot = slot, };
					} else {
						inventoryFile.lstItem[i] = new ItemFile() { index = i.ToString(), ID = IDName, Slot = slot, };
					}
				}
			}

			//Set the XmlFile element
			serializableClass.playerFile = playerFile;
			serializableClass.gameInfo = gameInfo;
			serializableClass.inventoryFile = inventoryFile;
		}

	}

	//Xml GameFile element
	[Serializable()]
	[DataContract]
	[XmlRoot(ElementName = "Player")]
	public class PlayerFile
	{
		[XmlElement(ElementName = "PosX")]
		public string PosX { get; set; }
		[XmlElement(ElementName = "PosY")]
		public string PosY { get; set; }
	}

	[XmlRoot(ElementName = "GameInfo")]
	public class GameInfo
	{
		[XmlElement(ElementName = "Time")]
		public string Time { get; set; }
		[XmlElement(ElementName = "CurrentMap")]
		public string CurrentMap { get; set; }
	}

	[XmlRoot(ElementName = "Inventory")]
	public class InventoryFile
	{
		[XmlElement(ElementName = "Item")]
		public ItemFile[] lstItem { get; set; }
	}

	[XmlRoot(ElementName = "Item")]
	public class ItemFile
	{
		[XmlAttribute(AttributeName = "index")]
		public string index { get; set; }
		[XmlElement(ElementName = "ID")]
		public string ID;
		[XmlElement(ElementName = "Number")]
		public string Number;
		[XmlElement(ElementName = "Slot")]
		public string Slot;
	}

	public class XmlGameFile //The File class
	{
		public PlayerFile playerFile;
		public GameInfo gameInfo;
		public InventoryFile inventoryFile;

		public XmlGameFile()
		{
		}

		public void SaveToXml(string FileName) //Save the data to the file
		{
			XmlSerializer ser = new XmlSerializer(typeof(XmlGameFile));
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
			ns.Add("", "");
			FileStream stream = File.Create(FileName + ".tmx");
			ser.Serialize(stream, this, ns);
			stream.Close();
		}

		public static XmlGameFile OpenFromXml(string FileName) //Open the file
		{
			XmlSerializer ser = new XmlSerializer(typeof(XmlGameFile));
			MemoryStream stream = new MemoryStream(File.ReadAllBytes(FileName + ".tmx"));
			return (XmlGameFile)ser.Deserialize(stream);                                       
		}
	}
}
