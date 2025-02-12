
#!/bin/bash

# Variables
cluster="https://<cluster-name>.<region>.kusto.windows.net"
database="<your-database-name>"
query="<your-kusto-query>"
client_id="<your-client-id>"
client_secret="<your-client-secret>"
tenant_id="<your-tenant-id>"

# Get access token
token=$(curl -X POST -H "Content-Type: application/x-www-form-urlencoded" \
    -d "grant_type=client_credentials&client_id=$client_id&client_secret=$client_secret&resource=https://kusto.kusto.windows.net&tenant_id=$tenant_id" \
    https://login.microsoftonline.com/$tenant_id/oauth2/token | jq -r '.access_token')

# Run Kusto query
response=$(curl -X POST -H "Authorization: Bearer $token" \
    -H "Content-Type: application/json" \
    -d "{\"db\":\"$database\",\"csl\":\"$query\"}" \
    $cluster/v1/rest/query)

# Output response
echo $response
