using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ASM2.model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ASM2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<Music> musics;

        private List<MenuIcon> menuIcons;

        private List<string> Suggestions;

        public MainPage()
        {
            this.InitializeComponent();
            musics = new ObservableCollection<Music>();
            MusicManager.getAllSounds(musics);
            menuIcons = new List<MenuIcon>();
            menuIcons.Add(new MenuIcon { IconFIle = "Assets/Icons/animals.png", Category = MusicCategory.chutinh });
            menuIcons.Add(new MenuIcon { IconFIle = "Assets/Icons/cartoon.png", Category = MusicCategory.nhacbuon });
            menuIcons.Add(new MenuIcon { IconFIle = "Assets/Icons/taunt.png", Category = MusicCategory.tinhyeu });
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySpilitView.IsPaneOpen = !MySpilitView.IsPaneOpen;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MusicManager.getAllSounds(musics);
            CategoryTextBlock.Text = "all Sounds";
            MenuItemListView.SelectedItem = null;
            BackButton.Visibility = Visibility.Collapsed;
            goBack();
        }

        private void SearchAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (string.IsNullOrEmpty(sender.Text)) goBack();
            MusicManager.getAllSounds(musics);
            Suggestions = musics.Where(p => p.Name.StartsWith(sender.Text)).Select(p => p.Name).ToList();
            SearchAutoSuggestBox.ItemsSource = Suggestions;
        }

        private void SearchAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            MusicManager.getSoundsByName(musics, sender.Text);
            CategoryTextBlock.Text = sender.Text;
            MenuItemListView.SelectedItem = null;
            BackButton.Visibility = Visibility.Visible;
        }

        private void goBack()
        {
            MusicManager.getAllSounds(musics);
            CategoryTextBlock.Text = "all sounds";
            MenuItemListView.SelectedItem = null;
            BackButton.Visibility = Visibility.Collapsed;
        }

        private void MenuItemListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var menuItem = (MenuIcon)e.ClickedItem;
            CategoryTextBlock.Text = menuItem.Category.ToString();
            MusicManager.GetSoundsByCategory(musics, menuItem.Category);
            BackButton.Visibility = Visibility.Collapsed;
        }

        private void SounlGridView_Drop(object sender, DragEventArgs e)
        {

        }

        private async void MusicGridView_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems)) ;
            {
                var item = await e.DataView.GetStorageItemsAsync();

                if (item.Any())
                {
                    var storageFile = item[0] as StorageFile;
                    var contentType = storageFile.ContentType;

                    StorageFolder folder = ApplicationData.Current.LocalFolder;

                    if (contentType == "audio/wabv" || contentType == "audio/mpeg")
                    {
                        StorageFile newFile = await storageFile.CopyAsync(folder, storageFile.Name, NameCollisionOption.GenerateUniqueName);
                        MyMediaElement.SetSource(await storageFile.OpenAsync(FileAccessMode.Read),
                            contentType);
                        MyMediaElement.Play();
                    }
                }
            }
        }

        private void MusicGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var musics = (Music)e.ClickedItem;
            MyMediaElement.Source = new Uri(this.BaseUri, musics.MusicFile);
        }

        private void MusicGridView_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
            e.DragUIOverride.Caption = "drop to create a custom sound and title";
            e.DragUIOverride.IsCaptionVisible = true;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsGlyphVisible = true;
        }
    }
}
