# Build Environment
FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /build

COPY Helper Helper
COPY FFMPEGWrapper FFMPEGWrapper
COPY MP4BoxWrapper MP4BoxWrapper
COPY ContentManager ContentManager

RUN mkdir app		
RUN cd Helper && dotnet publish -c Release -o out && cd out && cp Helper.dll Helper.pdb ../../app && cd ../../
RUN cd FFMPEGWrapper && dotnet publish -c Release -o out && cd out && cp FFMPEGWrapper.dll FFMPEGWrapper.pdb ../../app && cd ../../
RUN cd MP4BoxWrapper && dotnet publish -c Release -o out && cd out && cp MP4BoxWrapper.dll Helper.pdb ../../app && cd ../../
RUN cd ContentManager && dotnet publish -c Release -o out && cd out && cp * ../../app

# Production Environment
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /build/app .
COPY ffmpeg.gz /
COPY mp4box.gz /
RUN gzip -d /ffmpeg.gz && chmod +x /ffmpeg
RUN gzip -d /mp4box.gz && chmod +x /mp4box

# Environment Variables
ENV FFMPEG_PATH=/ffmpeg
ENV MP4BOX_PATH=/mp4box
ENV TEMP_FOLDER=content-converter/
ENV CONVERTED_FOLDER=converted/
ENV COMPRESSED_FOLDER=compressed/

ENTRYPOINT ["dotnet", "ContentManager.dll"]