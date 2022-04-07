namespace AMS.Tests.Mocks
{
    using Microsoft.AspNetCore.SignalR;

    using Moq;

    using AMS.Controllers.Hubs;

    public static class HubContextMock
    {
        public static IHubContext<BidHub> Instance
        {
            get
            {
                Mock<IHubClients> mockClients = new Mock<IHubClients>();
                mockClients.Setup(clients => clients.All).Returns(Mock.Of<IClientProxy>());

                var hubContext = new Mock<IHubContext<BidHub>>();
                hubContext.Setup(x => x.Clients).Returns(() => mockClients.Object);

                return hubContext.Object;
            }
        }
    }
}
