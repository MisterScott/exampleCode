using System;
namespace EKOS_TestLib
{
    /// <summary>
    /// Tests must implement this interface.
    /// </summary>
    public interface ITest
    {
        /// <summary>
        /// ID for this test.
        /// </summary>
        TestId TestID { get; }

        /// <summary>
        /// Specific type implementing ITestParam required for this test.
        /// </summary>
        /// <returns></returns>
        System.Type TestParamType { get; }

        /// <summary>
        /// Create instance of default test params.
        /// </summary>
        /// <returns></returns>
        ITestParam CreateDefaultTestParams();

        /// <summary>
        /// Specific type implementing ITestState required for this test.
        /// </summary>
        /// <returns></returns>
        System.Type TestStateType { get; }

        /// <summary>
        /// Return a list of the IDs for the specific controls ("uip", "fepa", "ccls", etc.) required
        /// in order to execute this test.
        /// If alternate configurations are supported, the active configuration should be recorded in inputParams.
        /// </summary>
        /// <param name="inputParams">Test parameters, may include channel selects etc.</param>
        /// <returns>List of IDs of the specific controls required to run this test</returns>
        System.Collections.Generic.List<string> GetRequiredControlsIds(ITestParam inputParams);

        /// <summary>
        /// Perform the test, updating TestState.Instance().
        /// </summary>
        /// <param name="testParam"></param>
        /// <returns></returns>
        TestResults Run(ITestParam testParam);
    }
}
