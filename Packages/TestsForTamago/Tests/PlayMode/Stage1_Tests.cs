using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

[Description("As Sure As Eggs Is Eggs"), Category("1")]
public class Stage1_Tests
{
    private GameObject cameraObj, egg;
    private SpriteRenderer eggSR;
    private Transform eggT;
    private Camera camera;
    private bool exist;
    
    [UnityTest]
    public IEnumerator ObjectsCheck()
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

        (cameraObj, exist) = PMHelper.Exist("Main Camera");
        if (!exist)
        {
            Assert.Fail("There is no \"Main Camera\" object on \"Game\" scene, or it might be inactive/misspelled");
        }
        
        camera = PMHelper.Exist<Camera>(cameraObj);
        if (!camera || !camera.enabled)
        {
            Assert.Fail("There is no \"Camera\" component on \"Main Camera\"'s object or it's disabled");
        }

        (egg, exist) = PMHelper.Exist("Egg");
        if (!exist)
        {
            Assert.Fail("There is no \"Egg\" object on \"Game\" scene, or it might be inactive/misspelled");
        }

        eggT = PMHelper.Exist<Transform>(egg);
        eggSR = PMHelper.Exist<SpriteRenderer>(egg);

        if (!eggT)
        {
            Assert.Fail("There is no \"Transform\" component on \"Egg\"'s object or it's disabled");
        }
        if (!eggSR || !eggSR.enabled)
        {
            Assert.Fail("There is no \"SpriteRenderer\" component on \"Egg\"'s object or it's disabled");
        }

        if (!eggSR.sprite)
        {
            Assert.Fail("There is no sprite attached to \"Egg\"'s \"SpriteRenderer\" component");
        }
        if (eggSR.maskInteraction != SpriteMaskInteraction.None)
        {
            Assert.Fail(
                "There is no no need to use masks in this project, so make sure, that \"Egg\"'s " +
                "\"SpriteRenderer\" component's <MaskInteraction> property is set to \"None\"");
        }
        if (!eggSR.material)
        {
            Assert.Fail(
                "There is no material attached to \"Egg\"'s \"SpriteRenderer\" component, switch the" +
                " <Material> property to \"Sprites-Default\", that is the default material for objects in 2D projects");
        }

        if (!PMHelper.CheckVisibility(camera, eggT, 2))
        {
            Assert.Fail("\"Egg\" object should be visible in the camera view.");
        }
    }
}
