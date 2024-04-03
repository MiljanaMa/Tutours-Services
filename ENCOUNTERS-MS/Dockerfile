FROM golang:alpine

WORKDIR /go

COPY go.mod .
COPY go.sum .
RUN go mod download
COPY . .
RUN go build -o ./build/encounter
EXPOSE 8083
ENTRYPOINT ["./build/encounter"]