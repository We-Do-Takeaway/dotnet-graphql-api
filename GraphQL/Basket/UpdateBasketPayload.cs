using System.Collections.Generic;
using WeDoTakeawayAPI.GraphQL.Common;
using WeDoTakeawayAPI.GraphQL.Model;

namespace WeDoTakeawayAPI.GraphQL.Baskets
{
    public class UpdateBasketPayload : Payload
    {
        public Basket? Basket { get; }
        
        public UpdateBasketPayload(Basket basket)
        {
            Basket = basket;
        }
        
        public  UpdateBasketPayload(UserError error)
            : base(new[] { error })
        {
        }
    }
}