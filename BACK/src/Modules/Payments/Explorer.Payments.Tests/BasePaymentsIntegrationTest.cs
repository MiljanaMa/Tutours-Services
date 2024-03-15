using Explorer.BuildingBlocks.Tests;

namespace Explorer.Payments.Tests;

public class BasePaymentsIntegrationTest : BaseWebIntegrationTest<PaymentsTestFactory>
{
    public BasePaymentsIntegrationTest(PaymentsTestFactory factory) : base(factory)
    {
    }
}