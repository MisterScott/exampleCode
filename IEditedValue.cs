
namespace BaseLib
{
    /// <summary> Parsed value has changed, possibly back to the original value.  Events only occur when the input
    /// text is successfully parsed!  notOriginal = the new value != original value.  Should not occur for 
    /// non-significant text formatting differences, e.g. for T = double a value of &quot; 0 &quot; should equal 
    /// &quot;0.000&quot; so changing 0 to 0.000 should not cause an event. </summary>
    /// <param name="sender">Originating object</param>
    /// <param name="newValue">New current value</param>
    /// <param name="newEqualsOriginal">True if value being set == original value </param>
    public delegate void EditedValueChangedEventHandler<in T>(object sender, T newValue, bool newEqualsOriginal);

    /// <summary>
    /// Base interface for IEditedValue&lt;T&gt;.  Edited value supporting editing values as text, change detect, 
    /// validation and cancel to restore the original state.
    /// </summary>
    public interface IEditedValueBase :  ICloseable
    {
        /// <summary> Associated control object </summary>
        object Control { get; set; }
        /// <summary> True if current (last successfully parsed) value == original value.  If the InputText value is 
        /// invalid then the current parsed value will be old!  Ignores non-significant text formatting issues such as 
        /// whitespace and trailing zeroes, e.g. for &lt;double&gt; &quot; 0&quot; == &quot;0.000 &quot; </summary>
        bool EqualsOriginal { get; }
        /// <summary> True if both current (last successfully parsed) value == original value AND the InputText value 
        /// matches the original InputText value, ignoring case and leading / trailing whitespace.  
        /// E.g. &quot; 0 &quot; == &quot;0&quot; </summary>
        bool InputAndValueEqualsOriginal { get; }
        /// <summary> Null if input text is valid, errorMessage from tryParse if not. </summary>
        string InputErrorMessage { get; }
        /// <summary> True if changed InputText passes parsing by tryParse.  Initial value is not required to be valid.
        /// </summary>
        bool InputValid { get; }

        /// <summary> 
        /// Cancel changes:  restore original value, issuing event on change. 
        /// </summary>
        void Cancel();
    }

    /// <summary>
    /// Edited value &lt;T&gt; supporting editing values as text, change detect, validation and cancel to restore the 
    /// original state.  
    /// </summary>
    public interface IEditedValue<out T> : IEditedValueBase
    {
        /// <summary> (Valid) parsed value is about to change </summary>
        event EditedValueChangedEventHandler<T> ValuePreChangeEvent;
        /// <summary> (Valid) parsed value has just changed </summary>
        event EditedValueChangedEventHandler<T> ValueChanged;

        /// <summary> Original value.  Can be changed by SetOriginal(). </summary>
        T OriginalValue { get; }
        /// <summary> Current value.  Will be old if inputText is invalid. </summary>
        T Value { get; }
    }
}