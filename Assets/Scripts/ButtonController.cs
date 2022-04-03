using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{

    
    public void PlayButton()
    {
        SceneManager.LoadScene("InstructionsScene");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
