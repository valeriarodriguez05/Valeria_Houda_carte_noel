using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using Valeria_Houda_carte_noel.Views;

namespace Valeria_Houda_carte_noel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private DispatcherTimer snowTimer; // pour animer les flocons
        private Random random = new Random(); // Une seule instance de Random
        private MediaPlayer musicPlayer = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            StartCountdown(); // Lance le compte à rebours + message
            StartSnow(); // Lance l'animation de neige
            PlayMusic(); // Lance la musique de Noël
        }

        private void BtnOuvrirCartePage_Click(object sender, RoutedEventArgs e)
        {
            // Crée la nouvelle fenêtre
            var cartePage = new cartewindow();

            // Affiche la nouvelle fenêtre
            cartePage.Show();

            // Ferme la fenêtre actuelle
            this.Close();
        }
        /// ////////////////////////////////////////////🎄 Compte à rebours + message 🎄////////////////////////////////////////////

        private void StartCountdown()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // mise à jour chaque seconde
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            DateTime noel = new DateTime(now.Year, 12, 25);

            // Si Noël est déjà passé cette année → viser l’an prochain
            if (now.Date > noel.Date)
                noel = noel.AddYears(1);

            // Calcul du nombre de jours
            TimeSpan restant = noel - now;

            // Juste les jours
            int joursRestants = (int)Math.Ceiling(restant.TotalDays);

            CountdownText.Text = $"Il reste {joursRestants} jours avant Noël 🎄";

            if (joursRestants == 0)
            {
                // Message spécial pour le jour J
                CountdownText.Text = "C'est aujourd'hui ! Joyeux Noël 🎅🎄";
            }
            else
            {
                CountdownText.Text = $"Il reste {joursRestants} jours avant Noël 🎄";
            }

        }

        /// ////////////////////////////////////////////❄ Animation de neige ❄////////////////////////////////////////////
        private void StartSnow()
        {
            snowTimer = new DispatcherTimer();
            snowTimer.Interval = TimeSpan.FromMilliseconds(40); // fréquence d'animation
            snowTimer.Tick += SnowTimer_Tick;
            snowTimer.Start();
        }

        /// <summary>
        /// Génère et anime les flocons.
        /// </summary>
        private void SnowTimer_Tick(object sender, EventArgs e)
        {
            // Création de nouveaux flocons
            if (random.NextDouble() < 0.3) // probabilité d'apparition
            {
                CreateSnowflake();
            }

            // Déplacement des flocons existants
            for (int i = SnowCanvas.Children.Count - 1; i >= 0; i--)
            {
                if (SnowCanvas.Children[i] is Ellipse flocon)
                {
                    // Descendre le flocon
                    double top = Canvas.GetTop(flocon);
                    Canvas.SetTop(flocon, top + 2);

                    // Légère dérive gauche/droite
                    double left = Canvas.GetLeft(flocon);
                    Canvas.SetLeft(flocon, left + (random.NextDouble() * 2 - 1));

                    // Si le flocon sort de la fenêtre, on le supprime
                    if (top > SnowCanvas.ActualHeight)
                        SnowCanvas.Children.RemoveAt(i);
                }
            }
        }
        /// Crée un flocon (un petit cercle blanc).
        private void CreateSnowflake()
        {
            double size = random.Next(3, 8);

            Ellipse flocon = new Ellipse
            {
                Width = size,
                Height = size,
                Fill = Brushes.White,
                Opacity = random.NextDouble() * 0.8 + 0.2
            };

            // Position initiale : en haut à un endroit aléatoire
            Canvas.SetLeft(flocon, random.NextDouble() * SnowCanvas.ActualWidth);
            Canvas.SetTop(flocon, -size);

            SnowCanvas.Children.Add(flocon);
        }

        ////////////////////////////////////////////🎵 Musique de Noël 🎵////////////////////////////////////////////
        private void PlayMusic()
        {
            try
            {
                musicPlayer.MediaEnded -= MusicPlayer_MediaEnded;
                var uri = new Uri("assets/sounds/music.mp3", UriKind.RelativeOrAbsolute);
                musicPlayer.Open(uri);
                musicPlayer.Volume = 0.5; // Volume moyen
                musicPlayer.MediaEnded += MusicPlayer_MediaEnded;
                musicPlayer.Play();
            }
            catch (Exception)
            {
                // Échec d'ouverture / lecture : ignored pour l'instant (ou logger si nécessaire)
            }
        }

        private void MusicPlayer_MediaEnded(object? sender, EventArgs e)
        {
            // Remet au début et relance en boucle
            musicPlayer.Position = TimeSpan.Zero;
            musicPlayer.Play();
        }
    }
}
