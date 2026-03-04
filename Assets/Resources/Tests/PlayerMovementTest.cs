using NSubstitute;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

public class PlayerMovementTest
{
    GameObject testPlayer;
    GameObject testCamera;
    Vector3 startPos = Vector3.zero;

    float speed = 5f;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {

        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.gameObject.transform.position = new Vector3(0, 0, 0);
        plane.gameObject.transform.localScale = (new Vector3(10, 1, 10));
        plane.AddComponent<BoxCollider>();

        testPlayer = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Entity/Player"));
        testPlayer.transform.localPosition = new Vector3(0, 0f, 0);

        testCamera = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Others/MainCamera"));
        testCamera.transform.SetParent(testPlayer.transform, false);
    }

    [SetUp]
    public void Setup()
    {
        startPos = testPlayer.transform.position;
    }

    [UnityTest]
    public IEnumerator Check_If_Its_Null()
    {
        Assert.NotNull(testPlayer, "Player is NULL");
        Assert.NotNull(testCamera, "Player is NULL");

        yield return new WaitForSeconds(1f);
    }

    #region PlayerMovement
    [UnityTest]
    public IEnumerator PlayerMovesForward()
    {
        testPlayer.GetComponent<PlayerController>().MoveWhenPossessed(new Vector2(0, 1).normalized); // Move forward
        yield return new WaitForSeconds(1f);

        FloatEqualityComparer floatEqualityComparer = new FloatEqualityComparer(0.01f);

        Assert.That(startPos.z, Is.LessThan(testPlayer.transform.position.z), "Player should move Forward.");
    }

    [UnityTest]
    public IEnumerator PlayerMovesBackward()
    {
        testPlayer.GetComponent<PlayerController>().MoveWhenPossessed(new Vector2(0, -1).normalized); // Move forward
        yield return new WaitForSeconds(1f);

        FloatEqualityComparer floatEqualityComparer = new FloatEqualityComparer(0.01f);

        Assert.That(startPos.z, Is.GreaterThan(testPlayer.transform.position.z), "Player should move backward");
    }

    [UnityTest]
    public IEnumerator PlayerMovesRight()
    {
        testPlayer.GetComponent<PlayerController>().MoveWhenPossessed(new Vector2(1, 0)); // Move right
        yield return new WaitForSeconds(1f);

        Assert.That(startPos.x, Is.LessThan(testPlayer.transform.position.x), "Player should move Right.");
    }

    [UnityTest]
    public IEnumerator PlayerMovesLeft()
    {
        testPlayer.GetComponent<PlayerController>().MoveWhenPossessed(new Vector2(-1, 0)); // Move left
        yield return new WaitForSeconds(1f);

        Assert.That(startPos.x, Is.GreaterThan(testPlayer.transform.position.x), "Player should move Left.");
    }

    [UnityTest]
    public IEnumerator PlayerDoesNotMoveWithoutInput()
    {
        yield return new WaitForSeconds(1f);
        Assert.AreEqual(testPlayer.transform.position, startPos, "Player should not move without input.");
    }

    [UnityTest]
    public IEnumerator PlayerMovesAtCorrectSpeed()
    {
        Vector3 startPos = testPlayer.transform.position;
        testPlayer.GetComponent<PlayerController>().MoveWhenPossessed(new Vector2(0, 1)); // Move forward
        yield return new WaitForSeconds(1f);

        float distanceMoved = Vector3.Distance(startPos, testPlayer.transform.position) * Time.fixedDeltaTime;
        float expectedDistance = speed * 1f * Time.fixedDeltaTime;

        Assert.AreEqual(expectedDistance, distanceMoved, 0.5f, "Player should move at the correct speed.");
    }
    #endregion
}
