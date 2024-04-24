# SimpliPay 💳💸
SimpliPay is a simplified payment service provider (PSP) that facilitates electronic payment transactions between various parties, such as customers, companies and banks. It provides credit card and debit card payments.

## Features
- HTTP API interface
- Topic-based message queue for processing payments
- ApiKey-based authentication
- Rate limiting
- Handling errors and exceptions globally
- Unit and integration tests
- Dockerized application

## Endpoints
- POST /api/v1/payments: Create a payment
- GET /api/v1/payments/{id} : Get payment details by id
- GET /api/v1/payments: Get all payments details
- GET /api/v1/balances: Get the balance summary of all payments

## Technologies
- .NET Core 8.0
- Docker/Docker Compose
- PostgreSQL
- Kafka
- MassTransit
- xUnit

## How to run

### Prerequisites

If you want to run the application locally, you need to have the following tools installed on your machine:

- .NET SDK >= 8.0.0
- Docker Engine >= 24.0.0 With Docker Compose

Or you can run the application using the [Dev Container](https://containers.dev/) feature of Visual Studio Code or the [GitHub Codespaces](https://docs.github.com/en/codespaces) feature.

#### Steps

1. Clone this repository to your local machine

```bash
git clone https://github.com/leoguilen/simplipay.git
```

2. Go to the project root folder

```bash
cd simplipay
```

3. Run the following command which will deploy the environment

```bash
docker compose -f ./deploy/docker/compose.yaml up -d --build
```

4. Wait for the command to finish and access the following services with the URLs below:

- Payment Gateway API: [http://localhost:8080/swagger](http://localhost:8080/swagger)
- Kafka UI (kafdrop): [http://localhost:19000](http://localhost:19000)

5. For testing the application, you can use any of the following ApiKeys:

- `<!eC8>8nYmKfJnT;B%EJH:J[f{WR5P]o`
- `o56nS@{7Kio?9[Dh:sx*U@g^:Eiu4(I+`
- `bmUOa*LOGR]beu8MUDs)+PM5f^3|lqBj`
- `);3:KT0OF]My$r}W-i,uQI|%o6k6z^X3`
- `$Uai9oyi>6e*4L$zDxCQL1X-9A9ABA9l`

6. For make a payment request, you can use the following payload:

```json
{
  "amount": {
    "value": 9.90,
    "currency": "BRL"
  },
  "card": {
    "number": "5278460298233958",
    "holderName": "Sylvester Stallone",
    "expiryDate": "2025-02-24",
    "cvv": "879"
  },
  "paymentMethod": 1, // 1 - CreditCard, 2 - DebitCard
  "description": "Machine Gun",
  "date": "2024-04-24T00:00:00"
}
```

7. To stop the environment, run the following command:

```bash
docker compose -f ./deploy/docker/compose.yaml down
```

8. To run the automated tests, run the following command:

```bash
dotnet test
```