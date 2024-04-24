# syntax=docker/dockerfile:1

ARG DOTNET_VERSION=8.0
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION}-alpine AS build
ARG TARGETPLATFORM
ARG TARGETARCH
ARG TARGETOS
WORKDIR /source
COPY . .
RUN dotnet publish -c Release -o /app -a ${TARGETARCH/amd64/x64} --os $TARGETOS

FROM --platform=$TARGETPLATFORM mcr.microsoft.com/dotnet/runtime:${DOTNET_VERSION}-alpine AS runtime
WORKDIR /app
COPY --from=build /app/ .
ENTRYPOINT ["dotnet", "PaymentProcessor.dll"]