DO $$
DECLARE
    r RECORD;
BEGIN
    FOR r IN (
        SELECT schema_name
        FROM information_schema.schemata
        WHERE schema_name IN ('shopia', 'action', 'log', 'auth')
    )
    LOOP
        EXECUTE 'DROP SCHEMA ' || quote_ident(r.schema_name) || ' CASCADE';
    END LOOP;
END $$;