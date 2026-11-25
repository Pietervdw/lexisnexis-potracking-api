## Solution Overview

I've used my go to design when building Web Api's. It's a relatively simple three-layer architecture. It consists of three parts:

1. API
2. Business or Service Layer
3. Data layer

The dependency diagram are:
![dependency diagram](https://res.cloudinary.com/derojx8ls/image/upload/v1795612958/solution_overview_ahtdzn.png)

## API

The controllers are very light on purpose adheres to the Single Responsibility principle, it's job is to serve the API, not do any business logic. The business logic always resides in the business layer.  I use dependency injection to inject the needed services into the controllers.

I've also followed a standard RESTful approach for the controller methods. We use POST to create a Purchase Order, and PATCH to set the new status for a PO. I've decided to use PATCH because we're not replacing an object, but only updating a single property.



### Business/Service Layer

All the business logic resides in service classes. If we take the **PurchaseOrderService** as an example, we inject the **IPurchaseOrderRepository** and **ICustomerService**, you'll notice we always code against the interfaces, this makes it easier if we want to swop out dependencies without breaking the system.

**a. Creating a Purchase Order**

The Create method returns a **PurchaseOrderCreateResponse** which is a subtype of **ApiResponse**. I used a a base class here because i wanted a standardized API response to make it easier to work with the data in the UI since we know what data shape to expect. The **ApiResponse** class is just a way to give us a standard way of indicating whether the request was successful and if not what the errors were.

When creating a new Purchase order we first validate it using the **Validate** method. This checks required fields, and other business rules. I considered using Data Annotations, but I made the trade-off to keep the validation logic in one place. 

**b. Changing the status for a Purchase Order**

The logic for the Purchase Order statuses resides in the **PurchaseOrder** class. The CanStatusMoveTo method is used to check against the *AllowedStatusTransitions* dictionary, whether a status can be moved to another. The status config are:
| Current Status | Allowed Next Statuses |
|---|---|
| `Pending` | `Confirmed`, `Cancelled` |
| `Confirmed` | `Shipped`, `Cancelled` |
| `Shipped` | `Fulfilled` |
| `Fulfilled` | None (final status) |
| `Cancelled` | None (final status) |

This follows the Open/Close principle were we do not change the **CanStatusMove** method, but add new config to the *AllowedStatusTransitions* dictionary.



### Data Layer

The Data layer consists of two repositories. Both keep stores their data in-memory, this means restarting the project will clear any data. I use *ConcurrentDictionary* to store the data since it is type safe. The **PurchaseOrderRepository's** *ConcurrentDictionary* uses the External Reference number of the PO for it's unique key, it is also case-insensitive so that we don't have a situation where reference numbers are duplicated but just difference cases.

One small thing that we can be pedantic about it that we assign a PO number in the Create method, this does break Single Responsibility.  I accepted that as an acceptable trade off. It should be fine if the **IPurchaseOrderRepository** uses a concrete implementation with a database store.

It should be relatively simple to swop to a database with this architecture because we've coded against the interface, we can simply add a new POTracking.Data.Sql project with it's own **PurchaseOrderDatabaseRepository** that implements *IPurchaseOrderRepository* we can then swop it in Program.cs e.g:

```
builder.Services.AddTransient<IPurchaseOrderRepository, PurchaseOrderDatabaseRepository>();
```

Quick note on the Dependency injection, we currently use .AddSingleton because we need to store the data in-memory.



### Tests

The unit tests cover most of the business logic. I've depended on AI to generate it. I did remove some in order to focus on the core tests for this project.



### UI

I come from a Vue/Nuxt background, so I understand JavaScript frameworks and TypeScript. I haven't worked with Angular much, so I relied heavily on Claude to help build the UI. I've created a new project using `ng new` and did some research on what is the accepted and recommended solution structure for an Angular project. I was happy to see that the recommended approach is a feature-based layout. This is similar to how I structure my Flutter projects (Which makes sense, since both are made by Google 🙂).

The concept is straight forward, the data is retrieved using a service and displayed on the UI. I've split the template and component code into separate files, this is a personal preference and is close to how Vue does it. I tried to keep the routing clear and concise almost mimicking the API.

The project includes this, I know this wasn't strictly required,  but I was curious to see how Angular does this.

I then employed the skills popularized by [Matt Pocock](https://www.aihero.dev/skills). Overview of my process:

1. I first use the **/grill-with-docs** skill with a brief:
   >We need to design a Purchase Order tracking system UI that uses the API as defined in C:\LexisNexis\lexisnexis-potracking-api\POTracking.Api. We need the following functionality:
   >- Ability to view all Purchase Orders, sorted by latest PO. Add Ability to filter/search by PO
   >- Create a Purchase Order
   >- View a Purchase Order
   >- Change the Purchase Order status on the View page/component
   >
   >Use Tailwind for styling and keep the UI minimal and clean

2.  We work through the questions, in this case about 12 and settle on an agreed requirement

3. Next, I use the **/to-spec** to turn the discussion into a written spec

4. This spec is then turned into smaller chunks using the **/to-ticket** skill and a GitHub issue is created for each

5. I then work through each issue, each issue is a new discussion as this keeps the AI in the 'smart zone' because we keep the context as small as possible.

6. Once an issue is completed, I use the **/handover** skill to write a handover comment to the next issue, so that it can be aware of any relevant changes

7. When committing the changes we make sure to mention the Issue number in the commit message, this makes it easy for us to look at a issue later on and see what was changed or added for it.

I prefer this method because a large unit of work can quickly spiral out of control, by breaking the work down into manageable chunks and keeping a record of change using Github issues you can maintain control. 

