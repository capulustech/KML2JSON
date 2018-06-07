using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace kml2json
{
    class Helper
    {
        public void GetCordinates(string fileUri, string folder)
        {
            XmlReader reader = XmlReader.Create(@""+fileUri);
            string filename = "default.txt";
            string compiled = "";
            while (reader.Read())
            {
                if ((reader.NodeType == XmlNodeType.Element) && reader.Name.Equals("name"))
                {
                    filename = reader.ReadString();
                }
                if ((reader.NodeType == XmlNodeType.Element) && reader.Name.Equals("coordinates"))
                {
                    compiled += ParseCordinates(reader.ReadString(),folder, filename);
                }
                 
            }

            using (StreamWriter writetext = new StreamWriter(folder + "\\json\\" + filename + ".txt"))
            {
                writetext.Write(compiled);
            }

        }

        public void GetCordinatesMultiGeometry(string fileUri, string folder)
        {
           

            XmlReader reader = XmlReader.Create(@"" + fileUri);
            string filename = "";
            int i = 1;
            while (reader.Read())
            {
                if ((reader.NodeType == XmlNodeType.Element) && reader.Name.Equals("name"))
                {
                    filename = reader.ReadString();
                }
                if ((reader.NodeType == XmlNodeType.Element) && reader.Name.Equals("coordinates"))
                {
                    ParseCordinates(reader.ReadString(), folder, filename + " " + i);
                    i++;
                }             

            }
        }

        public string ParseCordinates(string coOrdinates, string folder, string filename)
        {
            string compiled = "";
            string[] str = coOrdinates.Trim().Split(new string[] { ",0" }, StringSplitOptions.None); //separated by ,0
            if(str.Length<2)
            {
                str = coOrdinates.Trim().Split(new string[] { " " }, StringSplitOptions.None);//separated by ,<space>
            }
            if (str.Length < 2)
            {
                str = coOrdinates.Trim().Split(new string[] { "," }, StringSplitOptions.None);//separated by ,
            }

            int i = 0;
            Directory.CreateDirectory(folder + "\\json");
            JArray jsonArray = new JArray();
            foreach (string s in str)
            {
                if (!s.Equals(""))
                {
                    
                    string longitude = s.Trim().Split(new string[] { "," }, StringSplitOptions.None)[0];
                    string latitude = s.Trim().Split(new string[] { "," }, StringSplitOptions.None)[1];

                    JObject jsonObject = new JObject();
                    jsonObject.Add("lat", latitude.Trim());
                    jsonObject.Add("lng", longitude.Trim());

                    jsonArray.Add(jsonObject);

                    compiled = "\n"+ filename +","+ latitude.Trim() + "," + longitude.Trim();
                }
                i++;
            }
            Console.WriteLine("i: " + i);
            using (StreamWriter writetext = new StreamWriter(folder+"\\json\\"+filename+".json"))
            {
                writetext.Write(jsonArray);
            }

            return compiled;

        }

    }
}
