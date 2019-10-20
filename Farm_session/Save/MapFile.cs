using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Farming_session
{
	public class MapFile
	{
		//Map data
		public string currentMapLayer1String;
		public string currentMapLayer2String;
		public string collisionMapString;

		//Creating Tiled XML file 
		XmlMapFile serializableClass;

		public MapFile()
		{
			serializableClass = new XmlMapFile();
			currentMapLayer1String = "";
			currentMapLayer2String = "";
			collisionMapString = "";
		}

		private string EncodingMap(string mapString, int layer) //Get the map and put it in a string
		{
			List<int>[] ListInt = new List<int>[TileMap.MapHeight];
			List<string> LstString = new List<string>();

			string finalMapString = "";

			for (int i = 0; i < TileMap.MapHeight; i++) {

				ListInt[i] = new List<int>();

				for (int o = TileMap.MapWidth * i; o < TileMap.MapWidth * (i + 1); o++) { //Get the data from the gameplay map
					ListInt[i].Add(TileMap.lstLayer[layer][o].Gid);
				}

				LstString.Add(string.Join(",", ListInt[i])); //Put the data in the string
				string test = LstString[i];
				if (i < TileMap.MapHeight - 1) {
					test = test + ",\n";
				}
				finalMapString = finalMapString + test;
			}

			finalMapString = "\n" + finalMapString + "\n";

			return finalMapString;
		}

		public void LoadingMap()
		{
			SetMapFile();
			currentMapLayer1String = ImportingMapLayer("Content/level/PlayerLevel", 1); //Get the file map for layer1
			currentMapLayer2String = ImportingMapLayer("Content/level/PlayerLevel", 2); //Get the file map for layer2
			collisionMapString = ImportingMapLayer("Content/level/PlayerLevel", 3);

			//Console.WriteLine(currentMapLayer1String);
			//Console.WriteLine(currentMapLayer2String);
			//Console.WriteLine(collisionMapString);
		}

		public string ImportingMapLayer(string filename, int layer) //Get the map from the file for layer1
		{
			string mapString = "";

			XmlDocument doc = new XmlDocument();
			doc.Load(filename + ".tmx");

			List<string> mapLayer = new List<string>();

			foreach (XmlNode node in doc.DocumentElement.ChildNodes) {
				mapLayer.Add(node.InnerText);
			}

			mapString = mapLayer[layer];

			return mapString;
		}


		public void SavingMap()
		{
			//Encoding Map
			currentMapLayer1String = EncodingMap(currentMapLayer1String, 0);
			currentMapLayer2String = EncodingMap(currentMapLayer2String, 1);
			collisionMapString = EncodingMap(collisionMapString, 2);

			Console.WriteLine(currentMapLayer1String);
			Console.WriteLine("\n" + currentMapLayer2String);

			SetMapFile();

			serializableClass.SaveToXml("Content/level/PlayerLevelTemplate");
		}

		public void SetMapFile()
		{
			//Xml File element
			Image image = new Image()
			{
				Source = "tileset.png",
				Width = "96",
				Height = "48"
			};

			Tileset tileset = new Tileset()
			{
				Image = image,
				Firstgid = "1",
				Name = "tileset",
				Tilewidth = "16",
				Tileheight = "16",
				Tilecount = "120",
				Columns = "12",
			};

			Data data1 = new Data()
			{
				Encoding = "csv",
				Text = currentMapLayer1String
			};

			Data data2 = new Data()
			{
				Encoding = "csv",
				Text = currentMapLayer2String
			};

			Data CollisionData = new Data() 
			{
				Encoding = "csv",
				Text = collisionMapString,
			};

			Layer layer1 = new Layer()
			{
				Data = data1,
				Name = "Tile Layer 1",
				Width = "24",
				Height = "16" 
			};

			Layer layer2 = new Layer()
			{
				Data = data2,
				Name = "Tile Layer 2",
				Width = "24",
				Height = "16"
			};

			Layer collision = new Layer() 
			{
				Data = CollisionData,
				Name = "Collision",
				Width = "24",
				Height = "16"
			};

			//set the xml file element
			serializableClass.tileset = tileset;
			Layer[] lstLayer = new Layer[2];
			serializableClass.layer1 = layer1;
			serializableClass.layer2 = layer2;
			serializableClass.layer3 = collision;
			}
		}

	//Xlm Tiled file element
	[Serializable()]
	[DataContract]
	[XmlRoot(ElementName = "tileset")]
	public class Tileset
	{
		[XmlElement(ElementName = "image")]
		public Image Image { get; set; }
		[XmlAttribute(AttributeName = "firstgid")]
		public string Firstgid { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "tilewidth")]
		public string Tilewidth { get; set; }
		[XmlAttribute(AttributeName = "tileheight")]
		public string Tileheight { get; set; }
		[XmlAttribute(AttributeName = "tilecount")]
		public string Tilecount { get; set; }
		[XmlAttribute(AttributeName = "columns")]
		public string Columns { get; set; }
	}

	[XmlRoot(ElementName = "data")]
	public class Data
	{
		[XmlAttribute(AttributeName = "encoding")]
		public string Encoding { get; set; }
		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "image")]
	public class Image
	{
		[XmlAttribute(AttributeName = "source")]
		public string Source { get; set; }
		[XmlAttribute(AttributeName = "width")]
		public string Width { get; set; }
		[XmlAttribute(AttributeName = "height")]
		public string Height { get; set; }
	}

	[XmlRoot(ElementName = "layer")]
	public class Layer
	{
		[XmlElement(ElementName = "data")]
		public Data Data { get; set; }
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "width")]
		public string Width { get; set; }
		[XmlAttribute(AttributeName = "height")]
		public string Height { get; set; }
	}

	[XmlRoot(ElementName = "map")]
	public class XmlMapFile
	{

		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }
		[XmlAttribute(AttributeName = "tiledversion")]
		public string Tiledversion { get; set; }
		[XmlAttribute(AttributeName = "orientation")]
		public string Orientation { get; set; }
		[XmlAttribute(AttributeName = "renderorder")]
		public string Renderorder { get; set; }
		[XmlAttribute(AttributeName = "width")]
		public string Width { get; set; }
		[XmlAttribute(AttributeName = "height")]
		public string Height { get; set; }
		[XmlAttribute(AttributeName = "tilewidth")]
		public string Tilewidth { get; set; }
		[XmlAttribute(AttributeName = "tileheight")]
		public string Tileheight { get; set; }
		[XmlAttribute(AttributeName = "nextobjectid")]
		public string Nextobjectid { get; set; }

		public Tileset tileset;
		public Layer layer1;
		public Layer layer2;
		public Layer layer3;

		public XmlMapFile() //Set the xml Tiled file element
		{
			Version = "1.0";
			Tiledversion = "1.0.3";
			Orientation = "orthogonal";
			Renderorder = "right-down";
			Width = "24";
			Height = "16";
			Tilewidth = "16";
			Tileheight = "16";
			Nextobjectid = "1";
		}

		public void SaveToXml(string FileName)
		{
			XmlSerializer ser = new XmlSerializer(typeof(XmlMapFile));
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
			ns.Add("", "");
			FileStream stream = File.Create(FileName + ".tmx");
			ser.Serialize(stream, this, ns);
			stream.Close();

			StreamReader sr = new StreamReader("Content/level/PlayerLevelTemplate.tmx"); //Create the file from template

			StreamWriter sw = new StreamWriter("Content/level/PlayerLevel.tmx");

			string line = sr.ReadLine();

			line = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"; //Change the encoding

			sw.WriteLine(line);

			string test = sr.ReadToEnd();
			test = test.Replace("layer1", "layer");
			test = test.Replace("layer2", "layer");
			test = test.Replace("layer3", "layer");

			sw.WriteLine(test);

			sw.Close();

			sr.Close();
		}

		public static XmlMapFile OpenFromXml(string FileName)
		{
			XmlSerializer ser = new XmlSerializer(typeof(XmlMapFile));
			MemoryStream stream = new MemoryStream(File.ReadAllBytes(FileName + ".tmx"));
			return (XmlMapFile)ser.Deserialize(stream);
		}
	}
}
