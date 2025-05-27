using System;

namespace Hanodale.WebUI.UnityExtensions
{
    public interface IPerRequestStore
    {
        object GetValue(object key);
        void SetValue(object key, object value);
        void RemoveValue(object key);

        event EventHandler EndRequest;
    }
}