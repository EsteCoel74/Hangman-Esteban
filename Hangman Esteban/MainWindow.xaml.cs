// Importation des bibliothèques pour le système, collections, son et interface WPF
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
    // Classe principale du jeu "MainWindow"
    public partial class MainWindow : Window
    {
        // --- VARIABLES GLOBALES ---

        // Timer utilisé pour le compte à rebours
        System.Windows.Threading.DispatcherTimer countdownTimer;

        // Temps restant en secondes
        int timeRemaining = 60;

        // Lecteurs audio pour les sons du jeu
        MediaPlayer win = new MediaPlayer();
        MediaPlayer lose = new MediaPlayer();
        MediaPlayer click = new MediaPlayer();
        MediaPlayer wrong = new MediaPlayer();

        // Enumération pour les niveaux de difficulté
        enum Difficulty { Facile, Moyen, Difficile }

        // Difficulté actuellement active
        Difficulty currentDifficulty = Difficulty.Moyen;

        // Paramètres par défaut selon la difficulté
        int baseVies = 6;
        int baseTemps = 60;

        // Variables de jeu
        char guessletter = ' ';   // lettre actuellement tentée
        string motcache = "";     // mot à deviner
        int index = 0;            // index utilisé dans certaines boucles
        bool isMuted = false;     // indique si le son est coupé
        int vie = 6;              // vies restantes
        int jokersRestants = 0;   // nombre de jokers selon la difficulté

        // Liste de mots disponibles pour le jeu
        public List<string> ListeDeMots { get; } = new List<string>
        {
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
            InitializeComponent(); // initialise l'interface WPF

            double volumeGeneral = 0.1; // Volume sonore global à 10%

            // Chargement des fichiers audio
            win.Open(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "Sound", "win.wav")));
            lose.Open(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "Sound", "lose.wav")));
            click.Open(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "Sound", "click.wav")));
            wrong.Open(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "Sound", "wrong.wav")));

            // Application du volume à chaque son
            win.Volume = volumeGeneral;
            lose.Volume = volumeGeneral;
            click.Volume = volumeGeneral;
            wrong.Volume = volumeGeneral;

            // Configuration du timer
            countdownTimer = new System.Windows.Threading.DispatcherTimer();
            countdownTimer.Interval = TimeSpan.FromSeconds(1);  // déclenche chaque seconde
            countdownTimer.Tick += CountdownTimer_Tick;          // associe l’événement

            // Applique la difficulté par défaut (Moyen)
            SetDifficulty(currentDifficulty, false);
        }


        // --- MÉTHODES DE GESTION DU JEU ---
        // Bouton pour recommencer une partie
        public void TB_Restart_Click(object sender, RoutedEventArgs e)
        {
            // Réinitialise le nombre de vies
            vie = baseVies;
            TB_Life.Text = $"Vies restantes: {vie}";

            TB_Display.Text = ""; // efface l'affichage du mot

            // Met à jour l'image du pendu selon les vies
            Uri resourceUri = new Uri($"/Images/{vie}.png", UriKind.Relative);
            PenduImage.Source = new BitmapImage(resourceUri);

            // Image des vies restantes
            Uri VieUri = new Uri($"/Vie/{vie}.png", UriKind.Relative);
            VieImage.Source = new BitmapImage(VieUri);

            // Réactive toutes les lettres visuellement
            ResetLetterButtonColors(this);

            // choisit un nouveau mot aléatoire
            PrendreMotAleatoire();

            // Joue un son (si non muet)
            if (!isMuted)
            {
                click.Position = TimeSpan.Zero;
                click.Play();
            }

            // Réinitialise la difficulté courante
            SetDifficulty(currentDifficulty, false);

            // Redémarre le timer
            countdownTimer.Stop();
            StartTimer();
        }

        // Définit le niveau de difficulté
       private void SetDifficulty(Difficulty diff, bool showMessage = true)
        {
            currentDifficulty = diff; // enregistre la difficulté choisie

            // Change les valeurs vies/temps/jokers selon la difficulté
            switch (diff)
            {
                case Difficulty.Facile:
                    baseVies = 6;
                    baseTemps = 90;
                    jokersRestants = 3;
                    break;
                case Difficulty.Moyen:
                    baseVies = 6;
                    baseTemps = 60;
                    jokersRestants = 2;
                    break;
                case Difficulty.Difficile:
                    baseVies = 6;
                    baseTemps = 45;
                    jokersRestants = 1;
                    break;
            }

            // Met à jour l’interface
            vie = baseVies;
            TB_Life.Text = $"Vies restantes: {vie}";
            timeRemaining = baseTemps;
            TB_Timer.Text = $"Temps restant : {timeRemaining}";
            TB_Joker.Text = $"Jokers restants : {jokersRestants}";

            // Mets à jour les images
            Uri resourceUri = new Uri($"/Images/{vie}.png", UriKind.Relative);
            PenduImage.Source = new BitmapImage(resourceUri);

            // Relance une partie
            TB_Display.Text = "";
            PrendreMotAleatoire();
            ResetLetterButtonColors(this);

            countdownTimer.Stop();
            StartTimer();

            // son de clic si non muet
            if (!isMuted)
            {
                click.Position = TimeSpan.Zero;
                click.Play();
            }

            // popup d'information
            if (showMessage)
                MessageBox.Show($"Difficulté définie sur : {diff}", "Niveau modifié");
        }

        // Fonction Joker : révèle une lettre au hasard et retire une vie
         private void UtiliserJoker()
        {
            // Empêche l’utilisation si plus de jokers
            if (jokersRestants <= 0)
            {
                MessageBox.Show("Tu n’as plus de jokers disponibles !");
                return;
            }

            // Vérifie s’il reste des lettres cachées
            if (!TB_Display.Text.Contains("-"))
            {
                MessageBox.Show("Le mot est déjà entièrement révélé !");
                return;
            }

            // Sélectionne une lettre cachée au hasard
            Random rand = new Random();
            int index;
            do
            {
                index = rand.Next(motcache.Length);
            } while (TB_Display.Text[index] != '-');

            // Remplace le tiret par la lettre correspondante
            var affichage = new StringBuilder(TB_Display.Text);
            affichage[index] = motcache[index];
            TB_Display.Text = affichage.ToString();

            // Retire une vie
            vie--;
            TB_Life.Text = $"Vies restantes: {vie}";

            // Met à jour images
            Uri resourceUri = new Uri($"/Images/{vie}.png", UriKind.Relative);
            PenduImage.Source = new BitmapImage(resourceUri);

            // Diminue le nombre de jokers restants
            jokersRestants--;
            TB_Joker.Text = $"Jokers restants : {jokersRestants}";

            // son
            if (!isMuted)
            {
                click.Position = TimeSpan.Zero;
                click.Play();
            }

            // Si mot trouvé
            if (TB_Display.Text == motcache)
            {
                countdownTimer.Stop();
                if (!isMuted) win.Play();
                MessageBox.Show("Félicitations! Vous avez gagné!");
            }
            // Si joueur à 0 vie
            else if (vie <= 0)
            {
                countdownTimer.Stop();
                if (!isMuted) lose.Play();

                MessageBox.Show($"Dommage! Vous avez perdu! Le mot était: {motcache}");
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

            // Filtre la liste selon la difficulté
            switch (currentDifficulty)
            {
                case Difficulty.Facile:
                    motsFiltres = ListeDeMots.FindAll(m => m.Length <= 6);
                    break;
                case Difficulty.Moyen:
                    motsFiltres = ListeDeMots.FindAll(m => m.Length > 6 && m.Length < 10);
                    break;
                case Difficulty.Difficile:
                    motsFiltres = ListeDeMots.FindAll(m => m.Length >= 10);
                    break;
            }

            // Sécurité si liste vide
            if (motsFiltres.Count == 0)
                motsFiltres = ListeDeMots;

            // Tire un mot au hasard
            int N = rand.Next(motsFiltres.Count);
            motcache = motsFiltres[N];

            // Affiche uniquement des tirets
            TB_Display.Text = new string('-', motcache.Length);
        }

        // Quand une lettre est cliquée
                public void letter_clicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                // récupère la lettre du bouton
                guessletter = btn.Content.ToString().ToLower()[0];

                // désactive le bouton
                btn.IsEnabled = false;

                // teste si la lettre appartient au mot
                BTN_Guess_Click(guessletter, btn);

                // joue le clic
                if (!isMuted)
                {
                    click.Position = TimeSpan.Zero;
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
            if (motcache.Contains(letter))  // lettre correcte
            {
                // bouton devient vert clair
                btn.Background = new SolidColorBrush(Color.FromRgb(144, 238, 144));
                btn.Foreground = new SolidColorBrush(Colors.Black);

                // remplace les tirets dans l'affichage
                var affichage = new StringBuilder(TB_Display.Text);

                for (int i = 0; i < motcache.Length; i++)
                    if (motcache[i] == letter)
                        affichage[i] = letter;

                TB_Display.Text = affichage.ToString();

                // victoire ?
                if (TB_Display.Text == motcache)
                {
                    countdownTimer.Stop();
                    if (!isMuted) win.Play();
                    MessageBox.Show("Félicitations! Vous avez gagné!");
                }
            }
            else  // lettre incorrecte
            {
                // bouton devient rouge clair
                btn.Background = new SolidColorBrush(Color.FromRgb(255, 182, 193));
                btn.Foreground = new SolidColorBrush(Colors.Black);

                if (!isMuted)
                {
                    wrong.Position = TimeSpan.Zero;
                    wrong.Play();
                }

                // enlève une vie
                vie--;
                TB_Life.Text = $"Vies restantes: {vie}";

                // met à jour les images
                Uri resourceUri = new Uri($"/Images/{vie}.png", UriKind.Relative);
                PenduImage.Source = new BitmapImage(resourceUri);

                // défaite ?
                if (vie == 0)
                {
                    countdownTimer.Stop();
                    if (!isMuted) lose.Play();

                    MessageBox.Show($"Dommage! Vous avez perdu! Le mot était: {motcache}");
                    TB_Display.Text = motcache;
                    AllLetterButtonsOff(this);
                }
            }
        }
    }
}
