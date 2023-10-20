using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dictionary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnector connector;
        List<Word> allPolishWords = new List<Word>();
        List<Word> allEnglishWords = new List<Word>();
        List<Word> translatedWords = new List<Word>();
        List<Word> searchedWords = new List<Word>();
        List<Translations> allTranslations = new List<Translations>();

        SelectedLanguage SelectedLanguage = SelectedLanguage.Polish;
        public MainWindow()
        {
            InitializeComponent();

            connector = new SqlConnector();

            allPolishWords = connector.GetPolishWord_All();
            matchingWordsListView.ItemsSource = allPolishWords;

            allEnglishWords = connector.GetEnglishWord_All();

            allTranslations = connector.GetTranslation_All();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchedWord = searchTextBox.Text;
            searchedWords.Clear();

            switch (SelectedLanguage)
            {
                case SelectedLanguage.Undefined:
                    break;
                case SelectedLanguage.Polish:

                    matchingWordsListView.ItemsSource = allPolishWords;

                    foreach (Word word in allPolishWords)
                    {
                        if (word.Name.ToLower().Contains(searchedWord) || word.Description.ToLower().Contains(searchedWord))
                        {
                            searchedWords.Add(word);
                        }
                    }
                    break;
                case SelectedLanguage.English:

                    matchingWordsListView.ItemsSource = allEnglishWords;

                    foreach (Word word in allEnglishWords)
                    {
                        if (word.Name.ToLower().Contains(searchedWord) || word.Description.ToLower().Contains(searchedWord))
                        {
                            searchedWords.Add(word);
                        }
                    }
                    break;
                default:
                    break;
            }
            matchingWordsListView.ItemsSource = searchedWords;
        }

        private void matchingWordsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            translatedWords = new List<Word>();

            Word selectedWord = (Word)matchingWordsListView.SelectedItem;
            Word translatedWord = new Word();
            List<Translations> translations = new List<Translations>();
            int id = 0;

            if (selectedWord != null)
            {
                id = selectedWord.Id;
            }

            if (selectedWord != null)
            {
                switch (SelectedLanguage)
                {
                    case SelectedLanguage.Undefined:
                        break;
                    case SelectedLanguage.Polish:
                        translations = allTranslations.FindAll(x => x.PolishWord_id == id);
                        foreach (var ew in allEnglishWords)
                        {
                            foreach (var t in translations)
                            {
                                if (ew.Id == t.EnglishWord_id)
                                {
                                    translatedWords.Add(ew);
                                }
                            }
                        }
                        break;
                    case SelectedLanguage.English:
                        translations = allTranslations.FindAll(x => x.EnglishWord_id == id);
                        foreach (var pw in allPolishWords)
                        {
                            foreach (var t in translations)
                            {
                                if (pw.Id == t.PolishWord_id)
                                {
                                    translatedWords.Add(pw);
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            translatedWordsListView.ItemsSource = translatedWords;
            if (translatedWords.Count > 0)
            {
                translatedWordLabel.Content = selectedWord.Name;
            }
        }


        private void switchLanguageButton_Click(object sender, RoutedEventArgs e)
        {
            translatedWordLabel.Content = "";
            searchTextBox.Text = "";

            switch (SelectedLanguage)
            {
                case SelectedLanguage.Undefined:
                    break;
                case SelectedLanguage.Polish:
                    allEnglishWords = connector.GetEnglishWord_All();
                    matchingWordsListView.ItemsSource = allEnglishWords;
                    SelectedLanguage = SelectedLanguage.English;
                    ImgL.Source = new BitmapImage(new Uri("./../../Eng_flag.png", UriKind.Relative));
                    ImgP.Source = new BitmapImage(new Uri("./../../Pl_flag.png", UriKind.Relative));
                    search_bar_label.Content = "Search a word:";
                    switchLanguageButton.Content = "Change Language";
                    break;
                case SelectedLanguage.English:
                    allPolishWords = connector.GetPolishWord_All();
                    matchingWordsListView.ItemsSource = allPolishWords;
                    SelectedLanguage = SelectedLanguage.Polish;
                    ImgP.Source = new BitmapImage(new Uri("./../../Eng_flag.png", UriKind.Relative));
                    ImgL.Source = new BitmapImage(new Uri("./../../Pl_flag.png", UriKind.Relative));
                    search_bar_label.Content = "Wyszukaj słowo:";
                    switchLanguageButton.Content = "Zmiana Języka";
                    break;
                default:
                    break;
            }
        }

        private void translatedWordsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Word selectedItem = (Word)translatedWordsListView.Items.GetItemAt(translatedWordsListView.SelectedIndex);
                string target = "";
                switch (SelectedLanguage)
                {
                    case SelectedLanguage.Undefined:
                        break;
                    case SelectedLanguage.Polish:
                        target = $"https://dictionary.cambridge.org/dictionary/english-polish/{selectedItem.Name}";
                        break;
                    case SelectedLanguage.English:
                        target = $"https://dictionary.cambridge.org/dictionary/polish-english/{selectedItem.Name}";
                        break;
                    default:
                        break;
                }

                System.Diagnostics.Process.Start(target);
                translatedWordsListView.UnselectAll();
            }
            catch { }
        }
    }
}
