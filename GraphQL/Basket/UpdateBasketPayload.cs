using WeDoTakeawayAPI.GraphQL.Common;


namespace WeDoTakeawayAPI.GraphQL.Basket
{
    public class UpdateBasketPayload : Payload
    {
        public Model.Basket? Basket { get; }

        public UpdateBasketPayload(Model.Basket basket)
        {
        Basket = basket;
        }

        public UpdateBasketPayload(UserError error)
            : base(new[] { error })
        {
        }
    }
}
