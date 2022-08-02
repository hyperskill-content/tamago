using System;
using System.Collections;
using WindowsInput;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

[Description("Put All One’s Eggs In A Basket"), Category("4")]
public class Stage4_Tests
{
    private InputSimulator IS = new InputSimulator();
    private Text text;
    private string textBefore;
    private int count, count2;

    [UnityTest]
    public IEnumerator TapDecreaseCheck()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Game");

        float start = Time.unscaledTime;
        yield return new WaitUntil(() =>
            SceneManager.GetActiveScene().name == "Game" || (Time.unscaledTime - start) * Time.timeScale > 1);
        if (SceneManager.GetActiveScene().name != "Game")
        {
            Assert.Fail("\"Game\" scene can't be loaded");
        }

        Time.timeScale = 1;

        text = PMHelper.Exist<Text>(GameObject.Find("Countdown"));
        yield return null;

        count = -1;
        textBefore = text.text;
        
        try
        {
            count = Int32.Parse(text.text);
        }
        catch
        {
            Assert.Fail("The initial value of 'Text' property of \"Countdown\" object's \"Text\" component" +
                        " should be integer");
        }

        if (count != 100)
        {
            Assert.Fail("The initial value of 'Text' property of \"Countdown\" object's \"Text\" component" +
                        " should be equal to '100'");
        }

        EditorWindow game;
        double X, Y;
        (game, X, Y) = PMHelper.GetCoordinatesOnGameWindow(0.5f, 0.5f);
        
        if (!game)
        {
            Assert.Fail("Please, open, the \"Game\" window!");
        }
        IS.Mouse.MoveMouseTo(X, Y);
        yield return null;
        IS.Mouse.LeftButtonClick();

        start = Time.unscaledTime;
        yield return new WaitUntil(() => !text.text.Equals(textBefore) ||
                                         (Time.unscaledTime - start) * Time.timeScale > 1);
        if ((Time.unscaledTime - start) * Time.timeScale > 1)
        {
            Assert.Fail("'Text' property of \"Countdown\" object's \"Text\" component" +
                        " should change if the player pressed the LMB key");
        }
        
        try
        {
            count2 = Int32.Parse(text.text);
        }
        catch
        {
            Assert.Fail("Amount of taps left should decrease by 1 if the player pressed the LMB key");
        }

        if (count2 + 1 != count)
        {
            Assert.Fail("Amount of taps left should decrease by 1 if the player pressed the LMB key");
        }

        textBefore = text.text;
        count = count2;

        for (int i = 0; i < 5; i++)
        {
            IS.Mouse.LeftButtonClick();

            start = Time.unscaledTime;
            yield return new WaitUntil(() => !text.text.Equals(textBefore) ||
                                             (Time.unscaledTime - start) * Time.timeScale > 1);
            if ((Time.unscaledTime - start) * Time.timeScale > 1)
            {
                Assert.Fail("'Text' property of \"Countdown\" object's \"Text\" component" +
                            " should change if the player pressed the LMB key");
            }

            try
            {
                count2 = Int32.Parse(text.text);
            }
            catch
            {
                Assert.Fail("Amount of taps left should decrease by 1 if the player pressed the LMB key");
            }

            if (count2 + 1 != count)
            {
                Assert.Fail("Amount of taps left should decrease by 1 if the player pressed the LMB key");
            }

            textBefore = text.text;
            count = count2;
        }
    }
}
