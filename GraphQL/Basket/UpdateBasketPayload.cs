namespace WeDoTakeawayAPI.GraphQL.Basket
{
    public class UpdateBasketPayload
    {
        public Model.Basket? Basket { get; }

        public UpdateBasketPayload(Model.Basket basket)
        {
            Basket = basket;
        }
    }
}
