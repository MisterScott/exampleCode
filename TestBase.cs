using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ToolBox.Modules.LoggingManagement;
using ToolBox.Modules.Utils;

namespace EKOS_TestLib
{
    /// <summary>
    /// Base class for test objects, providing the information required for 
    /// selecting tests by name.
    /// </summary>
    /// <typeparam name="T">Type of class implementing ITest</typeparam>
    /// <typeparam name="U">Type of class implementing ITestParam</typeparam>
    /// <typeparam name="V">Type of class implementing ITestState</typeparam>
    public abstract class TestBase<T, U, V> : ITest
            where T : class, ITest
            where U : class, ITestParam
            where V : class, ITestState
    {
        /// <summary>
        /// Identifying information for this test instance
        /// </summary>
        public TestId TestID { get; private set; }
        private static object s_lockObject = new object();
        protected static V s_stateInternal = (V)Activator.CreateInstance<V>();
        
        public static object s_LockObject
        {
            get { return s_lockObject; }
        }

        public System.Type TestParamType {
            get { return typeof(U); }
        }

        public System.Type TestStateType {
            get { return typeof(V); }
        }

        public ITestParam CreateDefaultTestParams()
        {
            return (U)Activator.CreateInstance<U>();
        }

        public U AsTestParamsType(ITestParam testParam) { return testParam as U; }
        public U ToTestParamsType(ITestParam testParam) { return (U)testParam; }

        private static T s_instance;
        public static T Instance()
        {
            lock (s_LockObject)
            {
                if (null == s_instance)
                {
                    s_instance = (T)Activator.CreateInstance<T>();
                }
                return s_instance;
            }
        }

        protected TestBase(string testName)
        {
            this.TestID = new TestId(testName);
        }

        /// <summary>
        /// Get the list of required equipment for this test to run.  Expected that tests will
        /// override this.
        /// For 'adaptive' tests that support alternative equipment configurations, implementation may
        /// return a different list based on the available equipment; expected that parameters in inputParams
        /// will indicate the configuration chosen.
        /// See / use BaseGetRequiredControlsIds() for a default implementation.
        /// </summary>
        /// <returns></returns>
        public abstract List<string> GetRequiredControlsIds(ITestParam inputParams);

        /// <summary>
        /// Attempt to execute the test, updating TestState.Instance().
        /// </summary>
        /// <returns></returns>
        public abstract TestResults Run(ITestParam testParam);

        /// <summary>
        /// Base implementation of GetRequiredControlsIds() when there are no channel-related controls.
        /// </summary>
        /// <param name="_testParam"></param>
        /// <param name="controlList"></param>
        /// <param name="doLogControlList"></param>
        /// <returns></returns>
        public List<string> BaseGetRequiredControlsIds(ITestParam _testParam, 
            List<string> controlList,
            bool doLogControlList = true)
        {
            string method = this.TestID.TestName + ".GetRequiredControlsIds: ";

            var testParam = AsTestParamsType(_testParam);
            ArgCheck.AssertNonNull(testParam, "testParam");
            var list = new List<string>(controlList);
            if (doLogControlList)
            {
                AppLogger.WriteLineInfo("{0}Required controls:  {1}",
                    method, string.Join(", ", list));
            }
            return list;
        }
    }
}
