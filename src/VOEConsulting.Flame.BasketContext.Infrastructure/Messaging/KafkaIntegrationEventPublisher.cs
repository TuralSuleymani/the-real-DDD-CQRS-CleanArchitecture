using System.Text.Json;
using Confluent.Kafka;
using VOEConsulting.Flame.BasketContext.Application.Abstractions.Messaging;
using VOEConsulting.Flame.Common.Core.Events;

namespace Infrastructure.Messaging
{
    public class KafkaIntegrationEventPublisher : IIntegrationEventPublisher
    {
        private readonly IProducer<string, string> _producer;
        private readonly string _defaultTopic;

        public KafkaIntegrationEventPublisher(string bootstrapServers, string defaultTopic)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                Acks = Acks.All, // Wait for all replicas to acknowledge
                EnableIdempotence = true, // Ensure exactly-once delivery
                CompressionType = CompressionType.Snappy, // Optimize message size
                LingerMs = 5 // Batch messages for better throughput
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
            _defaultTopic = defaultTopic;
        }

        public async Task PublishAsync<T>(T @event) where T : IntegrationEvent
        {
            // Determine the topic based on the event type
            var topic = _defaultTopic ?? @event.GetType().Name;

            // Serialize the event to JSON
            var message = JsonSerializer.Serialize(@event);

            try
            {
                // Produce the message
                var kafkaMessage = new Message<string, string>
                {
                    Key = @event.AggregateId.ToString(),
                    Value = message
                };

                var deliveryResult = await _producer.ProduceAsync(topic, kafkaMessage);

                Console.WriteLine($"Delivered event to Kafka: {deliveryResult.TopicPartitionOffset}");
            }
            catch (ProduceException<string, string> ex)
            {
                // Log the exception or handle it as per your needs
                Console.WriteLine($"Failed to deliver event: {ex.Message}");
                throw;
            }
        }
    }
}
