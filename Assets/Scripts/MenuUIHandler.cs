using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.LowLevelPhysics2D.PhysicsLayers;

public class MenuUIHandler : MonoBehaviour
{


    public TMP_InputField nameInputField;
    public static string playerName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void StartNew()
    {
        playerName = nameInputField.text;
        SceneManager.LoadScene(1);
    }



    // Update is called once per frame
   
    public void Exit()
    {
#if UNITY_EDITOR

        EditorApplication.ExitPlaymode();
#else

        Application.Quit();
#endif

    }
}
