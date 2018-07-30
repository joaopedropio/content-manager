# Build Environment
FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /build

COPY Helper Helper
COPY FFMPEGWrapper FFMPEGWrapper
COPY MP4BoxWrapper MP4BoxWrapper
COPY ContentManager ContentManager

RUN cd ContentManager && dotnet publish -c Release -o out

# Production Environment
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /build/ContentManager/out .
COPY *.gz /
RUN gzip -d /ffmpeg.gz && chmod +x /ffmpeg \
 && gzip -d /mp4box.gz && chmod +x /mp4box

# Environment Variables
ENV FFMPEG_PATH=/ffmpeg
ENV MP4BOX_PATH=/mp4box
ENV TEMP_FOLDER=content-converter/
ENV CONVERTED_FOLDER=converted/
ENV COMPRESSED_FOLDER=compressed/

ENTRYPOINT ["dotnet", "ContentManager.dll"]