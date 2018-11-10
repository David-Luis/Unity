using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeUIController : MonoBehaviour {

	public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }
}
