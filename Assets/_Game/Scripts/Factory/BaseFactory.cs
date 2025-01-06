using System;
using UnityEngine;

public abstract class BaseFactory<T> where T : UnityEngine.Object
{
   public abstract T Create(params object[] args);

   
}

public static class TypeExtensions
{
    /// <summary>
    /// Verifies that the object is of the specified type.
    /// If the object is compatible, it is returned as.
    /// Otherwise, throws an <see cref="InvalidCastException" />.
    /// </summary>
    public static T VerifyType<T>(this object obj)
    {
        if (obj is T typedObj)
        {
            return typedObj;
        }

        throw new InvalidCastException($"Object type '{obj?.GetType()}' is not compatible with type {typeof(T)}.");

    }

}
