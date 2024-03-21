using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public Animator anim;
    public int levelToLoad;

    public async void OnFadeComlete()
    {
        anim.SetTrigger("fade");

        await Task.Delay(2500);

        SceneManager.LoadScene(levelToLoad);
    }
}
