🎮 Hangman_Esteban

Un jeu du Pendu en C# / WPF, développé par Esteban (Moi)
Interface fluide, sons immersifs, et plusieurs niveaux de difficulté ⚡

🧩 Aperçu

Hangman_Esteban est une version revisitée du jeu du Pendu, réalisée avec Windows Presentation Foundation (WPF).
Le but : deviner un mot avant que le temps ou les vies ne s’épuisent ⏳💀

Le jeu inclut :

Des sons dynamiques pour les actions (victoire, défaite, clic, erreur)

Trois niveaux de difficulté : Facile, Moyen, Difficile

Un minuteur et des jokers

Une interface moderne et intuitive

🚀 Fonctionnalités

Fonction	Description

🎚️ Difficulté réglable	Trois niveaux : Facile, Moyen, Difficile

🕐 Timer intégré	Compte à rebours variable selon la difficulté

❤️ Vies limitées	6 essais maximum avant le game over

🃏 Jokers	Révèlent une lettre au hasard (quantité dépendante de la difficulté)

🔈 Gestion du son	Activation/désactivation du son depuis l’interface

🎵 Effets audio	Sons pour victoire, défaite, clics, erreurs

🖼️ Images dynamiques	Le pendu évolue à chaque erreur

🔘 Bouton “Arrêter”	Permet de quitter proprement le jeu

🖥️ Technologies utilisées :

- Langage : C# (.NET 6 ou supérieur)

- Framework : WPF (Windows Presentation Foundation)

- Interface : XAML

- Audio : MediaPlayer

- Timer : DispatcherTimer

📁 Structure du projet

Hangman_Esteban/

├── Sound/

    ├── win.wav
    
    ├── lose.wav
    
    ├── click.wav
    
    └── wrong.wav

├── Images/

    ─ 6.png
    
    ─ 5.png
    
    ─ ...

├── Vie/

    ─ 6.png
    ─ 5.png
    ─ ...

├── MainWindow.xaml

├── MainWindow.xaml.cs

└── README.md


🗂️ Note :

Les fichiers audio sont stockés dans Sound/

Les images du pendu et des vies sont dans Images/ et Vie/

Les chemins audio et image sont relatifs, donc fonctionnent directement après compilation

⚙️ Installation & Exécution

🔧 Étape 1 — Cloner le projet
git clone https://github.com/<ton-pseudo>/Hangman_Esteban.git

🏗️ Étape 2 — Ouvrir le projet

Ouvre le fichier .sln dans Visual Studio 2022 ou plus récent.

▶️ Étape 3 — Exécuter

Appuie sur F5 pour lancer le jeu.

La fenêtre principale s’ouvre avec le menu du pendu.

Clique sur les lettres pour deviner le mot !

🎮 Règles du jeu

Devine le mot caché en cliquant sur les lettres de l’alphabet.

Chaque erreur te retire une vie.

Si tu perds toutes tes vies, le pendu est complet et c’est perdu 😢

Utilise tes jokers pour révéler des lettres, mais attention, ils coûtent une vie !

Gagne avant que le temps ne s’écoule !

🧠 Logique du code

Le cœur du jeu repose sur :

Un timer (DispatcherTimer) pour la gestion du temps.

Des MediaPlayer distincts pour les sons (win, lose, click, wrong).

Une liste de mots filtrée selon la difficulté.

Une gestion dynamique de l’interface (couleurs, images, lettres, etc.).

🔊 Paramètres audio

Les sons sont chargés au lancement du jeu :

- win.Open(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "Sound", "win.wav")));

- lose.Open(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "Sound", "lose.wav")));

- click.Open(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "Sound", "click.wav")));

- wrong.Open(new Uri(System.IO.Path.Combine(Environment.CurrentDirectory, "Sound", "wrong.wav")));



Le volume global est réglé ici :

double volumeGeneral = 0.1; // 10% du volume

🧑‍💻 Auteur :

- Esteban

💡 Développeur passionné par le C#, WPF et la création de jeux éducatifs.
📫 Tu peux me retrouver sur GitHub : github.com/<ton-pseudo>

📜 Licence

🆓 Ce projet est libre d’utilisation et de modification à des fins éducatives ou personnelles.
Si tu réutilises le code, pense à mentionner l’auteur original ❤️

🌟 Aperçu futur (idées d’amélioration)

🔢 Ajout d’un score ou d’un système de points

🌈 Thèmes visuels personnalisables

🏆 Tableau des meilleurs scores

💬 Mots issus d’un dictionnaire en ligne

-Interface	XAML (liée à MainWindow.xaml.cs)
