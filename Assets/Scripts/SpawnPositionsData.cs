using UnityEngine;

[CreateAssetMenu(fileName = "SpawnPositionsData", menuName = "MindiKot/SpawnPositionsData", order = 0)]
public class SpawnPositionsData : ScriptableObject
{
    public Vector2[] _12;
    public Vector2[] _13;
    public Vector2[] _15;
    public Vector2[] _16;
    public Vector2[] _18;

    public void GetSpawnPositions(int cardPerhand, out Vector2[] array)
    {
        switch (cardPerhand)
        {
            case 12:
                array = _12;
                return;
            case 13:
                array = _13;
                return;
            case 15:
                array = _15;
                return;
            case 16:
                array = _16;
                return;
            case 18:
                array = _18;
                return;
            default:
                array = null;
                return;
        }
    }

}
