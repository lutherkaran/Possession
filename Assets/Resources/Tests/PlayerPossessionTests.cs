using NSubstitute;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerPossessionTests
{
    private GameObject testCamera;
    private GameObject objectToPossess;
    private PossessionManager possessionManager;
    private IPossessable mockPossessable;

    //MethodName_WhenCondition_ShouldExpectedBehavior

    [OneTimeSetUp]
    public void OneTimeSetup_ShouldCreateRequiredSceneObjects()
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.gameObject.transform.position = new Vector3(0f, .4f, 0f);
        plane.gameObject.transform.localScale = (new Vector3(10f, 1, 10f));
        plane.AddComponent<BoxCollider>();

        objectToPossess = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Entity/Npc/Chicken"));
        objectToPossess.transform.localPosition = new Vector3(0f, .5f, 0f);

        testCamera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Others/MainCamera"));
        testCamera.transform.SetParent(objectToPossess.transform, false);

        possessionManager = new PossessionManager();
        mockPossessable = Substitute.For<IPossessable>();

        mockPossessable.GetPossessedEntity().Returns(objectToPossess.GetComponent<Entity>());
        possessionManager.ToPossess(mockPossessable);
    }

    [UnityTest]
    public IEnumerator Setup_ShouldInstantiatePlayerCameraAndNpc()
    {
        Assert.NotNull(testCamera, "Camera is NULL");
        Assert.NotNull(objectToPossess, "Object is NULL");

        yield return new WaitForSeconds(.1f);
    }

    [UnityTest]
    public IEnumerator ToPossess_ShouldCallPossessing_OnIPossessable()
    {
        mockPossessable.Received(1).Possessing(objectToPossess);

        yield return new WaitForSeconds(1f);
    }

    [UnityTest]
    public IEnumerator MoveWhenPossessed_ShouldMoveEntity_WhenPossessed()
    {
        yield return new WaitForSeconds(1f);
        mockPossessable.GetPossessedEntity().MoveWhenPossessed(new Vector2(0, 10f));

        yield return new WaitForSeconds(.5f);
    }

    [OneTimeTearDown]
    public void Cleanup()
    {
        Object.Destroy(testCamera);
        Object.Destroy(objectToPossess);
    }
}
