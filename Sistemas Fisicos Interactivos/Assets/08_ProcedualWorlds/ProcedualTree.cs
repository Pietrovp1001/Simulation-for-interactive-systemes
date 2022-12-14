using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedualTree : MonoBehaviour
{
    [SerializeField] private GameObject branchPrefab;
    [SerializeField] private float initialSize = 5f;
    private Queue<GameObject> rootBranchesQueue = new Queue<GameObject>();
    private int actualLevel = 1;
    [SerializeField] private int totalLevels = 3;
    [SerializeField, Range(0f, 1f)] private float reductionPerLevel = 0.1f;
    private const int indexChildSquare = 0;
    private const int indexChildCircle = 1;

    private void Start() 
    {
        var rootBranch = Instantiate(branchPrefab, transform);
        ChangeBranchSize(rootBranch, initialSize);
        rootBranchesQueue.Enqueue(rootBranch);
        GenerateTree();
    }

    private void GenerateTree()
    {
        while (true)
        {
            if (actualLevel >= totalLevels)
            {
                return;
            }

            Debug.Log(actualLevel);
            actualLevel++;

            float newSize = Mathf.Max(initialSize - initialSize * reductionPerLevel * (actualLevel - 1), 0.1f);
            List<GameObject> branchesCreatedThisCycle = new List<GameObject>();

            while (rootBranchesQueue.Count > 0)
            {
                var rootBranch = rootBranchesQueue.Dequeue();

                var leftBranch = CreateBranch(rootBranch, Random.Range(5f, 30f));
                var rightBranch = CreateBranch(rootBranch, -Random.Range(5f, 30f));
                ChangeBranchSize(leftBranch, newSize);
                ChangeBranchSize(rightBranch, newSize);

                branchesCreatedThisCycle.Add(leftBranch);
                branchesCreatedThisCycle.Add(rightBranch);
            }

            foreach (var newBranches in branchesCreatedThisCycle)
            {
                rootBranchesQueue.Enqueue(newBranches);
            }
        }
    }

    private GameObject CreateBranch(GameObject prevBranch, float relativeAngle) 
    {
        GameObject newBranch = Instantiate(branchPrefab, transform);
        newBranch.transform.localPosition = prevBranch.transform.localPosition + prevBranch.transform.up * GetBranchLength(prevBranch);
        newBranch.transform.localRotation = prevBranch.transform.localRotation * Quaternion.Euler(0, 0, relativeAngle);
        return newBranch;
    }

    private void ChangeBranchSize(GameObject branchInstance, float newSize) 
    {
        var square = branchInstance.transform.GetChild(indexChildSquare);
        var circle = branchInstance.transform.GetChild(indexChildCircle);

        var newScale = square.transform.localScale; newScale.y = newSize;
        square.transform.localScale = newScale;

        var newPosition = square.transform.localPosition; newPosition.y = newSize / 2f;
        square.transform.localPosition = newPosition;

        var newCirclePosition = circle.transform.localPosition; newCirclePosition.y = newSize;
        circle.transform.localPosition = newCirclePosition;
    }

    private float GetBranchLength(GameObject branchInstance) 
    {
        return branchInstance.transform.GetChild(indexChildSquare).localScale.y;
    }
}
