using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public Animator anim;
    public int levelToLoad;

    public async void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetTrigger("fade");

            await Task.Delay(2500);

            SceneManager.LoadScene(levelToLoad);
        }
    }
}
