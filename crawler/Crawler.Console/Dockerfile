#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

ADD https://dl.google.com/linux/direct/google-talkplugin_current_amd64.deb /src/google-talkplugin_current_amd64.deb

RUN apt-get update && apt-get install -y \
apt-transport-https \
ca-certificates \
curl \
gnupg \
hicolor-icon-theme \
libcanberra-gtk* \
libgl1-mesa-dri \
libgl1-mesa-glx \
libpango1.0-0 \
libpulse0 \
libv4l-0 \
unzip \
fonts-symbola \
--no-install-recommends \
&& curl -sSL https://dl.google.com/linux/linux_signing_key.pub | apt-key add - \
&& echo "deb [arch=amd64] https://dl.google.com/linux/chrome/deb/ stable main" > /etc/apt/sources.list.d/google.list \
&& apt-get update && apt-get install -y \
google-chrome-stable \
--no-install-recommends \
#&& apt-get purge --auto-remove -y curl \
&& rm -rf /var/lib/apt/lists/*

RUN set -x \
&& curl -sSL "https://dl.google.com/linux/direct/google-talkplugin_current_amd64.deb" -o /tmp/google-talkplugin-amd64.deb \
&& dpkg -i /tmp/google-talkplugin-amd64.deb \
&& mkdir \opt\selenium \
&& curl -sSL "https://chromedriver.storage.googleapis.com/88.0.4324.96/chromedriver_linux64.zip" -o /tmp/chromedriver.zip \
&& unzip -o /tmp/chromedriver -d /opt/selenium/ \
&& rm -rf /tmp/*.deb 
#&& apt-get purge -y --auto-remove curl unzip

RUN groupadd -r chrome && useradd -r -g chrome -G audio,video chrome \
&& mkdir -p /home/chrome/Downloads && chown -R chrome:chrome /home/chrome


FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Crawler.Console/Crawler.Console.csproj", "Crawler.Console/"]
COPY ["Crawler.Pages/Crawler.Pages.csproj", "Crawler.Pages/"]
RUN dotnet restore "Crawler.Console/Crawler.Console.csproj"
COPY . .
WORKDIR "/src/Crawler.Console"
RUN dotnet build "Crawler.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Crawler.Console.csproj" -c Release -o /app/publish

FROM base AS final
ENV PAGE_URL="http://localhost:80/"
ENV IMAGES_DIRECTORY="/images"
ENV WEB_DRIVER_DIRECTORY="/opt/selenium"
ENV MONGODB_URL="mongodb://mongouser:mongopwd@localhost:27017/admin"
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Crawler.Console.dll"]