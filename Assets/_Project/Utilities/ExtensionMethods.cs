using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Utilities
{
    public static class ExtensionMethods
    {
        public static void SafeInvoke(this Action source)
        {
            if (source != null) source.Invoke();
        }

        public static void SafeInvoke<T>(this Action<T> source, T value)
        {
            if (source != null) source.Invoke(value);
        }

        public static void SafeInvoke<T1, T2>(this Action<T1, T2> source, T1 firstValue, T2 secondValue)
        {
            if (source != null) source.Invoke(firstValue, secondValue);
        }

        public static void SafeInvoke<T1, T2, T3>(this Action<T1, T2, T3> source, T1 firstValue, T2 secondValue,
            T3 thirdValue)
        {
            if (source != null) source.Invoke(firstValue, secondValue, thirdValue);
        }

        public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static T GetRandomElementFromList<T>(this List<T> list)
        {
            int random = Random.Range(0, list.Count);
            return list[random];
        }

        public static T GetRandomElementFromList<T>(this List<T> list, T exclude)
        {
            int random = Random.Range(0, list.Count);
            while (Equals(list[random], exclude))
            {
                random = Random.Range(0, list.Count);
            }

            return list[random];
        }
        public static Vector3 SetX(this Vector3 v,float x)
        {
            return new Vector3(x, v.y, v.z);
        }
        public static Vector3 SetY(this Vector3 v,float y)
        {
            return new Vector3(v.x, y, v.z);
        }

        public static Vector3 SetZ(this Vector3 v,float z)
        {
            return new Vector3(v.x, v.y, z);
        }
        public static void ChangeScaleY(this Transform thisTransform, float change)
        {
            var firstScale = thisTransform.transform.localScale;
            firstScale.y = change;
            thisTransform.transform.localScale = firstScale;
        }

        private static List<Transform> GetAllChild(this Transform thisTransform)
        {
            return thisTransform.Cast<Transform>().ToList();
        }

        public static void ChangePositionWithChild(this Transform thisTransform, string childname)
        {
            var childs = thisTransform.GetAllChild();
            var changedChild = childs.FirstOrDefault(x => x.name == childname);
            if (changedChild == null)
                return;

            childs.ForEach(x => x.SetParent(null));
            (changedChild.position, thisTransform.position) = (thisTransform.position, changedChild.position);

            childs.ForEach(x => x.SetParent(thisTransform));
        }

        public static class RandomItemGeneric<T>
        {
            public static T GetRandom(params T[] array)
            {
                var rand = Random.Range(0, array.Length);
                return array[rand];
            }

            public static T[] GetRandomMultiple(int count, bool canRepeat = true, params T[] array)
            {
                var result = new List<T>();
                count = Mathf.Clamp(count, 0, array.Length);
                for (int i = 0; i < count; i++)
                {
                    T randItem;
                    do
                    {
                        randItem = GetRandom(array);
                    } while (!canRepeat && result.Contains(randItem));

                    result.Add(randItem);
                }

                return result.ToArray();
            }

            public static T[] GetMixedArray(params T[] array)
            {
                var result = array;
                for (int i = 0; i < array.Length; i++)
                {
                    var rand = Random.Range(0, result.Length);
                    (result[i], result[rand]) = (result[rand], result[i]);
                }

                return result;
            }
        }
    }
}