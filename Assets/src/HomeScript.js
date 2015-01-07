function OnGUI() {
    username = ApplicationModel.username; // récupère la variable static
    GUI.Label(Rect (90, 10, 100, 20), 'Hello ' + username); // et l'affiche
}