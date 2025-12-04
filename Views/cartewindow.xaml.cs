using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using static Valeria_Houda_carte_noel.Views.cartewindow;

namespace Valeria_Houda_carte_noel.Views
{
    public static class CarteLoader
    {
        // Charge toutes les cartes (image + texte) depuis les dossiers du projet
        public static List<Carte> LoadCartes()
        {
            // Dossiers contenant les images et les textes
            string imageFolder = "assets/cartes";
            string textFolder = "assets/textes";

            List<Carte> cartes = new List<Carte>();

            // Récupère toutes les images .png
            string[] images = Directory.GetFiles(imageFolder, "*.png");

            // Récupère tous les fichiers texte .txt
            string[] textes = Directory.GetFiles(textFolder, "*.txt");

            // On limite au plus petit nombre pour éviter les décalages
            int count = Math.Min(images.Length, textes.Length);

            // Assemble chaque image et texte dans une Carte
            for (int i = 0; i < count; i++)
            {
                cartes.Add(new Carte
                {
                    ImagePath = images[i],
                    Message = File.ReadAllText(textes[i]) // Lit le contenu du fichier texte
                });
            }

            return cartes;
        }
    }


    public partial class cartewindow : Window
    {
        // Un seul random, mais basé sur la date pour obtenir un tirage stable par jour
        private static Random random;
        public cartewindow()
        {
            InitializeComponent();
            // Charge la carte du jour quand la fenêtre s'ouvre
            LoadDailyCard();
        }

        /// <summary>
        /// Charge une carte aléatoire, mais toujours identique pour un même jour.
        /// </summary>
        private void LoadDailyCard()
        {
            // Charge toutes les cartes disponibles (images + textes)
            var cartes = CarteLoader.LoadCartes();

            // Si aucune carte trouvée, on affiche un message d'erreur
            if (cartes.Count == 0)
            {
                MessageBox.Show("Aucune carte trouvée !");
                return;
            }

            // Seed basé sur le numéro du jour dans l'année (1 → 365)
            // Cela garantit qu'un même jour donne toujours la même carte
            int seed = DateTime.Now.DayOfYear;
            random = new Random(seed);

            // Sélectionne une carte aléatoire parmi la liste
            int index = random.Next(0, cartes.Count);

            var carte = cartes[index];

            // -------------------------------------
            // AFFICHAGE DE LA CARTE DANS LE XAML
            // -------------------------------------

            // Affiche l'image de la carte
         //   CarteImage.Source = new BitmapImage(new Uri(carte.ImagePath, UriKind.RelativeOrAbsolute));

            // Affiche le message dans un TextBlock
           // CarteText.Text = carte.Message;
        }
    }
    public class Carte
        {
            public string ImagePath { get; set; }
            public string Message { get; set; }
        }
        public static class MusicManager
        {
            private static MediaPlayer player = new MediaPlayer();
            private static bool isInitialized = false;

            public static void PlayMusic()
            {
                if (!isInitialized)
                {
                    var uri = new Uri("assets/sounds/music.mp3", UriKind.RelativeOrAbsolute);
                    player.Open(uri);
                    player.Volume = 0.5;
                    player.MediaEnded += (s, e) =>
                    {
                        player.Position = TimeSpan.Zero;
                        player.Play();
                    };

                    isInitialized = true;
                }

                player.Play();
            }

            public static void StopMusic()
            {
                player.Stop();
            }
        }


}
