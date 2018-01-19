using UnityEngine;

public class PlayButton : MonoBehaviour {

    public void PlayButtonPressed() {
        FindObjectOfType<HomeController>().LoadGame();
    }
}
