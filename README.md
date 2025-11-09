# Hangman Esteban
🎮 Hangman_Esteban
🧩 Présentation

Hangman_Esteban est un jeu du Pendu réalisé en C# / WPF.
Le joueur doit deviner un mot caché en proposant des lettres.
Chaque erreur fait perdre une vie, et un compte à rebours limite le temps disponible.
Des sons, difficultés ajustables et jokers rendent le jeu plus dynamique et immersif.

🚀 Fonctionnalités principales

✅ Trois niveaux de difficulté :

Facile → plus de temps, plus de jokers

Moyen → équilibre entre durée et difficulté

Difficile → temps limité et peu de jokers

✅ Gestion du temps :

Un minuteur de compte à rebours avec affichage du temps restant.

Si le temps atteint zéro, la partie est perdue.

✅ Système de vies :

Le joueur démarre avec un certain nombre de vies (6 par défaut).

Chaque erreur retire une vie et met à jour l’image du pendu.

✅ Système de jokers :

Permet de révéler une lettre aléatoire du mot.

Consomme une vie et un joker.

✅ Gestion des sons :

Sons de victoire, défaite, clics, et erreur.

Option pour désactiver/réactiver le son.

Volume global ajustable.

✅ Bouton Arrêter :

Permet de fermer proprement l’application.

🖥️ Technologies utilisées
Élément	Description
Langage	C# (.NET WPF)
Framework UI	Windows Presentation Foundation (WPF)
Audio	MediaPlayer pour lire les fichiers .wav
Timer	DispatcherTimer pour le compte à rebours
Interface	XAML (liée à MainWindow.xaml.cs)
