using System;
using GameFramework;

namespace SG1
{
    public interface IContext
    {
        void SetPropertyValue(string propertyName, object value);

        Property FindProperty(string propertyName);

        Collection FindCollection(string collectionName);

        void AddPropertyRuntime(string propertyName, Type type);
    }
}