namespace Solarflare.LifxAPI
{
    [PayloadCollection("power-test")]
    public class PowerOn : Payload
    {
        [PayloadName("power")]
        public string Power { get; set; }
    }
}