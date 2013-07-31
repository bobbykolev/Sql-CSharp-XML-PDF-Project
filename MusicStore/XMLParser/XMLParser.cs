using MusicStore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLParser
{
    public class XMLParser
    {
        static void Main()
        {           
        }

        public static void ParseDataFromXML(string path)
        {
            //Invariant Culture for the dates and decimas
            System.Threading.Thread.CurrentThread.CurrentCulture =
               System.Globalization.CultureInfo.InvariantCulture;


            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            string xPath = "/albums/album";
            int indexer = 0;

            XmlNodeList albumCollection = xmlDoc.SelectNodes(xPath);
            foreach (XmlNode album in albumCollection)
            {
                indexer++;
                string albumName = GetChildText(album, "name");
                string artistName = GetChildText(album, "artist");
                string year = GetChildText(album, "year");
                string price = GetChildText(album, "price");
                string producerName = GetChildText(album, "producer");
                string genresString = GetChildText(album, "genre");
                string[] genres = { };

                if (genresString != null)
                {
                    genres = genresString.Split(',');
                    for (int i = 0; i < genres.Length; i++)
                    {
                        genres[i] = genres[i].Trim();
                    }
                }

                List<string> songs = new List<string>();
                List<string> duration = new List<string>();

                XmlNodeList songsCollection = xmlDoc.SelectNodes("albums/album[" + indexer + "]/songs/song");

                foreach (XmlNode song in songsCollection)
                {
                    songs.Add(song.InnerText.ToString());
                    duration.Add(song.SelectSingleNode("@duration").FirstChild.Value.ToString());
                }

                MusicStoreDAL.AddAlbumToDB(albumName, artistName, year, price, producerName, genres, songs, duration);
            }
        }

        private static string GetChildText(XmlNode album, string element)
        {
            XmlNode childNode = album.SelectSingleNode(element);
            if (childNode == null)
            {
                return null;
            }
            return childNode.InnerText.Trim();
        }
    }
}
