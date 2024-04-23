namespace PaymentGateway.Integration.Test;

public class ContainersFixture : IAsyncLifetime
{
    public ContainersFixture()
    {
        PostgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:alpine")
            .WithDatabase("SimpliPay")
            .WithResourceMapping(
                resourceContent: Encoding.UTF8.GetBytes(
                    """
                    CREATE SCHEMA clients;
                    CREATE SCHEMA payments;
                    CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
                    CREATE TABLE clients.clients (
                        id UUID PRIMARY KEY,
                        name VARCHAR(100) NOT NULL,
                        identification_number VARCHAR(20) NOT NULL,
                        email VARCHAR(100) NOT NULL,
                        phone VARCHAR(20) NOT NULL,
                        address TEXT NOT NULL,
                        city VARCHAR(50) NOT NULL,
                        state CHAR(2) NOT NULL,
                        postal_code VARCHAR(20) NOT NULL,
                        creation_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
                    );
                    CREATE TABLE clients.client_api_keys (
                        client_id UUID REFERENCES clients.clients(id),
                        api_key VARCHAR(100) UNIQUE NOT NULL,
                        expiration_date TIMESTAMP NOT NULL,
                        is_active BOOLEAN NOT NULL DEFAULT TRUE,
                        creation_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
                    );
                    CREATE TABLE payments.payment_methods (
                        id SERIAL PRIMARY KEY,
                        name VARCHAR(20) UNIQUE NOT NULL,
                        description VARCHAR(100),
                        creation_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
                    );
                    CREATE TABLE clients.client_payment_methods (
                        client_id UUID REFERENCES clients.clients(id),
                        payment_method_id INT REFERENCES payments.payment_methods(id),
                        fee REAL NOT NULL,
                        creation_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                    
                        CONSTRAINT unique_client_payment_method UNIQUE (client_id, payment_method_id)
                    );
                    CREATE TABLE payments.transactions (
                        id UUID PRIMARY KEY,
                        client_id UUID REFERENCES clients.clients(id),
                        payment_method_id INT REFERENCES payments.payment_methods(id),
                        amount DECIMAL(10,2) NOT NULL,
                        currency VARCHAR(3) NOT NULL,
                        description VARCHAR(255) NOT NULL,
                        card_number VARCHAR(16) NOT NULL,
                        card_holder VARCHAR(100) NOT NULL,
                        card_expiry DATE NOT NULL,
                        cvv CHAR(3) NOT NULL,
                        date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        creation_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
                    );
                    CREATE TABLE payments.transaction_status (
                        transaction_id UUID REFERENCES payments.transactions(id),
                        status SMALLINT NOT NULL,
                        details TEXT,
                        timestamp TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
                    );
                    CREATE TABLE payments.payables (
                        id UUID PRIMARY KEY,
                        transaction_id UUID REFERENCES payments.transactions(id),
                        client_id UUID REFERENCES clients.clients(id),
                        amount DECIMAL(10,2) NOT NULL,
                        currency VARCHAR(3) NOT NULL,
                        status SMALLINT NOT NULL,
                        payment_date DATE NOT NULL,
                        creation_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
                    );
                    INSERT INTO clients.clients (
                        id,
                        name,
                        identification_number,
                        email,
                        phone,
                        address,
                        city,
                        state,
                        postal_code
                    )
                    VALUES
                    (
                        '875cce09-eb96-4ed2-bab2-728b40cc0a98',
                        'César e Teresinha Joalheria ME',
                        '61575815000193',
                        'cobranca@cesareteresinhajoalheriame.com.br',
                        '1826794064',
                        'Rua Armando Rodrigues Tavares, 108 - Alto da Riviera',
                        'São Paulo',
                        'SP',
                        '19063450'
                    );
                    INSERT INTO clients.client_api_keys (
                        client_id,
                        api_key,
                        expiration_date
                    )
                    VALUES
                    (
                        '875cce09-eb96-4ed2-bab2-728b40cc0a98',
                        '<!eC8>8nYmKfJnT;B%EJH:J[f{WR5P]o',
                        CURRENT_TIMESTAMP + INTERVAL '1 year'
                    );
                    INSERT INTO payments.payment_methods (
                        name,
                        description
                    )
                    VALUES
                    (
                        'Credit Card',
                        'Payment method using credit card'
                    ),
                    (
                        'Debit Card',
                        'Payment method using debit card'
                    );
                    INSERT INTO clients.client_payment_methods (
                        client_id,
                        payment_method_id,
                        fee
                    )
                    VALUES
                        ('875cce09-eb96-4ed2-bab2-728b40cc0a98', 1, 0.05),
                        ('875cce09-eb96-4ed2-bab2-728b40cc0a98', 2, 0.03);
                    """
                ),
                filePath: "/docker-entrypoint-initdb.d/init.sql")
            .WithAutoRemove(true)
            .Build();

        KafkaContainer = new KafkaBuilder()
            .WithImage("confluentinc/cp-kafka:latest")
            .WithAutoRemove(true)
            .Build();
    }

    public PostgreSqlContainer PostgreSqlContainer { get; }

    public KafkaContainer KafkaContainer { get; }

    public Task DisposeAsync()
    {
        return Task.WhenAll(
            PostgreSqlContainer.DisposeAsync().AsTask(),
            KafkaContainer.DisposeAsync().AsTask());
    }

    public Task InitializeAsync()
    {
        return Task.WhenAll(
            PostgreSqlContainer.StartAsync(),
            KafkaContainer.StartAsync());
    }
}
