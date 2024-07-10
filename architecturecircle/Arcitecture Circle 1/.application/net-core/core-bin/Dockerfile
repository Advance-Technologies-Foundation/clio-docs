ARG NetCoreVersion=6.0
ARG AspEnvironment=Development

FROM mcr.microsoft.com/dotnet/sdk:${NetCoreVersion}

ENV ASPNETCORE_ENVIRONMENT=${AspEnvironment} \
    TZ=Europe/Kiev

RUN apt-get update && \
    apt-get -y --no-install-recommends install \
    libgdiplus \
    libc6-dev \
    gss-ntlmssp && \
    apt-get clean all && \
    rm -rf /var/lib/apt/lists/* /var/cache/apt/* && \
    sed -i 's/openssl_conf/#openssl_conf/g' /etc/ssl/openssl.cnf

WORKDIR /app
COPY . ./

EXPOSE 5000 5002
ENTRYPOINT ["dotnet", "Terrasoft.WebHost.dll"]
