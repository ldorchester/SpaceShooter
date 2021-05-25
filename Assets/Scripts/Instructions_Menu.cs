using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Instructions_Menu : MonoBehaviour
{
    public void LoadMainMenu()
    {
        //load the game scene
        SceneManager.LoadScene(0);
    }

}
