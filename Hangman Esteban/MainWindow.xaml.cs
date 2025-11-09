// Importation des bibliothèques nécessaires
using System;
using System.Collections.Generic;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hangman_Esteban
{
    /// <summary>
    /// Code-behind principal de la fenêtre du jeu du pendu
    /// </summary>
    public partial class MainWindow : Window
    {
        // --- VARIABLES GLOBALES ---

        // Minuteur utilisé pour le compte à rebours
        System.Windows.Threading.DispatcherTimer countdownTimer;

        // Temps restant (en secondes)
        int timeRemaining = 60;

        // Objets audio pour les sons du jeu
        MediaPlayer win = new MediaPlayer();
        MediaPlayer lose = new MediaPlayer();
        MediaPlayer click = new MediaPlayer();
        MediaPlayer wrong = new MediaPlayer();

        // Enumération des niveaux de difficulté
        enum Difficulty { Facile, Moyen, Difficile }

        // Difficulté actuelle
        Difficulty currentDifficulty = Difficulty.Moyen;

        // Valeurs de base pour les vies et le temps
        int baseVies = 6;
        int baseTemps = 60;

        // Variables de jeu
        char guessletter = ' ';   // lettre actuellement devinée
        string motcache = "";     // mot à deviner
        int index = 0;            // position utilisée dans certaines boucles
        bool isMuted = false;     // état du son
        int vie = 6;              // nombre de vies restantes
        int jokersRestants = 0; // Nombre de jokers disponibles selon la difficulté

        // Liste de mots disponibles pour le jeu
        public List<string> ListeDeMots { get; } = new List<string>
        {
            // (liste complète des mots français)
            "abricot","accident","acier","actrice","adresse","aigle","aile","aimant","air","alarme",
            "album","algue","alliance","allie","alouette","amande","ami","amour","ananas","ancre",
            "ange","animal","anneau","annee","antilope","appareil","appel","arbre","argent","arme",
            "armoire","armure","arome","arret","article","asile","aspect","assiette","astre","atelier",
            "atome","attente","auberge","aube","audace","auteur","automne","aventure","avion","axe",
            "azur","bague","bagage","bal","balai","ballon","banc","bande","banque","bar","barbe",
            "barque","base","basilic","bateau","baton","batterie","beaute","bebe","bec","benefice",
            "berge","besoin","bete","bijou","billet","biere","ble","bloc","bois","boite","bonbon",
            "bonheur","bord","bouche","boulon","bouton","bras","brise","broche","brouillard","bruit",
            "bulle","bureau","bus","cabane","cabas","cabine","cable","cadeau","cafe","cage","caillou",
            "cake","calcul","calme","camp","canal","canard","canon","cantine","cap","capitaine","car",
            "caravane","carte","cascade","casque","cause","cave","ceinture","centre","cerf","cerise",
            "chagrin","chaise","champ","chance","chanson","chant","chaos","chapeau","char","chariot",
            "chat","chaussure","chef","chemin","cheval","chien","chiffre","chocolat","choix","chorale",
            "ciel","cime","citron","cite","classe","cle","climat","cloche","clou","coeur","coffre",
            "coin","col","colis","collier","colline","combat","comete","commerce","compagnon",
            "comptoir","concert","conducteur","confiance","conseil","contraste","coq","corde","corps",
            "cote","couleur","coup","cour","courant","couronne","course","crabe","crayon","crepuscule",
            "cri","crime","croix","cuisine","cuisse","cumulus","cure","cycle","dame","danger","danse",
            "debat","debut","defi","degre","delai","dent","desert","destin","detail","devoir",
            "diamant","dictionnaire","dieu","digue","dinde","diner","diplome","doute","dragon",
            "drapeau","droit","duree","ecole","ecran","ecriture","effort","eglise","elan","election",
            "elephant","element","emeraude","emotion","empire","emploi","encens","endroit","ennemi",
            "enfant","ensemble","entree","envie","epaule","epee","epoque","epreuve","erreur","escalier",
            "espace","espoir","esprit","essai","etable","etage","etang","etat","ete","etoile","etranger",
            "evenement","exemple","exil","experience","face","facteur","faim","famille","fantome",
            "farine","fatigue","faveur","fee","feu","feuille","fievre","fille","film","fil","fin",
            "fleur","fleuve","flocon","flore","flotte","foi","foie","foin","folie","fond","force",
            "foret","formule","fortune","fosse","foule","four","fraise","framboise","frere","front",
            "fruit","fume","funerailles","galerie","garde","garcon","gaz","gel","genou","geste",
            "geant","glace","gloire","gout","grain","gramme","grandpere","grange","grappe","greve",
            "gris","grotte","guerre","guide","habitude","haie","haine","halte","harpe","hasard",
            "hate","haut","herbe","heros","heure","hibou","histoire","hiver","homme","honneur",
            "horizon","hopital","horloge","huile","humeur","humour","idee","ile","image","immeuble",
            "impasse","imposteur","index","indice","industrie","infini","insecte","instant","insulte",
            "interet","inventeur","iris","isolement","jambe","janvier","jardin","jean","jeu","joie",
            "jour","journal","juge","juin","jumeau","jungle","jupiter","justice","kepi","lacet","lac",
            "lame","lampe","langue","lapin","larme","laser","lave","legende","lezard","liberte","lien",
            "lievre","ligne","lit","livre","lune","lutin","luxe","machine","magie","magasin","maillot",
            "main","maison","mal","manche","manoir","manteau","marbre","marchand","maree","mariage",
            "marin","marmite","masque","mat","matiere","medecin","memoire","melange","melodie",
            "membre","mensonge","mer","merci","metal","meteore","methode","midi","miel","milieu",
            "minute","miroir","modele","moment","monde","monnaie","montagne","monstre","montre",
            "morceau","mort","mot","moteur","moulin","mouvement","moyen","mur","musee","musique",
            "mystere","nage","nappe","nation","navire","neige","nid","noeud","nom","nombre","note",
            "nourriture","nuage","nuit","objet","odeur","oiseau","olive","ombre","onde","opera","orage",
            "orange","orchestre","ordre","outil","ours","ouverture","page","pain","palais","palme",
            "panda","panier","pantalon","papier","parfum","parole","part","passage","patience","pays",
            "peau","peche","pente","peuple","phare","phrase","photo","piece","pierre","pigeon","pilote",
            "pin","piscine","place","plage","plaisir","plan","plante","plateau","pluie","poeme","poids",
            "poignet","poil","point","poire","poisson","pomme","pont","porte","portefeuille","poste",
            "pot","pouce","poule","poussiere","pouvoir","prairie","present","prince","printemps",
            "prix","probleme","projet","promesse","protection","proverbe","public","puissance",
            "quartier","question","queue","quotidien","race","radio","raison","rang","rayon","realite",
            "recompense","reflet","regard","reve","rivage","riviere","robe","roi","rocher","rond",
            "rose","route","ruban","rue","sable","sage","saison","salle","salon","salut","sang","sapin",
            "savoir","science","scene","secret","secteur","sein","semaine","sentier","sentiment",
            "serpent","service","siecle","siege","signal","silence","singe","site","situation","socle",
            "soir","soleil","soldat","somme","sommet","son","sorcier","sort","sourire","souris",
            "souvenir","spectacle","statue","style","sujet","sucre","suite","surface","table","taille",
            "tache","talent","tapis","temps","terre","test","the","theatre","tigre","titre","tonnerre",
            "torrent","touche","tour","train","travail","tresor","tribu","trou","tulipe","tunnel","type",
            "univers","usine","vacance","vache","valeur","vallee","vent","verre","vers","veste",
            "village","ville","vin","violon","visage","voile","voisin","voix","vol","voyage","wagon",
            "zebre","zone"
        };

        // --- CONSTRUCTEUR PRINCIPAL ---
        public MainWindow()
        {
            InitializeComponent();

            // Configuration du volume global (entre 0.0 et 1.0)
            double volumeGeneral = 0.1; // ici 10% du volume

            // Chargement des sons
            win.Open(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "Sound", "win.wav")));
            lose.Open(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "Sound", "lose.wav")));
            click.Open(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "Sound", "click.wav")));
            wrong.Open(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "Sound", "wrong.wav")));

            // Application du volume
            win.Volume = volumeGeneral;
            lose.Volume = volumeGeneral;
            click.Volume = volumeGeneral;
            wrong.Volume = volumeGeneral;

            // Configuration du timer
            countdownTimer = new System.Windows.Threading.DispatcherTimer();
            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            countdownTimer.Tick += CountdownTimer_Tick;

            SetDifficulty(currentDifficulty, false);
        }


        // --- MÉTHODES DE GESTION DU JEU ---
        // Bouton pour recommencer une partie
        public void TB_Restart_Click(object sender, RoutedEventArgs e)
        {
            vie = baseVies;
            TB_Life.Text = $"Vies restantes: {vie}";
            TB_Display.Text = ""; // efface l'affichage
            Uri resourceUri = new Uri($"/Images/{vie}.png", UriKind.Relative);
            PenduImage.Source = new BitmapImage(resourceUri);
            Uri VieUri = new Uri($"/Vie/{vie}.png", UriKind.Relative);
            VieImage.Source = new BitmapImage(VieUri);
            ResetLetterButtonColors(this); // <-- Réinitialise les couleurs des lettres
            PrendreMotAleatoire(); // choisit un nouveau mot

            if (!isMuted)
            {
                click.Position = TimeSpan.Zero; // pour rejouer le son
                click.Play();
            }

            SetDifficulty(currentDifficulty, false);
            // Redémarre le compte à rebours
            countdownTimer.Stop();
            StartTimer();
        }

        // Définit le niveau de difficulté
        private void SetDifficulty(Difficulty diff, bool showMessage = true)
        {
            currentDifficulty = diff;

            switch (diff)
            {
                case Difficulty.Facile:
                    baseVies = 6;
                    baseTemps = 90;
                    jokersRestants = 3; // 3 jokers en facile
                    break;
                case Difficulty.Moyen:
                    baseVies = 6;
                    baseTemps = 60;
                    jokersRestants = 2; // 2 jokers en moyen
                    break;
                case Difficulty.Difficile:
                    baseVies = 6;
                    baseTemps = 45;
                    jokersRestants = 1; // 1 joker en difficile
                    break;
            }

            // Met à jour les affichages
            vie = baseVies;
            TB_Life.Text = $"Vies restantes: {vie}";
            timeRemaining = baseTemps;
            TB_Timer.Text = $"Temps restant : {timeRemaining}";
            TB_Joker.Text = $"Jokers restants : {jokersRestants}"; // 👈 ajoute un texte dans ton interface XAML
            Uri resourceUri = new Uri($"/Images/{vie}.png", UriKind.Relative);
            PenduImage.Source = new BitmapImage(resourceUri);
            Uri VieUri = new Uri($"/Vie/{vie}.png", UriKind.Relative);
            VieImage.Source = new BitmapImage(VieUri);

            // Relance une nouvelle partie
            TB_Display.Text = "";
            PrendreMotAleatoire();
            ResetLetterButtonColors(this); // <-- Réinitialise les couleurs
            countdownTimer.Stop();
            StartTimer();

            if (!isMuted)
            {
                click.Position = TimeSpan.Zero; // pour rejouer depuis le début
                click.Play();
            }

            if (showMessage)
                MessageBox.Show($"Difficulté définie sur : {diff}", "Niveau modifié", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Fonction Joker : révèle une lettre au hasard et retire une vie
        private void UtiliserJoker()
        {
            // Vérifie si on peut encore utiliser un joker
            if (jokersRestants <= 0)
            {
                MessageBox.Show("Tu n’as plus de jokers disponibles !", "Joker épuisé", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Vérifie qu’il reste des lettres à révéler
            if (!TB_Display.Text.Contains("-"))
            {
                MessageBox.Show("Le mot est déjà entièrement révélé !");
                return;
            }

            // Choisit un index au hasard parmi les lettres encore cachées
            Random rand = new Random();
            int index;

            do
            {
                index = rand.Next(motcache.Length);
            } while (TB_Display.Text[index] != '-');

            // Révèle la lettre correspondante
            var affichage = new StringBuilder(TB_Display.Text);
            affichage[index] = motcache[index];
            TB_Display.Text = affichage.ToString();

            // Enlève une vie
            vie--;
            TB_Life.Text = $"Vies restantes: {vie}";
            Uri resourceUri = new Uri($"/Images/{vie}.png", UriKind.Relative);
            PenduImage.Source = new BitmapImage(resourceUri);
            Uri VieUri = new Uri($"/Vie/{vie}.png", UriKind.Relative);
            VieImage.Source = new BitmapImage(VieUri);

            // Diminue le nombre de jokers restants
            jokersRestants--;
            TB_Joker.Text = $"Jokers restants : {jokersRestants}";

            if (!isMuted)
            {
                click.Position = TimeSpan.Zero; // pour rejouer depuis le début
                click.Play();
            }

            // Vérifie si la partie est terminée
            if (TB_Display.Text == motcache)
            {
                countdownTimer.Stop();
                if (!isMuted) win.Play();
                MessageBox.Show("Félicitations! Vous avez gagné!", "Victoire", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (vie <= 0)
            {
                countdownTimer.Stop();
                if (!isMuted)
                {
                    lose.Position = TimeSpan.Zero; // pour rejouer depuis le début
                    lose.Play();
                }
                MessageBox.Show($"Dommage! Vous avez perdu! Le mot était: {motcache}",
                                "Défaite", MessageBoxButton.OK, MessageBoxImage.Information);
                TB_Display.Text = motcache;
                AllLetterButtonsOff(this);
            }
        }
        private void TB_Joker_Click(object sender, RoutedEventArgs e)
        {
            UtiliserJoker();
        }

        // Réactive tous les boutons de lettres (A-Z)
        private void EnableAllLetterButtons(DependencyObject parent)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is Button btn && btn.Content is string content)
                {
                    if (content.Length == 1 && char.IsLetter(content[0]))
                        btn.IsEnabled = true;
                }
                else
                {
                    // Appel récursif pour trouver les boutons imbriqués
                    EnableAllLetterButtons(child);
                }
            }
        }

        // Désactive tous les boutons de lettres (lorsqu’on perd)
        private void AllLetterButtonsOff(DependencyObject parent)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is Button btn && btn.Content is string content)
                {
                    if (content.Length == 1 && char.IsLetter(content[0]))
                        btn.IsEnabled = false;
                }
                else
                {
                    AllLetterButtonsOff(child);
                }
            }
        }

        // Boutons de sélection de difficulté
        private void BTN_Easy_Click(object sender, RoutedEventArgs e) => SetDifficulty(Difficulty.Facile);
        private void BTN_Medium_Click(object sender, RoutedEventArgs e) => SetDifficulty(Difficulty.Moyen);
        private void BTN_Hard_Click(object sender, RoutedEventArgs e) => SetDifficulty(Difficulty.Difficile);

        // Active/Désactive le son
        public void TB_Mute_Click(object sender, RoutedEventArgs e)
        {
            isMuted = !isMuted;
            TB_Mute.Content = isMuted ? "🔇 Son OFF" : "🔊 Son ON";
            if (!isMuted)
            {
                click.Position = TimeSpan.Zero; // pour rejouer depuis le début
                click.Play();
            }
        }

        // Démarre le compte à rebours
        private void StartTimer()
        {
            timeRemaining = baseTemps;
            TB_Timer.Text = $"Temps restant : {timeRemaining}";
            countdownTimer.Start();
        }

        // Événement déclenché chaque seconde
        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            timeRemaining--;
            TB_Timer.Text = $"Temps restant : {timeRemaining}";

            if (timeRemaining <= 0)
            {
                countdownTimer.Stop();
                if (!isMuted)
                {
                    lose.Position = TimeSpan.Zero; // pour rejouer depuis le début
                    lose.Play();
                }

                MessageBox.Show($"Temps écoulé ! Vous avez perdu 😢\nLe mot était : {motcache}",
                                "Temps écoulé", MessageBoxButton.OK, MessageBoxImage.Information);

                TB_Display.Text = motcache; // révèle le mot
            }
        }

        //Arrète l'application
        private void btnArreter_Click(object sender, RoutedEventArgs e)
        {
            // Par exemple, fermer l'application
            Application.Current.Shutdown();
        }

        // Sélectionne un mot au hasard dans la liste
        public void PrendreMotAleatoire()
        {
            Random rand = new Random();
            List<string> motsFiltres = new List<string>();

            // Filtrage des mots selon la difficulté actuelle
            switch (currentDifficulty)
            {
                case Difficulty.Facile:
                    motsFiltres = ListeDeMots.FindAll(m => m.Length <= 6);
                    break;
                case Difficulty.Moyen:
                    motsFiltres = ListeDeMots.FindAll(m => m.Length > 6 && m.Length <10);
                    break;
                case Difficulty.Difficile:
                    motsFiltres = ListeDeMots.FindAll(m => m.Length >= 10);
                    break;
            }

            // Sécurité : si la liste filtrée est vide (par précaution)
            if (motsFiltres.Count == 0)
                motsFiltres = ListeDeMots;

            // Choisir un mot au hasard dans la liste filtrée
            int N = rand.Next(motsFiltres.Count);
            motcache = motsFiltres[N];

            // Prépare l'affichage avec des tirets
            TB_Display.Text = "";
            for (int i = 0; i < motcache.Length; i++)
                TB_Display.Text += "-";
        }

        // Quand une lettre est cliquée
        public void letter_clicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                // Récupère la lettre du bouton et la désactive
                guessletter = btn.Content.ToString().ToLower()[0];
                btn.IsEnabled = false;

                // Vérifie la lettre
                BTN_Guess_Click(guessletter, btn);

                if (!isMuted)
                {
                    click.Position = TimeSpan.Zero; // pour rejouer depuis le début
                    click.Play();
                }
            }
        }
        
        // Réinitialise les couleurs de tous les boutons de lettres
        private void ResetLetterButtonColors(DependencyObject parent)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is Button btn && btn.Content is string content)
                {
                    if (content.Length == 1 && char.IsLetter(content[0]))
                    {
                        btn.IsEnabled = true;
                        btn.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#607D8B")); // ta couleur par défaut
                        btn.Foreground = new SolidColorBrush(Colors.White); // texte blanc pour bon contraste
                    }
                }
                else
                {
                    ResetLetterButtonColors(child); // récursif sur les sous-éléments
                }
            }
        }

        // Vérifie si la lettre proposée est dans le mot
        private void BTN_Guess_Click(char letter, Button btn)
        {
            if (motcache.Contains(letter))
            {
                // --- Lettre correcte ---
                btn.Background = new SolidColorBrush(Color.FromRgb(144, 238, 144)); // vert clair
                btn.Foreground = new SolidColorBrush(Colors.Black);
                btn.IsEnabled = false;

                var affichage = new StringBuilder(TB_Display.Text);
                int index = 0;
                foreach (var l in motcache)
                {
                    if (l == letter)
                    {
                        affichage[index] = letter;
                    }
                    index++;
                }
                TB_Display.Text = affichage.ToString();

                if (TB_Display.Text == motcache)
                {
                    countdownTimer.Stop();
                    if (!isMuted)
                    {
                        win.Position = TimeSpan.Zero; // pour rejouer depuis le début
                        win.Play();
                    }
                    MessageBox.Show("Félicitations! Vous avez gagné!", "Victoire", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                // --- Lettre incorrecte ---
                btn.Background = new SolidColorBrush(Color.FromRgb(255, 182, 193)); // rose/rouge clair
                btn.Foreground = new SolidColorBrush(Colors.Black);
                btn.IsEnabled = false;

                if (!isMuted)
                {
                    wrong.Position = TimeSpan.Zero; // pour rejouer depuis le début
                    wrong.Play();
                }
                vie--;
                TB_Life.Text = $"Vies restantes: {vie}";
                Uri resourceUri = new Uri($"/Images/{vie}.png", UriKind.Relative);
                PenduImage.Source = new BitmapImage(resourceUri);
                Uri VieUri = new Uri($"/Vie/{vie}.png", UriKind.Relative);
                VieImage.Source = new BitmapImage(VieUri);

                if (vie == 0)
                {
                    countdownTimer.Stop();
                    if (!isMuted) lose.Play();
                    MessageBox.Show($"Dommage! Vous avez perdu! Le mot était: {motcache}",
                                    "Défaite", MessageBoxButton.OK, MessageBoxImage.Information);

                    TB_Display.Text = motcache;
                    AllLetterButtonsOff(this);
                }
            }
        }
    }
}