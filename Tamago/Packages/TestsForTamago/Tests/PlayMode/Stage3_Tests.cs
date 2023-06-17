using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

[Description("Chicken And Egg Problem"), Category("3")]
public class Stage3_Tests
{
    private GameObject egg;
    private Transform eggT;
    private Vector2 scaleBig, scaleSmall;
    [UnityTest]
    public IEnumerator TapResizeCheck()
    {
        PlayerPrefs.DeleteAll();
        if (!Application.CanStreamedLevelBeLoaded("Game"))
        {
            Assert.Fail("\"Game\" scene is misspelled or was not added to build settings");
        }
        SceneManager.LoadScene("Game");

        float start = Time.unscaledTime;
        yield return new WaitUntil(() =>
            SceneManager.GetActiveScene().name == "Game" || (Time.unscaledTime - start) * Time.timeScale > 1);
        if (SceneManager.GetActiveScene().name != "Game")
        {
            Assert.Fail("\"Game\" scene can't be loaded");
        }

        Time.timeScale = 1;
        
        egg = GameObject.Find("Egg");
        yield return null;
        eggT = PMHelper.Exist<Transform>(egg);
        yield return null;
        scaleBig = eggT.localScale;

        EditorWindow game;
        double X, Y;
        (game, X, Y) = PMHelper.GetCoordinatesOnGameWindow(0.5f, 0.5f);
        
        if (!game)
        {
            Assert.Fail("Please, open, the \"Game\" window!");
        }
        
        VInput.MoveMouseTo(X,Y);
        yield return null;
        VInput.LeftButtonDown();

        start = Time.unscaledTime;
        yield return new WaitUntil(() => eggT.localScale.x < scaleBig.x &&
                                         eggT.localScale.y < scaleBig.y ||
                                         (Time.unscaledTime - start) * Time.timeScale > 1);
        if ((Time.unscaledTime - start) * Time.timeScale > 1)
        {
            Assert.Fail("\"Egg\" object should become smaller if the player pressed the LMB key, " +
                        "so it's scale by x and y axis should decrease");
        }

        scaleSmall = eggT.localScale;
        
        VInput.LeftButtonUp();
        
        start = Time.unscaledTime;
        yield return new WaitUntil(() => (Vector2)eggT.localScale == scaleBig ||
                                         (Time.unscaledTime - start) * Time.timeScale > 1);
        if ((Time.unscaledTime - start) * Time.timeScale > 1)
        {
            Assert.Fail("\"Egg\" object should resize back if the player released the LMB key, " +
                        "so it's scale by x and y axis should increase to initial size");
        }

        for (int i = 0; i < 10; i++)
        {
            VInput.LeftButtonDown();

            start = Time.unscaledTime;
            yield return new WaitUntil(() => (Vector2)eggT.localScale == scaleSmall ||
                                             (Time.unscaledTime - start) * Time.timeScale > 1);
            if ((Time.unscaledTime - start) * Time.timeScale > 1)
            {
                Assert.Fail("\"Egg\" object's scale properties should be the same " +
                            "every time the player presses the LMB key");
            }

            scaleSmall = eggT.localScale;
        
            VInput.LeftButtonUp();
        
            start = Time.unscaledTime;
            yield return new WaitUntil(() => (Vector2)eggT.localScale == scaleBig ||
                                             (Time.unscaledTime - start) * Time.timeScale > 1);
            if ((Time.unscaledTime - start) * Time.timeScale > 1)
            {
                Assert.Fail("\"Egg\" object's scale properties should be the same " +
                            "every time the player releases the LMB key (initial size)");
            }
        }

        game.maximized = false;
    }
}
