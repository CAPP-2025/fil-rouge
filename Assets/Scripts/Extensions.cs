using UnityEngine;

public static class Extensions
{
    private static LayerMask layerMaskDefault = LayerMask.GetMask("Default", "Enemy", "Water", "Moving");

    public static bool Raycast(this Rigidbody2D rigidbody, Vector2 direction, float distance = 0.375f, float radius = 0.25f, LayerMask layerMask = default)
    {
        if (layerMask == default) {
            layerMask = layerMaskDefault;
        }
        if (rigidbody.isKinematic) {
            return false;
        }

        RaycastHit2D[] hits = Physics2D.CircleCastAll(rigidbody.position, radius, direction.normalized, distance, layerMask);
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider != null && hit.rigidbody != rigidbody) {
                return true;
            }
        }
        return false;
    }

    public static bool DotTest(this Transform transform, Transform other, Vector2 testDirection)
    {
        Vector2 direction = other.position - transform.position;
        bool inDirection = Vector2.Dot(direction.normalized, testDirection) > 0.25f;
        return inDirection;
    }

}
