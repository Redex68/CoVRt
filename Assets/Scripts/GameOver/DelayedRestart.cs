using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DelayedRestart : MonoBehaviour
{
   [SerializeField] float delay;

   public void Restart()
   {
        StartCoroutine(DoRestart());
   }

   private IEnumerator DoRestart()
   {
        yield return new WaitForSeconds(delay);
        State.gameOver = false;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
   }
}
