using UnityEditor.Animations;
using UnityEngine;

namespace Assets.Scripts.Test
{
    public static class ObjectBuilder
    {

        public static GameObject SetScale(this GameObject gameObject, Vector3 scale)
        {
            gameObject.transform.localScale = scale;
            return gameObject;
        }

        public static T AddOrGetComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.TryGetComponent<T>(out var component))
            {
                return component;
            }
            else return gameObject.AddComponent<T>();
        }

        public static GameObject SetSprite(this GameObject gameObject, Sprite sprite)
        {
            gameObject.AddOrGetComponent<SpriteRenderer>().sprite = sprite;
            return gameObject;
        }

        public static GameObject ChangeColor(this GameObject gameObject, Color color)
        {
            gameObject.AddOrGetComponent<SpriteRenderer>().color = color;
            return gameObject;
        }

        public static GameObject SetAnimatorController(this GameObject gameObject, AnimatorController controller)
        {
            gameObject.AddOrGetComponent<Animator>().runtimeAnimatorController = controller;
            return gameObject;
        }

        public static GameObject SetBoxCollider2D(this GameObject gameObject, Vector2 offset, Vector2 size)
        {
            var collider = gameObject.AddOrGetComponent<BoxCollider2D>();
            collider.offset = offset;
            collider.size = size;
            return gameObject;
        }
    }
}