namespace AMS.Services.Contracts
{
    using AMS.Services.Models;

    public interface IStatisticService
    {
        public Task<StatisticsServiceModel> Total();
    }
}
