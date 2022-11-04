using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

[Description("A Curate’s Egg"), Category("2")]
public class Stage2_Tests
{
    private GameObject canvas, count, cameraObj;
    private RectTransform canvasRT, countRT;
    
    private Canvas canvasC;
    private CanvasScaler canvasCS;
    private Camera camera;

    private CanvasRenderer countCR;
    private Text countText;
    
    private bool exist;
    
    [UnityTest]
    public IEnumerator UIObjectsCheck()
    {
        SceneManager.LoadScene("Game");
            
        float start = Time.unscaledTime;
        yield return new WaitUntil(() =>
            SceneManager.GetActiveScene().name == "Game" || (Time.unscaledTime - start) * Time.timeScale > 1);
        if (SceneManager.GetActiveScene().name != "Game")
        {
            Assert.Fail("\"Game\" scene can't be loaded");
        }

        (cameraObj, exist) = PMHelper.Exist("Main Camera");
        camera = PMHelper.Exist<Camera>(cameraObj);

        (canvas, exist) = PMHelper.Exist("Canvas");
        if (!exist)
        {
            Assert.Fail("There is no \"Canvas\" object on \"Game\" scene, or it might be inactive/misspelled");
        }
        
        canvasRT = PMHelper.Exist<RectTransform>(canvas);
        if (!canvasRT)
        {
            Assert.Fail("There is no \"RectTransform\" component on \"Canvas\"' object or it's disabled");
        }
        
        canvasC = PMHelper.Exist<Canvas>(canvas);
        canvasCS = PMHelper.Exist<CanvasScaler>(canvas);

        if (!canvasC)
        {
            Assert.Fail("There is no \"Canvas\" component on \"Canvas\"' object or it's disabled");
        }
        if (canvasC.renderMode != RenderMode.ScreenSpaceCamera)
        {
            Assert.Fail("\"Canvas\"' <Render Mode> should be switched to \"Screen Space - Camera\", so that " +
                        "the camera settings affect the appearance of the UI");
        }
        if (canvasC.worldCamera != camera || canvasC.planeDistance > camera.farClipPlane)
        {
            Assert.Fail("Make sure, that \"Main Camera\"'s \"Camera\" component is attached to \"Canvas\"' " +
                        "<Render Camera> property and the <Plane Distance> property value is equal or less than \"Camera\"'s" +
                        "<Far> property, so UI will be visible in the camera view.");
        }

        if (!canvasCS)
        {
            Assert.Fail("There is no \"Canvas Scaler\" component on \"Canvas\"' object or it's disabled");
        }

        if (canvasCS.uiScaleMode != CanvasScaler.ScaleMode.ConstantPixelSize || canvasCS.scaleFactor != 1 ||
            canvasCS.referencePixelsPerUnit != 100)
        {
            Assert.Fail("There is no need to change any \"Canvas Scaler\" properties yet, leave them as default");
        }
        
        //Text object check
        
        (count, exist) = PMHelper.Exist("Countdown");
        if (!exist)
        {
            Assert.Fail("There is no \"Countdown\" object on \"Game\" scene, or it might be inactive/misspelled");
        }
        
        countRT = PMHelper.Exist<RectTransform>(count);
        if (!countRT)
        {
            Assert.Fail("There is no \"RectTransform\" component on \"Countdown\" object or it's disabled");
        }
        if (!PMHelper.CheckRectTransform(countRT))
        {
            Assert.Fail(
                "Make sure, that anchors of \"RectTransform\" component on \"Countdown\" object are between 0" +
                " and 1 and offsets are set to 0, this way an UI-element will be 'glued' to exact position");
        }
        countCR = PMHelper.Exist<CanvasRenderer>(count);
        if (!countCR)
        {
            Assert.Fail("There is no \"Canvas Renderer\" component on \"Countdown\" object or it's disabled");
        }
        
        countText = PMHelper.Exist<Text>(count);
        if (!countText)
        {
            Assert.Fail("There is no \"Text\" component on \"Countdown\" object or it's disabled");
        }
        if (!countText.font)
        {
            Assert.Fail("There is no font attached to \"Countdown\"'s \"Text\" component");
        }
        if (countText.alignment != TextAnchor.MiddleCenter)
        {
            Assert.Fail("Set the alignment of \"Countdown\"'s \"Text\" component as the middle and centered");
        }

        int x=-1;
        
        try
        {
            int.TryParse(countText.text, out x);
        }
        catch
        {
            Assert.Fail("Set the value of <Text> property of \"Countdown\"'s \"Text\" component as any positive integer number");
        }

        if (x <= 0)
        {
            Assert.Fail("Set the value of <Text> property of \"Countdown\"'s \"Text\" component as any positive integer number");
        }
    }
}
