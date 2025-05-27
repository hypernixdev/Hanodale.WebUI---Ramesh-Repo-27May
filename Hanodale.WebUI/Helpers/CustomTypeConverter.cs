using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;


namespace Hanodale.WebUI.Helpers
{
    public class CustomTypeConverter : ExpandableObjectConverter
    {

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            PropertyDescriptorCollection props = base.GetProperties(context, value, attributes);
            List<PropertyDescriptor> list = new List<PropertyDescriptor>(props.Count);
            foreach (PropertyDescriptor prop in props)
            {
                switch (prop.Name)
                {
                    case "code":
                        list.Add(new DisplayNamePropertyDescriptor(
                            prop, "your magic code here"));
                        break;
                    default:
                        list.Add(prop);
                        break;
                }
            }
            return new PropertyDescriptorCollection(list.ToArray(), true);
        }
    }

    class DisplayNamePropertyDescriptor : PropertyDescriptor
    {
        private readonly string displayName;
        private readonly PropertyDescriptor parent;
        public DisplayNamePropertyDescriptor(
            PropertyDescriptor parent, string displayName)
            : base(parent)
        {
            this.displayName = displayName;
            this.parent = parent;
        }
        public override string DisplayName
        { get { return displayName; } }

        public override bool ShouldSerializeValue(object component)
        { return parent.ShouldSerializeValue(component); }

        public override void SetValue(object component, object value)
        {
            parent.SetValue(component, value);
        }
        public override object GetValue(object component)
        {
            return parent.GetValue(component);
        }
        public override void ResetValue(object component)
        {
            parent.ResetValue(component);
        }
        public override bool CanResetValue(object component)
        {
            return parent.CanResetValue(component);
        }
        public override bool IsReadOnly
        {
            get { return parent.IsReadOnly; }
        }
        public override void AddValueChanged(object component, EventHandler handler)
        {
            parent.AddValueChanged(component, handler);
        }
        public override void RemoveValueChanged(object component, EventHandler handler)
        {
            parent.RemoveValueChanged(component, handler);
        }
        public override bool SupportsChangeEvents
        {
            get { return parent.SupportsChangeEvents; }
        }
        public override Type PropertyType
        {
            get { return parent.PropertyType; }
        }
        public override TypeConverter Converter
        {
            get { return parent.Converter; }
        }
        public override Type ComponentType
        {
            get { return parent.ComponentType; }
        }
        public override string Description
        {
            get { return parent.Description; }
        }
        public override PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
        {
            return parent.GetChildProperties(instance, filter);
        }
        public override string Name
        {
            get { return parent.Name; }
        }

    }

    //public class DynamicDisplayNameMetadataProvider : DataAnnotationsModelMetadataProvider
    //{
    //    private Dictionary<string, string> _dynamicDisplayNames = new Dictionary<string, string>();

    //    public void SetDynamicDisplayName<TModel>(string propertyName, string displayName)
    //    {
    //        var key = typeof(TModel).FullName + "." + propertyName;
    //        _dynamicDisplayNames[key] = displayName;
    //    }

    //    protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
    //    {
    //        var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
    //        var key = containerType.FullName + "." + propertyName;
    //        if (_dynamicDisplayNames.ContainsKey(key))
    //        {
    //            metadata.DisplayName = _dynamicDisplayNames[key];
    //        }
    //        return metadata;
    //    }
    //}

    //public class DynamicDisplayNameMetadataProvider : DataAnnotationsModelMetadataProvider
    //{
    //    private readonly Dictionary<string, string> _dynamicDisplayNames = new Dictionary<string, string>();

    //    public void SetDynamicDisplayName<TModel>(string propertyName, string displayName)
    //    {
    //        var key = typeof(TModel).FullName + "." + propertyName;
    //        _dynamicDisplayNames[key] = displayName;
    //    }

    //    protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
    //    {
    //        var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
    //        var key = containerType.FullName + "." + propertyName;
    //        if (_dynamicDisplayNames.ContainsKey(key))
    //        {
    //            metadata.DisplayName = _dynamicDisplayNames[key];
    //        }
    //        return metadata;
    //    }
    //}
}