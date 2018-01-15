using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour
{

    private const string LEVEL_01 = "Level_01_Ward";


    void Start () {
		// update high score board
	}

    public void LoadGame()
    {
        SceneManager.LoadScene(LEVEL_01);
    }
	


}
