@ECHO OFF

docker run --name keycloak_test -d -p 8080:8080 -e KEYCLOAK_ADMIN=admin -e KEYCLOAK_ADMIN_PASSWORD=admin quay.io/keycloak/keycloak:20.0.1 start-dev --http-relative-path=/auth

timeout /t 35

dotnet run --project ./Keycloak.Migrator --framework net6.0  migrate --keycloak-url="http://localhost:8080" --keycloak-password=admin --keycloak-username=admin --keycloak-json-migration="./roles.json" --keycloak-client-id=master

dotnet run --project ./Keycloak.Migrator --framework net6.0  validate --keycloak-url="http://localhost:8080" --keycloak-password=admin --keycloak-username=admin --keycloak-json-migration="./roles.json" --keycloak-client-id=master

docker kill keycloak_test
docker container rm keycloak_test