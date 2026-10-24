// Assets/Scripts/Grid/EntityMover.cs
using UnityEngine;

public class EntityMover : MonoBehaviour
{
    public RoomPosition CurrentPosition { get; private set; }

    [SerializeField] private float moveSpeed = 4f; // マス/秒
    [SerializeField] private float floorY = 0f;    // 部屋内でのY座標(床の高さ、部屋ごとに固定)

    private float startWorldX;
    private float targetWorldX;
    private float moveProgress = 1f;

    public void SetInitialPosition(RoomPosition pos)
    {
        CurrentPosition = pos;
        transform.position = new Vector3(pos.X, floorY, 0);
        moveProgress = 1f;
    }

    /// <summary>
    /// 同じ部屋内でのX移動、または部屋またぎの移動。どちらもこれ1つで済む
    /// </summary>
    public void MoveTo(RoomPosition newPos)
    {
        bool roomChanged = newPos.RoomId != CurrentPosition.RoomId;
        CurrentPosition = newPos; // 論理位置は即確定

        if (roomChanged)
        {
            // 部屋またぎは補間せず即座にワープ(部屋自体が画面上別の場所にあるため)
            transform.position = new Vector3(newPos.X, floorY, 0);
            moveProgress = 1f;
        }
        else
        {
            startWorldX = transform.position.x;
            targetWorldX = newPos.X;
            moveProgress = 0f;
        }
    }

    void Update()
    {
        if (moveProgress >= 1f) return;
        moveProgress += Time.deltaTime * moveSpeed;
        float x = Mathf.Lerp(startWorldX, targetWorldX, Mathf.Clamp01(moveProgress));
        transform.position = new Vector3(x, floorY, transform.position.z);
    }
}