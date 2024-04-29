FROM golang:alpine

WORKDIR /go

COPY go.mod .
COPY go.sum .
RUN go mod download
COPY . .
RUN go build -o ./build/tour
EXPOSE 8000
ENTRYPOINT ["./build/tour"]