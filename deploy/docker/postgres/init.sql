-- Schemas
CREATE SCHEMA clients;
CREATE SCHEMA payments;

-- Extensions
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Tables
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

-- Indexes
CREATE INDEX idx_client_payable ON payments.payables (client_id, id);
CREATE INDEX idx_client_api_key ON clients.client_api_keys (client_id, api_key);
CREATE INDEX idx_client_payment_method ON clients.client_payment_methods (client_id, payment_method_id);

-- Seed data
INSERT INTO clients.clients (
    id,
    name,
    identification_number,
    email,
    phone,
    address,
    city,
    state,
    postal_code)
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
    ),
    (
        '9612309e-12c4-4b54-b96c-cb925fd77791',
        'Giovanna e Thales Informática Ltda',
        '01888287000109',
        'comunicacoes@giovannaethalesinformaticaltda.com.br',
        '8725661853',
        'Rua Clemilda Gomes da Silva, 938 - Pedro Raimundo',
        'Petrolina',
        'PE',
        '56318255'
    ),
    (
        '2cb49547-9e34-4a50-8d39-b056d5cc5182',
        'Rita e Simone Adega ME',
        '44772184000183',
        'presidencia@ritaesimoneadegame.com.br',
        '2137563710',
        'Rua Clara Nunes, 134 - Campo Grande',
        'Rio de Janeiro',
        'RJ',
        '23071651'
    ),
    (
        'a8b71cf6-7d3d-46f0-908b-0f39abde25fb',
        'Bryan e Helena Esportes Ltda',
        '13726943000148',
        'fiscal@bryanehelenaesportesltda.com.br',
        '4825760013',
        'Rua Pedro Jeremias Mendes, 310 - São João (Margem Esquerda)',
        'Tubarão',
        'SC',
        '88708535'
    ),
    (
        'aed1eb74-5584-4a9b-a6fb-6729254e5765',
        'Aurora e Luís Filmagens ME',
        '11458305000168',
        'fiscal@auroraeluisfilmagensme.com.br',
        '3729122914',
        'Rua Três, 160 - Marajó',
        'Divinópolis',
        'MG',
        '35501542'
    );

INSERT INTO clients.client_api_keys (
    client_id,
    api_key,
    expiration_date)
VALUES
    (
        '875cce09-eb96-4ed2-bab2-728b40cc0a98',
        '<!eC8>8nYmKfJnT;B%EJH:J[f{WR5P]o',
        CURRENT_TIMESTAMP + INTERVAL '1 year'
    ),
    (
        '9612309e-12c4-4b54-b96c-cb925fd77791',
        'o56nS@{7Kio?9[Dh:sx*U@g^:Eiu4(I+',
        CURRENT_TIMESTAMP + INTERVAL '1 year'
    ),
    (
        '2cb49547-9e34-4a50-8d39-b056d5cc5182',
        'bmUOa*LOGR]beu8MUDs)+PM5f^3|lqBj',
        CURRENT_TIMESTAMP + INTERVAL '1 year'
    ),
    (
        'a8b71cf6-7d3d-46f0-908b-0f39abde25fb',
        ');3:KT0OF]My$r}W-i,uQI|%o6k6z^X3',
        CURRENT_TIMESTAMP + INTERVAL '1 year'
    ),
    (
        'aed1eb74-5584-4a9b-a6fb-6729254e5765',
        '$Uai9oyi>6e*4L$zDxCQL1X-9A9ABA9l',
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
    ('875cce09-eb96-4ed2-bab2-728b40cc0a98', 2, 0.03),
    ('9612309e-12c4-4b54-b96c-cb925fd77791', 1, 0.05),
    ('9612309e-12c4-4b54-b96c-cb925fd77791', 2, 0.03),
    ('2cb49547-9e34-4a50-8d39-b056d5cc5182', 1, 0.05),
    ('2cb49547-9e34-4a50-8d39-b056d5cc5182', 2, 0.03),
    ('a8b71cf6-7d3d-46f0-908b-0f39abde25fb', 1, 0.05),
    ('a8b71cf6-7d3d-46f0-908b-0f39abde25fb', 2, 0.03),
    ('aed1eb74-5584-4a9b-a6fb-6729254e5765', 1, 0.05),
    ('aed1eb74-5584-4a9b-a6fb-6729254e5765', 2, 0.03);