using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoad : MonoBehaviour
{
    public void LoadSceneByID(int id) {
        SceneManager.LoadScene(id);
    }

    public void LoadSceneByName(string name) {
        SceneManager.LoadScene(name);
    }
}
