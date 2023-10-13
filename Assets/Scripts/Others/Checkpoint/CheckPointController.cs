using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointController : MonoBehaviour
{
    [SerializeField] private AudioSource checkpoint_Sound;
    private bool checkActivated = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !checkActivated)
        {
            checkActivated = true; //Make sure only activate once
            checkpoint_Sound.Play();
            Invoke("LoadScene", 2f); //Call "function" after 2sec
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Switch next scene 0->1
    }
}
