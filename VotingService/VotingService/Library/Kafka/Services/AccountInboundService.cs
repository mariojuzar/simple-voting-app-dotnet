using IdentityService.Library.Logger.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using VotingService.Models.DAL;

namespace VotingService.Library.Kafka.Services
{
    public class AccountInboundService : BackgroundService
    {
        private const String topicKafka = "identity.service.user";

        ILoggerManager logger;

        private readonly IServiceScopeFactory scopeFactory;

        private KafkaConsumer kafkaConsumer;

        public AccountInboundService(ILoggerManager logger, IServiceScopeFactory scopeFactory)
        {
            this.logger = logger;
            this.scopeFactory = scopeFactory;
            kafkaConsumer = new KafkaConsumer(topicKafka);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() => StartConsumer(stoppingToken));
            return Task.CompletedTask;
        }

        private void StartConsumer(CancellationToken stoppingToken)
        {
            logger.LogInfo("Listener for new account user started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = kafkaConsumer.GetConsumer().Consume(stoppingToken);
                
                if (consumeResult.Message != null)
                {
                    var request = consumeResult.Message.Value;

                    logger.LogInfo("listen new message account user : " + request);

                    KafkaEntityWrapper<Users> kafkaEntity = JsonConvert.DeserializeObject<KafkaEntityWrapper<Users>>(request);

                    if (kafkaEntity != null && kafkaEntity.actionType == "CREATE")
                    {
                        SaveAccountUser(kafkaEntity.data);
                    }
                    else
                    {
                        logger.LogError("failed to convert message");
                    }
                }
                
            }
        }

        private bool SaveAccountUser(Users user)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<voting_dbContext>();

                using (IDbContextTransaction transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        dbContext.Users.Add(user);
                        dbContext.SaveChanges();
                        transaction.Commit();
                        logger.LogInfo("success save new account user");

                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        logger.LogError("failed to save new account user");

                        return false;
                    }
                }
            }
            
        }
    }
}
