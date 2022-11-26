# Checkout.com Payment Gateway Test

This document describes my approach to solving the test as well as the instructions how to run it.

## 1. Running solution

Solution makes use of a Docker Compose project which describes two services: .NET Core API and Postgres local DB.

In order to run the soluction locally, execute the following:

`docker-compose build`
`docker-compose up`

The above will:

1. Build the .NET Core API
2. Fetch the Postgres image and run an instance locally, exposing it on the host port `5532`
3. Run the API which will create a database and run all the necessary migrations. The API binds to the port `5080`

Alternatively, the solution can be run with Visual Studio in the debug mode. In this case make sure `docker-compose` mode is selected in VS.

### 1.1. Issuing requests

After starting the solution, the API will be availabe on http://localhost:5080

#### 1.1.1. Making a payment

Issue a POST request to the http://localhost:5080/payments endpoint with your tool of choice. 

Example JSON payload:

```json
{
"merchantId": "941d05b6-ce68-4dd9-b973-5134ccca0637",
"cardNumber": "4012888888881881",
"expiryMonth": 2,
"expiryYear": 2040,
"cvv": "332",
"amount": 350,
"currency": "USD"
}
```

That would requrn a new payment id. 

Example successful response (HTTP code 200):

```
"41396ef9-1417-4049-8635-28b5095eaefc"
```

Example unsuccessful response (HTTP code 400):

```json
{
    "CardNumber": [ "Invalid card details" ]
}
```

### 1.1.2. Querying a payment

Issue a GET request to the http://localhost:5080/payments/{paymentId}?merchantId={merchantId} endpoint with your tool of choice. 

Example:

http://localhost:5080/payments/32900fbf-532e-461d-89f0-0ca9ccc2cf16?merchantId=941d05b6-ce68-4dd9-b973-5134ccca0637

Returns 200 OK with the following body if the payment exists and belongs to the merchant:

```json
{
"amount": 350,
"currency": "USD",
"cardNumber": "************1881",
"expiryMonth": 2,
"expiryYear": 2040,
"status": "Success"
}
```

The endpoint returns 404 Not Found if the payment does not exist or if it does not belong to the merchant.

### 1.2. Running tests

In order to run unit tests, perform the following command:

```
dotnet test test/CKO.PaymentGateway.Domain.UnitTests/CKO.PaymentGateway.Domain.UnitTests.csproj
```

## 2. Solution Structure

My goal was to separate business, coordination, and infrastructure concerns so that the solution would be easily to modify without introducing unexpected side effects. I've opted for using MediatR to separat commands and queries, as well as to support events. That allowed to build the solution around in-memory messaging, which, if needed, would allow to easily slice system capabilities into independently deployed services.

Solution is divided in separate projects in order to maintain loose coupling between various concerns:

- **CKO.PaymentGateway.Api** - a .NET Core API. Responsible for composing the components of the solution up, mapping requests and responses, and issuing MediatR commands and queries
- **CKO.PaymentGateway.Application** - a class library which contains commands, queries, events, and respective handlers. This project is responsible for domain coordination: it fetches domain entities from the DB, invokes appropriate methods, and saves the result back to the DB.
- **CKO.PaymentGateway.Domain** - contains the domain entities and all the domain logic related to the processing of payments: `Payment` and `CardDetails` classes. It also contains domain events that can be raised by domain entities, e.g., `PaymentCreated` domain event which is created by the `Payment` class.
- **CKO.PaymentGateway.DataAccess** - contains the data access layer concerns. The project describes the mapping between the domain entities and the database. It also contains a set of migrations that allow to configure the DB schema.
- **CKO.PaymentGateway.Domain.UnitTests** - contains unit tests for business logic.
- **CKO.PaymentGateway.BankSimulator** - contains an in-memory bank simulator. The simulator provides an in-memory interface to process the payment, and issues `PaymentSucceeded` and `PaymentFailed` MediatR events. There's a 50% chance for a payment to succeed. I have left the simulator as simple as possible since it was out of the scope of the test.

## 3. Tests

My testing strategy was to focus on testing the domain logic. I have added unit tests under the `CKO.PaymentGateway.Domain.UnitTests` project in order to test the behavior of payments and related entities. 

I left integration tests out because of the time limitations.

I have left the unit tests for `PaymentAmount` class because the lack of time.

## 4. Points of improvement

This is solution is not production ready. To make it such, I would do the following:

- Add authentication and authorization. Currently, merchant is identified by an ID passed in the payload. Ideally, OAuth should be used.
- Banking Simulator is an in-memory stub. In a real-world scenario, it would be a third-party service.
- There's no support for non-localhost environments. Ideally, DB connection strings and all other sensitive resources would be stored in a secret manager, provided by the cloud provider or a third-party vault system.
- There's no centralized log collection. Ideally, all the system logs would be streamed in a centralized system.

## 5. Migrations

The following are the commands to add, remove, and run migrations during dev work. The API project will also run the migrations automatically on a startup.

Add:
`dotnet ef migrations add <Migration Name> --context PaymentGatewayContext -s src/CKO.PaymentGateway.Api/CKO.PaymentGateway.Api.csproj -p src/CKO.PaymentGateway.DataAccess/CKO.PaymentGateway.DataAccess.csproj --output-dir Migrations`

Remove:
`dotnet ef migrations remove --context PaymentGatewayContext -s src/CKO.PaymentGateway.Api/CKO.PaymentGateway.Api.csproj -p src/CKO.PaymentGateway.DataAccess/CKO.PaymentGateway.DataAccess.csproj`

Update DB:
`dotnet ef database update --context PaymentGatewayContext -s src/CKO.PaymentGateway.Api/CKO.PaymentGateway.Api.csproj -p src/CKO.PaymentGateway.DataAccess/CKO.PaymentGateway.DataAccess.csproj`
