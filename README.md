# GraphQL API for We Do Takeaway

## Tools
The GraphQL API is created using the amazing Hot Chocolate GraphQL framework and is written in C# and makes use of .Net frameworks.

## Settings
Database config should be done via an environment variable:
i.e. : ConnectionStrings__DefaultConnection="Host=postgres;Username=order;Password=password;Database=order"

## Scope and purpose

The We Do Takeaway service is created using an API first approach, this means that the API for the service is created first and the various apps and front-end clients come next and drive the API.

The API for We Do Takeaway takes the form of a GraphQL API. GraphQL has some advantages over REST, allow scalable development and reduced network requests.

## Entities

### Menu
The service is made up of one or more menus, giving a top level name and introduction to a menu the user can pick items from.

### Section
A menu has one or more sections, e.g. Desert. Sections group together items the user can pick from

### Item
A menu item is a thing you order from the menu.

### Ingredient
A menu item has one or more ingredients that go into creating it

### Basket
A basket is a container to keep a collection of items and the associated quantities the user wishes to purchase of each item.

### Order
The contents of a basket are turned into an order at checkout. From that point onwards the order is a central entity recording the items the customer has ordered and their status in the kitchen through to delivery progress

## Order Processing
The order section is more complex and records and updates:

* Orders associated with a customer
* Order items and their qty
* Order fulfillment status for each order (new, awaiting payment, being cooked, ready for pickup, in transit, delivered, returned, lost)
* Payment details associated with an order
* Delivery instructions for order
* Allocated driver
* Allocated chef (maybe)
* Audit trail of order status changes and times


