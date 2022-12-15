using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    private int nivel = 0;
    private string nivelPref = "nivelPref";

    private int exp = 0;
    private string expPref = "expPref";

    private int pts = 0;
    private string ptsPref = "ptsPref";


    public void jugar()
    {
        loadData();
        nivel = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void salir()
    {
        Application.Quit();
    }
    private void OnDestroy()
    {
        saveData();
    }
    private void saveData()
    {
        PlayerPrefs.SetInt(nivelPref, nivel);
        PlayerPrefs.SetInt(expPref, exp);
        PlayerPrefs.SetInt(ptsPref, pts);
    }
    private void loadData()
    {
        nivel = PlayerPrefs.GetInt(nivelPref, 0);
        exp = PlayerPrefs.GetInt(expPref, 0);
        pts = PlayerPrefs.GetInt(ptsPref, 0);
    }


}
