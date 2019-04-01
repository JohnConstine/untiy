using System;
using GameFramework;

namespace SG1
{
    public interface IContext
    {
        void SetPropertyValue(string propertyName, object value);

        Property FindProperty(string propertyName);

        void AddPropertyRuntime(string propertyName, Type type);
    }
}