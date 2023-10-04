using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService.Consumers
{
    public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
    {
        private readonly IMapper _mapper;

        public AuctionCreatedConsumer(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<AuctionCreated> context)
        {
            Console.WriteLine("--> Consuming auction created: " + context.Message.Id);
            //int retryAttemp=context.GetRetryAttempt();
            //int retry = context.GetRetryCount();
            var item = _mapper.Map<Models.Item>(context.Message);

            if (item.Model == "Foo") throw new ArgumentException("Cannot sell cars with name of Foo");

            await item.SaveAsync();
        }
    }
}
