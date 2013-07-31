using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Data
{
    public class MusicStoreDAL
    {
        public static void AddAlbumToDB(string albumName, string artistName, string year, string price, string producerName, string[] genres, List<string> songs, List<string> duration)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;

            using (var context = new MusicStoreEntities())
            {
                Artist newArtist = AddOrLoadArtist(context, artistName);

                Producer newProducer = AddOrLoadProducer(context, producerName);

                Album newAlbum = new Album();
                newAlbum.AlbumName = albumName;
                newAlbum.ArtistID = newArtist.ArtistID;
                newAlbum.ProducerID = newProducer.ProducerID;
                newAlbum.Date = DateTime.ParseExact(year.ToString(), "yyyy.mm.dd", provider);
                newAlbum.Price = decimal.Parse(price, provider);

                foreach (var genre in genres)
                {
                    Genre newGenre = AddOrLoadGenre(context, genre);
                    newAlbum.Genres.Add(newGenre);
                }

                int counter = 0;
                foreach (var song in songs)
                {
                    Song newSong = AddOrLoadSong(context, song, duration[counter]);
                    newAlbum.Songs.Add(newSong);
                    counter++;
                }

                context.Albums.Add(newAlbum);
                context.SaveChanges();
            }
        }

        private static Artist AddOrLoadArtist(MusicStoreEntities context, string artistName)
        {
            Artist existingArtist =
            (from a in context.Artists
             where a.ArtistName == artistName
             select a).FirstOrDefault();
            if (existingArtist != null)
            {
                return existingArtist;
            }

            Artist newArtist = new Artist();
            newArtist.ArtistName = artistName;
            context.Artists.Add(newArtist);
            context.SaveChanges();

            return newArtist;
        }

        private static Producer AddOrLoadProducer(MusicStoreEntities context, string producerName)
        {
            Producer existingProducer =
            (from p in context.Producers
             where p.ProducerName == producerName
             select p).FirstOrDefault();

            if (existingProducer != null)
            {
                return existingProducer;
            }

            Producer newProducer = new Producer();
            newProducer.ProducerName = producerName;
            context.Producers.Add(newProducer);
            context.SaveChanges();

            return newProducer;
        }

        private static Genre AddOrLoadGenre(MusicStoreEntities context, string genreName)
        {
            Genre existingGenre =
            (from g in context.Genres
             where g.GenreName == genreName
             select g).FirstOrDefault();

            if (existingGenre != null)
            {
                return existingGenre;
            }

            Genre newGenre = new Genre();
            newGenre.GenreName = genreName;
            context.Genres.Add(newGenre);
            context.SaveChanges();

            return newGenre;
        }

        private static Song AddOrLoadSong(MusicStoreEntities context, string song, string duration)
        {
            Song newSong = new Song();
            newSong.SongName = song;
            newSong.Duration = TimeSpan.Parse("00:"+duration);
            context.SaveChanges();

            return newSong;
        }
    }
}
