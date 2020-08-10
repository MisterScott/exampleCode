using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolBox.Modules.Utils
{
    /// <summary>
    /// Argument-checking methods that throw if invalid parameters are seen.
    /// </summary>
    public static class ArgCheck
    {
        // Require a > b.
        public static void AssertGreaterThan<T>(T a, string aName, T b, string bName) where T : IComparable<T>
        {
            AssertNonNullNonEmpty(aName, "aName");
            AssertNonNullNonEmpty(bName, "bName");
            if (a.CompareTo(b) <= 0)
            {
                throw new ArgumentOutOfRangeException("a", String.Format("Require {0}({1}) > {2}({3}", aName, a.ToString(), bName, b.ToString()));
            }
        }

        public static void AssertGreaterThanOrEqual<T>(T a, string aName, T b, string bName) where T : IComparable<T>
        {
            AssertNonNullNonEmpty(aName, "aName");
            AssertNonNullNonEmpty(bName, "bName");
            if (a.CompareTo(b) < 0)
            {
                throw new ArgumentOutOfRangeException("a", String.Format("Require {0}({1}) >= {2}({3}", aName, a.ToString(), bName, b.ToString()));
            }
        }

        public static void AssertMinLessThanMax<T>(T min, string minName, T max, string maxName) where T : IComparable<T>
        {
            AssertNonNullNonEmpty(minName, "minName");
            AssertNonNullNonEmpty(maxName, "maxName");
            if (min.CompareTo(max) >= 0)
            {
                throw new ArgumentOutOfRangeException("max", String.Format("Require {0}({1}) > {2}({3}", minName, min.ToString(), maxName, max.ToString()));
            }
        }

        /// <summary>
        /// Assert that:  min &lt;= a &lt;= max, also that min &lt;= max.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="aName"></param>
        /// <param name="min"></param>
        /// <param name="minName"></param>
        /// <param name="max"></param>
        /// <param name="maxName"></param>
        public static void AssertInRange<T>(T a, string aName, T min, string minName, T max, string maxName) where T : IComparable<T>
        {
            AssertNonNullNonEmpty(aName, "aName");
            AssertNonNullNonEmpty(minName, "minName");
            AssertNonNullNonEmpty(minName, "maxName");
            // a.CompareTo(b):  < 0 if 'a' precedes 'b' in the sort order, = 0 if equal, > 0 if follows.
            if (min.CompareTo(max) > 0) {
                throw new ArgumentOutOfRangeException("max", String.Format("Require {0}({1}) <= {2}({3})",
                    minName, min.ToString(), maxName, max.ToString()));
            }
            if (min.CompareTo(a) > 0) {
                throw new ArgumentOutOfRangeException("a", String.Format("Require {0}({1}) <= {2}({3}", 
                    minName, min.ToString(), aName, a.ToString()));
            }
            if (a.CompareTo(max) > 0) {
                throw new ArgumentOutOfRangeException("a", String.Format("Require {0}({1}) <= {2}({3}", 
                    aName, a.ToString(), maxName, max.ToString()));
            }
        }

        /// <summary>
        /// Require string parameter param to be non-null, non-empty.  
        /// Throw exception with name paramName if null or empty.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="paramName"></param>
        public static void AssertNonNullNonEmpty(string param, string paramName)
        {
            if (string.IsNullOrEmpty(paramName))
            {
                throw new ArgumentException("Must provide a parameter name", "paramName");
            }
            if (null == param)
            {
                throw new ArgumentNullException(String.Format(
                    "Parameter {0} is null, expected non-null, non-empty string", paramName));
            }
            if (String.IsNullOrEmpty(param))
            {
                throw new ArgumentException(String.Format(
                    "Parameter {0} is empty, expected non-null, non-empty string", paramName));
            }
        }

        /// <summary>
        /// Require List&lt;string&gt; parameter param to be non-null, non-empty; does NOT check content strings.
        /// Throw exception with name paramName if null or empty.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="paramName"></param>
        public static void AssertNonNullNonEmpty(List<string> param, string paramName)
        {
            if (string.IsNullOrEmpty(paramName)) {
                throw new ArgumentException("Must provide a parameter name", "paramName");
            }
            if (null == param) {
                throw new ArgumentNullException(String.Format(
                    "Parameter {0} is null, expected non-null, non-empty List<string>", paramName));
            }
            if (0 == param.Count) {
                throw new ArgumentException(String.Format(
                    "Parameter {0} is empty, expected non-null, non-empty List<string>", paramName));
            }
        }

        /// <summary>
        /// Require List&lt;string&gt; parameter param to be non-null, non-empty; ditto content strings.
        /// Throw exception with name paramName if null or empty.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="paramName"></param>
        public static void AssertNonNullNonEmptyEntriesNonNullNonEmpty(List<string> param, string paramName)
        {
            AssertNonNullNonEmpty(param, paramName);
            for (int index = 0; index < param.Count; ++index) {
                if (null == param[index]) {
                    throw new ArgumentNullException(String.Format(
                        "Parameter {0}[{1}] is null, expected non-null, non-empty List<string>" +
                        " containing non-null, non-empty strings", paramName, index));
                }
                if (String.IsNullOrEmpty(param[index])) {
                    throw new ArgumentNullException(String.Format(
                        "Parameter {0}[{1}] is empty, expected non-null, non-empty List<string>" +
                        " containing non-null, non-empty strings", paramName, index));
                }
            }
        }

        /// <summary>
        /// Require object parameter param to be non-null.  
        /// Throw exception with name paramName if null or empty.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="paramName"></param>
        public static void AssertNonNull(object param, string paramName)
        {
            if (string.IsNullOrEmpty(paramName))
            {
                throw new ArgumentException("Must provide a parameter name", "paramName");
            }
            if (null == param)
            {
                throw new ArgumentNullException(String.Format(
                    "Parameter {0} is null, expected non-null", paramName));
            }
        }

        /// <summary>
        /// Throw exception with name paramName if param is not false.
        /// Require object parameter param to be non-null.  
        /// </summary>
        /// <param name="param"></param>
        /// <param name="paramName"></param>
        public static void AssertFalse(bool param, string paramName)
        {
            if (string.IsNullOrEmpty(paramName)) {
                throw new ArgumentException("Must provide a parameter name", "paramName");
            }
            if (false != param) {
                throw new ArgumentOutOfRangeException(String.Format(
                    "Parameter {0} is true, expected false", paramName));
            }
        }

        /// <summary>
        /// Throw exception with name paramName if param is not true.
        /// Require object parameter param to be non-null.  
        /// </summary>
        /// <param name="param"></param>
        /// <param name="paramName"></param>
        public static void AssertTrue(bool param, string paramName)
        {
            if (string.IsNullOrEmpty(paramName)) {
                throw new ArgumentException("Must provide a parameter name", "paramName");
            }
            if (true != param) {
                throw new ArgumentOutOfRangeException(String.Format(
                    "Parameter {0} is false, expected true", paramName));
            }
        }
    }
}
