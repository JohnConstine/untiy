using System;
using System.Reflection;
using GameFramework;
using UnityEngine;

namespace SG1
{
    public abstract class ModelViewCore : UICustomBehaviour
    {
        public delegate T Converter<out T>(IContext node, string path);

        public const char PropertySeparator = '.';

        protected static Property NodeToProperty(IContext context, string path)
        {
            return context == null ? (Property) null : context.FindProperty(path);
        }

        protected static Action NodeToAction(IContext context, string path)
        {
            if (context == null)
            {
                return (Action) null;
            }

            var methodInfo = context.GetType().GetMethod(path,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            return methodInfo == null
                ? (Action) null
                : (Action) Delegate.CreateDelegate(typeof(Action), context, methodInfo);
        }

        protected static Collection NodeToCollection(IContext context, string path)
        {
            return context == null ? (Collection) null : context.FindCollection(path);
        }

        protected static T Find<T>(IContext context, string path, Converter<T> converter)
        {
            if (context == null)
            {
                return default(T);
            }

            path = path.Trim();

            var pointPos = path.IndexOf(PropertySeparator);
            if (pointPos < 0)
            {
                return converter(context, path);
            }

            var mainPropertyName = path.Substring(0, pointPos);
            var subPath = path.Substring(pointPos + 1);
            var mainPropertyInfo = context.GetType().GetProperty(mainPropertyName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var subContext = mainPropertyInfo != null ? mainPropertyInfo.GetValue(context, null) : mainPropertyInfo;
            return subContext == null ? default(T) : Find(subContext as IContext, subPath, converter);
        }
    }
}