using UnityEngine;

/// <summary>
/// Shared rules for logical ground vs PlatformEffector2D one-way colliders (queries ignore the effector).
/// Use one CompositeCollider2D per platform so <see cref="Collider2D.bounds"/> matches the walkable surface.
/// </summary>
public static class OneWayPlatformGrounding
{
	public const float OverlapEpsilon = 0.15f;

	public static bool ColliderCountsAsGround(Collider2D col, float lowestFootWorldY)
	{
		if (col == null)
			return false;

		PlatformEffector2D effector = col.GetComponentInParent<PlatformEffector2D>();
		if (effector == null || !effector.useOneWay)
			return true;

		return lowestFootWorldY >= col.bounds.max.y - OverlapEpsilon;
	}
}
