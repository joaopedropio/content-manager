# Build Environment
FROM microsoft/dotnet:2.2-sdk-alpine AS build-env
WORKDIR /build

COPY ContentManager ContentManager
COPY FFMPEG FFMPEG
COPY Helper Helper
COPY Manager Manager
COPY MP4Box MP4Box

RUN cd ContentManager && dotnet publish -c Release -o out

# Production Environment
FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine
WORKDIR /app
COPY --from=build-env /build/ContentManager/out .
COPY *.gz /
RUN gzip -d /ffmpeg.gz  && chmod +x /ffmpeg  && \
    gzip -d /ffprobe.gz && chmod +x /ffprobe && \
    gzip -d /mp4box.gz  && chmod +x /mp4box

ENTRYPOINT ["dotnet", "ContentManagerWeb.dll"]
