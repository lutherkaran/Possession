using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

public class PlayerMovementTest
{
    private GameObject testPlayer;
    private GameObject myCamera;

    private Vector3 startPos = Vector3.zero;

    private float speed = 5f;

    [OneTimeSetUp]
    public void OneTimeSetup_ShouldCreateRequiredSceneObjects()
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.gameObject.transform.position = new Vector3(0, .4f, 0);
        plane.gameObject.transform.localScale = (new Vector3(10f, 1, 10f));
        plane.AddComponent<BoxCollider>();

        testPlayer = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Entity/Player"));
        if (testPlayer == null)
        {
            Debug.LogError("PlayerISNULL");
        }
        testPlayer.transform.position = new Vector3(0, 0f, 0);
    }

    [SetUp]
    public void Setup()
    {
        startPos = testPlayer.transform.position;
    }

    [UnityTest]
    public IEnumerator Setup_ShouldInstantiatePlayer()
    {
        Assert.NotNull(testPlayer, "Player is NULL");

        yield return new WaitForSeconds(.1f);
    }

    #region PlayerMovement

    [UnityTest]
    public IEnumerator DoesNotMove_WhenNoInput()
    {
        testPlayer.GetComponent<PlayerController>().MoveWhenPossessed(new Vector2(0, 0).normalized);
        yield return new WaitForSeconds(.5f);
        Assert.AreEqual(startPos, testPlayer.transform.position, "Player should not move without input.");
    }

    [UnityTest]
    public IEnumerator MovesForward_WhenInputIsPositiveY()
    {
        testPlayer.GetComponent<PlayerController>().MoveWhenPossessed(new Vector2(0, 1).normalized); // Move forward
        yield return new WaitForSeconds(.1f);

        FloatEqualityComparer floatEqualityComparer = new FloatEqualityComparer(0.01f);

        Assert.That(startPos.z, Is.LessThan(testPlayer.transform.position.z), "Player should move Forward.");
    }

    [UnityTest]
    public IEnumerator MovesBackward_WhenInputIsNegativeY()
    {
        testPlayer.GetComponent<PlayerController>().MoveWhenPossessed(new Vector2(0, -1).normalized); // Move forward
        yield return new WaitForSeconds(.1f);

        FloatEqualityComparer floatEqualityComparer = new FloatEqualityComparer(0.01f);

        Assert.That(startPos.z, Is.GreaterThan(testPlayer.transform.position.z), "Player should move backward");
    }

    [UnityTest]
    public IEnumerator MovesRight_WhenInputIsPositiveX()
    {
        testPlayer.GetComponent<PlayerController>().MoveWhenPossessed(new Vector2(1, 0)); // Move right
        yield return new WaitForSeconds(.1f);

        Assert.That(startPos.x, Is.LessThan(testPlayer.transform.position.x), "Player should move Right.");
    }

    [UnityTest]
    public IEnumerator MovesLeft_WhenInputIsNegativeX()
    {
        testPlayer.GetComponent<PlayerController>().MoveWhenPossessed(new Vector2(-1, 0)); // Move left
        yield return new WaitForSeconds(.1f);

        Assert.That(startPos.x, Is.GreaterThan(testPlayer.transform.position.x), "Player should move Left.");
    }

    [UnityTest]
    public IEnumerator MovesAtCorrectSpeed_WhenInputIsGiven()
    {
        Vector3 startPos = testPlayer.transform.position;
        testPlayer.GetComponent<PlayerController>().MoveWhenPossessed(new Vector2(0, 1)); // Move forward
        yield return new WaitForSeconds(.1f);

        float distanceMoved = Vector3.Distance(startPos, testPlayer.transform.position) * Time.fixedDeltaTime;
        float expectedDistance = speed * 1f * Time.fixedDeltaTime;

        Assert.AreEqual(expectedDistance, distanceMoved, 0.5f, "Player should move at the correct speed.");
    }
    #endregion

    [OneTimeTearDown]
    public void Cleanup()
    {
        Object.Destroy(testPlayer);
    }
}
