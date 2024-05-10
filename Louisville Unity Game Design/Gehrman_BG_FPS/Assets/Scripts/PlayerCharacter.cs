using UnityEngine;
using System.Collections;
using System.Threading.Tasks;


public class PlayerCharacter : MonoBehaviour
{
    private int _health;

    [SerializeField] private AudioClip gameover;
    [SerializeField] private AudioClip playerHit;
    CharacterController cc;

    void Start()
    {
        _health = 10;
        cc = (CharacterController)GetComponent<CharacterController>();

    }

    async public void Hurt(int damage)
    {
        GetComponent<AudioSource>().PlayOneShot(playerHit);
        _health -= damage;
        print("Health: " + _health);
        if (_health == 0)
        {
            GetComponent<AudioSource>().PlayOneShot(gameover);
            print("GameOver");
            await Task.Delay(1000);
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}
