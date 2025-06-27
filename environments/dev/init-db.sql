-- PostgreSQL initialization script for AICO development database

-- Create the database (this will be handled by Docker environment variables)
-- CREATE DATABASE aico_dev;

-- Create the user (this will be handled by Docker environment variables)
-- CREATE USER aico_user WITH PASSWORD 'aico_password';
-- GRANT ALL PRIVILEGES ON DATABASE aico_dev TO aico_user;

-- Connect to the aico_dev database
\c aico_dev;

-- Grant schema permissions
GRANT ALL ON SCHEMA public TO aico_user;
GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO aico_user;
GRANT ALL PRIVILEGES ON ALL SEQUENCES IN SCHEMA public TO aico_user;

-- Enable UUID extension (useful for generating UUIDs)
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Create any additional extensions that might be needed
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

SELECT 'Database initialized successfully' AS status;