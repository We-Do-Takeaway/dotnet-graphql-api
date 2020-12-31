namespace WeDoTakeawayAPI.GraphQL.Order
{
    public class AddOrderPayload
    {
        public Model.Order? Order { get; }

        public AddOrderPayload(Model.Order order)
        {
            Order = order;
        }
    }
}
