# GraphQL API for We Do Takeaway


Note you need to set an environment variable for the database connection

e.g. : ConnectionStrings__DefaultConnection="Host=postgres;Username=order;Password=password;Database=order"

## Scope and purpose

This project hosts the complete backend API for We Do Takeaway. Initially this just includes the Order and Basket API's but is also being expanded to include customers, menus and drivers.

### Basket

The basket API and data allows the client to update items that the customer is currently interested in buying, this includes the items and their associated quantities.

### Order
The order section is more complex and records and updates:

* Orders associated with a customer
* Order items and their qty
* Order fulfillment status for each order (new, awaiting payment, being cooked, ready for pickup, in transit, delivered, returned, lost)
* Payment details associated with an order
* Delivery instructions for order
* Allocated driver
* Allocated chef (maybe)
* Audit trail of order status changes and times

Payment details should probably have their own area too.

Returns are not supported at this time

Refunds are not supported at this time

Offers are not supported at this time

