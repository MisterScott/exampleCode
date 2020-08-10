using System;
using System.Collections.Generic;
using System.Net;

namespace BaseLib
{
    /// <summary>
    /// Utilities for argument checking
    /// </summary>
    public static class BaseArgUtils
    {
        /// <summary>
        /// Check maximum value, display units as inches.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxValue"></param>
        /// <param name="argumentName"></param>
        public static void ArgumentMaxCheckInch(double value, double maxValue,
            string argumentName)
        {
            if (maxValue < value)
            {
                throw new ArgumentException(argumentName + "=" + value +
                    "\", above maximum expected " + maxValue + "\".");
            }
        }

        /// <summary>
        /// Check minimum value, display units as inches.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="argumentName"></param>
        public static void ArgumentMinCheckInch(double value, double minValue,
            string argumentName)
        {
            if (minValue > value)
            {
                throw new ArgumentException(argumentName + "=" + value +
                    "\", below minimum expected " + minValue + "\".");
            }
        }

        /// <summary>
        /// Throw if list contains aValue.  If it does, throw an exception with message including
        /// valueName.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="aValue"></param>
        /// <param name="valueName"></param>
        public static void CheckListDoesNotContain<T>(List<T> list, T aValue, string valueName)
        {
            NullCheck(list, "list");
            if (list.Contains(aValue))
            {
                throw new ArgumentException("List should not contain " + valueName + "=" + aValue);
            }
        }

        /// <summary>
        /// Throw ArgumentOutOfRangeException with argumentMinName, argumentMaxName 
        /// etc. in the message if min is &gt; max.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="argumentMinName"></param>
        /// <param name="argumentMaxName"></param>
        /// <returns></returns>
// ReSharper disable InconsistentNaming
        public static void CheckMinLEMax<T>(T min, T max, string argumentMinName, 
// ReSharper restore InconsistentNaming
            string argumentMaxName) where T : IComparable<T>
        {
            if (min.CompareTo(max) > 0)
            {
                NullCheck(argumentMinName, "argumentMinName");
                NullCheck(argumentMaxName, "argumentMaxName");
                throw new ArgumentOutOfRangeException(
                    argumentMinName + "=" + min + " must be < " + 
                    argumentMaxName + "=" + max + ".");
            }
        }

        /// <summary>
        /// Throw ArgumentOutOfRangeException with argumentName etc. in the 
        /// message if value is outside minValue..maxValue.
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="value"></param>
        /// <param name="argumentName"></param>
        public static T CheckRange<T>(T minValue, T maxValue, T value,
            string argumentName) where T : IComparable<T>
        {
            CheckMinLEMax(minValue, maxValue, "minValue", "maxValue");
            if (minValue.CompareTo(value) > 0)
            {
                throw new ArgumentOutOfRangeException(argumentName + "=" + value + 
                    ", valid range " + minValue + " to " + maxValue + ".");
            }
            if (maxValue.CompareTo(value) < 0)
            {
                throw new ArgumentOutOfRangeException(argumentName + "=" + value + 
                    ", valid range " + minValue + " to " + maxValue + ".");
            }
            return value;
        }

        /// <summary>
        /// Throw IndexOutOfRangeException with argument name etc. in the message if index is outside the range
        /// 0..(length-1).  Return index.
        /// </summary>
        /// <param name="length"> </param>
        /// <param name="index"> </param>
        /// <param name="indexName"> </param>
        /// <returns></returns>
        public static int CheckIndex(int length, int index, string indexName)
        {
            if ((index < 0) || (index >= length))
            {
                throw new IndexOutOfRangeException(indexName + "=" + index +
                    ", valid range 0 to " + (length - 1) + ".");
            }
            return index;
        }

        /// <summary>
        /// Throw ArgumentOutOfRangeException if (byte) argument is not equal to required value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="requiredValue"></param>
        /// <param name="argumentName"></param>
        public static byte CheckEqual(byte value, byte requiredValue, string argumentName)
        {
            if (requiredValue != value)
            {
                throw new ArgumentOutOfRangeException(argumentName + "(byte)=" +
                    value + ", must be " + requiredValue + ".");
            }
            return value;
        }

        /// <summary>
        /// Throw if array lengths are not equal or on null a, b.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="argumentNameA"></param>
        /// <param name="argumentNameB"></param>
// ReSharper disable InconsistentNaming
        public static void CheckEqualLength<T,U>(T[] a, U[] b, string argumentNameA,
// ReSharper restore InconsistentNaming
            string argumentNameB)
        {
            NullCheck(a, argumentNameA);
            NullCheck(b, argumentNameB);
            if (a.Length != b.Length)
            {
                throw new ArgumentOutOfRangeException("Length of array " + argumentNameA +
                    "[" + a.Length + "] must match array " + argumentNameB + 
                    "[" + b.Length + "].");
            }
        }

        /// <summary>
        /// Throw if array lengths are not equal or if any of a, b, c are null.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="argumentNameA"></param>
        /// <param name="argumentNameB"></param>
        /// <param name="argumentNameC"></param>
// ReSharper disable InconsistentNaming
        public static void CheckEqualLength<T, U, V>(T[] a, U[] b, V[] c, string argumentNameA,
// ReSharper restore InconsistentNaming
            string argumentNameB, string argumentNameC)
        {
            NullCheck(a, argumentNameA);
            NullCheck(b, argumentNameB);
            NullCheck(c, argumentNameC);
            if ((a.Length != b.Length) || (a.Length != c.Length))
            {
                throw new ArgumentOutOfRangeException("Length of arrays " + 
                    argumentNameA + "[" + a.Length + "]," +
                    argumentNameB + "[" + b.Length + "]," +
                    argumentNameC + "[" + c.Length + "] must match");
            }
        }

        /// <summary>
        /// Throw if array lengths are not equal.  Accepts null == null.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="argumentNameA"></param>
        /// <param name="argumentNameB"></param>
        public static T[] CheckEqualLengthNullOk<T>(T[] a, T[] b, string argumentNameA,
            string argumentNameB)
        {
            int lengthA = (null == a) ? -1 : a.Length;
            int lengthB = (null == b) ? -1 : b.Length;
            if (lengthA != lengthB)
            {
                throw new ArgumentOutOfRangeException("Length of array " + argumentNameA +
                    ((null == a) ? "null" : ("[" + lengthA + "]")) + " must match array " + argumentNameB +
                    ((null == b) ? "null" : ("[" + lengthB + "]")) + ".");
            }
            return a;
        }

        /// <summary>
        /// Parse and check IP address in string form.  Requires ipAddressString is not null or empty (once trimmed).
        /// IP address must be of form a.b.c.d (e.g. 192.168.1.234).
        /// On success return the resulting IPAddress and set errorMessage = null.  
        /// On failure throw.
        /// </summary>
        public static IPAddress CheckIPv4Address(string ipAddressString, string argumentName)
        {
            ipAddressString = BaseStringUtils.TrimThrowIfNullOrEmpty(ipAddressString, 
                "Expected IPv4 address (example 192.168.10.4) in " + argumentName);
            IPAddress ipAddress;
            bool success;
            try
            {
                success = IPAddress.TryParse(ipAddressString, out ipAddress);
            }
            catch (ArgumentException ex)
            {
                // Thrown for Unicode characters.
                throw new ArgumentException("Invalid IPAddress in " + argumentName + 
                    ".  Expected IPv4 address (example \"192.168.10.4\"), found \"" + ipAddressString + 
                    "\".  Only characters 0-9 and . are allowed.", ex);
            }
            if (!success)
            {
                throw new ArgumentException("Invalid IPAddress in " + argumentName +
                    ".  Expected IPv4 address (example \"192.168.10.4\"), found \"" + ipAddressString +
                    "\".  Only characters 0-9 and . are allowed.");
            }
            return ipAddress;
        }

        /// <summary>
        /// Return true if a == b, accepts null == null.  Checks array lengths
        /// and all elements.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static bool EqualsNullOK(byte[] a, byte[] b)
        {
            bool aNull = null == a;
            bool bNull = null == b;
            bool equal = aNull == bNull;
            if (!aNull && !bNull)
            {
                int aLength = a.Length;
                int bLength = b.Length;
                equal = aLength.Equals(bLength);
                for (int index = 0; equal && (index < aLength); ++index)
                {
                    equal = a[index] == b[index];
                }
            }
            return equal;
        }

        /// <summary>
        /// Throw if (byte[]) argument is null or not equal to required value.
        /// passes if non-null and array lengths and contents are equal.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="requiredValue"></param>
        /// <param name="argumentName"></param>
        public static byte[] CheckEqual(byte[] value, byte[] requiredValue, 
            string argumentName)
        {
            NullCheck(value, "value");
            NullCheck(requiredValue, "requiredValue");
            int lengthValue = value.Length;
            int lengthRequired = requiredValue.Length;
            if (lengthValue != lengthRequired)
            {
                throw new ArgumentOutOfRangeException("length mismatch:  " +
                    argumentName + "(byte[" + lengthValue + 
                    "]) must be byte[" + lengthRequired + "])");
            }
            for (int index = 0; index < lengthRequired; ++index)
            {
                if (requiredValue[index] != value[index])
                {
                    throw new ArgumentOutOfRangeException(argumentName + "[" +
                        index + "]=0x" + value[index].ToString("X") +
                        ", must be 0X" + requiredValue[index].ToString("X") +
                        ".");
                }
            }
            return value;
        }

        /// <summary>
        /// Get hashcode for byte[] array contents.  Note:  if array contents
        /// will change, they don't belong in an object's hashcode!
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetHashCode(byte[] value)
        {
            int hash = 0;
            int length = value.Length;
            hash = (hash * 17) + length;
            for (int index = 0; index < length; ++index)
            {
                hash = hash * 17 + value[index];
            }
            return hash;
        }

        /// <summary>
        /// Throw if (bool) argument is not equal to required value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="requiredValue"></param>
        /// <param name="argumentName"></param>
        public static bool CheckEqual(bool value, bool requiredValue, 
            string argumentName)
        {
            if (requiredValue != value)
            {
                throw new ArgumentOutOfRangeException(argumentName + "(bool)=" +
                    value + ", must be " + requiredValue + ".");
            }
            return value;
        }

        /// <summary>
        /// Throw if (int) argument is not equal to required value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="requiredValue"></param>
        /// <param name="argumentName"></param>
        public static int CheckEqual(int value, int requiredValue, string argumentName)
        {
            if (requiredValue != value)
            {
                throw new ArgumentOutOfRangeException(argumentName + "(int)=" +
                    value + ", must be " + requiredValue + ".");
            }
            return value;
        }

        /// <summary>
        /// Throw if (UInt16) argument is not equal to required value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="requiredValue"></param>
        /// <param name="argumentName"></param>
        public static UInt16 CheckEqual(UInt16 value, UInt16 requiredValue,
            string argumentName)
        {
            if (requiredValue != value)
            {
                throw new ArgumentOutOfRangeException(argumentName + "(UInt16)=" +
                    value + ", must be " + requiredValue + ".");
            }
            return value;
        }

        /// <summary>
        /// Throw if (UInt32) argument is not equal to required value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="requiredValue"></param>
        /// <param name="argumentName"></param>
        public static UInt32 CheckEqual(UInt32 value, UInt32 requiredValue, 
            string argumentName)
        {
            if (requiredValue != value)
            {
                throw new ArgumentOutOfRangeException(argumentName + "(UInt32)=" +
                    value + ", must be " + requiredValue + ".");
            }
            return value;
        }

        /// <summary>
        /// Throw if (string) argument is not equal to required value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="requiredValue"></param>
        /// <param name="argumentName"></param>
        public static string CheckEqual(string value, string requiredValue,
            string argumentName)
        {
            bool nullValue = null == value;
            bool nullRequired = null == requiredValue;
            if ((nullValue != nullRequired) ||
                (!nullValue && (!requiredValue.Equals(value))))
            {
                throw new ArgumentOutOfRangeException(argumentName + "(string)=\"" +
                    (nullValue ? "null" : value) + 
                    "\", must be \"" + (nullRequired ? "null" : requiredValue) + "\".");
            }
            return value;
        }

        /// <summary>
        /// Throw if (string) argument is not equal to required value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="requiredValue"></param>
        /// <param name="argumentName"></param>
        public static string CheckEqualIgnoreCase(string value, string requiredValue,
            string argumentName)
        {
            bool nullValue = null == value;
            bool nullRequired = null == requiredValue;
            if ((nullValue != nullRequired) ||
                (!nullValue && (!requiredValue.Equals(value, StringComparison.InvariantCultureIgnoreCase))))
            {
                throw new ArgumentOutOfRangeException(argumentName + "(string)=\"" +
                    (nullValue ? "null" : value) +
                    "\", must be \"" + (nullRequired ? "null" : requiredValue) + "\" ignoring case.");
            }
            return value;
        }

        /// <summary>
        /// Add error message to results and return true if (string) argument is not equal to required value,
        /// else return false.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="requiredValue"></param>
        /// <param name="argumentName"></param>
        /// <param name="results"></param>
        public static bool RecordIfMismatch(string value, string requiredValue,
            string argumentName, InfoWarningErrorLists results)
        {
            NullCheck(results, "results");
            bool nullValue = null == value;
            bool nullRequired = null == requiredValue;
            if ((nullValue != nullRequired) ||
                (!nullValue && (!requiredValue.Equals(value))))
            {
                results.TryAddError(argumentName + " mismatch!  Required=" + BaseStringUtils.QuotedOrNull(requiredValue) +
                    ", value=" + BaseStringUtils.QuotedOrNull(value));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add error message to results and return true if (bool) argument is not equal to required value,
        /// else return false.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="requiredValue"></param>
        /// <param name="argumentName"></param>
        /// <param name="results"></param>
        public static bool RecordIfMismatch(bool value, bool requiredValue,
            string argumentName, InfoWarningErrorLists results)
        {
            NullCheck(results, "results");
            if (!requiredValue.Equals(value))
            {
                results.TryAddError(argumentName + " mismatch!  Required=" + requiredValue +
                    ", value=" + value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add error message to results and return true if (int) argument is not equal to required value,
        /// else return false.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="requiredValue"></param>
        /// <param name="argumentName"></param>
        /// <param name="results"></param>
        public static bool RecordIfMismatch(int value, int requiredValue,
            string argumentName, InfoWarningErrorLists results)
        {
            NullCheck(results, "results");
            if (!requiredValue.Equals(value))
            {
                results.TryAddError(argumentName + " mismatch!  Required=" + requiredValue +
                    ", value=" + value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Add error message to results and return true if (int) argument is not within required value range,
        /// else return false.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueRangeMin"></param>
        /// <param name="valueRangeMax"></param>
        /// <param name="argumentName"></param>
        /// <param name="results"></param>
        public static bool RecordIfMismatchRange(int value, int valueRangeMin, int valueRangeMax,
            string argumentName, InfoWarningErrorLists results)
        {
            NullCheck(results, "results");
            CheckMinLEMax(valueRangeMin, valueRangeMax, "valueRangeMin", "valueRangeMax");
            if ((value < valueRangeMin) || (value > valueRangeMax))
            {
                results.TryAddError(argumentName + " mismatch!  Range=" + valueRangeMin +
                    " to " + valueRangeMax + ", value=" + value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Throw if (byte) argument is equal to required value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bannedValue"></param>
        /// <param name="argumentName"></param>
        public static byte CheckNotEqual(byte value, byte bannedValue, 
            string argumentName)
        {
            if (bannedValue == value)
            {
                throw new ArgumentOutOfRangeException(argumentName + "(byte)=" +
                    value + ", must NOT be " + bannedValue + ".");
            }
            return value;
        }

        /// <summary>
        /// Throw if double argument is above maximum value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxValue"></param>
        /// <param name="argumentName"></param>
        public static double CheckMax(double value, double maxValue, 
            string argumentName)
        {
            if (maxValue < value)
            {
                throw new ArgumentOutOfRangeException(argumentName +
                    "(double)=" + value + ", above maximum expected " + 
                    maxValue + ".");
            }
            return value;
        }

        /// <summary>
        /// Throw if int argument is above maximum value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxValue"></param>
        /// <param name="argumentName"></param>
        public static int CheckMax(int value, int maxValue, string argumentName)
        {
            if (maxValue < value)
            {
                throw new ArgumentOutOfRangeException(argumentName + "(int)=" + 
                    value + ", above maximum expected " + 
                    maxValue + ".");
            }
            return value;
        }

        /// <summary>
        /// Throw if byte argument is above maximum value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxValue"></param>
        /// <param name="argumentName"></param>
        public static byte CheckMax(byte value, byte maxValue, string argumentName)
        {
            if (maxValue < value)
            {
                throw new ArgumentOutOfRangeException(argumentName + "(byte)=" +
                    value + ", above maximum expected " +
                    maxValue + ".");
            }
            return value;
        }

        /// <summary>
        /// Throw if UInt32 argument is above maximum value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxValue"></param>
        /// <param name="argumentName"></param>
        public static UInt32 CheckMax(UInt32 value, UInt32 maxValue, 
            string argumentName)
        {
            if (maxValue < value)
            {
                throw new ArgumentOutOfRangeException(argumentName + "(UInt32)=" +
                    value + ", above maximum expected " + 
                    maxValue + ".");
            }
            return value;
        }

        /// <summary>
        /// Throw ArgumentOutOfRangeException with argumentName etc. in the 
        /// message if argument is below minimum value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="argumentName"></param>
        public static double CheckMin(double value, double minValue, string argumentName)
        {
            if (minValue > value)
            {
                throw new ArgumentOutOfRangeException(argumentName + "=" + value +
                    ", below minimum expected " + minValue + ".");
            }
            return value;
        }

        /// <summary>
        /// Throw ArgumentOutOfRangeException with argumentName etc. in the 
        /// message if argument is below minimum value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="argumentName"></param>
        public static Int16 CheckMin(Int16 value, Int16 minValue, string argumentName)
        {
            if (minValue > value)
            {
                throw new ArgumentOutOfRangeException(argumentName + "(Int16)=" +
                    value + ", below minimum expected " + minValue + ".");
            }
            return value;
        }

        /// <summary>
        /// Throw ArgumentOutOfRangeException with argumentName etc. in the 
        /// message if argument is below minimum value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="argumentName"></param>
        public static int CheckMin(int value, int minValue, string argumentName)
        {
            if (minValue > value)
            {
                throw new ArgumentOutOfRangeException(argumentName + "(int)=" + 
                    value + ", below minimum expected " + minValue + ".");
            }
            return value;
        }

        /// <summary>
        /// Throw InvalidProgramException, calling getErrorMessage to get the
        /// message, if value is below minValue.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="errorMessage"></param>
        public static int ThrowIfBelowMin(int value, int minValue, string errorMessage)
        {
            if (minValue > value)
            {
                throw new ArgumentOutOfRangeException(errorMessage + "(int)=" +
                    value + " must be >= " + minValue + ".");
            }
            return value;
        }

        /// <summary>
        /// Throw InvalidProgramException, calling getErrorMessage to get the
        /// message, if value is below minValue.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="getErrorMessage"></param>
        public static int ThrowIfBelowMin(int value, int minValue, GetErrorMessage getErrorMessage)
        {
            if (minValue > value)
            {
                throw new ArgumentOutOfRangeException(getErrorMessage(value) + "(int)=" +
                    value + " must be >= " + minValue + ".");
            }
            return value;
        }

        /// <summary>
        /// Throw ArgumentOutOfRangeException with argumentName etc. in the
        /// message if argument is below minimum value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="argumentName"></param>
        public static UInt16 CheckMin(UInt16 value, UInt16 minValue, string argumentName)
        {
            if (minValue > value)
            {
                throw new ArgumentOutOfRangeException(argumentName + "(UInt16)=" + value + ", below minimum expected " + minValue + ".");
            }
            return value;
        }

        /// <summary>
        /// Throw ArgumentOutOfRangeException with argumentName etc. in the message if argument is below minimum value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="minValue"></param>
        /// <param name="argumentName"></param>
        public static UInt32 CheckMin(UInt32 value, UInt32 minValue, string argumentName)
        {
            if (minValue > value)
            {
                throw new ArgumentOutOfRangeException(argumentName + "(UInt32)=" + value + ", below minimum expected " + minValue + ".");
            }
            return value;
        }

        /// <summary>
        /// Givan an enum value, check whether it's valid.  Throw ArgumentOutOfRangeException if not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T EnumCheck<T>(T value)
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                throw new ArgumentOutOfRangeException("Expected value of type " + typeof(T) + ", found \"" + value + "\"");
            }
            return value;
        }

        public static T EnumCheck<T>(T value, string valueName)
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                throw new ArgumentOutOfRangeException("Expected " + typeof(T) + " value in field \"" + valueName + "\", found \"" + value +
                "\"");
            }
            return value;
        }

        /// <summary>
        /// If item is null, throw ArgumentNullException with name in the message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T NullCheck<T>(T item, string name) where T : class
        {
            if (null == item)
            {
                throw new ArgumentNullException(name);
            }
            return item;
        }

        /// <summary>
        /// Use to prevent a value from being set to null or set non-null twice, important if e.g. we attach event 
        /// handlers.
        /// If value is null, throw ArgumentNullException with name in the message.
        /// If destination value is non-null (already set), throw InvalidOperationException with name in the message.
        /// Set destination to value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="destination"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static void NullOrSetTwiceCheck<T>(T value, ref T destination, string name) where T : class
        {
            if (null == value)
            {
                throw new ArgumentNullException(name);
            }
            if (null != destination)
            {
                throw new InvalidOperationException(name);
            }
            destination = value;
            //return value;
        }

        /// <summary>
        /// If item is null, throw ArgumentNullException with name in the message.
        /// If item is empty, throw ArgumentException with name in the message.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string NullEmptyCheck(string item, string name)
        {
            if (null == item)
            {
                throw new ArgumentNullException(name);
            }
            if (0 == item.Length)
            {
                throw new ArgumentException("Need non-empty string for " + name);
            }
            return item;
        }

        /// <summary>
        /// Use to prevent a value from being set twice.  Use for setting values that are allowed to be null,
        /// so an additional valueSet flag is used.
        /// If valueSet is true, throw InvalidOperationException with name in the message.
        /// Set destination to value, set valueSet to true.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="valueSet"></param>
        /// <param name="destination"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T NullOrSetTwiceCheck<T>(T value, ref bool valueSet, ref T destination, string name) where T : class
        {
            if (valueSet)
            {
                throw new InvalidOperationException("Already set!  " + name);
            }
            valueSet = true;
            destination = value;
            return value;
        }

        /// <summary>
        /// If item is null, throw ArgumentNullException with destinationTypeName, valueName, recordNumber in the message.
        /// </summary>
        public static T NullCheck<T>(T item, string destinationTypeName, string valueName, uint recordNumber) where T : class
        {
            NullCheck(destinationTypeName, "destinationTypeName");
            NullCheck(valueName, "valueName");
            if (null == item)
            {
                throw new FormatException("Expected " + destinationTypeName + " value in field \"" +
                    valueName + "\" in record " + recordNumber + ", got null.");
            }
            return item;
        }
    }
}
