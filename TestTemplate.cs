using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

using EKOS_TestLib;
using EKOS_TestLibPt4;
using ToolBox.Modules.AutomationControl;
using ToolBox.Modules.StreamManagement;
using ToolBox.Modules.LoggingManagement;
using ToolBox.Modules.Utils;

namespace TestMonkey
{
    public class TestParamTemplate : ITestParam
    {
        public UInt32 IterationsToExecute { get; set; }

        public TestParamTemplate()
        {
            IterationsToExecute = 1;
        }
    }

    public class TestStateTemplate : TestStateBase<TestStateTemplate>, ITestState
    {
        public UInt32 IterationCount { get; set; }

        public void ResetCounts()
        {
            IterationCount = 0;
        }

        public TestStateTemplate()
        {
            ResetCounts();
        }
    }

    public class TestTemplate : TestBase<TestTemplate,
        TestParamTemplate,
        TestStateTemplate>,
        ITest
    {
        /// <summary> Base equipment list; need to add per-channel lists.
        /// </summary>
        static List<string> s_EquipmentListBase = new List<string> {
            ControlIds.PT4.Uip,
            ControlIds.General.AcUsb,
        };

        //static TestStateTemplate s_stateInternal = new TestStateTemplate();

        public TestTemplate() :
            base("Template")
        {
        }

        public override List<string> GetRequiredControlsIds(ITestParam _testParam)
        {
            string method = this.TestID.TestName + ".GetRequiredControlsIds: ";

            TestParamTemplate testParam = _testParam as TestParamTemplate;
            ArgCheck.AssertNonNull(testParam, "testParam");
            var list = new List<string>(s_EquipmentListBase);
            AppLogger.WriteLineInfo("{0}Required controls:  {1}",
                method, string.Join(", ", list));
            return list;
        }

        /// <summary>
        /// Perform soft power cycle
        /// Optional:  pass TestParamTemplate as testParam to control test.
        /// </summary>
        /// <param name="testParam"></param>
        /// <returns></returns>
        public override TestResults Run(ITestParam testParam)
        {
            string method = this.TestID.TestName + ".Run: ";

            var settings = (testParam as TestParamTemplate) ?? new TestParamTemplate();
            var state = s_stateInternal;
            var testState = TestState.Instance();

            UInt32 iterationsToExecute = settings.IterationsToExecute;
            UInt32 iterationCount = 0;
            int secondsToWait = 30;

            AppLogger.WriteLine(AppLogger.LEVEL_INFO, Consts.DoubleDividerLine);
            AppLogger.WriteLine(AppLogger.LEVEL_INFO, "\tBegin {0} ({1})",
                method, settings.IterationsToExecute);
            AppLogger.WriteLine(AppLogger.LEVEL_INFO, Consts.DoubleDividerNewLine);

            // Test implemented here

            while (++iterationCount <= iterationsToExecute)
            {
                AppLogger.WriteLineInfo("{0}Executing iteration {1} of {2}...", 
                    method, iterationCount, iterationsToExecute);

                // Test implemented here

                if (<something fails>)
                {
                    AppLogger.WriteLineError("{0}<something> failed!", method);
                    testState.ReportTestIndeterminate("<something> failed");
                    return TestResults.Indeterminate;
                }

                // Do more testing
            }

            return TestResults.Pass;
        }
    } // class TestTemplate

} // namespace EKOS_TestMonkey
