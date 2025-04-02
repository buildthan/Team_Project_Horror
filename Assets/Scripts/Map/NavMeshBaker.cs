using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour
{
    private GameObject spawnedMap;
    public NavMeshSurface[] navMeshSurfaces;

    public void MapBake(GameObject makedMap)
    {
        spawnedMap = makedMap;
        navMeshSurfaces = spawnedMap.GetComponentsInChildren<NavMeshSurface>();
        StartCoroutine(BakeNavMeshAfterDelay(1f));
    }
    IEnumerator BakeNavMeshAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (navMeshSurfaces.Length > 0)
        {
            for (int i = 0; i < navMeshSurfaces.Length - 1; i++)
            {
                navMeshSurfaces[i].RemoveData();
                Debug.Log($"NavMeshSurface {i} 데이터 삭제 완료");
            }

            // 마지막 Surface만 NavMesh를 다시 빌드
            NavMeshSurface lastSurface = navMeshSurfaces[navMeshSurfaces.Length - 1];
            lastSurface.RemoveData();
            lastSurface.BuildNavMesh();
        }
        else
        {
            Debug.LogError("NavMeshSurface를 찾을 수 없습니다!");
        }
    }
}
