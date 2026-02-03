-- Activate PostGIS in DB in order to create geometry, and geography types, functions, and indexes
CREATE EXTENSION IF NOT EXISTS postgis SCHEMA public;
CREATE EXTENSION IF NOT EXISTS vector SCHEMA public;
GRANT USAGE ON SCHEMA public TO "shopia-test";

-- GRANT EXECUTE ON ALL FUNCTIONS IN SCHEMA public TO tu_usuario;

--ALTER DEFAULT PRIVILEGES IN SCHEMA public
--GRANT EXECUTE ON FUNCTIONS TO tu_usuario;
