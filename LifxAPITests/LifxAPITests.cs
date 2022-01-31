using Solarflare.LifxAPI;
using Xunit;
using Xunit.Abstractions;


namespace LifxAPITests
{
    public class LifxAPITests
    {
        private readonly ITestOutputHelper testOutputHelper;
        
        public LifxAPITests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }
        
        private readonly LifxService lifxService = new("cd93c061f0ebdc2f181af319cea72ef6344cffd2200ffaaf95691ee028e6fe5e");
        
        [Fact]
        public void Test1()
        {
            testOutputHelper.WriteLine(lifxService.GetDevices().Result);
        }

        private PowerOn payload = new PowerOn
        {
            Power = "on"
        };
            
        [Fact]
        public void TestSetState()
        {
            testOutputHelper.WriteLine(lifxService.SetState(payload).Result);
        }
    }
}

