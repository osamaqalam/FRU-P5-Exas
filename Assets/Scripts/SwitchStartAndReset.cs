using UnityEngine;

public class SwitchStartAndReset : MonoBehaviour
{
    public GameObject startButton;
    public GameObject resetButton;

    public void SwitchButtons ()
    {
        if (startButton != null && resetButton != null)
        {
            if (startButton.activeSelf)
            { 
                startButton.SetActive(false);
                resetButton.SetActive(true);
            }
            else
            {
                startButton.SetActive(true);
                resetButton.SetActive(false);
            }

        }    
    }

}
