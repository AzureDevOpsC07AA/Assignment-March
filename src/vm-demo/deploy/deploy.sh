#!/bin/bash

docker pull ghcr.io/YOUR_ORG/legacy-inventory-api:latest

docker stop legacy-api || true
docker rm legacy-api || true

docker run -d \
  --name legacy-api \
  -p 8080:8080 \
  ghcr.io/YOUR_ORG/legacy-inventory-api:latest