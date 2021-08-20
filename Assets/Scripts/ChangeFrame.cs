using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeFrame : MonoBehaviour
{
    Scene currentScene;
    public int sceneIndex = 0;
    public Animator canvasAnimator;

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            PlayerController.Instance.canMove = false;

            if (currentScene.buildIndex == 0)
                StartCoroutine(LoadLevel(currentScene.buildIndex + 1));

            if (currentScene.buildIndex == 1)
                StartCoroutine(LoadLevel(currentScene.buildIndex - 1));

            PlayerController.Instance.canMove = true;
        }
        
    }

    IEnumerator LoadLevel(int index)
    {
        canvasAnimator.SetTrigger("Fade");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(index);

    }
}
