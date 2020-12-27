using WeDoTakeawayAPI.GraphQL.Common;


namespace WeDoTakeawayAPI.GraphQL.Order
{
    public class AddOrderPayload : Payload
    {
        public Model.Order? Order { get; }

        public AddOrderPayload(Model.Order order)
        {
            Order = order;
        }

        public AddOrderPayload(UserError error)
            : base(new[] { error })
        {
        }
    }
}
