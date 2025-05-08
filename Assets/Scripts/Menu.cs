using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public void loadScene(string escena) 
    {
    
    SceneManager.LoadScene(escena); 
    
    
    }

    public void QuitGame()
    {

        Application.Quit();
        Debug.Log("Adios papu");
        

    }

}
