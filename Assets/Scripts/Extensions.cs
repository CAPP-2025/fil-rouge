using UnityEngine;

public static class Extensions
{
    private static LayerMask layerMask = LayerMask.GetMask("Default", "Enemy", "Water");

    public static bool Raycast(this Rigidbody2D rigidbody, Vector2 direction)
    {
        if (rigidbody.isKinematic) {
            return false;
        }

        float radius = 0.25f;
        float distance = 0.375f;

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
        return Vector2.Dot(direction.normalized, testDirection) > 0.25f;
    }

}
