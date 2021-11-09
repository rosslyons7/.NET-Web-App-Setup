namespace AuthorisationService.Producers {
    public interface IRabbitProducer {
        void ProduceMessage(object message, string exchange, string routingKey);
    }
}