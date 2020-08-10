using System.Collections.Generic;

namespace BaseLib
{
    /// <summary>
    /// Generic value that sends ValuePreChangeEvent before and ValueChanged event 
    /// after the value is changed.
    /// 
    /// For value types uses value.Equals() for comparisons.
    /// For reference types tests reference equality (null == null) then value.Equals().
    /// 
    /// Don't need for IObserved, IPropertyNotifying container types that 
    /// aren't changed directly but contain fields that do change.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NotifyingValue<T> : BaseCloseable, ICloseable
    {
        public new const string ClassName = "NotifyingValue<T>";
        public delegate void ValueChangedEventHandler(object sender, T newValue);
        /// <summary> Value is about to change </summary>
        public event ValueChangedEventHandler ValuePreChangeEvent;
        /// <summary> Value has just changed </summary>
        public event ValueChangedEventHandler ValueChanged;

        private bool _enableNotify;
        private readonly bool _isValueType = typeof(T).IsValueType;
        private T _value;

        /// <summary> Get / set the value.  
        /// If the new value does not match the old value (null == null, null != any
        /// non-null value) then:
        /// <list type="bullet">
        /// <item><description>ValuePreChangeEvent event is fired if enableNotify is true</description></item>
        /// <item><description>the value is updated</description></item>
        /// <item><description>ValueChanged event is fired if enableNotify is true</description></item>
        /// </list>
        /// When modifying the value without using property set (e.g. for a Point3D using Value.SetTo()) you
        /// must use .ForceNotify() to cause the events.
        /// </summary>
        public virtual T Value
        {
            get { return _value; }
            set
            {
                // Attempt to properly handle value vs. reference types with minimal overhead.
                if (_isValueType 
                    ? !value.Equals(_value) 
// ReSharper disable CompareNonConstrainedGenericWithNull
                    : ((null == value) && (null != _value)) ||
                      ((null != value) && (null == _value)) ||
                      ((null != value) && (null != _value) && !value.Equals(_value)))
// ReSharper restore CompareNonConstrainedGenericWithNull
                {
                    if (_enableNotify && (null != ValuePreChangeEvent))
                    {
                        ValuePreChangeEvent(this, value);
                    }
                    _value = value;
                    if (_enableNotify && (null != ValueChanged))
                    {
                        ValueChanged(this, value);
                    }
                }
            }
        }

        /// <summary>  REALLY test-only </summary>
        public ValueChangedEventHandler TestOnlyValuePreChangeEvent
        {
            get { return ValuePreChangeEvent; }
        }

        /// <summary>  REALLY test-only </summary>
        public ValueChangedEventHandler TestOnlyValueChangedEvent
        {
            get { return ValueChanged; }
        }

        /// <summary>
        /// Ctor, initial value is default for T, enableNotify == true
        /// </summary>
        public NotifyingValue()
        {
            _enableNotify = true;
        }

        /// <summary>
        /// Ctor with initial value, enableNotify == true
        /// </summary>
        /// <param name="value"></param>
        public NotifyingValue(T value)
        {
            _value = value;
            _enableNotify = true;
        }

        /// <summary>
        /// Ctor with initial value, enableNotify == true, add this to iCloseable list
        /// </summary>
        /// <param name="value"></param>
        /// <param name="iCloseable"></param>
        public NotifyingValue(T value, List<ICloseable> iCloseable)
            : this(value)
        {
            // Register self for closing later.
            iCloseable.Add(this);
        }

        public void CutEventHandlers()
        {
            ValuePreChangeEvent = null;
            ValueChanged = null;
        }

        /// <summary>
        /// Close, cut event handlers, if value is IObservable unregister.
        /// </summary>
        protected override void DoClose()
        {
            base.DoClose();
            CutEventHandlers();
        }

        /// <summary>
        /// Suspend / resume notifications resulting from changing Value (doesn't 
        /// affect other notifications)
        /// </summary>
        public void EnableNotify(bool enableNotify)
        {
            _enableNotify = enableNotify;
        }

        /// <summary>
        /// For use after we add our event handlers to get everything updated.
        /// Fires ValuePreChangeEvent, ValueChanged events.
        /// NOTE:  If anyone assumes the value DID change for them to get an event, could cause problems.
        /// </summary>
        public void ForceNotify()
        {
            if (null != ValuePreChangeEvent)
            {
                ValuePreChangeEvent(this, _value);
            }
            if (null != ValueChanged)
            {
                ValueChanged(this, _value);
            }
        }

        /// <summary>
        /// Use for initialization without updates.
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetValueNoUpdate(T value)
        {
            _value = value;
        }

        /// <summary>
        /// Set value with fine-grained control of updates.
        /// Notify if (value changes AND allowNotify) OR forceNotify.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="allowNotify"></param>
        /// <param name="forceNotify"></param>
        public virtual void SetValueConditionalUpdate(T value, bool allowNotify, bool forceNotify)
        {
            bool doNotify = forceNotify ||
                (allowNotify && _enableNotify &&
                // Attempt to properly handle value vs. reference types with minimal overhead.
                 (_isValueType 
                  ? !value.Equals(_value) 
// ReSharper disable CompareNonConstrainedGenericWithNull
                  : (((null == value) && (null != _value)) ||
                     ((null != value) && (null == _value)) ||
                     ((null != value) && (null != _value) && (!value.Equals(_value))))));
// ReSharper restore CompareNonConstrainedGenericWithNull
            if (doNotify && (null != ValuePreChangeEvent))
            {
                ValuePreChangeEvent(this, value);
            }
            _value = value;
            if (doNotify && (null != ValueChanged))
            {
                ValueChanged(this, value);
            }
        }

        /// <summary>
        /// Use when manually handling updates.
        /// </summary>
        public virtual void SetValueForceUpdate(T value)
        {
            if (null != ValuePreChangeEvent)
            {
                ValuePreChangeEvent(this, value);
            }
            _value = value;
            if (null != ValueChanged)
            {
                ValueChanged(this, value);
            }
        }

        /// <summary>
        /// Customized, includes class name.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "NotifyingValue<" + typeof(T) + "(" + (_isValueType ? "V" : "R") + ")>(" + _value + ",en=" + _enableNotify + ")";
        }

        /// <summary>
        /// Short-form string, no class name
        /// </summary>
        /// <returns></returns>
        public virtual string ToShortString()
        {
            return "(" + _value + ",en=" + _enableNotify + ")";
        }

        /// <summary>
        /// Shortest-form string, no class name or parentheses
        /// </summary>
        /// <returns></returns>
        protected virtual string ToStringInner()
        {
            return _value + "," + _enableNotify;
        }
    }
}
