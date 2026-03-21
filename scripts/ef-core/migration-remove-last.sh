#!/bin/bash

YELLOW='\033[1;33m'
NC='\033[0m'

cd "$(dirname "$0")/.."

STARTUP_PROJECT="../../apps/VaccineManager.Api/VaccineManager.Api.csproj"
INFRA_PROJECT="../../libs/backend/VaccineManager.Infrastructure/VaccineManager.Infrastructure.csproj"

echo -e "${YELLOW}Removendo a última migração do código (C#)...${NC}"
echo "Nota: Se isso falhar, é porque a migração já foi aplicada no banco. Use db-revert primeiro."

dotnet ef migrations remove \
    --project "$INFRA_PROJECT" \
    --startup-project "$STARTUP_PROJECT" -v