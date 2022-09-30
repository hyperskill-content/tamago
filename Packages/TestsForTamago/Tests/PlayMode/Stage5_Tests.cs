using System;
using System.Collections;
using System.Runtime.InteropServices;
using WindowsInput;
using LinuxInput;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

[Description("Make An Omelet Without Breaking Some Eggs"), Category("5")]
public class Stage5_Tests
{
    private InputSimulator IS = new InputSimulator();
    private LinuxInputSimulator ISL = new LinuxInputSimulator();
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

        count=-1;
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

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            IS.Mouse.MoveMouseTo(X, Y);
        else
            ISL.Mouse.MoveMouseTo(X, Y);
        yield return null;

        for (int i = 0; i < 5; i++)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                IS.Mouse.LeftButtonClick();
            else
                ISL.Mouse.LeftButtonClick();

            start = Time.unscaledTime;
            yield return new WaitUntil(() => !text.text.Equals(textBefore) ||
                                             (Time.unscaledTime - start) * Time.timeScale > 1);
            if ((Time.unscaledTime - start) * Time.timeScale > 1)
            {
                Assert.Fail();
            }
            textBefore = text.text;
        }
        try
        {
            count = Int32.Parse(textBefore);
        }
        catch
        {
            Assert.Fail("The value of 'Text' property of \"Countdown\" object's \"Text\" component" +
                        " should be integer until it reaches 0 taps left");
        }

        Scene prev = SceneManager.GetActiveScene();
        yield return null;
        
        SceneManager.LoadScene("Game");

        start = Time.unscaledTime;
        yield return new WaitUntil(() =>
            SceneManager.GetActiveScene() != prev || (Time.unscaledTime - start) * Time.timeScale > 1);
        if ((Time.unscaledTime - start) * Time.timeScale > 1)
        {
            Assert.Fail("\"Game\" scene can't be loaded");
        }
        
        text = PMHelper.Exist<Text>(GameObject.Find("Countdown"));
        yield return null;
        
        try
        {
            count2 = Int32.Parse(text.text);
        }
        catch
        {
            Assert.Fail("The value of 'Text' property of \"Countdown\" object's \"Text\" component" +
                        " should be integer until it reaches 0 taps left");
        }

        if (count != count2)
        {
            Assert.Fail("When the player has left the game and started it again the 'Text' property of " +
                        "\"Countdown\" object's \"Text\" component should display the result where the player left off");
        }
        for (int i = 94; i > 0; i--)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                IS.Mouse.LeftButtonClick();
            else
                ISL.Mouse.LeftButtonClick();

            start = Time.unscaledTime;
            yield return new WaitUntil(() => !text.text.Equals(textBefore) ||
                                             (Time.unscaledTime - start) * Time.timeScale > 1);
            if ((Time.unscaledTime - start) * Time.timeScale > 1)
            {
                Assert.Fail();
            }
            textBefore = text.text;
            try
            {
                count2 = Int32.Parse(textBefore);
            }
            catch
            {
                Assert.Fail("The value of 'Text' property of \"Countdown\" object's \"Text\" component" +
                            " should be integer until it reaches 0 taps left");
            }

            if (count2 != i)
            {
                Assert.Fail("By tapping one time, the value of 'Text' property of \"Countdown\" object's " +
                            "\"Text\" component did not decrease by 1");
            }
        }
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            IS.Mouse.LeftButtonClick();
        else
            ISL.Mouse.LeftButtonClick();
        
        start = Time.unscaledTime;
        yield return new WaitUntil(() => !text.text.Equals(textBefore) ||
                                         (Time.unscaledTime - start) * Time.timeScale > 1);
        if ((Time.unscaledTime - start) * Time.timeScale > 1)
        {
            Assert.Fail("If the player is 1 tap away from the end, after tapping last time the " +
                        "value of 'Text' property of \"Countdown\" object's \"Text\" component should be" +
                        "equal to phrase 'So, what?'");
        }
        textBefore = text.text;
        if (!textBefore.Equals("So, what?"))
        {
            Assert.Fail("If the player is 1 tap away from the end, after tapping last time the " +
                        "value of 'Text' property of \"Countdown\" object's \"Text\" component should be" +
                        "equal to phrase 'So, what?'");
        }
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            IS.Mouse.LeftButtonClick();
        else
            ISL.Mouse.LeftButtonClick();
        yield return new WaitForSeconds(1);
        textBefore = text.text;
        if (!textBefore.Equals("So, what?"))
        {
            Assert.Fail("If the player taps after getting the 'So, what?' ending phrase, the " +
                        "value of 'Text' property of \"Countdown\" object's \"Text\" component should not change anymore");
        }
        
        prev = SceneManager.GetActiveScene();
        yield return null;
        
        SceneManager.LoadScene("Game");

        start = Time.unscaledTime;
        yield return new WaitUntil(() =>
            SceneManager.GetActiveScene() != prev || (Time.unscaledTime - start) * Time.timeScale > 1);
        if ((Time.unscaledTime - start) * Time.timeScale > 1)
        {
            Assert.Fail("\"Game\" scene can't be loaded");
        }
        
        text = PMHelper.Exist<Text>(GameObject.Find("Countdown"));
        yield return null;
        textBefore = text.text;
        if (!textBefore.Equals("So, what?"))
        {
            Assert.Fail("If the player restarts the game after getting the 'So, what?' ending phrase, the " +
                        "value of 'Text' property of \"Countdown\" object's \"Text\" component should not change");
        }
        
    }
}
