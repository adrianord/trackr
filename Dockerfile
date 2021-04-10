#syntax=docker/dockerfile:1.2

#######################################
## This Dockerfile requires BuildKit ##
#######################################

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS dotnet_build

ARG DOTNET_BASE_DIR=api
ARG CONFIG_PROFILE=Release
ARG PROJECT_DIR
ARG PROJECT_NAME

ENV PROJECT=${PROJECT_DIR}/${PROJECT_NAME}.csproj
WORKDIR /app

COPY ${DOTNET_BASE_DIR}/nuget.config* ./
COPY ${DOTNET_BASE_DIR}/*.sln ./

## Copy .csproj files into the correct file structure
RUN apt update && apt-get install -y bash && rm -rf /var/lib/apt/lists/*
SHELL ["/bin/bash", "-O", "globstar", "-c"]
RUN --mount=target=docker_build_context \
cd docker_build_context/${DOTNET_BASE_DIR}/;\
cp **/*.csproj /app --parents;
RUN rm -rf docker_build_context
SHELL ["/bin/sh", "-c"]

RUN dotnet restore ${PROJECT}
COPY ${DOTNET_BASE_DIR} ./
RUN dotnet publish --no-restore -c ${CONFIG_PROFILE} -o out ${PROJECT}

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS dotnet_final
ARG PROJECT_NAME

WORKDIR /app

COPY --from=dotnet_build /app/out .
ENV ASPNETCORE_URLS=http://+:80

RUN ln -s ${PROJECT_NAME}.dll Entrypoint.dll

ENTRYPOINT [ "dotnet", "Entrypoint.dll" ]

FROM dotnet_final AS dotnet_debug

RUN apt-get update &&\ 
    apt-get install -y bash netcat curl &&\
    rm -rf /var/lib/apt/lists/*
