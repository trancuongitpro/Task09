using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM2.model
{
    public class Music
    {
        public string Name { get; set; }
        public MusicCategory Category { get; set; }
        public string MusicFile { get; set;}
        public string ImageFile { get; set; }

        public Music(string name, MusicCategory category)
        {
            Name = name;
            Category = category;
            MusicFile = string.Format("/Assets/music/{0}/{1}.mp3", category, name);
            ImageFile = string.Format("/Assets/image/{0}/{1}.png", category, name);
        }

    }
    public enum MusicCategory 
    { 
        chutinh,
        nhacbuon,
        tinhyeu
    }
    public class MusicManager
    {
        private static List<Music> GetMusics()
        {
            var musics = new List<Music>();
            musics.Add(new Music("hatbui", MusicCategory.chutinh));
            musics.Add(new Music("tauanhquanui", MusicCategory.chutinh));

            musics.Add(new Music("phutinh", MusicCategory.nhacbuon));
            musics.Add(new Music("thattinh", MusicCategory.nhacbuon));

            musics.Add(new Music("duongtachoemve", MusicCategory.tinhyeu));
            musics.Add(new Music("votuyetvoinhat", MusicCategory.tinhyeu));

            return musics;

        }
        public static void getAllSounds(ObservableCollection<Music> musics)
        {
            var AllSounds = GetMusics();
            musics.Clear();
            AllSounds.ForEach(p => musics.Add(p));
        }

        public static void GetSoundsByCategory(ObservableCollection<Music> musics, MusicCategory musicCategory)
        {
            var allSouds = GetMusics();
            var filteredSounds = allSouds.Where(p => p.Category == musicCategory).ToList();
            musics.Clear();
            filteredSounds.ForEach(p => musics.Add(p));
        }

        public static void getSoundsByName(ObservableCollection<Music> musics, string name)
        {
            var allSounds = GetMusics();
            var fillteredSounds = allSounds.Where(p => p.Name == name).ToList();
            musics.Clear();
            fillteredSounds.ForEach(p => musics.Add(p));
        }
    }
}
